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
        private Funcionario func;
        private Projeto_Info proj_info;
        private GitMetadata git;
        private Projeto projeto;

        public NormalizarJson(string json)
        {
            dynamic array = JsonConvert.DeserializeObject(json);
            foreach(var item in array)
            {
                // Jira
                if (item.id != string.Empty)
                {
                    func =
                        new(
                            item.user.id,
                            item.user.avatar,
                            item.user.first_name,
                            item.user.last_name,
                            item.user.email
                        );
                    proj_info =
                        new(
                            item.project,
                            item.cardDescription,
                            item.amountHours
                        );
                    git =
                        new(
                            item.gitMetadata.branch,
                            item.gitMetadata.hash
                        );
                    projeto =
                        new(
                            item.id,
                            item.startedAt,
                            item.status,
                            item.finished
                        );
                }
                // Trello
                else if(item._id != string.Empty)
                {
                    func =
                        new(
                            item.user._id,
                            item.user.avatar,
                            item.user.userName,
                            item.user.userLastName,
                            item.user.userEmail
                        );
                    proj_info =
                        new(
                            item.project,
                            item.cardDescription,
                            item.hours
                        );
                    git =
                        new(
                            item.gitMetadata.branch,
                            item.gitMetadata.hash
                        );
                    projeto =
                        new(
                            item._id,
                            item.startedAt,
                            item.status,
                            item.isFinished
                        );
                }

            }
        }
        public string GetJson()
        {
            
            
            string r = JsonConvert.SerializeObject(func);

            return r;
        }

        public class Funcionario
        {
            private string id_func;
            private string primeiro_nome;
            private string ultimo_nome;
            private string avatar;
            private string email;

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
            private string nome;
            private string descr;
            private string horas;

            public Projeto_Info(string nome,string descr,string horas)
            {
                this.nome = nome;
                this.descr = descr;
                this.horas = horas;
            }
        }

        public class GitMetadata
        {
            private string branch;
            private string hash;

            public GitMetadata(string branch, string hash)
            {
                this.branch = branch;
                this.hash = hash;
            }
        }

        public class Projeto
        {
            private string id;
            private string iniciado;
            private string status;
            private string finalizado;

            public Projeto(
                string id,
                string iniciado,
                string status,
                string finalizado)
            {
                this.id = id;
                this.iniciado = iniciado;
                this.status = status;
                this.finalizado = finalizado;
            }
        }
    }
}
