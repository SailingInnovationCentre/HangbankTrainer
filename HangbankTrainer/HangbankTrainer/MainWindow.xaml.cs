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
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }


        private string _serialInput; 
        public string SerialInput { 
            get
            {
                return _serialInput;
            }
            set
            {
                _serialInput = value;
                NotifyPropertyChanged(); 
            }
        }

        private readonly SerialPortListener _listener;

        public MainWindow()
        {
            InitializeComponent();

            //SeriesCollection = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Links",
            //        Values = new ChartValues<int>(Enumerable.Range(1, 100).Select(x => 0))
            //    },
            //    new LineSeries
            //    {
            //        Title = "Rechts",
            //        Values = new ChartValues<int>(Enumerable.Range(1, 100).Select(x => 0))
            //    }

            //};

            //_listener = new SerialPortListener("COM4");
            //_listener.NewMessage += (s, e) =>
            //{
            //    var eventArgs = (SerialPortEventArgs)e; 
            //    SeriesCollection[0].Values.Add(eventArgs.Left);
            //    SeriesCollection[0].Values.RemoveAt(0); 
            //    SeriesCollection[1].Values.Add(eventArgs.Right);
            //    SeriesCollection[1].Values.RemoveAt(0);
            //};
            //_listener.Open();

            DataContext = new HangbankTrainerConfiguration();
            ConfigurationUserControl.DataContext = DataContext; 

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //_listener.Close(); 
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}
