using System;
using System.Collections.Generic;
using System.Globalization;
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
        public QuantHoras(string Type, string xNome, string xHoras, string comp)
        {
            InitializeComponent();
            tag = string.Empty;
            switch (Type)
            {
                case "ano":
                    tag = "Ano: " + xNome;
                    break;

                case "mes":
                    var mes = DateTimeFormatInfo.CurrentInfo.GetMonthName(int.Parse(xNome));
                    tag = char.ToUpper(mes[0]) + mes[1..] + " / " + comp;
                    break;

                default:
                    tag = xNome;
                    break;

            }
            
            Nome.Text = tag;
            Horas.Content = "Horas: " + xHoras.Replace(",", ":");
        }
    }
}
