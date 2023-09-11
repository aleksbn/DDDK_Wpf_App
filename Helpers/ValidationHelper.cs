using System;
using System.Windows;
using System.Windows.Controls;

namespace DDDK_Wpf.Helpers
{
    public enum ValidationType
    {
        Text,
        Date,
        Password,
        Email,
        PhoneNumber,
        PositiveNumber,
        StringEquality
    }

    internal static class ValidationHelper
    {
        public static bool ValidateElement(TextBox element, string message, ValidationType type, string forComparison = "")
        {
            bool result = true;
            switch (type)
            {
                case ValidationType.Text:
                    result = string.IsNullOrWhiteSpace(element.Text);
                    break;
                case ValidationType.Date:
                    result = !DataCheckers.IsDate(element.Text);
                    break;
                case ValidationType.Email:
                    result = !DataCheckers.IsEmail(element.Text);
                    break;
                case ValidationType.PhoneNumber:
                    result = !DataCheckers.IsPhoneNumber(element.Text);
                    break;
                case ValidationType.PositiveNumber:
                    result = !DataCheckers.IsPositiveNumber(element.Text);
                    break;
                case ValidationType.StringEquality:
                    result = !element.Text.Equals(forComparison);
                    break;
                default: MessageBox.Show("There is an error in calling Validation Helper.");
                    break;
            }
            if (result)
            {
                MessageBox.Show(message, "Error in validation");
                element.BorderThickness = new Thickness(7);
                element.Focus();
                element.TextChanged += TextBox_edited;
            }
            return result;
        }

        public static bool ValidateElement(PasswordBox element, string message, ValidationType type, string forComparison = "")
        {
            bool result = true;
            switch (type)
            {
                case ValidationType.Password:
                    result = !DataCheckers.IsPassword(element.Password);
                    break;
                case ValidationType.StringEquality:
                    result = !element.Password.Equals(forComparison);
                    break;
                default:
                    MessageBox.Show("There is an error in calling Validation Helper.");
                    break;
            }
            if (result)
            {
                MessageBox.Show(message, "Error in validation");
                element.BorderThickness = new Thickness(7);
                element.Focus();
                element.PasswordChanged += TextBox_edited;
            }
            return result;
        }

        private static void TextBox_edited(object sender, EventArgs e)
        {
            if(sender is TextBox)
            {
                var element = (TextBox)sender;
                element.BorderThickness = new Thickness(1);
                element.TextChanged -= TextBox_edited;
            }
            else
            {
                var element = (PasswordBox)sender;
                element.BorderThickness = new Thickness(1);
                element.PasswordChanged -= TextBox_edited;
            }
        }
    }
}
