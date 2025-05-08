using System;
using System.Collections.Generic;

namespace CalculatorApp.Models
{
    public class CalculatorModel
    {
        public enum Operator
        {
            Add,
            Subtract,
            Multiply,
            Divide,
            Modulo,
            None
        }

        public double CurrentValue { get; private set; } = 0;
        public double LastValue { get; private set; } = 0;
        public Operator CurrentOperator { get; private set; } = Operator.None;
        public string Expression { get; private set; } = "";
        public bool IsNewValue { get; private set; } = true;
        public string Error { get; private set; } = null;

        private readonly MemoryManager _memory = new MemoryManager();

        public CalculatorModel()
        {
            Reset();
        }

        public void Reset()
        {
            CurrentValue = 0;
            LastValue = 0;
            CurrentOperator = Operator.None;
            Expression = "";
            IsNewValue = true;
            Error = null;
        }

        public void ClearEntry()
        {
            CurrentValue = 0;
            IsNewValue = true;
            Error = null;
        }

        public void Backspace()
        {
            if (Error != null)
            {
                ClearEntry();
                return;
            }

            string valueStr = CurrentValue.ToString();
            if (valueStr.Length > 1)
            {
                valueStr = valueStr.Substring(0, valueStr.Length - 1);
                if (double.TryParse(valueStr, out double newValue))
                {
                    CurrentValue = newValue;
                }
                else
                {
                    CurrentValue = 0;
                }
            }
            else
            {
                CurrentValue = 0;
            }
        }

        public void AddDigit(string digit)
        {
            if (Error != null)
            {
                ClearEntry();
            }

           
            double digitValue;
            if (digit.Length == 1 && char.IsLetter(digit[0]))
            {
                char hexChar = char.ToUpper(digit[0]);
                if (hexChar >= 'A' && hexChar <= 'F')
                {
                    digitValue = hexChar - 'A' + 10;
                }
                else
                {
                    throw new ArgumentException($"The input string '{digit}' was not in a correct format.");
                }
            }
            else
            {
                if (!double.TryParse(digit, out digitValue))
                {
                    throw new ArgumentException($"The input string '{digit}' was not in a correct format.");
                }
            }

            if (IsNewValue)
            {
                CurrentValue = digitValue;
                IsNewValue = false;
            }
            else
            {
               
                bool isProgrammerMode = Settings.CalculatorMode == "Programmer";
                
                if (isProgrammerMode)
                {
                  
                    string baseStr = Settings.NumberBase.ToUpper();
                    int baseValue = 10;
                    
                    switch (baseStr)
                    {
                        case "HEX": baseValue = 16; break;
                        case "DEC": baseValue = 10; break;
                        case "OCT": baseValue = 8; break;
                        case "BIN": baseValue = 2; break;
                    }
                    
              
                    if (digitValue >= baseValue)
                    {
                        throw new ArgumentException($"The digit '{digit}' is not valid in {baseStr} mode.");
                    }
                    
                  
                    long currentValueLong = (long)CurrentValue;
                    currentValueLong = currentValueLong * baseValue + (long)digitValue;
                    CurrentValue = currentValueLong;
                }
                else
                {
                   
                    string valueStr = CurrentValue.ToString() + digit;
                    if (double.TryParse(valueStr, out double newValue))
                    {
                        CurrentValue = newValue;
                    }
                }
            }
        }

        public void AddDecimalPoint()
        {
            if (Error != null)
            {
                ClearEntry();
            }

            if (IsNewValue)
            {
                CurrentValue = 0;
                IsNewValue = false;
            }

            string valueStr = CurrentValue.ToString();
            if (!valueStr.Contains("."))
            {
                valueStr += ".";
                CurrentValue = Convert.ToDouble(valueStr);
            }
        }

