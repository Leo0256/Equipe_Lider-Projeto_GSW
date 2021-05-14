using System;
using System.Collections.Generic;
using System.Linq;
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

namespace GSWSystem
{
    /// <summary>
    /// Interação lógica para QuantHoras.xam
    /// </summary>
    public partial class QuantHoras : Page
    {
        private string tag;
        public QuantHoras(string Type, string xNome, string xHoras)
        {
            InitializeComponent();
            setNome(Type, xNome);
            Horas.Content = "Horas: " + xHoras.Replace(".", ":");
        }

        private void setNome(string Type, string xNome)
        {
            tag = string.Empty;
            switch (Type)
            {
                case "ano":
                    tag = "Ano: ";
                    break;

                case "mes":
                    tag = "Mês: ";
                    break;

                default:
                    break;

            }
            Nome.Text = tag + xNome;
        }

        public string getNome()
        {
            if (tag != string.Empty)
            {
                return Nome.Text.ToString();
            }
            else
            {
                return Nome.Text.ToString()[5..];
            }
        }
    }
}
