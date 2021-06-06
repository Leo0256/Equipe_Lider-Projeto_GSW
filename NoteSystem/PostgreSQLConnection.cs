using System;
using Npgsql;
using System.Data;
using System.Windows;

namespace NoteSystem
{
    class PostgreSQLConnection
    {
        readonly static string connString = String.Format(
            "Host={0};User id={1};Password={2};Database={3}",
            /* Host */ "ec2-54-91-188-254.compute-1.amazonaws.com",
            /* Port */ "5432",
            /* User */ "cudcngngacdtgr",
            /* Pass */ "0bc28e311e588e8d900d1ba0432ea4de31efb665ef700bb1324ac58e16a771e6",
            /*  DB  */ "dctk4pu52nq3m8");

        NpgsqlConnection conn;
        //NpgsqlCommand cmd;


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