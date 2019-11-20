using HangbankTrainer;
using HangbankTrainer.Model;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace HangbankTrainer.View
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
        }

        private void AddAthleteButton_Click(object sender, RoutedEventArgs e)
        {
            var athlete = new Athlete("Nieuwe atleet", 180, 85, 60, 80, 100, 3);
            _model.Athletes.Add(athlete);
            _model.CurrentAthlete = athlete; 
        }

        private void DeleteAthleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_model.CurrentAthlete != null && _model.Athletes.Contains(_model.CurrentAthlete))
            {
                _model.Athletes.Remove(_model.CurrentAthlete); 
            }

            _model.CurrentAthlete = _model.Athletes.FirstOrDefault(); 
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Somehow, the ComboBox doesn't update properly after the change of name. 
            // This routine enforces this behaviour. 
            _model.CurrentAthlete.Name = NameTextBox.Text;
            int selectedIndex = AthletesComboBox.SelectedIndex;
            AthletesComboBox.SelectedIndex = -1;
            AthletesComboBox.Items.Refresh();
            AthletesComboBox.SelectedIndex = selectedIndex;
        }

        private void StartTrainingButton_Click(object sender, RoutedEventArgs e)
        {
            if (_model.Training.IntensityType == IntensityTypeEnum.High)
            {
                _model.Training.Target = _model.CurrentAthlete.MomentMax;
            }
            else if (_model.Training.IntensityType == IntensityTypeEnum.Mid)
            {
                _model.Training.Target = _model.CurrentAthlete.MomentMid;
            }
            else if (_model.Training.IntensityType == IntensityTypeEnum.Low)
            {
                _model.Training.Target = _model.CurrentAthlete.MomentMin;
            }

            _model.Training.Bandwidth = _model.CurrentAthlete.Bandwidth; 

            _mainWindow.StartTraining(); 
        }

        private void ConfigurationButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.StartConfiguration();
        }

        private void CalibrationButton_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.StartCalibration(); 
        }
    }
}
