using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class HangbankMainWindow : Window
    {

        private HangbankModel _model;

        private FrontPageUserControl _frontPageUserControl;
        private TrainingUserControl _trainingUserControl;
        private ConfigurationUserControl _configUserControl; 

        public HangbankMainWindow()
        {
            InitializeComponent();
            
            _model = new HangbankModel();
            _model.Listener.SerialPortName = "COM4";

            AthletePersister.AssertFilesPresent();
            _model.Athletes = new ObservableCollection<Athlete>(AthletePersister.Read());

            StartFrontPage(); 
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _model.Listener.CloseSerialPort(); 
        }

        internal void StartFrontPage()
        {
            _frontPageUserControl = new FrontPageUserControl(this, _model);
            MainContentControl.Content = _frontPageUserControl; 
        }

        internal void StartTraining()
        {
            _trainingUserControl = new TrainingUserControl(this, _model);
            MainContentControl.Content = _trainingUserControl; 
        }

        internal void StartConfiguration()
        {
            _configUserControl = new ConfigurationUserControl(this, _model);
            MainContentControl.Content = _configUserControl;
        }

        internal void StartCalibration()
        {
            var calibrationControl = new CalibrationUserControl(this, _model);
            MainContentControl.Content = calibrationControl; 
        }
    }
}
