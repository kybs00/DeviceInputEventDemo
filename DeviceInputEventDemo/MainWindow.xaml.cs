using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DeviceInputEventDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var deviceEventTransformer = new DeviceEventTransformer(this);
            deviceEventTransformer.DeviceDown += DeviceEventTransformer_DeviceDown;
            deviceEventTransformer.Register();
        }

        private void DeviceEventTransformer_DeviceDown(object? sender, DeviceInputArgs e)
        {
            MessageBox.Show($"{e.DeviceId},{e.DeviceType},({e.Position.X},{e.Position.Y})");
        }
    }
}