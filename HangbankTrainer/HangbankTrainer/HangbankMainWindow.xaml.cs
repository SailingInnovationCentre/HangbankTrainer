using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
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
    public partial class HangbankMainWindow : Window, INotifyPropertyChanged
    {

        private HangbankModel _model;

        private FrontPageUserControl _frontPageUserControl;
        private TrainingUserControl _trainingUserControl;
        private ConfigurationUserControl _configUserControl; 

        public HangbankMainWindow()
        {
            InitializeComponent();
            
            _model = new HangbankModel();
            _model.SetSerialPort("COM4");

            StartFrontPage(); 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
        }

        internal void StartFrontPage()
        {
            _frontPageUserControl = new FrontPageUserControl(this, _model);
            MainContentControl.Content = _frontPageUserControl; 
        }

        internal void StartTraining(Athlete athlete, Training training)
        {
            // Create UserControl. 
        }

        internal void EditConfiguration()
        {
            // Create UserControl. 
        }

    }
}
