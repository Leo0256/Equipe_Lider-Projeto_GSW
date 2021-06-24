using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NoteSystem
{
    public partial class Calendario : Page
    {
        private List<DateTime> markDates = new();
        private List<string[]> gridInfo = new();

        public Calendario(string[] data, string[] nome)
        {
            InitializeComponent();
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");
            calendar.SelectedDate = DateTime.Today;

            for(int x = 0; x < data.Length; x++)
            {
                markDates.Add(DateTime.Parse(data[x]));

                string[] var =
                {
                    data[x],
                    nome[x]
                };
                
                gridInfo.Add(var);
            }
            DateChange(DateTime.Today);
        }

        private void CalendarButton_Loaded(object sender, EventArgs e)
        {
            CalendarDayButton button = (CalendarDayButton)sender;
            DateTime date = (DateTime)button.DataContext;
            HighlightDay(button, date);
            button.DataContextChanged += new DependencyPropertyChangedEventHandler(CalendarButton_DataContextChanged);
        }

        private void HighlightDay(CalendarDayButton button, DateTime date)
        {
            if (markDates.Contains(date))
                button.Background = Brushes.LightSkyBlue;
            else
                button.Background = Brushes.White;
        }

        private void CalendarButton_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CalendarDayButton button = (CalendarDayButton)sender;
            DateTime date = (DateTime)button.DataContext;
            DateChange(date.AddMonths(-1));
            HighlightDay(button, date);
        }

        private void defineGrid(string data, string nome, int i)
        {
            var txt1 = setData(data);
            Grid.SetColumn(txt1, 0);
            Grid.SetRow(txt1, i);

            var txt2 = setData(nome);
            Grid.SetColumn(txt2, 1);
            Grid.SetRow(txt2, i);

            RowDefinition row = new();
            grid.RowDefinitions.Add(row);
            grid.RowDefinitions[i].Height = GridLength.Auto;

            grid.Children.Add(txt1);
            grid.Children.Add(txt2);
        }

        private TextBlock setData(string str)
        {
            TextBlock txt = new();

            txt.Text = str;
            txt.MaxWidth = 320;
            txt.FontFamily = new FontFamily("Arial");
            txt.FontSize = 25;
            txt.Foreground = new SolidColorBrush(Color.FromRgb(21, 21, 21));
            txt.HorizontalAlignment = HorizontalAlignment.Left;
            txt.VerticalAlignment = VerticalAlignment.Center;
            txt.Margin = new Thickness(5,0,5,2);
            txt.TextWrapping = TextWrapping.Wrap;

            return txt;
        }

        private void GetDateChange(object sender, CalendarDateChangedEventArgs e) => DateChange(sender);
        private void GetDateChange(object sender, SelectionChangedEventArgs e) => DateChange(sender);

        private void DateChange(object sender)
        {
            var mes = DateTime.Parse(sender.ToString()).Month;
            var ano = DateTime.Parse(sender.ToString()).Year;
            grid.Children.Clear();

            int index = 0;
            for (int x = 0; x < gridInfo.Count; x++) {
                var data = DateTime.Parse(gridInfo[x][0]);
                if (data.Month == mes && data.Year == ano)
                    defineGrid(gridInfo[x][0], gridInfo[x][1], index++);
            }
        }
    }
}
