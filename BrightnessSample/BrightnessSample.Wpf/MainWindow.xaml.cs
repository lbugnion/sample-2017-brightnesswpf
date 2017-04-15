using System.Management;
using System.Windows;

namespace BrightnessSample.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int _currentValue;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BrightnessButtonClick(object sender, RoutedEventArgs e)
        {
            _currentValue += 10; // percents

            if (_currentValue > 100)
            {
                _currentValue = 0; // cycle
            }

            // Get the class by executing the query
            var searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM WmiMonitorBrightness");
            var results = searcher.Get();
            var resultEnum = results.GetEnumerator();
            resultEnum.MoveNext();

            var result = resultEnum.Current;
            var instanceName = (string)result["InstanceName"];

            // Create the instance
            var classInstance = new ManagementObject(
                "root\\WMI", 
                "WmiMonitorBrightnessMethods.InstanceName='" + instanceName + "'", 
                null);

            // Get the parameters for the method
            var inParams = classInstance.GetMethodParameters("WmiSetBrightness");
            inParams["Brightness"] = _currentValue;
            inParams["Timeout"] = 0;

            classInstance.InvokeMethod("WmiSetBrightness", inParams, null);
        }
    }
}
