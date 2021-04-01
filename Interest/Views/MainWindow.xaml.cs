using Interest.Options;
using Interest.ViewModels;
using System;
using System.Windows;
using System.Windows.Input;

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
            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (DataContext is IOnClose ioc)
            {
                ioc.OnClose();
            }
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

            if (DataContext is IShowMessage ism)
            {
                ism.ShowMessage = (s) =>
                {
                    MessageBox.Show(s, "", MessageBoxButton.OK);
                };
            }

        }
    }
}
