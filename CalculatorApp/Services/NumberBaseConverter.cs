using System;

namespace CalculatorApp.Services
{
    public static class NumberBaseConverter
    {
        public static string ToBase(long decimalValue, int targetBase)
        {
            if (targetBase < 2 || targetBase > 16)
            {
                throw new ArgumentException("Base must be between 2 and 16", nameof(targetBase));
            }

            if (decimalValue == 0)
            {
                return "0";
            }

            const string digits = "0123456789ABCDEF";
            
            bool isNegative = decimalValue < 0;
            if (isNegative)
            {
                decimalValue = -decimalValue;
            }

            string result = "";
            
            while (decimalValue > 0)
            {
                result = digits[(int)(decimalValue % targetBase)] + result;
                decimalValue /= targetBase;
            }

            return isNegative ? "-" + result : result;
        }

        public static long FromBase(string value, int sourceBase)
        {
            if (sourceBase < 2 || sourceBase > 16)
            {
                throw new ArgumentException("Base must be between 2 and 16", nameof(sourceBase));
            }

            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }

            bool isNegative = value.StartsWith("-");
            if (isNegative)
            {
                value = value.Substring(1);
            }

            long result = 0;
            foreach (char c in value.ToUpper())
            {
                int digit;
                if (char.IsDigit(c))
                {
                    digit = c - '0';
                }
                else if (c >= 'A' && c <= 'F')
                {
                    digit = c - 'A' + 10;
                }
                else
                {
                    throw new ArgumentException($"Invalid character '{c}' for the specified base", nameof(value));
                }

                if (digit >= sourceBase)
                {
                    throw new ArgumentException($"Character '{c}' is outside the range for base {sourceBase}", nameof(value));
                }

                result = result * sourceBase + digit;
            }

            return isNegative ? -result : result;
        }

        public static char GetMaxValidDigit(int baseValue)
        {
            if (baseValue <= 0 || baseValue > 16)
                throw new ArgumentOutOfRangeException(nameof(baseValue), "Base must be between 1 and 16");
                
            if (baseValue <= 10)
                return (char)('0' + (baseValue - 1));
            else
                return (char)('A' + (baseValue - 11));
        }

        public static bool IsValidDigit(char digit, int baseValue)
        {
            if (char.IsDigit(digit))
            {
                return (digit - '0') < baseValue;
            }
            else if (char.ToUpper(digit) >= 'A' && char.ToUpper(digit) <= 'F')
            {
                return (char.ToUpper(digit) - 'A' + 10) < baseValue;
            }
            
            return false;
        }
    }
}