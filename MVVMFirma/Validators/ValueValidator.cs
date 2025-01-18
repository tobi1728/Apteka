using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Validators
{
    public static class ValueValidator
    {
        public static string ValidateValue(decimal? value)
        {
            if (!value.HasValue)
            {
                return "Pole wyamgane";
            }

            if (value < 0)
            {
                return "Liczba powinna byc wieksza od 0";
            }
            return string.Empty;

        }

        public static string ValidateString(string value, int minLength = 1)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Pole nie może być puste.";
            if (value.Length < minLength)
                return $"Pole musi zawierać co najmniej {minLength} znaków.";
            return string.Empty;
        }

        public static string ValidatePhoneNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Numer telefonu nie może być pusty.";
            if (!value.All(char.IsDigit))
                return "Numer telefonu może zawierać tylko cyfry.";
            if (value.Length != 9)
                return "Numer telefonu musi mieć 9 cyfr.";
            return string.Empty;
        }

        public static string ValidatePostalCode(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Kod pocztowy nie może być pusty.";
            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d{2}-\d{3}$"))
                return "Kod pocztowy musi mieć format XX-XXX.";
            return string.Empty;
        }

        public static string ValidatePositiveDecimal(decimal value)
        {
            return value > 0 ? string.Empty : "Kwota musi być liczbą dodatnią.";
        }

        public static string ValidateDate(DateTime? date)
        {
            if (date == null)
                return "Data nie może być pusta.";
            if (date > DateTime.Now)
                return "Data nie może być przyszła.";
            return string.Empty;
        }
        public static string ValidateLicenseNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "Numer licencji nie może być pusty.";
            if (!System.Text.RegularExpressions.Regex.IsMatch(value, @"^\d+$"))
                return "Numer licencji może zawierać tylko cyfry.";
            return string.Empty;
        }
        public static string ValidateTime(TimeSpan? time)
        {
            if (time == null)
                return "Godzina nie może być pusta.";
            return string.Empty; // Możesz dodać inne warunki, jeśli konieczne.
        }
        public static string ValidateEndTime(TimeSpan? startTime, TimeSpan? endTime)
        {
            if (endTime == null)
                return "Godzina zakończenia nie może być pusta.";
            if (startTime != null && endTime <= startTime)
                return "Godzina zakończenia musi być późniejsza niż godzina rozpoczęcia.";
            return string.Empty;
        }
        public static string ValidatePharmacist(int pharmacistId)
        {
            return pharmacistId > 0 ? string.Empty : "Musisz wybrać farmaceutę.";
        }
        public static string ValidateFutureDate(DateTime date)
        {
            if (date < DateTime.Now.Date)
                return "Data nie może być w przeszłości.";
            return string.Empty;
        }

        public static string ValidateSelection(int id)
        {
            if (id <= 0)
                return "Musisz dokonać wyboru.";
            return string.Empty;
        }
        public static string ValidateOptionalString(string value, int maxLength)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
                return $"Pole nie może przekraczać {maxLength} znaków.";
            return string.Empty;
        }
        public static string ValidatePositiveInteger(int value)
        {
            if (value <= 0)
                return "Wartość musi być większa od 0.";
            return string.Empty;
        }

        public static string ValidatePESEL(string pesel)
        {
            if (string.IsNullOrWhiteSpace(pesel))
                return "PESEL nie może być pusty.";
            if (!System.Text.RegularExpressions.Regex.IsMatch(pesel, @"^\d{11}$"))
                return "PESEL musi składać się z dokładnie 11 cyfr.";
            return string.Empty;
        }
        public static string ValidatePastDate(DateTime date)
        {
            if (date > DateTime.Today)
                return "Data urodzenia nie może być w przyszłości.";
            return string.Empty;
        }
        public static string ValidatePastOrTodayDate(DateTime date)
        {
            if (date > DateTime.Today)
                return "Data wystawienia nie może być w przyszłości.";
            return string.Empty;
        }

        public static string ValidateEndAfterStartDate(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                return "Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.";
            return string.Empty;
        }
        public static string ValidateNonNegativeInteger(int value)
        {
            if (value < 0)
                return "Wartość nie może być ujemna.";
            return string.Empty;
        }

        public static string ValidateEndAfterStartDate(DateTime startDate, DateTime? endDate)
        {
            if (endDate.HasValue && endDate.Value < startDate)
                return "Data realizacji nie może być wcześniejsza niż data wystawienia.";
            return string.Empty;
        }
        public static string ValidateOptionalSelection(int? id)
        {
            if (id.HasValue && id <= 0)
                return "Wybrano niepoprawną wartość.";
            return string.Empty;
        }


    }
}
