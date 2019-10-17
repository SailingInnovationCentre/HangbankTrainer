using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HangbankTrainer
{
    /// <summary>
    /// Interaction logic for TrainingConfigurationUserControl.xaml
    /// </summary>
    public partial class TrainingConfigurationUserControl : UserControl, INotifyPropertyChanged
    {
        public TrainingConfigurationUserControl()
        {
            InitializeComponent();
            DataContext = this; 

            AthletePersister.AssertFilesPresent();

            Athletes = new ObservableCollection<Athlete>(AthletePersister.Read());
        }

        private Athlete _currentAthlete;
        public Athlete CurrentAthlete
        {
            get => _currentAthlete;
            set => SetField(ref _currentAthlete, value);
        }

        public ObservableCollection<Athlete> Athletes { get; set; }


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
