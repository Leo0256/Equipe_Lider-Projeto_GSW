using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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

        private PostgreSQLConnection conn;
        private string function;
        private string texto;

        public MainWindow()
        {
            InitializeComponent();

            conn = new();
            texto = string.Empty;
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
        
        private void CmdProjeto()
        {
            string sql = string.Format(@"select * from pesquisa_nome_projeto('{0}')", texto);
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<Projeto> projetos = new();
            foreach (DataRow data in row)
            {
                projetos.Add(
                    new Projeto(
                        data["nome"].ToString(),
                        data["status"].ToString(),
                        data["decr"].ToString(),
                        data["iniciado"].ToString(),
                        data["finalizado"].ToString()
                    ));
            }

            Panel.Children.Clear();
            foreach (Projeto x in projetos)
            {
                Panel.Children.Add(
                    new Frame
                    {
                        Content = x
                    });
            }
        }

        private void ViewProjeto(object sender, RoutedEventArgs e)
        {
            function = "projeto";

            Text.Text = string.Empty;
            Panel.Children.Clear();
        }
    
        private void CmdHFunc()
        {
            string sql = string.Format(@"select * from pesquisa_horas_funcionario('{0}')", texto);
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<Funcionario> funcionarios = new();
            foreach (DataRow data in row)
            {
                funcionarios.Add(
                    new Funcionario(
                        data["primeiro_nome"].ToString(),
                        data["ultimo_nome"].ToString(),
                        data["horas"].ToString(),
                        data["email"].ToString(),
                        data["avatar"].ToString()
                    ));
            }

            Panel.Children.Clear();
            foreach (Funcionario x in funcionarios)
            {
                Panel.Children.Add(
                    new Frame
                    {
                        Content = x
                    });
            }
        }

        private void ViewHorasFuncionario(object sender, RoutedEventArgs e)
        {
            function = "hfunc";

            Text.Text = string.Empty;
            Panel.Children.Clear();
        }

        private void CmdHProj()
        {
            string sql = string.Format(@"select * from pesquisa_horas_projeto()");
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<QuantHoras> hProjetos = new();
            foreach (DataRow data in row)
            {
                hProjetos.Add(
                    new QuantHoras(
                        null,
                        data["projeto"].ToString(),
                        data["horas"].ToString()
                    ));
            }

            Panel.Children.Clear();
            foreach (QuantHoras x in hProjetos)
            {
                Panel.Children.Add(
                    new Frame
                    {
                        Content = x
                    });
            }
        }

        private void ViewHorasProjeto(object sender, RoutedEventArgs e)
        {
            function = "hproj";

            Text.Text = string.Empty;
            Panel.Children.Clear();
        }

        private void CmdHAno()
        {
            string sql = string.Format(@"select * from pesquisa_horas_ano({0})", texto);
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<QuantHoras> hAno = new();
            foreach (DataRow data in row)
            {
                hAno.Add(
                    new QuantHoras(
                        "ano",
                        data["ano"].ToString(),
                        data["horas"].ToString()
                    ));
            }

            Panel.Children.Clear();
            foreach (QuantHoras x in hAno)
            {
                Panel.Children.Add(
                    new Frame
                    {
                        Content = x
                    });
            }
        }

        private void ViewHorasAno(object sender, RoutedEventArgs e)
        {
            function = "hano";

            Text.Text = string.Empty;
            Panel.Children.Clear();
        }

        private void CmdHMes()
        {
            int ano = 2021;
            string sql = string.Format(@"select * from pesquisa_horas_mes({0},{1})", texto, ano);
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<QuantHoras> hMes = new();
            foreach (DataRow data in row)
            {
                hMes.Add(
                    new QuantHoras(
                        "mes",
                        data["mes"].ToString(),
                        data["horas"].ToString()
                    ));
            }

            Panel.Children.Clear();
            foreach (QuantHoras x in hMes)
            {
                Panel.Children.Add(
                    new Frame
                    {
                        Content = x
                    });
            }
        }

        private void ViewHorasMes(object sender, RoutedEventArgs e)
        {
            function = "hmes";

            Text.Text = string.Empty;
            Panel.Children.Clear();
        }

        private void CmdPercent()
        {
            string sql = string.Format(@"select * from pesquisa_dedicacao_func('{0}')", texto);
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<string> pNome = new(),
                         uNome = new(),
                         xPercent = new();

            foreach(DataRow data in row)
            {
                
                pNome.Add(data["p_nome"].ToString());
                uNome.Add(data["u_nome"].ToString());
                xPercent.Add(data["porcentagem"].ToString());
            }

            Dedicacao percent = new(
                    pNome.ToArray(),
                    uNome.ToArray(),
                    xPercent.ToArray());

            Panel.Children.Clear();
            Panel.Children.Add(
                new Frame
                {
                    Content = percent
                });
        }

        private void ViewDedicacao(object sender, RoutedEventArgs e)
        {
            function = "percent";

            Text.Text = string.Empty;
            Panel.Children.Clear();
        }

        private void CmdQStatus()
        {
            string sql = string.Format(@"select * from pesquisa_quant_status()");
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<string> xStatus = new(),
                         xTotal = new();

            foreach(DataRow data in row)
            {
                xStatus.Add(data["status"].ToString());
                xTotal.Add(data["total"].ToString());
            }

            GraficoTotal status = new(
                true,
                xStatus.ToArray(),
                xTotal.ToArray());

            Panel.Children.Clear();
            Panel.Children.Add(
                new Frame
                {
                    Content = status
                });
        }

        private void ViewQuantStatus(object sender, RoutedEventArgs e)
        {
            function = "qstatus";

            Text.Text = string.Empty;
            Panel.Children.Clear();
        }

        private void CmdTask()
        {
            string sql = string.Format(@"select * from pesquisa_tasks_abertas()");
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<string> xProj = new(),
                         xStatus = new();

            foreach(DataRow data in row)
            {
                xProj.Add(
                    string.Concat(
                        data["id"].ToString(),
                        " - ",
                        data["projeto"].ToString()
                        ));
                xStatus.Add(data["status"].ToString());
            }

            GraficoTotal status = new(
                true,
                xProj.ToArray(),
                xStatus.ToArray());

            Panel.Children.Clear();
            Panel.Children.Add(
                new Frame
                {
                    Content = status
                });
        }

        private void ViewTasksAbertas(object sender, RoutedEventArgs e)
        {
            function = "task";

            Text.Text = string.Empty;
            Panel.Children.Clear();
        }

        private void EntradaTexto(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                texto = Text.Text;

                switch (function)
                {
                    case "projeto":
                        CmdProjeto();
                        break;

                    case "hfunc":
                        CmdHFunc();
                        break;

                    case "hproj":
                        CmdHProj();
                        break;

                    case "hano":
                        CmdHAno();
                        break;

                    case "hmes":
                        CmdHMes();
                        break;

                    case "percent":
                        CmdPercent();
                        break;

                    case "qstatus":
                        CmdQStatus();
                        break;

                    case "task":
                        CmdTask();
                        break;
                };
            }
        }
    }
}
