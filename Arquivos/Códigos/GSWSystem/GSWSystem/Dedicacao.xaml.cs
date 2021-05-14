using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                defineGrid(string.Concat(xPNomes[i], " ", xUnomes[i]),xPercent[i], i);
            }
            setChart();
        }

        private void defineGrid(string nome, string percent, int i)
        {
            RowDefinition row = new();
            DataGrid.RowDefinitions.Add(row);
            DataGrid.RowDefinitions[i].Height = new GridLength(50);

            Border border = setBorderGraph(percent);
            Grid.SetColumn(border, 0);
            Grid.SetRow(border, i);

            Label lbl = setData(nome);
            Grid.SetColumn(lbl, 1);
            Grid.SetRow(lbl, i);

            DataGrid.Children.Add(border);
            DataGrid.Children.Add(lbl);
        }

        private Border setBorderGraph(string percent)
        {
            Random r = new();
            Brush brush = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),
                (byte)r.Next(1, 255), (byte)r.Next(1, 255)));

            Categories.Add(
                new Category 
                { 
                    Percentage = float.Parse(percent),
                    ColorBrush = brush
                });

            Border border = new();

            border.Background = brush;
            border.CornerRadius = new CornerRadius(8);
            border.Margin = new Thickness(10);

            return border;
        }

        private Label setData(string nome)
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

        /*
         * Códigos que cria gráficos do tipo "Pie Chart"
         * código fonte: https://github.com/kareemsulthan07/Charts 
         */
        private List<Category> Categories { get; set; } = new();

        public class Category
        {
            public float Percentage { get; set; }
            public Brush ColorBrush { get; set; }
        }

        private void setChart()
        {
            float pieWidth = 400,
                  pieHeight = 400,
                  centerX = pieWidth / 2,
                  centerY = pieHeight / 2,
                  radius = pieWidth / 2;

            mainCanvas.Width = pieWidth;
            mainCanvas.Height = pieHeight;


            float angle = 0, prevAngle = 0;
            foreach (var category in Categories)
            {
                double line1X = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double line1Y = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                angle = category.Percentage * (float)360 / 100 + prevAngle;
                Debug.WriteLine(angle);

                double arcX = (radius * Math.Cos(angle * Math.PI / 180)) + centerX;
                double arcY = (radius * Math.Sin(angle * Math.PI / 180)) + centerY;

                var line1Segment = new LineSegment(new Point(line1X, line1Y), false);
                double arcWidth = radius, arcHeight = radius;
                bool isLargeArc = category.Percentage > 50;
                var arcSegment = new ArcSegment()
                {
                    Size = new Size(arcWidth, arcHeight),
                    Point = new Point(arcX, arcY),
                    SweepDirection = SweepDirection.Clockwise,
                    IsLargeArc = isLargeArc,
                };
                var line2Segment = new LineSegment(new Point(centerX, centerY), false);

                var pathFigure = new PathFigure(
                    new Point(centerX, centerY),
                    new List<PathSegment>()
                    {
                        line1Segment,
                        arcSegment,
                        line2Segment,
                    },
                    true);

                var pathFigures = new List<PathFigure>() { pathFigure, };
                var pathGeometry = new PathGeometry(pathFigures);
                var path = new Path()
                {
                    Fill = category.ColorBrush,
                    Data = pathGeometry,
                };
                mainCanvas.Children.Add(path);

                prevAngle = angle;

                var outline1 = new Line()
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = line1Segment.Point.X,
                    Y2 = line1Segment.Point.Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                };
                var outline2 = new Line()
                {
                    X1 = centerX,
                    Y1 = centerY,
                    X2 = arcSegment.Point.X,
                    Y2 = arcSegment.Point.Y,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1,
                };

                mainCanvas.Children.Add(outline1);
                mainCanvas.Children.Add(outline2);
            }
        }

        
    }
}
