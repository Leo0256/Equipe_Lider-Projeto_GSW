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
using GSWSQL;


namespace GSWSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            /*
            var projeto = new Projeto(
                "Empresa X",
                "IN__PROGRESS",
                "Front-end do site da Empresa X, com todas as telas",
                "2021-02-23 14:30:00",
                "Sim");
            Frame.Content = projeto;
            */
            /*
            Frame.Content = new Funcionario(
                "Robson",
                "Oliveira",
                "25.40",
                "robson.oliveira@email",
                "http://placeimg.com/640/480/people");
            */
            /*
            Frame.Content = new QuantHoras(
                null, 
                "Projeto Golden-eye", 
                "42.42");
            */
            string[] pNome = {"Robson","Maria","Miriam"};
            string[] uNome = {"Oliveira","dos Santos","Pascal"};
            string[] percent = {"35","25","40"};

            Frame.Content = new Dedicacao(
                pNome,
                uNome,
                percent);
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            Frame.Content = new Page2();
        }
    }
}
