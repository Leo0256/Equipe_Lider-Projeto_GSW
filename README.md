# Sprint 4

## Objetivo

> **Finalizar o sistema, fazendo os teste de qualidade, verificação do layout e a leitura de novos dados em formato JSON.**

---
### Valores entregues
![Card4]()

---
### Gif do Sistema
![Gif]()

### MainWindow - Evento de Upload dos Dados
```C#


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
                    row.Add(conn.ExecuteCmd(
                        string.Format("select * from inserir_projeto('{0}'::json)", item[3])).Select());

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
```


### Classe de Normalização
```C#

using System.Collections.Generic;
using Newtonsoft.Json;

namespace NoteSystem
{
    class NormalizarJson
    {
        private List<Funcionario> func = new();
        private List<Projeto_Info> proj_info = new();
        private List<GitMetadata> git = new();
        private List<Projeto> projeto = new();

        public NormalizarJson(string json)
        {

            dynamic array = JsonConvert.DeserializeObject(json);
            foreach(var item in array)
            {
                // Jira
                if ((string)item.id != null)
                {
                    func.Add(
                        new (
                            (string)item.user.id,
                            (string)item.user.first_name,
                            (string)item.user.last_name,
                            (string)item.user.avatar,
                            (string)item.user.email));

                    proj_info.Add(
                        new(
                            (string)item.project,
                            (string)item.cardDescription,
                            (string)item.amountHours));

                    projeto.Add(
                        new(
                            (string)item.id,
                            (string)item.user.id,
                            (string)item.project,
                            (string)item.cardDescription,
                            (string)item.gitMetadata.hash,
                            (string)item.startedAt,
                            (string)item.status,
                            (string)item.finished));
                }
                // Trello
                else if((string)item._id != null)
                {
                    func.Add(
                        new(
                            (string)item.user._id,
                            (string)item.user.userName,
                            (string)item.user.userLastName,
                            (string)item.user.avatar,
                            (string)item.user.userEmail));

                    proj_info.Add(
                        new(
                            (string)item.project,
                            (string)item.cardDescription,
                            (string)item.hours));

                    projeto.Add(
                        new(
                            (string)item._id,
                            (string)item.user._id,
                            (string)item.project,
                            (string)item.cardDescription,
                            (string)item.gitMetadata.hash,
                            (string)item.startedAt,
                            (string)item.status,
                            (string)item.isFinished));
                }

                git.Add( 
                    new(
                        (string)item.gitMetadata.branch,
                        (string)item.gitMetadata.hash));
            }
        }
        public string[][] GetJson()
        {
            int count = projeto.Count;
            List<string[]> retorno = new();
            List<string> valor = new();

            for (int i = 0; i < count; i++)
            {
                valor.Add(JsonConvert.SerializeObject(func[i]));
                valor.Add(JsonConvert.SerializeObject(proj_info[i]));
                valor.Add(JsonConvert.SerializeObject(git[i]));
                valor.Add(JsonConvert.SerializeObject(projeto[i]));

                retorno.Add(valor.ToArray());
                valor.Clear();
            }
            return retorno.ToArray();
        }

        public class Funcionario
        {
            public string id_func;
            public string primeiro_nome;
            public string ultimo_nome;
            public string avatar;
            public string email;

            public Funcionario(
                string id_func,
                string primeiro_nome,
                string ultimo_nome,
                string avatar,
                string email)
            {
                this.id_func = id_func;
                this.primeiro_nome = primeiro_nome;
                this.ultimo_nome = ultimo_nome;
                this.avatar = avatar;
                this.email = email;
            }
        }

        public class Projeto_Info
        {
            public string nome;
            public string descr;
            public string horas;

            public Projeto_Info(string nome,string descr,string horas)
            {
                this.nome = nome;
                this.descr = descr;
                this.horas = horas;
            }
        }

        public class GitMetadata
        {
            public string branch;
            public string hash;

            public GitMetadata(string branch,string hash)
            {
                this.branch = branch;
                this.hash = hash;
            }
        }

        public class Projeto
        {
            public string id;
            public string id_func;

            public string nome_info;
            public string descr_info;

            public string hash_git;
            public string iniciado;
            public string status;
            public string finalizado;

            public Projeto(
                string id,
                string id_func,
                string nome_info,
                string descr_info,
                string hash_git,
                string iniciado,
                string status,
                string finalizado)
            {
                this.id = id;
                this.id_func = id_func;
                this.nome_info = nome_info;
                this.descr_info = descr_info;
                this.hash_git = hash_git;
                this.iniciado = iniciado;
                this.status = status;
                this.finalizado = finalizado;
            }
        }
    }
}
```

### Planilha de Teste

![Planilha_Testes]()

---
### Links

- [Figma](https://google.com)
- [Heroku](https://www.heroku.com/)
- [Códigos](https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/)

---
