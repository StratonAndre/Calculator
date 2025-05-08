using System;
using System.Windows;
using CalculatorApp.Models;

namespace CalculatorApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                Exception ex = (Exception)args.ExceptionObject;
                MessageBox.Show($"An unhandled exception occurred: {ex.Message}\n\n{ex.StackTrace}", 
                    "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };
            
            try
            {
              
                Settings.Load();
                
               
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Loaded, new System.Action(() => {
                    ThemeManager.ApplyTheme(Settings.CurrentTheme);
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during application startup: {ex.Message}", "Startup Error", 
                    MessageBoxButton.OK, MessageBoxImage.Error);
                Settings.ResetToDefaults();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                Settings.Save();
            }
            catch
            {
               
            }
            
            base.OnExit(e);
        }
    }
}