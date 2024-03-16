using System.Windows;

namespace VerasDotNetSample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ImageCapture imageCapture = new ImageCapture(HelixView);
            VerasWebUi veras = new VerasWebUi(imageCapture);
            veras.Show();
        }
    }
}
