using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public partial class BarChart : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string info) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

        private double value;
        public double Value
        {
            get => value;
            set
            {
                this.value = value;
                UpdateBarHeight();
                NotifyPropertyChanged("Value");
            }
        }

        private double maxValue;
        public double MaxValue
        {
            get => maxValue; 
            set
            {
                maxValue = value;
                UpdateBarHeight();
                NotifyPropertyChanged("MaxValue");
            }
        }

        private double barHeight;
        public double BarHeight
        {
            get => barHeight;
            private set
            {
                barHeight = value;
                NotifyPropertyChanged("BarHeight");
            }
        }

        private Brush color;
        public Brush Color
        {
            get => color; 
            set
            {
                color = value;
                NotifyPropertyChanged("Color");
            }
        }

        private double valueBar;
        public double ValueBar
        {
            get => valueBar;
            set => valueBar = value;
        }

        private void UpdateBarHeight()
        {
            //BarHeight = _value < 100 ? _value * 8 : _value;
            
            if (maxValue > 0)
            {
                var percent = value * 100 / maxValue;
                BarHeight = percent * ActualHeight / 100;
            }
            
        }

        public BarChart()
        {
            InitializeComponent();
            this.DataContext = this;
            Color = Brushes.DarkGray;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateBarHeight();
        }

        private void Grid_SizeChange(object sender, SizeChangedEventArgs e)
        {
            UpdateBarHeight();
        }
    }
}
