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
        private List<double> graphHeight = new();

        public RankHoras(string titulo, string[] nome, double[] horas)
        {
            InitializeComponent();

            this.titulo.Content = titulo;

            foreach(var val in horas)
                graphHeight.Add(val * 100 / horas[0]);

            Random r = new();
            Brush brush;
            for (int x = 0; x < nome.Length; x++)
            {
                brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255), 
                    (byte)r.Next(1, 255), (byte)r.Next(1, 255)));

                SetGrafico(graphHeight[x], horas[x], brush);
                SetLegenda(nome[x], brush, x);
            }
        }

        private void SetGrafico(double tamanho, double valor , Brush brush)
        {
            grafico.Children.Add(
                new BarChart() {
                    Color = brush,
                    Value = tamanho,
                    MaxValue = 100,
                    ValueBar = valor,
                    Height = 250,
                    Margin = new Thickness(5)
                });
        }

        private void SetValor(double valor, double y)
        {
            y = y < -12 ? 
                -12 : y;

            valores.Children.Add(
                new Label() {
                    Content = valor,
                    HorizontalAlignment = HorizontalAlignment.Right,
                    Foreground = Brushes.White,
                    FontSize = 10,
                    Margin = new Thickness(0,0,0,y)
                });
        }

        private void SetLegenda(string txt, Brush brush, int i)
        {
            RowDefinition row = new();
            legenda.RowDefinitions.Add(row);
            legenda.RowDefinitions[i].Height = new GridLength(1, GridUnitType.Auto);

            Border border = new()
            {
                Background = brush,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(8),
                Margin = new Thickness(8),
                Height = 20,
                Width = 20
            };

            TextBlock block = new()
            {
                Text = txt,
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 20,
                MaxWidth = 360,
                Width = 360,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(8, 10, 8, 10)
            };

            Grid.SetColumn(border, 0);
            Grid.SetRow(border, i);

            Grid.SetColumn(block, 1);
            Grid.SetRow(block, i);

            legenda.Children.Add(border);
            legenda.Children.Add(block);
        }
    }
}
