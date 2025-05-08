using System;
using System.Windows;

namespace CalculatorApp.Services
{
    public static class ClipboardService
    {
        public static void CopyToClipboard(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                try
                {
                    Clipboard.SetText(text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Unable to copy to clipboard: {ex.Message}", 
                        "Clipboard Error", 
                        MessageBoxButton.OK, 
                        MessageBoxImage.Error);
                }
            }
        }

        public static string GetFromClipboard()
        {
            try
            {
                if (Clipboard.ContainsText())
                {
                    return Clipboard.GetText();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to paste from clipboard: {ex.Message}", 
                    "Clipboard Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
            return string.Empty;
        }
    }
}