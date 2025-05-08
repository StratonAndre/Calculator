using System.Collections.Generic;
using System.Linq;

namespace CalculatorApp.Models
{
    public class MemoryManager
    {
        private List<double> _memoryList = new List<double>();

        public void Add(double value)
        {
            _memoryList.Add(value);
        }

        public void AddToLast(double value)
        {
            if (_memoryList.Count > 0)
            {
                _memoryList[_memoryList.Count - 1] += value;
            }
            else
            {
                _memoryList.Add(value);
            }
        }

        public void SubtractFromLast(double value)
        {
            if (_memoryList.Count > 0)
            {
                _memoryList[_memoryList.Count - 1] -= value;
            }
            else
            {
                _memoryList.Add(-value);
            }
        }

        public double GetLast()
        {
            return _memoryList.Count > 0 ? _memoryList.Last() : 0;
        }

        public bool HasValues()
        {
            return _memoryList.Count > 0;
        }

        public void Clear()
        {
            _memoryList.Clear();
        }

        public IReadOnlyList<double> GetAll()
        {
            return _memoryList.AsReadOnly();
        }
    }
}