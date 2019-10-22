using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows;

namespace HangbankTrainer
{
    /// <summary>
    /// Interaction logic for ConfigurationUserControl.xaml
    /// </summary>
    public partial class ConfigurationUserControl : UserControl, INotifyPropertyChanged
    {

        private HangbankMainWindow _mainWindow;

        public ConfigurationUserControl(HangbankMainWindow mainWindow, HangbankModel model)
        {
            DataContext = model; 
            _mainWindow = mainWindow; 
            InitializeComponent();
        }

        private void FrontScreenButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.StartFrontPage();
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        #endregion
    }
}
