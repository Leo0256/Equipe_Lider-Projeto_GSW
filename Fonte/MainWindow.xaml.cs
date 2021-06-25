﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Microsoft.Win32;

namespace NoteSystem
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

        private void Upload(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new();

            bool? resp = openFile.ShowDialog();
            if (resp == true)
            {
                string filepath = openFile.FileName;
                using var reader = new StreamReader(filepath);
                
                var json = new NormalizarJson(reader.ReadToEnd());
                List<DataRow[]> row = new();
                
                foreach (var item in json.GetJson())
                {
                    try
                    {
                        row.Add(conn.ExecuteCmd(
                            string.Format("select * from inserir_func('{0}'::json)", item[0])).Select());
                        row.Add(conn.ExecuteCmd(
                            string.Format("select * from inserir_projinfo('{0}'::json)", item[1])).Select());
                        row.Add(conn.ExecuteCmd(
                            string.Format("select * from inserir_git('{0}'::json)", item[2])).Select());

                        if (
                            bool.Parse(row[0][0]["inserir_func"].ToString()) &&
                            bool.Parse(row[1][0]["inserir_projinfo"].ToString()) &&
                            bool.Parse(row[2][0]["inserir_git"].ToString())
                            )
                        {
                            row.Add(conn.ExecuteCmd(string.Format("select * from inserir_projeto('{0}'::json)", item[3])).Select());
                            
                        }
                    }
                    catch(IndexOutOfRangeException ex)
                    {
                        MessageBox.Show("Falha ao Adicionaar os dados:\n  " + ex.Message, null, MessageBoxButton.OK);
                    }
                    finally
                    {
                        row.Clear();
                    }
                }
            }
        }

        private void foo(object sender, RoutedEventArgs e)
        {

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
            Text.IsReadOnly = false;
            Panel.Children.Clear();
            CmdProjeto();
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
            Text.IsReadOnly = false;
            Panel.Children.Clear();
            CmdHFunc();
        }

        private void CmdHProj()
        {
            string sql = string.Format(@"select * from pesquisa_horas_projeto('{0}')",texto);
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<QuantHoras> hProjetos = new();
            foreach (DataRow data in row)
            {
                hProjetos.Add(
                    new QuantHoras(
                        null,
                        data["projeto"].ToString(),
                        data["horas"].ToString(),
                        null
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
            Text.IsReadOnly = false;
            Panel.Children.Clear();
            CmdHProj();
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
                        data["horas"].ToString(),
                        null
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
            Text.IsReadOnly = false;
            Panel.Children.Clear();
            CmdHAno();
        }

        private void CmdHMes()
        {
            int ano = DateTime.Now.Year;
            Panel.Children.Clear();

            for(int i = 0; i < 3; i++)
            {
                if (texto.Equals(string.Empty))
                    texto = "0";
                else
                {
                    try
                    {
                        texto = DateTime.Parse("1/" + texto + "/2000").Month.ToString();

                    }catch (FormatException)
                    {
                        MessageBox.Show("Informe um mês válido.\nEx.: \"fevereiro\", \"fev\" ou \"2\"","",MessageBoxButton.OK);
                        texto = "0";
                    }
                }


                string sql = string.Format(@"select * from pesquisa_horas_mes({0},{1})", texto, ano);
                DataRow[] row = conn.ExecuteCmd(sql).Select();

                List<QuantHoras> hMes = new();
                foreach (DataRow data in row)
                {
                    hMes.Add(
                        new QuantHoras(
                            "mes",
                            data["mes"].ToString(),
                            data["horas"].ToString(),
                            data["ano"].ToString()
                        ));
                }

                foreach (QuantHoras x in hMes)
                {
                    Panel.Children.Add(
                        new Frame
                        {
                            Content = x
                        });
                }

                ano--;
                if (texto.Equals("0"))
                    break;
            }
            texto = string.Empty;
        }

        private void ViewHorasMes(object sender, RoutedEventArgs e)
        {
            function = "hmes";
            Text.IsReadOnly = false;
            Panel.Children.Clear();
            CmdHMes();
        }

        private void CmdPercent()
        {
            string sql = string.Format(@"select * from pesquisa_dedicacao_func('{0}')", texto);
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<int> xId = new();
            string xProj = string.Empty;
            List<string> pNome = new(),
                         uNome = new(),
                         xPercent = new();

            foreach (DataRow data in row)
                xId.Add(int.Parse(data["id"].ToString()));


            var index = xId.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count);

            List<Dedicacao> percent = new();

            foreach (var x in index)
            {
                for(int i = 0; i < xId.Count; i++)
                {
                    if (x.Value == int.Parse(row[i]["id"].ToString()))
                    {
                        xProj = row[i]["projeto"].ToString();
                        pNome.Add(row[i]["p_nome"].ToString());
                        uNome.Add(row[i]["u_nome"].ToString());
                        xPercent.Add(row[i]["porcentagem"].ToString());
                    }
                }

                percent.Add(
                    new(
                        xProj,
                        pNome.ToArray(),
                        uNome.ToArray(),
                        xPercent.ToArray()
                    ));
                xProj = string.Empty;
                pNome.Clear();
                uNome.Clear();
                xPercent.Clear();
            }

            Panel.Children.Clear();
            foreach (Dedicacao x in percent)
                Panel.Children.Add(
                    new Frame
                    {
                        Content = x
                    });
        }

        private void ViewDedicacao(object sender, RoutedEventArgs e)
        {
            function = "percent";
            Text.IsReadOnly = false;
            Panel.Children.Clear();
            CmdPercent();
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
            Text.IsReadOnly = true;
            Panel.Children.Clear();
            CmdQStatus();
        }

        private void CmdTask()
        {
            string sql = string.Format(@"select * from pesquisa_tasks_abertas()");
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<string> xProj = new(),
                         xStatus = new();

            foreach(DataRow data in row)
            {
                string foo = data["projeto"].ToString();
                if (foo.Length > 30)
                {
                    foo = foo[..25] + "\n" + foo[25..];
                    if (foo.Length > 50)
                        foo = foo[..50] + "(...)";
                }
                xProj.Add(
                    string.Concat(
                        foo,
                        "\n    id: ",
                        data["id"].ToString()
                        ));
                xStatus.Add(data["status"].ToString());
            }

            GraficoTotal status = new(
                false,
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
            Text.IsReadOnly = true;
            Panel.Children.Clear();
            CmdTask();
        }

        private void CmdCalendario()
        {
            string sql = string.Format(@"select * from pesquisa_datas_projeto()");
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<string> xData = new(),
                         xProj = new();
            foreach(var data in row)
            {
                xData.Add(data["data"].ToString()[..10]);
                xProj.Add(data["nome"].ToString());
            }

            Panel.Children.Clear();
            Panel.Children.Add(
                new Frame
                {
                    Content = new Calendario(xData.ToArray(), xProj.ToArray())
                }
            );
        }

        private void ViewCalendarioTasks(object sender, RoutedEventArgs e)
        {
            Text.IsReadOnly = true;
            Panel.Children.Clear();
            CmdCalendario();
        }

        private void CmdRankFunc()
        {
            string sql = string.Format(@"select * from pesquisa_rank_func('{0}')",texto);
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<string> Nome = new();
            List<double> Horas = new();

            string xProj = "";

            foreach (var data in row)
            {
                xProj = !(!texto.Equals(string.Empty) || data["proj"].ToString().Equals(null)) ?
                    "Rank de Horas dos Funcionarios" : row[0]["proj"].ToString();

                Nome.Add(string.Format("{0} {1}",
                    data["pnome"].ToString(),
                    data["unome"].ToString()));

                Horas.Add(double.Parse(data["horas"].ToString()));
            }

            Panel.Children.Clear();
            Panel.Children.Add(
                new Frame
                {
                    Content = new RankHoras(
                        xProj,
                        Nome.ToArray(),
                        Horas.ToArray())
                }
            );
            
        }

        private void ViewRankFunc(object sender, RoutedEventArgs e)
        {
            function = "rank";
            Text.IsReadOnly = false;
            Panel.Children.Clear();
            CmdRankFunc();
        }
        
        private void CmdRankProj()
        {
            string sql = string.Format(@"select * from pesquisa_rank_proj()");
            DataRow[] row = conn.ExecuteCmd(sql).Select();

            List<string> xProj = new();
            List<double> Horas = new();

            foreach (var data in row)
            {
                xProj.Add(data["nome"].ToString());
                Horas.Add(double.Parse(data["horas"].ToString()));
            }

            Panel.Children.Clear();
            Panel.Children.Add(
                new Frame
                {
                    Content = new RankHoras(
                        "Rank de Horas dos Projetos",
                        xProj.ToArray(),
                        Horas.ToArray())
                }
            );
        } 

        private void ViewRankProj(object sender, RoutedEventArgs e)
        {
            Text.IsReadOnly = true;
            Panel.Children.Clear();
            CmdRankProj();
        }

        private void EntradaTexto(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                texto = Text.Text;
                Text.Text = string.Empty;
                
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

                    case "rank":
                        CmdRankFunc();
                        break;
                }
                texto = string.Empty;
            }
        }
    }
}