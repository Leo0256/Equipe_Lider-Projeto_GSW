using System;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NoteSystem
{
    /// <summary>
    /// Interação lógica para Funcionario.xam
    /// </summary>
    public partial class Funcionario : Page
    {

        private string PNome;
        private string UNome;

        public Funcionario(string xPNome, string xUNome, string xHoras, string xEmail, string xAvatar)
        {
            InitializeComponent();
            PNome = xPNome;
            UNome = xUNome;
            setNome();

            Horas.Content = "Horas: " + xHoras.Replace(",",":");
            Email.Content = "E-mail: " + xEmail;
            setAvatar(xAvatar);
        }

        private void setNome()
        {
            Nome.Text = string.Format("{0} {1}", PNome, UNome);
        }

        private void setAvatar(string url)
        {
            BitmapImage bitmap = new();

            bitmap.BeginInit();
            bitmap.UriSource = new Uri(url,UriKind.Absolute);
            bitmap.EndInit();

            Avatar.Source = bitmap;
        }

        public string getPNome()
        {
            return PNome;
        }

        public string getUNome()
        {
            return UNome;
        }

        public string getHoras()
        {
            return Horas.Content.ToString()[7..];
        }

        public string getEmail()
        {
            return Email.Content.ToString()[8..];
        }

        /*
        public string getAvatar()
        {
            return null;
        }
        */
    }
}
