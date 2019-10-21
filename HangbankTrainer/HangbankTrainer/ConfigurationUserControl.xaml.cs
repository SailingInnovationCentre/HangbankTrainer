using System.Windows.Controls;

namespace HangbankTrainer
{
    /// <summary>
    /// Interaction logic for ConfigurationUserControl.xaml
    /// </summary>
    public partial class ConfigurationUserControl : UserControl
    {

        private HangbankModel _model;

        public ConfigurationUserControl(HangbankModel model)
        {
            _model = model; 
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComPortComboBox.SelectedValue != null)
            {
                string comPort = ComPortComboBox.SelectedValue.ToString();
                _model.SetSerialPort(comPort);
            }
        }
    }
}
