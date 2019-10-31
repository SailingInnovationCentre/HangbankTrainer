using System.Windows;
using System.Windows.Controls;

namespace HangbankTrainer
{
    /// <summary>
    /// Interaction logic for TrainingConfigurationUserControl.xaml
    /// </summary>
    public partial class FrontPageUserControl : UserControl
    {
        private HangbankMainWindow _mainWindow;
        private HangbankModel _model;

        internal FrontPageUserControl(HangbankMainWindow mainWindow, HangbankModel model)
        {
            _mainWindow = mainWindow;
            _model = model; 

            InitializeComponent();
            DataContext = model;

            ActionComboBox.SelectedIndex = 0;
        }

        public Athlete CurrentAthlete => _model.CurrentAthlete; 

        private void ActionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string action = ((ComboBoxItem)e.AddedItems[0]).Name;
            if (action == "CurrentAthleteCbi")
            {
                AthletesComboBox.Visibility = Visibility.Visible;
                NameTextBox.IsEnabled = true;
                LengthTextBox.IsEnabled = true;
                WeightTextBox.IsEnabled = true;
                MomentMinTextBox.IsEnabled = true;
                MomentMidTextBox.IsEnabled = true;
                MomentMaxTextBox.IsEnabled = true;
                ActionButton.IsEnabled = true;
                ActionButton.Content = "Sla gegevens op";
            }
            else if (action == "AddAthleteCbi")
            {
                AthletesComboBox.Visibility = Visibility.Hidden;
                NameTextBox.IsEnabled = true;
                LengthTextBox.IsEnabled = true;
                WeightTextBox.IsEnabled = true;
                MomentMinTextBox.IsEnabled = true;
                MomentMidTextBox.IsEnabled = true;
                MomentMaxTextBox.IsEnabled = true;

                ActionButton.IsEnabled = true;
                ActionButton.Content = "Voeg atleet toe";
                _model.Athletes.Add(_model.CurrentAthlete);
                _model.CurrentAthlete = Athlete.CreateNew();
            }
            else if (action == "DeleteAthleteCbi")
            {
                AthletesComboBox.Visibility = Visibility.Visible;
                AthletesComboBox.SelectedIndex = 0;
                NameTextBox.IsEnabled = false;
                LengthTextBox.IsEnabled = false;
                WeightTextBox.IsEnabled = false;
                MomentMinTextBox.IsEnabled = false;
                MomentMidTextBox.IsEnabled = false;
                MomentMaxTextBox.IsEnabled = false;

                ActionButton.IsEnabled = true;
                ActionButton.Content = "Verwijder atleet";
            }
        }

        private void AthletesComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                _model.CurrentAthlete = (Athlete)e.AddedItems[0];
            }
        }

        private void ActionButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string action = (string)((Button)sender).Content;

            if (action.Contains("Verwijder"))
            {
                _model.Athletes.Remove(_model.CurrentAthlete);
            }
            else if (action.Contains("Voeg"))
            {
                _model.Athletes.Add(_model.CurrentAthlete);
            }

            AthletePersister.Write(_model.Athletes);
            MessageBox.Show("Gegevens zijn geactualiseerd.");

            var athlete = _model.CurrentAthlete;
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

        private void StartTrainingButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.StartTraining(); 
        }

        private void ConfigurationButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.StartConfiguration();
        }

        private void CalibrationButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _mainWindow.StartCalibration(); 
        }
    }
}
