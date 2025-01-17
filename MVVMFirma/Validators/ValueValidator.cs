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


    }
}