        public void SetOperator(Operator op)
        {
            if (Error != null)
            {
                Reset();
                return;
            }

           
            if (CurrentOperator != Operator.None && !Settings.PrecedenceEnabled)
            {
                Calculate();
            }

         
            if (Settings.PrecedenceEnabled)
            {
                string opSymbol = GetOperatorSymbol(op);
                
             
                if (string.IsNullOrEmpty(Expression))
                {
                    Expression = CurrentValue.ToString() + opSymbol;
                }
                else
                {
                 
                    if (!IsNewValue)
                    {
                   
                        Expression += CurrentValue.ToString() + opSymbol;
                    }
                    else
                    {
                      
                        char lastChar = Expression[Expression.Length - 1];
                        if (lastChar == '+' || lastChar == '-' || lastChar == '×' || lastChar == '÷' || 
                            (lastChar == 'd' && Expression.EndsWith("mod")))
                        {
                            if (lastChar == 'd' && Expression.EndsWith("mod"))
                            {
                                Expression = Expression.Substring(0, Expression.Length - 3) + opSymbol;
                            }
                            else
                            {
                                Expression = Expression.Substring(0, Expression.Length - 1) + opSymbol;
                            }
                        }
                        else
                        {
                          
                            Expression += opSymbol;
                        }
                    }
                }
            }

            LastValue = CurrentValue;
            CurrentOperator = op;
            IsNewValue = true;
        }

        public void Calculate()
        {
            if (Error != null)
            {
                return;
            }

            if (Settings.PrecedenceEnabled && !string.IsNullOrEmpty(Expression))
            {
                EvaluateExpression();
                return;
            }

            if (CurrentOperator == Operator.None)
            {
                return;
            }

            try
            {
                switch (CurrentOperator)
                {
                    case Operator.Add:
                        CurrentValue = LastValue + CurrentValue;
                        break;
                    case Operator.Subtract:
                        CurrentValue = LastValue - CurrentValue;
                        break;
                    case Operator.Multiply:
                        CurrentValue = LastValue * CurrentValue;
                        break;
                    case Operator.Divide:
                        if (CurrentValue == 0)
                        {
                            Error = "Cannot divide by zero";
                            return;
                        }
                        CurrentValue = LastValue / CurrentValue;
                        break;
                    case Operator.Modulo:
                        if (CurrentValue == 0)
                        {
                            Error = "Invalid operation";
                            return;
                        }
                        CurrentValue = LastValue % CurrentValue;
                        break;
                }

                LastValue = CurrentValue;
                CurrentOperator = Operator.None;
                IsNewValue = true;
                Expression = "";
            }
            catch (Exception ex)
            {
                Error = $"Calculation error: {ex.Message}";
            }
        }

