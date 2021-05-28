using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GSWSystem
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
        public string[] GetJson()
        {

            string r = JsonConvert.SerializeObject(func);

            string[] valor = { 
                JsonConvert.SerializeObject(func), 
                JsonConvert.SerializeObject(proj_info) ,
                JsonConvert.SerializeObject(git),
                JsonConvert.SerializeObject(projeto)
            };

            return valor;

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
            public string iniciado;
            public string status;
            public string finalizado;

            public Projeto(string id,string iniciado,string status,string finalizado)
            {
                this.id = id;
                this.iniciado = iniciado;
                this.status = status;
                this.finalizado = finalizado;
            }
        }
    }
}
