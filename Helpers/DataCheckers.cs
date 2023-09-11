using System;
using System.Text.RegularExpressions;
using System.Windows;

namespace DDDK_Wpf.Helpers
{
    internal static class DataCheckers
    {
        public static bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, "^[0-9+-/]+$");
        }

        public static bool IsEmail(string text)
        {
            return Regex.IsMatch(text, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        }

        public static bool IsDate(string text)
        {
            return DateTime.TryParseExact(text, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out _);
        }

        public static bool IsPassword(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$");
        }

        public static bool IsPositiveNumber(string number)
        {
            int x;
            if (int.TryParse(number, out x) && x >= 0) 
                return true;
            return false;
        }
    }
}
