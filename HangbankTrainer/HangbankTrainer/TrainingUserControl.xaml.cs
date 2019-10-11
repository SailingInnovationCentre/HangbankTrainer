using LiveCharts;
using LiveCharts.Wpf;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace HangbankTrainer
{
    /// <summary>
    /// Interaction logic for TrainingUserControl.xaml
    /// </summary>
    public partial class TrainingUserControl : UserControl, INotifyPropertyChanged
    {
          
        public TrainingUserControl()
        {
            InitializeComponent();

            SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Links",
                    Values = new ChartValues<int>(Enumerable.Range(1, 100).Select(x => 0))
                },
                new LineSeries
                {
                    Title = "Rechts",
                    Values = new ChartValues<int>(Enumerable.Range(1, 100).Select(x => 0))
                }
            };

            DataContext = this; 
        }

        private HangbankTrainerConfiguration _config; 
        internal HangbankTrainerConfiguration Config
        {
            get
            {
                return _config;
            }
            set
            {
                _config = value;
                _config.Listener.NewMessage += (s, e) =>
                {
                    var eventArgs = (SerialPortEventArgs)e;

                    LinksVolt = eventArgs.Left;
                    LinksMoment = (int)(1000 * (LinksVolt - Config.LinksOnbelast) / (Config.LinksBelast - Config.LinksOnbelast));
                    SeriesCollection[0].Values.Add(LinksMoment);
                    SeriesCollection[0].Values.RemoveAt(0);

                    RechtsVolt = eventArgs.Right;
                    RechtsMoment = (int)(1000 * (RechtsVolt - Config.RechtsOnbelast) / (Config.RechtsBelast - Config.RechtsOnbelast));
                    SeriesCollection[1].Values.Add(RechtsMoment);
                    SeriesCollection[1].Values.RemoveAt(0);
                };
            }
        }

        public SeriesCollection SeriesCollection { get; set; }

        #region Bindings

        private string _serialInput;
        public string SerialInput
        {
            get => _serialInput;
            set => SetField(ref _serialInput, value);
        }

        private int _linksVolt;
        public int LinksVolt
        {
            get => _linksVolt;
            set => SetField(ref _linksVolt, value);
        }

        private int _linksMmoment;
        public int LinksMoment
        {
            get => _linksMmoment;
            set => SetField(ref _linksMmoment, value);
        }

        private double _linksPercentage;
        public double LinksPercentage
        {
            get => _linksPercentage;
            set => SetField(ref _linksPercentage, value);
        }

        private int _rechtsVolt;
        public int RechtsVolt
        {
            get => _rechtsVolt;
            set => SetField(ref _rechtsVolt, value);
        }

        private int _rechtsMmoment;
        public int RechtsMoment
        {
            get => _rechtsMmoment;
            set => SetField(ref _rechtsMmoment, value);
        }

        private double _rechtsPercentage;
        public double RechtsPercentage
        {
            get => _rechtsPercentage;
            set => SetField(ref _rechtsPercentage, value);
        }

        #endregion

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
