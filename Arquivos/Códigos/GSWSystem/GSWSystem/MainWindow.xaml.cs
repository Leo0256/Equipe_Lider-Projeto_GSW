using System;
using System.Collections.Generic;
using System.Linq;
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


namespace GSWSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Frame.Content = new Page1();
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            Frame.Content = new Page1();
        }

        private void BackClick(object sender, RoutedEventArgs e)
        {
            Frame.Content = new Page2();
        }
    }
}
