using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
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
using Microsoft.Win32;

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

        private void Upload(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new();

            bool? resp = openFile.ShowDialog();
            if (resp == true)
            {
                string filepath = openFile.FileName;
                using var reader = new StreamReader(filepath);
                //MessageBox.Show(reader.ReadToEnd(),"",MessageBoxButton.OK);
                
                NormalizarJson json;

                json = new(reader.ReadToEnd());

                MessageBox.Show(json.GetJson()[0]);

                /**
                 * 
                /*
                Como deve ficar o Json 
                [
	                {
		                "func":{
			                "id_func": "8c7d3ba7-9b51-4f78-a863-b3074af5f7a1",
			                "primeiro_nome": "Cecília",
			                "ultimo_nome": "Moreira",
			                "avatar": "http://placeimg.com/640/480/people",
			                "email": "cecilia.moreira@gsw.com.br"
		                },
		                "projInfo":{
			                "nome": "[Pereira - Barros Comércio] - Mandatory fault-tolerant Graphical User Interface",
			                "descr": "synthesize mobile protocol",
			                "horas": 9.71
		                },
		                "gitMetadata":{
			                "branch": "array-synthesize",
			                "hash": "b16181f46f9318acd5ac50f760f30bf2428a28b4"
		                },
	    	            "projeto":{
			                "id": "0ac33506-5610-418f-9b7b-fe9eaacf4f53",
			                "iniciado": "2020-04-06T03:42:23.453Z",
			                "status": "PROD_DEPLOYING",
			                "finalizado": 1
		                }
	                }
                ]
                --------------
                Como ele vai vir do:
                Trello:

                [
	                {
		                "_id": "0ac33506-5610-418f-9b7b-fe9eaacf4f53",
		                "status": "PROD_DEPLOYING",
		                "user": {
			                "_id": "8c7d3ba7-9b51-4f78-a863-b3074af5f7a1",
			                "avatar": "http://placeimg.com/640/480/people",
			                "userName": "Cecília",
			                "userLastName": "Moreira",
			                "userEmail": "cecilia.moreira@gsw.com.br"
		                },
		                "hours": 9.71,
		                "startedAt": "2020-04-06T03:42:23.453Z",
		                "isFinished": true,
		                "project": "[Pereira - Barros Comércio] - Mandatory fault-tolerant Graphical User Interface",
		                "cardDescription": "synthesize mobile protocol",
		                "gitMetadata": {
			                "branch": "array-synthesize",
			                "hash": "b16181f46f9318acd5ac50f760f30bf2428a28b4"
		                }
	                }
                ]

                Jira:

                [
	                {
		                "id": "0db2dfdc-a4b8-4055-b2ad-6655c06a7663",
		                "status": "PROD_DEPLOYING",
		                "user": {
			                "id": "659e4749-0237-4b89-9ad7-7402491b5bb5",
			                "avatar": "http://placeimg.com/640/480/people",
			                "first_name": "Gustavo",
			                "last_name": "Santos",
			                "email": "gustavo.santos@gsw.com.br"
		                },
		                "amountHours": 2.42,
		                "startedAt": "2020-04-25T21:01:17.288Z",
		                "finished": true,
		                "project": "[Souza Comércio e Associados] - Innovative background implementation",
		                "cardDescription": "index optical capacitor",
		                "gitMetadata": {
			                "branch": "microchip-parse",
			                "hash": "bb84719579164f7c68a171d224ae533c7471cd40"
		                }
	                }
                ]
                 */
            }
        }

        public class Item 
        {
            public string id;
            public string status;
            public string user;
            public List<List<string>> user_id;
            public string horas;
            public string inicio;
            public string fim;
            public string projeto;
            public string descrip;
            public List<List<string>> git;
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
                xProj.Add(
                    string.Concat(
                        data["id"].ToString(),
                        " - ",
                        data["projeto"].ToString()
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
            Panel.Children.Clear();
            CmdTask();
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
                }
                texto = string.Empty;
            }
        }
    }
}
