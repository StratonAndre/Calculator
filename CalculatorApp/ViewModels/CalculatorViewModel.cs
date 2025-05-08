using CalculatorApp.Models;
using CalculatorApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace CalculatorApp.ViewModels
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private CalculatorModel _calculator = new CalculatorModel();
        private string _displayText = "0";
        private string _equationText = "";
        private string _hexValue = "0";
        private string _decValue = "0";
        private string _octValue = "0";
        private string _binValue = "0";
        private bool _isDigitGroupingEnabled;
        private bool _isPrecedenceEnabled;
        private bool _isStandardMode;
        private bool _isProgrammerMode;
        private string _errorText;
        private bool _hasError;

        public event PropertyChangedEventHandler PropertyChanged;

        public CalculatorViewModel()
        {
            _isDigitGroupingEnabled = Settings.DigitGroupingEnabled;
            _isPrecedenceEnabled = Settings.PrecedenceEnabled;
            _isStandardMode = Settings.CalculatorMode == "Standard";
            _isProgrammerMode = Settings.CalculatorMode == "Programmer";
        }

        public string DisplayText
        {
            get => _displayText;
            set
            {
                if (_displayText != value)
                {
                    _displayText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string EquationText
        {
            get => _equationText;
            set
            {
                if (_equationText != value)
                {
                    _equationText = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ErrorText
        {
            get => _errorText;
            set
            {
                if (_errorText != value)
                {
                    _errorText = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool HasError
        {
            get => _hasError;
            set
            {
                if (_hasError != value)
                {
                    _hasError = value;
                    OnPropertyChanged();
                }
            }
        }

        public string HexValue
        {
            get => _hexValue;
            set
            {
                if (_hexValue != value)
                {
                    _hexValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DecValue
        {
            get => _decValue;
            set
            {
                if (_decValue != value)
                {
                    _decValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string OctValue
        {
            get => _octValue;
            set
            {
                if (_octValue != value)
                {
                    _octValue = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BinValue
        {
            get => _binValue;
            set
            {
                if (_binValue != value)
                {
                    _binValue = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public bool IsDigitGroupingEnabled
        {
            get => _isDigitGroupingEnabled;
            set
            {
                if (_isDigitGroupingEnabled != value)
                {
                    _isDigitGroupingEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsPrecedenceEnabled
        {
            get => _isPrecedenceEnabled;
            set
            {
                if (_isPrecedenceEnabled != value)
                {
                    _isPrecedenceEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStandardMode
        {
            get => _isStandardMode;
            set
            {
                if (_isStandardMode != value)
                {
                    _isStandardMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsProgrammerMode
        {
            get => _isProgrammerMode;
            set
            {
                if (_isProgrammerMode != value)
                {
                    _isProgrammerMode = value;
                    OnPropertyChanged();
                }
            }
        }

        public void AddDigit(string digit)
        {
            try
            {
                _calculator.AddDigit(digit);
                UpdateDisplay();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Error adding digit: {ex.Message}");
            }
        }

        public void AddDecimalPoint()
        {
            try
            {
                _calculator.AddDecimalPoint();
                UpdateDisplay();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Error adding decimal point: {ex.Message}");
            }
        }

        public void SetOperator(string op)
        {
            try
            {
                CalculatorModel.Operator calcOp = CalculatorModel.Operator.None;
                
                switch (op)
                {
                    case "+": calcOp = CalculatorModel.Operator.Add; break;
                    case "-": calcOp = CalculatorModel.Operator.Subtract; break;
                    case "×": calcOp = CalculatorModel.Operator.Multiply; break;
                    case "÷": calcOp = CalculatorModel.Operator.Divide; break;
                    case "mod": calcOp = CalculatorModel.Operator.Modulo; break;
                }
                
                _calculator.SetOperator(calcOp);
                UpdateDisplay();
          
                EquationText = _calculator.Expression;
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Operation error: {ex.Message}");
            }
        }

        public void PerformUnaryOperation(string operation)
        {
            try
            {
                _calculator.PerformUnaryOperation(operation);
                UpdateDisplay();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Operation error: {ex.Message}");
            }
        }

        public void Calculate()
        {
            try
            {
                _calculator.Calculate();
                UpdateDisplay();
                EquationText = "";
                if (_calculator.Error != null)
                {
                    SetError(_calculator.Error);
                }
                else
                {
                    ClearError();
                }
            }
            catch (Exception ex)
            {
                SetError($"Calculation error: {ex.Message}");
            }
        }

        public void Clear()
        {
            try
            {
                _calculator.Reset();
                UpdateDisplay();
                EquationText = "";
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Error clearing calculator: {ex.Message}");
            }
        }

        public void ClearEntry()
        {
            try
            {
                _calculator.ClearEntry();
                UpdateDisplay();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Error clearing entry: {ex.Message}");
            }
        }

        public void Backspace()
        {
            try
            {
                _calculator.Backspace();
                UpdateDisplay();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Error removing last digit: {ex.Message}");
            }
        }

        public void MemoryStore()
        {
            try
            {
                _calculator.MemoryStore();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Memory error: {ex.Message}");
            }
        }

        public void MemoryRecall()
        {
            try
            {
                _calculator.MemoryRecall();
                UpdateDisplay();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Memory error: {ex.Message}");
            }
        }

        public void MemoryClear()
        {
            try
            {
                _calculator.MemoryClear();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Memory error: {ex.Message}");
            }
        }

        public void MemoryAdd()
        {
            try
            {
                _calculator.MemoryAdd();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Memory error: {ex.Message}");
            }
        }

        public void MemorySubtract()
        {
            try
            {
                _calculator.MemorySubtract();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Memory error: {ex.Message}");
            }
        }

        public List<string> GetMemoryList()
        {
            try
            {
                var memoryValues = _calculator.GetMemoryList();
                var formattedValues = new List<string>();
                
                foreach (var value in memoryValues)
                {
                    formattedValues.Add(CalculationService.FormatNumber(value, Settings.DigitGroupingEnabled));
                }
                
                return formattedValues;
            }
            catch (Exception ex)
            {
                SetError($"Memory error: {ex.Message}");
                return new List<string>();
            }
        }

        public void SwitchToStandardMode()
        {
            try
            {
                Settings.CalculatorMode = "Standard";
                IsStandardMode = true;
                IsProgrammerMode = false;
                
               
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null)
                {
                
                    mainWindow.Width = 450; 
                    mainWindow.Height = 800;
                }
                
                Settings.Save();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Error switching mode: {ex.Message}");
            }
        }

        public void SwitchToProgrammerMode()
        {
            try
            {
                Settings.CalculatorMode = "Programmer";
                IsStandardMode = false;
                IsProgrammerMode = true;
                UpdateBaseDisplays();
                
             
                var mainWindow = Application.Current.MainWindow;
                if (mainWindow != null)
                {
                   
                    mainWindow.Width = 450;  
                    mainWindow.Height = 800; 
                }
                
                Settings.Save();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Error switching mode: {ex.Message}");
            }
        }

        public void SetNumberBase(string baseType)
        {
            try
            {
                Settings.NumberBase = baseType;
                Settings.Save();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Error setting base: {ex.Message}");
            }
        }

        public void ToggleDigitGrouping()
        {
            try
            {
                Settings.DigitGroupingEnabled = !Settings.DigitGroupingEnabled;
                IsDigitGroupingEnabled = Settings.DigitGroupingEnabled;
                UpdateDisplay();
                Settings.Save();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Settings error: {ex.Message}");
            }
        }

        public void TogglePrecedence()
        {
            try
            {
                Settings.PrecedenceEnabled = !Settings.PrecedenceEnabled;
                IsPrecedenceEnabled = Settings.PrecedenceEnabled;
                
             
                _calculator.Reset();
                UpdateDisplay();
                EquationText = "";
                
                Settings.Save();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Settings error: {ex.Message}");
            }
        }

        public void ChangeTheme(string themeName)
        {
            try
            {
                ThemeManager.ApplyTheme(themeName);
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Theme error: {ex.Message}");
            }
        }

        public void CopyToClipboard()
        {
            try
            {
                ClipboardService.CopyToClipboard(DisplayText);
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Clipboard error: {ex.Message}");
            }
        }

        public void CutToClipboard()
        {
            try
            {
                ClipboardService.CopyToClipboard(DisplayText);
                _calculator.ClearEntry();
                UpdateDisplay();
                ClearError();
            }
            catch (Exception ex)
            {
                SetError($"Clipboard error: {ex.Message}");
            }
        }

        public void PasteFromClipboard()
        {
            try
            {
                string clipboardText = ClipboardService.GetFromClipboard();
                
                if (!string.IsNullOrEmpty(clipboardText))
                {
                    double value;
                    if (double.TryParse(clipboardText.Replace(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator, ""), 
                                       out value))
                    {
                        _calculator.Reset();
                        _calculator.AddDigit(value.ToString());
                        UpdateDisplay();
                        ClearError();
                    }
                    else
                    {
                        SetError("Invalid number format in clipboard");
                    }
                }
            }
            catch (Exception ex)
            {
                SetError($"Clipboard error: {ex.Message}");
            }
        }

        private void UpdateDisplay()
        {
            DisplayText = CalculationService.FormatNumber(_calculator.CurrentValue, Settings.DigitGroupingEnabled);
            
            if (IsProgrammerMode)
            {
                UpdateBaseDisplays();
            }
        }

        private void UpdateBaseDisplays()
        {
            try
            {
                long value = (long)_calculator.CurrentValue;

                HexValue = NumberBaseConverter.ToBase(value, 16);
                DecValue = value.ToString();
                OctValue = NumberBaseConverter.ToBase(value, 8);
                BinValue = NumberBaseConverter.ToBase(value, 2);
            }
            catch
            {
                HexValue = "Error";
                DecValue = "Error";
                OctValue = "Error";
                BinValue = "Error";
            }
        }

        private void SetError(string errorMessage)
        {
            HasError = true;
            ErrorText = errorMessage;
        }

        private void ClearError()
        {
            HasError = false;
            ErrorText = null;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}