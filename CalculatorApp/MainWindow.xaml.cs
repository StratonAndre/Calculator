using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media; 
using CalculatorApp.Models;
using CalculatorApp.ViewModels;
using CalculatorApp.Views;
using CalculatorApp.Services; 
using System.Collections.Generic;

namespace CalculatorApp
{
    public partial class MainWindow : Window
    {
        private CalculatorViewModel ViewModel => (CalculatorViewModel)DataContext;
        private List<Button> hexButtons = new List<Button>();
        private List<Button> decButtons = new List<Button>();
        private List<Button> octButtons = new List<Button>();
        private List<Button> binButtons = new List<Button>();

        public MainWindow()
        {
            InitializeComponent();
            
            // Create BooleanToVisibilityConverter for binding
            Resources.Add("BooleanToVisibilityConverter", new BooleanToVisibilityConverter());
            
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Group digit buttons for different bases
            // Hex buttons include A-F and 0-9
            hexButtons.AddRange(new[] { ButtonA, ButtonB, ButtonC, ButtonD, ButtonE, ButtonF,
                                        Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9 });
            
            // Dec buttons include 0-9
            decButtons.AddRange(new[] { Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7, Button8, Button9 });
            
            // Oct buttons include 0-7
            octButtons.AddRange(new[] { Button0, Button1, Button2, Button3, Button4, Button5, Button6, Button7 });
            
            // Bin buttons include 0-1
            binButtons.AddRange(new[] { Button0, Button1 });
            
            // Update UI for current mode
            UpdateUIForCurrentMode();

            // Set the correct base radio button
            string baseType = Settings.NumberBase;
            switch (baseType)
            {
                case "HEX":
                    hexRadioButton.IsChecked = true;
                    break;
                case "DEC":
                    decRadioButton.IsChecked = true;
                    break;
                case "OCT":
                    octRadioButton.IsChecked = true;
                    break;
                case "BIN":
                    binRadioButton.IsChecked = true;
                    break;
                default:
                    decRadioButton.IsChecked = true;
                    break;
            }
            
            // Update button states for the selected base
            UpdateButtonStatesForBase(Settings.NumberBase);
        }

        private void UpdateUIForCurrentMode()
        {
            if (ViewModel.IsStandardMode)
            {
                calculatorModeLabel.Content = "Standard";
                standardButtonsPanel.Visibility = Visibility.Visible;
                baseDisplayPanel.Visibility = Visibility.Collapsed;
                baseSelectionPanel.Visibility = Visibility.Collapsed;
            }
            else if (ViewModel.IsProgrammerMode)
            {
                calculatorModeLabel.Content = "Programmer";
                standardButtonsPanel.Visibility = Visibility.Collapsed;
                baseDisplayPanel.Visibility = Visibility.Visible;
                baseSelectionPanel.Visibility = Visibility.Visible;
                
                // Update button states for the selected base
                UpdateButtonStatesForBase(Settings.NumberBase);
            }
        }

        private void NumberButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string digit = button.Content.ToString();
            ViewModel.AddDigit(digit);
        }

