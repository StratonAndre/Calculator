using System;
using System.Globalization;

namespace CalculatorApp.Services
{
    public static class CalculationService
    {
        public static string FormatNumber(double value, bool useDigitGrouping)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return "Error";
            }

            string format = useDigitGrouping ? "N" : "G";
            
         
            if (Math.Floor(value) == value)
            {
                format = useDigitGrouping ? "N0" : "G0";
            }

            return value.ToString(format, CultureInfo.CurrentCulture);
        }

        public static double ParseFormattedNumber(string formattedNumber)
        {
            if (string.IsNullOrEmpty(formattedNumber) || formattedNumber == "Error")
            {
                return 0;
            }

          
            string cleanNumber = formattedNumber.Replace(
                CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, "");

            if (double.TryParse(cleanNumber, NumberStyles.Any, CultureInfo.CurrentCulture, out double result))
            {
                return result;
            }

            return 0;
        }
    }
}