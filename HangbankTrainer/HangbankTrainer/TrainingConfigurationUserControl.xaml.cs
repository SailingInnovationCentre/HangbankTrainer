using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;

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
            Athletes.Add(Athlete.CreateNew());

            AthletesComboBox.SelectedIndex = 0; 
        }

        private Athlete _currentAthlete;
        public Athlete CurrentAthlete
        {
            get => _currentAthlete;
            set => SetField(ref _currentAthlete, value);
        }

        public ObservableCollection<Athlete> Athletes { get; set; }

        private void AthletesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurrentAthlete = (Athlete) e.AddedItems[0];
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
