using GalaSoft.MvvmLight.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class AllPatientsViewModel : AllViewModel<PatientForAllView>
    {
        private List<PatientForAllView> _allPatients;

        #region Filtry
        private DateTime? _dataUrodzeniaOd;
        public DateTime? DataUrodzeniaOd
        {
            get => _dataUrodzeniaOd;
            set
            {
                _dataUrodzeniaOd = value;
                OnPropertyChanged(() => DataUrodzeniaOd);
                // Jeśli chcesz również auto-filtrowanie dla daty, odkomentuj:
                // Filter();
            }
        }

        private DateTime? _dataUrodzeniaDo;
        public DateTime? DataUrodzeniaDo
        {
            get => _dataUrodzeniaDo;
            set
            {
                _dataUrodzeniaDo = value;
                OnPropertyChanged(() => DataUrodzeniaDo);
                // Jeśli chcesz również auto-filtrowanie, odkomentuj:
                // Filter();
            }
        }

        // TylkoPelnoletni => natychmiastowe filtrowanie w seterze
        private bool _tylkoPelnoletni;
        public bool TylkoPelnoletni
        {
            get => _tylkoPelnoletni;
            set
            {
                _tylkoPelnoletni = value;
                OnPropertyChanged(() => TylkoPelnoletni);
                // Natychmiast wywołujemy Filter()
                Filter();
            }
        }
        #endregion

        #region Statystyki
        private int _countOfPatients;
        public int CountOfPatients
        {
            get => _countOfPatients;
            set
            {
                _countOfPatients = value;
                OnPropertyChanged(() => CountOfPatients);
            }
        }

        private int _countOfAdults;
        public int CountOfAdults
        {
            get => _countOfAdults;
            set
            {
                _countOfAdults = value;
                OnPropertyChanged(() => CountOfAdults);
            }
        }

        private double _avgAge;
        public double AvgAge
        {
            get => _avgAge;
            set
            {
                _avgAge = value;
                OnPropertyChanged(() => AvgAge);
            }
        }
        #endregion

        #region Wykres (grupy wiekowe)
        private SeriesCollection _seriesCollection;
        public SeriesCollection SeriesCollection
        {
            get => _seriesCollection;
            set
            {
                _seriesCollection = value;
                OnPropertyChanged(() => SeriesCollection);
            }
        }
        #endregion

        #region Komendy
        private ICommand _filterCommand;
        public ICommand FilterCommand
        {
            get => _filterCommand;
            set
            {
                _filterCommand = value;
                OnPropertyChanged(() => FilterCommand);
            }
        }

        private ICommand _exportCsvCommand;
        public ICommand ExportCsvCommand
        {
            get => _exportCsvCommand;
            set
            {
                _exportCsvCommand = value;
                OnPropertyChanged(() => ExportCsvCommand);
            }
        }
        #endregion

        public AllPatientsViewModel()
            : base("Wszyscy pacjenci")
        {
            FilterCommand = new BaseCommand(() => Filter());
            ExportCsvCommand = new BaseCommand(() => ExportCsv());
            SeriesCollection = new SeriesCollection();
        }

        #region Sort & Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Imię", "Nazwisko", "PESEL" };
        }

        public override void Sort()
        {
            if (SortField == "Imię")
            {
                List = new ObservableCollection<PatientForAllView>(
                    List.OrderBy(item => item.Imię).ToList()
                );
            }
            else if (SortField == "Nazwisko")
            {
                List = new ObservableCollection<PatientForAllView>(
                    List.OrderBy(item => item.Nazwisko).ToList()
                );
            }
            else if (SortField == "PESEL")
            {
                List = new ObservableCollection<PatientForAllView>(
                    List.OrderBy(item => item.PESEL).ToList()
                );
            }
            UpdateStatistics();
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Imię", "Nazwisko" };
        }

        public override void Find()
        {
            Load();

            if (FindField == "Imię")
            {
                List = new ObservableCollection<PatientForAllView>(
                    List.Where(item => item.Imię != null &&
                        item.Imię.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Nazwisko")
            {
                List = new ObservableCollection<PatientForAllView>(
                    List.Where(item => item.Nazwisko != null &&
                        item.Nazwisko.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            UpdateStatistics();
        }
        #endregion

        #region Load & Filter
        public override void Load()
        {
            _allPatients = (
                from patient in aptekaEntities.Pacjenci
                select new PatientForAllView
                {
                    ID_Pacjenta = patient.ID_Pacjenta,
                    Imię = patient.Imię,
                    Nazwisko = patient.Nazwisko,
                    Data_Urodzenia = patient.Data_Urodzenia,
                    PESEL = patient.PESEL
                }
            ).ToList();

            List = new ObservableCollection<PatientForAllView>(_allPatients);
            UpdateStatistics();
        }

        private void Filter()
        {
            if (_allPatients == null)
                return;

            var filtered = _allPatients.AsEnumerable();

            // data od/do
            if (DataUrodzeniaOd.HasValue)
                filtered = filtered.Where(p => p.Data_Urodzenia >= DataUrodzeniaOd.Value);

            if (DataUrodzeniaDo.HasValue)
                filtered = filtered.Where(p => p.Data_Urodzenia <= DataUrodzeniaDo.Value);

            // checkbox: TylkoPelnoletni
            if (TylkoPelnoletni)
                filtered = filtered.Where(p => p.IsAdult);

            List = new ObservableCollection<PatientForAllView>(filtered.ToList());
            UpdateStatistics();
        }
        #endregion

        #region UpdateStatistics
        private void UpdateStatistics()
        {
            if (List == null || List.Count == 0)
            {
                CountOfPatients = 0;
                CountOfAdults = 0;
                AvgAge = 0;
                BuildChartDataAgeGroups(new Dictionary<string, int>());
                return;
            }

            CountOfPatients = List.Count;
            CountOfAdults = List.Count(p => p.IsAdult);

            AvgAge = Math.Round(List.Average(p => (double)CalculateAge(p.Data_Urodzenia)), 0);

            // Budujemy mapę przedziałów wiekowych
            var ageGroupsDict = new Dictionary<string, int>
            {
                {"0-12",0},
                {"13-17",0},
                {"18-25",0},
                {"26-39",0},
                {"40-59",0},
                {"60+",0}
            };

            foreach (var pat in List)
            {
                int age = CalculateAge(pat.Data_Urodzenia);
                var groupName = GetAgeGroup(age);
                ageGroupsDict[groupName]++;
            }

            BuildChartDataAgeGroups(ageGroupsDict);
        }

        private int CalculateAge(DateTime birthDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age))
                age--;
            return age;
        }

        private string GetAgeGroup(int age)
        {
            // Przedziały wiekowe
            if (age <= 12) return "0-12";
            else if (age <= 17) return "13-17";
            else if (age <= 25) return "18-25";
            else if (age <= 39) return "26-39";
            else if (age <= 59) return "40-59";
            else return "60+";
        }

        private void BuildChartDataAgeGroups(Dictionary<string, int> ageGroups)
        {
            var newSeries = new SeriesCollection();
            foreach (var kvp in ageGroups)
            {
                if (kvp.Value > 0)
                {
                    newSeries.Add(
                        new PieSeries
                        {
                            Title = kvp.Key,
                            Values = new LiveCharts.ChartValues<int> { kvp.Value },
                            DataLabels = true
                        }
                    );
                }
            }
            SeriesCollection = newSeries;
        }
        #endregion

        #region ExportCsv
        private void ExportCsv()
        {
            try
            {
                string csvPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Patients.csv"
                );

                using (var writer = new StreamWriter(csvPath))
                {
                    writer.WriteLine("ID_Pacjenta;Imię;Nazwisko;Data_Urodzenia;PESEL;Pełnoletni?");

                    foreach (var pat in List)
                    {
                        writer.WriteLine($"{pat.ID_Pacjenta};" +
                                         $"{pat.Imię};" +
                                         $"{pat.Nazwisko};" +
                                         $"{pat.Data_Urodzenia:d};" +
                                         $"{pat.PESEL};" +
                                         (pat.IsAdult ? "TAK" : "NIE"));
                    }
                }

                ShowMessageBox($"Zapisano plik CSV: {csvPath}");
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd przy eksporcie CSV: {ex.Message}");
            }
        }
        #endregion
    }
}
