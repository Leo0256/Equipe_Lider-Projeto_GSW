using System;
using Npgsql;
using System.Data;
using System.Windows;

namespace NoteSystem
{
    class PostgreSQLConnection
    {
        
        
        readonly static string connString = String.Format(
            "Server={0}; Database={1}; User id={2}; Password={3}; SslMode=Require; Trust Server Certificate=true",
            /* Host */ "ec2-54-91-188-254.compute-1.amazonaws.com",
            /*  DB  */ "dctk4pu52nq3m8",
            /* User */ "cudcngngacdtgr",
            /* Pass */ "0bc28e311e588e8d900d1ba0432ea4de31efb665ef700bb1324ac58e16a771e6");
        

        NpgsqlConnection conn;


        public PostgreSQLConnection()
        {
            conn = new NpgsqlConnection(connString);
        }

        public DataTable ExecuteCmd(string query)
        {
            DataTable table = new();

            try
            {
                using (conn = new NpgsqlConnection(connString))
                {
                    conn.Open();
                    using NpgsqlDataAdapter Adpt = new(query, conn);
                    Adpt.Fill(table);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na Execução: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

            return table;
        }

        public void Open()
        {
            try
            {
                conn.Open();

            }
            catch (Exception ex)
            {
                Close();
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        public void Close()
        {
            conn.Close();
        }
    }

}