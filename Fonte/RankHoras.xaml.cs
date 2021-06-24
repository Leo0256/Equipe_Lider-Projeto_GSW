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

namespace NoteSystem
{
    public partial class RankHoras : Page
    {
        public RankHoras(string titulo, string[] nome, double[] horas)
        {
            InitializeComponent();

            this.titulo.Content = titulo;

            for (int x = 0; x < nome.Length; x++)
            {
                defineGrid(
                    nome[x],
                    horas[x],
                    x
                );
            }
        }

        private void defineGrid(string txt, double value, int i)
        {
            ColumnDefinition col = new();
            grafico.ColumnDefinitions.Add(col);
            grafico.ColumnDefinitions[i].Width = new GridLength(1, GridUnitType.Star);

            BarChart chart = new()
            {
                Color = (SolidColorBrush) new BrushConverter().ConvertFrom("#FF0F3460"),
                Value = value,
                Margin = new Thickness(5),
                VerticalAlignment = VerticalAlignment.Bottom,
                Height = 200,
                Width = 35
            };

            TextBlock block = new()
            {
                Text = txt,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                FontSize = 16,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(8,5,8,5)
            };

            Grid.SetColumn(chart, i);
            Grid.SetRow(chart, 0);

            Grid.SetColumn(block, i);
            Grid.SetRow(block, 1);

            grafico.Children.Add(chart);
            grafico.Children.Add(block);
        }
    }
}
