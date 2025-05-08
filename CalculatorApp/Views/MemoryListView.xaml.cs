using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace CalculatorApp.Views
{
    public partial class MemoryListView : UserControl
    {
        public delegate void MemoryClearEventHandler();
        public event MemoryClearEventHandler MemoryClear;
        
        public MemoryListView()
        {
            InitializeComponent();
        }
        
        public void SetMemoryItems(List<string> items)
        {
            MemoryListBox.ItemsSource = items;
        }
        
        private void MemoryListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This is handled in the parent window
        }
        
        private void ClearAllButton_Click(object sender, RoutedEventArgs e)
        {
            MemoryClear?.Invoke();
            MemoryListBox.ItemsSource = null;
        }
    }
}