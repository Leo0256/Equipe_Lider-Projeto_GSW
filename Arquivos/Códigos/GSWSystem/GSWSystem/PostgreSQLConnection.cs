using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Npgsql;
using System.Data;
using System.Windows;

namespace GSWSQL
{
    class PostgreSQLConnection
    {
        readonly static string connString = String.Format("Host={0};User id={1};Password={2};Database={3}",
            /* Host */ "127.0.0.1",
            /* User */ "postgres",
            /* Pass */ "0256067Lr@",
            /*  DB  */ "GSWSystem");

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