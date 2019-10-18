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
            ActionComboBox.SelectedIndex = 0;
        }

        private Athlete _currentAthlete;
        public Athlete CurrentAthlete
        {
            get => _currentAthlete;
            set => SetField(ref _currentAthlete, value);
        }

        public ObservableCollection<Athlete> Athletes { get; set; }

        private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string action = ((ComboBoxItem)e.AddedItems[0]).Name;
            if (action == "CurrentAthleteCbi")
            {
                AthletesComboBox.Visibility = System.Windows.Visibility.Visible;
                AthletesComboBox.SelectedIndex = 0;
                NameTextBox.IsEnabled = true;
                LengthTextBox.IsEnabled = true;
                WeightTextBox.IsEnabled = true;
                MomentMaxTextBox.IsEnabled = true;
                Moment75TextBox.IsEnabled = true;
                Moment145TextBox.IsEnabled = true;
                ActionButton.IsEnabled = true;
                ActionButton.Content = "Sla gegevens op";
            }
            else if (action == "AddAthleteCbi")
            {
                AthletesComboBox.Visibility = System.Windows.Visibility.Hidden;
                NameTextBox.IsEnabled = true;
                LengthTextBox.IsEnabled = true;
                WeightTextBox.IsEnabled = true;
                MomentMaxTextBox.IsEnabled = true;
                Moment75TextBox.IsEnabled = true;
                Moment145TextBox.IsEnabled = true;
                ActionButton.IsEnabled = true;
                ActionButton.Content = "Voeg atleet toe";
                CurrentAthlete = Athlete.CreateNew();
            }
            else if (action == "DeleteAthleteCbi")
            {
                AthletesComboBox.Visibility = System.Windows.Visibility.Visible;
                AthletesComboBox.SelectedIndex = 0;
                NameTextBox.IsEnabled = false;
                LengthTextBox.IsEnabled = false;
                WeightTextBox.IsEnabled = false;
                MomentMaxTextBox.IsEnabled = false;
                Moment75TextBox.IsEnabled = false;
                Moment145TextBox.IsEnabled = false;
                ActionButton.IsEnabled = true;
                ActionButton.Content = "Verwijder atleet";
            }
        }

        private void AthletesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                CurrentAthlete = (Athlete)e.AddedItems[0];
            }
        }

        private void ActionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string action = (string)((Button)sender).Content;

            if (action.Contains("Verwijder"))
            {
                Athletes.Remove(CurrentAthlete);
            }
            else if (action.Contains("Voeg"))
            {
                Athletes.Add(CurrentAthlete);
            }

            AthletePersister.Write(Athletes);

            var athlete = CurrentAthlete;
            ActionComboBox.SelectedIndex = 0;

            if (action.Contains("Verwijder"))
            {
                AthletesComboBox.SelectedIndex = 0;
            }
            else
            {
                AthletesComboBox.SelectedItem = athlete;
            }
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
