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
            
            */
            /*
            
            */
            /*
            
            */
            /*
            
            */

            string[] Projeto = { 
                "Empresa X - [Front-end]",
                "Projeto Golden Eye [Debug]",
                "ABCD Ltda. [Pesquisa de Mercado]" 
            };
            string[] Total = { "Stand__BY", "IN__PROGRESS", "IN__PROGRESS"};
            Frame.Content = new GraficoTotal(false, Projeto, Total);
        }

        private void HomeClick(object sender, RoutedEventArgs e)
        {
            Frame.Content = new Page2();
        }

        private void OpenMenu(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void CloseMenu(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
        }

        private void ViewProjeto(object sender, RoutedEventArgs e)
        {
            var projeto = new Projeto(
                "Empresa X",
                "IN__PROGRESS",
                "Front-end do site da Empresa X, com todas as telas",
                "2021-02-23 14:30:00",
                "Sim");
            Frame.Content = projeto;
        }

        private void ViewHorasFuncionario(object sender, RoutedEventArgs e)
        {
            Frame.Content = new Funcionario(
                "Robson",
                "Oliveira",
                "25.40",
                "robson.oliveira@email",
                "http://placeimg.com/640/480/people");
        }

        private void ViewHorasProjeto(object sender, RoutedEventArgs e)
        {
            Frame.Content = new QuantHoras(
                null,
                "Projeto Golden-eye",
                "42.42");
        }

        private void ViewHorasAno(object sender, RoutedEventArgs e)
        {
            Frame.Content = new QuantHoras(
                null,
                "Projeto Golden-eye",
                "42.42");
        }

        private void ViewHorasMes(object sender, RoutedEventArgs e)
        {
            Frame.Content = new QuantHoras(
                null,
                "Projeto Golden-eye",
                "42.42");
        }

        private void ViewDedicacao(object sender, RoutedEventArgs e)
        {
            string[] pNome = { "Robson", "Maria", "Miriam" };
            string[] uNome = { "Oliveira", "dos Santos", "Pascal" };
            string[] percent = { "35", "25", "40" };

            Frame.Content = new Dedicacao(
                pNome,
                uNome,
                percent);
        }

        private void ViewQuantStatus(object sender, RoutedEventArgs e)
        {
            string[] Status = { "DONE", "IN__PROGRESS", "STAND__BY" };
            string[] Total = { "3", "6", "4" };
            Frame.Content = new GraficoTotal(true, Status, Total);
        }

        private void ViewTasksAbertas(object sender, RoutedEventArgs e)
        {
            var projeto = new Projeto(
                "Empresa X",
                "IN__PROGRESS",
                "Front-end do site da Empresa X, com todas as telas",
                "2021-02-23 14:30:00",
                "Sim");
            Frame.Content = projeto;
        }
    }
}