        private void OperatorButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string op = button.Content.ToString();
            ViewModel.SetOperator(op);
        }

        private void UnaryOperationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string operation = button.Tag?.ToString() ?? "";
            ViewModel.PerformUnaryOperation(operation);
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Calculate();
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Clear();
        }

        private void ClearEntryButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearEntry();
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Backspace();
        }

        private void DecimalButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.AddDecimalPoint();
        }

        private void MemoryStoreButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MemoryStore();
        }

        private void MemoryRecallButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MemoryRecall();
        }

        private void MemoryClearButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MemoryClear();
        }

        private void MemoryAddButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MemoryAdd();
        }

        private void MemorySubtractButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.MemorySubtract();
        }

        private void MemoryListButton_Click(object sender, RoutedEventArgs e)
        {
            var memoryValues = ViewModel.GetMemoryList();
            if (memoryValues.Count > 0)
            {
                memoryListBox.ItemsSource = memoryValues;
                memoryListPopup.PlacementTarget = (UIElement)sender;
                memoryListPopup.IsOpen = true;
            }
        }

        private void MemoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (memoryListBox.SelectedItem != null)
            {
                memoryListPopup.IsOpen = false;
                ViewModel.MemoryRecall();
            }
        }

        private void StandardModeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SwitchToStandardMode();
            UpdateUIForCurrentMode();
        }

        private void ProgrammerModeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SwitchToProgrammerMode();
            UpdateUIForCurrentMode();
        }

        private void BaseRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Tag != null)
            {
                string baseType = radioButton.Tag.ToString();
                SetBaseMode(baseType);
                UpdateButtonStatesForBase(baseType);
            }
        }

        private void SetBaseMode(string baseType)
        {
            if (DataContext == null)
                return;
                
            ViewModel.SetNumberBase(baseType);
        }

        private void UpdateButtonStatesForBase(string baseType)
        {
            // Disable all digit buttons first
            foreach (Button button in hexButtons)
            {
                if (button != null)
                    button.IsEnabled = false;
            }
            
            // Enable appropriate buttons based on the selected base
            List<Button> enabledButtons = null;
            
            switch (baseType)
            {
                case "HEX":
                    enabledButtons = hexButtons;
                    break;
                case "DEC":
                    enabledButtons = decButtons;
                    break;
                case "OCT":
                    enabledButtons = octButtons;
                    break;
                case "BIN":
                    enabledButtons = binButtons;
                    break;
                default:
                    enabledButtons = decButtons;
                    break;
            }
            
            // Enable the appropriate buttons
            foreach (Button button in enabledButtons)
            {
                if (button != null)
                    button.IsEnabled = true;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 && !e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                int digit = e.Key - Key.D0;
                if (IsDigitValid(digit))
                {
                    ViewModel.AddDigit(digit.ToString());
                    e.Handled = true;
                }
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                int digit = e.Key - Key.NumPad0;
                if (IsDigitValid(digit))
                {
                    ViewModel.AddDigit(digit.ToString());
                    e.Handled = true;
                }
            }
            else if ((e.Key >= Key.A && e.Key <= Key.F) && ViewModel.IsProgrammerMode && Settings.NumberBase == "HEX")
            {
                char letter = (char)('A' + (e.Key - Key.A));
                ViewModel.AddDigit(letter.ToString());
                e.Handled = true;
            }
            else
            {
                switch (e.Key)
                {
                    case Key.Add:
                    case Key.OemPlus when e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift):
                        ViewModel.SetOperator("+");
                        e.Handled = true;
                        break;
                    case Key.Subtract:
                    case Key.OemMinus:
                        ViewModel.SetOperator("-");
                        e.Handled = true;
                        break;
                    case Key.Multiply:
                    case Key.OemQuestion when e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift) && e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift):
                        ViewModel.SetOperator("×");
                        e.Handled = true;
                        break;
                    case Key.Divide:
                    case Key.OemQuestion:
                        ViewModel.SetOperator("÷");
                        e.Handled = true;
                        break;
                    case Key.Enter:
                        ViewModel.Calculate();
                        e.Handled = true;
                        break;
                    case Key.Escape:
                        ViewModel.Clear();
                        e.Handled = true;
                        break;
                    case Key.Back:
                        ViewModel.Backspace();
                        e.Handled = true;
                        break;
                    case Key.Decimal:
                    case Key.OemPeriod:
                        if (ViewModel.IsStandardMode)
                        {
                            ViewModel.AddDecimalPoint();
                            e.Handled = true;
                        }
                        break;
                }
            }

            // Handle keyboard shortcuts for clipboard operations
            if (e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Control))
            {
                switch (e.Key)
                {
                    case Key.C:
                        ViewModel.CopyToClipboard();
                        e.Handled = true;
                        break;
                    case Key.V:
                        ViewModel.PasteFromClipboard();
                        e.Handled = true;
                        break;
                    case Key.X:
                        ViewModel.CutToClipboard();
                        e.Handled = true;
                        break;
                }
            }
        }

        private bool IsDigitValid(int digit)
        {
            if (ViewModel.IsProgrammerMode)
            {
                string baseType = Settings.NumberBase;
                switch (baseType)
                {
                    case "BIN":
                        return digit <= 1;
                    case "OCT":
                        return digit <= 7;
                    case "DEC":
                        return digit <= 9;
                    case "HEX":
                        return true;
                    default:
                        return true;
                }
            }
            return true;
        }

        private void CopyMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CopyToClipboard();
        }

        private void CutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.CutToClipboard();
        }

        private void PasteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.PasteFromClipboard();
        }

        private void DigitGroupingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ToggleDigitGrouping();
        }

        private void PrecedenceMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.TogglePrecedence();
        }

        private void ThemeMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is string themeName)
            {
                ViewModel.ChangeTheme(themeName);
            }
        }

        private void AboutMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try {
                AboutWindow aboutWindow = new AboutWindow();
                aboutWindow.Owner = this;
                aboutWindow.ShowDialog();
            }
            catch (Exception ex) {
                MessageBox.Show($"Error showing About window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

    public static class VisualTreeExtensions
    {
        public static IEnumerable<T> FindVisualChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent != null)
            {
                int childCount = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < childCount; i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                    if (child is T t)
                    {
                        yield return t;
                    }

                    foreach (T descendant in FindVisualChildren<T>(child))
                    {
                        yield return descendant;
                    }
                }
            }
        }
    }
}