        private void EvaluateExpression()
        {
            try
            {
                string expr = Expression;
                
              
                if (string.IsNullOrEmpty(expr))
                {
                    return;
                }
                
             
                char lastChar = expr[expr.Length - 1];
                if (lastChar == '+' || lastChar == '-' || lastChar == '×' || lastChar == '÷' || 
                    (lastChar == 'd' && expr.EndsWith("mod")))
                {
                    expr = expr + CurrentValue.ToString();
                }
                
                expr = expr.Replace("×", "*").Replace("÷", "/").Replace("mod", "%");
                
              
                
                List<string> tokens = new List<string>();
                string currentNumber = "";
                bool parsingNumber = false;
                
                for (int i = 0; i < expr.Length; i++)
                {
                    char c = expr[i];
                    if (char.IsDigit(c) || c == '.' || (i == 0 && c == '-') || 
                        (i > 0 && (expr[i-1] == '+' || expr[i-1] == '-' || expr[i-1] == '*' || expr[i-1] == '/' || expr[i-1] == '%') && c == '-'))
                    {
                        if (!parsingNumber && c == '-' && i > 0)
                        {
                        
                            currentNumber = "-";
                        }
                        else
                        {
                            currentNumber += c;
                        }
                        parsingNumber = true;
                    }
                    else if ("+-*/%".Contains(c.ToString()))
                    {
                        if (parsingNumber && !string.IsNullOrEmpty(currentNumber))
                        {
                            tokens.Add(currentNumber);
                            currentNumber = "";
                        }
                        tokens.Add(c.ToString());
                        parsingNumber = false;
                    }
                }
                
                if (!string.IsNullOrEmpty(currentNumber))
                {
                    tokens.Add(currentNumber);
                }
                
          
                if (tokens.Count == 0)
                {
                    return;
                }
                
                
                if (tokens.Count == 1)
                {
                    if (double.TryParse(tokens[0], out double singleValue))
                    {
                        CurrentValue = singleValue;
                        LastValue = singleValue;
                        CurrentOperator = Operator.None;
                        IsNewValue = true;
                        Expression = "";
                        return;
                    }
                }
                
            
                for (int i = 1; i < tokens.Count - 1; i += 2)
                {
                    if (tokens[i] == "*" || tokens[i] == "/" || tokens[i] == "%")
                    {
                        double left = double.Parse(tokens[i - 1]);
                        double right = double.Parse(tokens[i + 1]);
                        double result = 0;
                        
                        switch (tokens[i])
                        {
                            case "*":
                                result = left * right;
                                break;
                            case "/":
                                if (right == 0)
                                {
                                    Error = "Cannot divide by zero";
                                    return;
                                }
                                result = left / right;
                                break;
                            case "%":
                                if (right == 0)
                                {
                                    Error = "Invalid operation";
                                    return;
                                }
                                result = left % right;
                                break;
                        }
                        
                        tokens[i - 1] = result.ToString();
                        tokens.RemoveAt(i);
                        tokens.RemoveAt(i);
                        i -= 2;
                    }
                }
                
              
                double finalResult = double.Parse(tokens[0]);
                for (int i = 1; i < tokens.Count; i += 2)
                {
                    double right = double.Parse(tokens[i + 1]);
                    
                    switch (tokens[i])
                    {
                        case "+":
                            finalResult += right;
                            break;
                        case "-":
                            finalResult -= right;
                            break;
                    }
                }
                
                CurrentValue = finalResult;
                LastValue = finalResult;
                CurrentOperator = Operator.None;
                IsNewValue = true;
                Expression = "";
            }
            catch (Exception ex)
            {
                Error = $"Expression error: {ex.Message}";
            }
        }

        private bool ContainsOperator(string expr)
        {
            return expr.Contains("+") || expr.Contains("-") || expr.Contains("×") || 
                   expr.Contains("÷") || expr.Contains("mod");
        }

        public void PerformUnaryOperation(string operation)
        {
            if (Error != null)
            {
                Reset();
                return;
            }

            try
            {
                switch (operation)
                {
                    case "sqrt":
                        if (CurrentValue < 0)
                        {
                            Error = "Cannot calculate square root of negative number";
                            return;
                        }
                        CurrentValue = Math.Sqrt(CurrentValue);
                        break;
                    case "square":
                        CurrentValue = Math.Pow(CurrentValue, 2);
                        break;
                    case "reciprocal":
                        if (CurrentValue == 0)
                        {
                            Error = "Cannot divide by zero";
                            return;
                        }
                        CurrentValue = 1 / CurrentValue;
                        break;
                    case "negate":
                        CurrentValue = -CurrentValue;
                        break;
                }

                IsNewValue = true;
            }
            catch (Exception ex)
            {
                Error = $"Operation error: {ex.Message}";
            }
        }

        public static string GetOperatorSymbol(Operator op)
        {
            switch (op)
            {
                case Operator.Add: return "+";
                case Operator.Subtract: return "-";
                case Operator.Multiply: return "×";
                case Operator.Divide: return "÷";
                case Operator.Modulo: return "mod";
                default: return "";
            }
        }

        public void MemoryStore()
        {
            _memory.Add(CurrentValue);
        }

        public void MemoryRecall()
        {
            if (_memory.HasValues())
            {
                CurrentValue = _memory.GetLast();
                IsNewValue = true;
            }
        }

        public void MemoryClear()
        {
            _memory.Clear();
        }

        public void MemoryAdd()
        {
            _memory.AddToLast(CurrentValue);
        }

        public void MemorySubtract()
        {
            _memory.SubtractFromLast(CurrentValue);
        }

        public IReadOnlyList<double> GetMemoryList()
        {
            return _memory.GetAll();
        }
    }
}