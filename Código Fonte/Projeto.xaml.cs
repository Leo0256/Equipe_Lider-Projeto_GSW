using System.Windows.Controls;

namespace NoteSystem
{
    public partial class Projeto : Page
    {
        public Projeto(string xNome, string xStatus, string xDescricao, string xData, string xFinalizado)
        {
            InitializeComponent();
            Nome.Text = xNome;
            Status.Content = xStatus;
            Desc.Text = xDescricao;
            Data.Content = xData;
            if (int.Parse(xFinalizado)==1)
                Fim.Content = "Finalizado: Sim";
            else
                Fim.Content = "Finalizado: Não";
        }

        public string getNome()
        {
            return Nome.Text.ToString();
        }

        public string getStatus()
        {
            return Status.Content.ToString();
        }
        public string getDescricaoe()
        {
            return Desc.Text;
        }
        public string getData()
        {
            return Data.Content.ToString();
        }

        public string getFinalizado()
        {
            return Fim.Content.ToString()[12..];
        }
    }
}
