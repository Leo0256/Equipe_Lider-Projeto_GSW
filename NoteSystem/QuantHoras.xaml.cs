using System.Globalization;
using System.Windows.Controls;

namespace NoteSystem
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
