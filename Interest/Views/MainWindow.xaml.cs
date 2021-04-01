using Interest.ViewModels;
using System.Windows;

namespace Interest.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext is ICreateWindow icw)
            {
                icw.CreateWindow = () =>
                {
                    var v = new About();
                    v.ShowDialog();
                };
            }
        }
    }
}
