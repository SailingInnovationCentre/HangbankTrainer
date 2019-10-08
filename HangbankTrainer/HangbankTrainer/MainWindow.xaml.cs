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

        SerialPortListener _listener;

        public MainWindow()
        {
            DataContext = this; 
            InitializeComponent();

            _listener = new SerialPortListener("COM3");
            _listener.NewMessage += (s, e) =>
            {
                string message = ((SerialPortEventArgs)e).Message;

                //double messageAsDouble; 
                if (double.TryParse(message.Trim(), out double messageAsDouble))
                {
                    double d = Math.Sin(messageAsDouble / 5.0);
                    SerialInput = $"{d:0.##}";
                }
            };
            _listener.Open(); 

        }



        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _listener.Close(); 
        }
    }
}
