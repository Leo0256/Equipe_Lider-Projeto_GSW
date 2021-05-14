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
    public partial class Dedicacao : Page
    {
        public Dedicacao(string[] xPNomes, string[] xUnomes, string[] xPercent)
        {
            InitializeComponent();

            for(int i = 0; i < xPNomes.Length; i++)
            {
                defineGrid(string.Concat(xPNomes[i], " ", xUnomes[i]), i);
            }
        }

        private void defineGrid(string nome, int i)
        {
            RowDefinition row = new();
            DataGrid.RowDefinitions.Add(row);
            DataGrid.RowDefinitions[i].Height = new GridLength(50);

            Border border = setColor();
            Grid.SetColumn(border, 0);
            Grid.SetRow(border, i);

            Label lbl = setData(nome,i);
            Grid.SetColumn(lbl, 1);
            Grid.SetRow(lbl, i);

            DataGrid.Children.Add(border);
            DataGrid.Children.Add(lbl);
        }

        private Border setColor()
        {
            Random r = new();
            Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),
                (byte)r.Next(1, 255), (byte)r.Next(1, 255)));

            Border border = new();

            border.Background = brush;
            border.CornerRadius = new CornerRadius(8);
            border.Margin = new Thickness(10);

            return border;
        }

        private Label setData(string nome, int row)
        {
            Label lbl = new();

            lbl.Content = nome;
            lbl.FontFamily = new FontFamily("Arial");
            lbl.FontSize = 26;
            lbl.Foreground = new SolidColorBrush(Color.FromRgb(21, 21, 21));
            lbl.HorizontalContentAlignment = HorizontalAlignment.Left;
            lbl.VerticalContentAlignment = VerticalAlignment.Center;

            return lbl;
        }
    }
}
