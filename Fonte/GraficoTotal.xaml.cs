using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NoteSystem
{
    /// <summary>
    /// Interação lógica para TotalStatus.xam
    /// </summary>
    public partial class GraficoTotal : Page
    {
        public GraficoTotal(bool type, string[] param1, string[] param2)
        {
            InitializeComponent();

            if (type)
            {
                Nome.Content = "Status";
            }
            else
            {
                Nome.Content = "Projeto";
            }

            for(int i = 0; i < param1.Length; i++)
            {
                defineGrid(param1[i], param2[i], i);
                Divisor.Height += 110;
            }
        }

        private void defineGrid(string status, string total, int i)
        {
            RowDefinition row = new();
            DataGrid.RowDefinitions.Add(row);
            DataGrid.RowDefinitions[i].Height = new GridLength(120);

            Label lbl1 = setData(status);
            Grid.SetColumn(lbl1, 0);
            Grid.SetRow(lbl1, i);

            Label lbl2 = setData(total);
            Grid.SetColumn(lbl2, 1);
            Grid.SetRow(lbl2, i);

            DataGrid.Children.Add(lbl1);
            DataGrid.Children.Add(lbl2);
        }

        private Label setData(string str)
        {
            Label lbl = new();

            lbl.Content = str;
            lbl.MaxWidth = 390;
            lbl.FontFamily = new FontFamily("Arial");
            lbl.FontSize = 26;
            lbl.Foreground = new SolidColorBrush(Color.FromRgb(21, 21, 21));
            lbl.HorizontalAlignment = HorizontalAlignment.Left;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;

            return lbl;
        }
    }
}
