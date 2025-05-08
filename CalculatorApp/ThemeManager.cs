using System;
using System.IO;
using System.Windows;
using System.Windows.Data;
using System.Diagnostics;
using CalculatorApp.Models;

namespace CalculatorApp
{
    public static class ThemeManager
    {
        public static void ApplyTheme(string themeName)
        {
            try
            {
           
                Settings.CurrentTheme = themeName;
                Settings.Save();

              
                ResourceDictionary appResources = Application.Current.Resources;

            
                ResourceDictionary oldTheme = null;
                foreach (ResourceDictionary dict in appResources.MergedDictionaries)
                {
                    string uri = dict.Source?.ToString() ?? "";
                    if (uri.Contains("Theme.xaml"))
                    {
                        oldTheme = dict;
                        break;
                    }
                }

                if (oldTheme != null)
                {
                    appResources.MergedDictionaries.Remove(oldTheme);
                }

                
                ResourceDictionary newTheme = null;
                
             
                try 
                {
                    string uri = $"pack://application:,,,/Themes/{themeName}Theme.xaml";
                    newTheme = new ResourceDictionary { Source = new Uri(uri) };
                }
                catch (Exception)
                {
                    
                    try
                    {
                        string assemblyName = typeof(ThemeManager).Assembly.GetName().Name;
                        string uri = $"pack://application:,,,/{assemblyName};component/Themes/{themeName}Theme.xaml";
                        newTheme = new ResourceDictionary { Source = new Uri(uri) };
                    }
                    catch (Exception)
                    {
                      
                        newTheme = CreateFallbackTheme(themeName);
                    }
                }

           
                appResources.MergedDictionaries.Add(newTheme);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading theme: {ex.Message}", 
                    "Theme Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static ResourceDictionary CreateFallbackTheme(string themeName)
        {
      
            ResourceDictionary theme = new ResourceDictionary();
            
         
            if (themeName.Equals("Light", StringComparison.OrdinalIgnoreCase))
            {
                theme["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);
                theme["ForegroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                theme["BorderBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["DisplayBackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                theme["DisplayTextBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                theme["NumberButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                theme["NumberButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["NumberButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Silver);
                theme["OperatorButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["OperatorButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Silver);
                theme["OperatorButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGray);
                theme["EqualsButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80));
                theme["EqualsButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(67, 160, 71));
                theme["EqualsButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(56, 142, 60));
                theme["MemoryButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);
                theme["MemoryButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["MemoryButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Silver);
                theme["ClearButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 204, 128));
                theme["ClearButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 183, 77));
                theme["ClearButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 152, 0));
                theme["MenuBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);
                theme["MenuItemHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["MenuItemPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Silver);
            }
            else if (themeName.Equals("Dark", StringComparison.OrdinalIgnoreCase))
            {
                theme["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(48, 48, 48));
                theme["ForegroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                theme["BorderBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(64, 64, 64));
                theme["DisplayBackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 33, 33));
                theme["DisplayTextBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                theme["NumberButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(66, 66, 66));
theme["NumberButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(79, 79, 79));
                theme["NumberButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(96, 96, 96));
                theme["OperatorButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(80, 80, 80));
                theme["OperatorButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(96, 96, 96));
                theme["OperatorButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(112, 112, 112));
                theme["EqualsButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80));
                theme["EqualsButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(67, 160, 71));
                theme["EqualsButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(56, 142, 60));
                theme["MemoryButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(56, 56, 56));
                theme["MemoryButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(72, 72, 72));
                theme["MemoryButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(80, 80, 80));
                theme["ClearButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 109, 0));
                theme["ClearButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 143, 0));
                theme["ClearButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 152, 0));
                theme["MenuBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(33, 33, 33));
                theme["MenuItemHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(66, 66, 66));
                theme["MenuItemPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(90, 90, 90));
            }
            else if (themeName.Equals("Blue", StringComparison.OrdinalIgnoreCase))
            {
                theme["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(26, 35, 126));
                theme["ForegroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                theme["BorderBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(48, 63, 159));
                theme["DisplayBackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(13, 71, 161));
                theme["DisplayTextBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                theme["NumberButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(57, 73, 171));
                theme["NumberButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(92, 107, 192));
                theme["NumberButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(121, 134, 203));
                theme["OperatorButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(40, 53, 147));
                theme["OperatorButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(57, 73, 171));
                theme["OperatorButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(92, 107, 192));
                theme["EqualsButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 191, 165));
                theme["EqualsButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0, 229, 188));
                theme["EqualsButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(29, 233, 182));
                theme["MemoryButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 101, 192));
                theme["MemoryButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(25, 118, 210));
                theme["MemoryButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(30, 136, 229));
                theme["ClearButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 109, 0));
                theme["ClearButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 143, 0));
                theme["ClearButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 152, 0));
                theme["MenuBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(26, 35, 126));
                theme["MenuItemHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(48, 63, 159));
                theme["MenuItemPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(57, 73, 171));
            }
            else
            {
               
                theme["BackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);
                theme["ForegroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                theme["BorderBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["DisplayBackgroundBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                theme["DisplayTextBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                theme["NumberButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
                theme["NumberButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["NumberButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Silver);
                theme["OperatorButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["OperatorButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Silver);
                theme["OperatorButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DarkGray);
                theme["EqualsButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(76, 175, 80));
                theme["EqualsButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(67, 160, 71));
                theme["EqualsButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(56, 142, 60));
                theme["MemoryButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);
                theme["MemoryButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["MemoryButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Silver);
                theme["ClearButtonBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 204, 128));
                theme["ClearButtonHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 183, 77));
                theme["ClearButtonPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(255, 152, 0));
                theme["MenuBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.WhiteSmoke);
                theme["MenuItemHoverBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.LightGray);
                theme["MenuItemPressedBrush"] = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Silver);
            }

            return theme;
        }
    }
}