# Sprint de Recuperação
## Objetivo :dart:
    Implementar a visualização das datas de inicio das tasks (tarefas) e 2 (dois) gráficos contendo:
    - Rank de horas dos funcionarios em geral e por projeto específico;
    - Rank de horas dos projetos.

## Backlog :clipboard:
<p align="center">
  <img src="https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/blob/Sprint_Rec/Card%20Rec.png">
</p>

## Gif das Funcionalidades Implementadas :movie_camera:
<p align="center">
    <img width="625.6" height="351.4" src="https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/blob/Sprint_Rec/gif/calendario.gif"><br>
    <img width="625.6" height="351.4" src="http://placeimg.com/625/351/arch">
</p>

## Sistema :scroll:
### Calendario.xaml.cs
```c#
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
```
### Calendario.xaml
```xaml
<Page x:Class="NoteSystem.Calendario"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
      xmlns:local="clr-namespace:NoteSystem"
      
      mc:Ignorable="d" 
      Title="Calendario"
      Width="936" 
      MinHeight="250" MaxHeight="400">

    <Page.Resources>
        <DropShadowEffect 
                    x:Key="dropShadow" 
                    BlurRadius="20" 
                    Direction="270"
                    Color="Black"
                    />
    </Page.Resources>

    <Border Background="white" BorderBrush="Black" BorderThickness="1" CornerRadius="8,8,8,8">
        <Grid Margin="0,0,0,10">
            <StackPanel Margin="40" Orientation="Horizontal">

                <Calendar 
                    x:Name="calendar" 
                    IsTodayHighlighted="False"
                    FontFamily="Arial"
                    Language="pt-BR"
                    MinWidth="270"
                    Margin="10"
                    SelectedDatesChanged="GetDateChange"
                    DisplayDateChanged="GetDateChange"
                    Effect="{StaticResource dropShadow}">
                    
                    <Calendar.CalendarDayButtonStyle>
                        <Style TargetType="CalendarDayButton" BasedOn="{StaticResource {x:Type CalendarDayButton}}">
                            <EventSetter Event="Loaded" Handler="CalendarButton_Loaded" />
                        </Style>
                    </Calendar.CalendarDayButtonStyle>
                </Calendar>

                <Border 
                    BorderBrush="Black" 
                    BorderThickness="1" 
                    CornerRadius="2"
                    Background="GhostWhite"
                    Margin="20,15"
                    Width="500" 
                    VerticalAlignment="Top"
                    >
                    <ScrollViewer 
                        VerticalScrollBarVisibility="Auto" 
                        MaxHeight="300" 
                        MinHeight="50" >
                        
                        <Grid 
                            x:Name="grid" 
                            Width="Auto"
                            VerticalAlignment="Top"
                            >
                            
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="Auto"/>
                                <ColumnDefinition Width="Auto"/>
                            </Grid.ColumnDefinitions>
                        </Grid>
                    </ScrollViewer>
                </Border>
            </StackPanel>
        </Grid>
    </Border>
</Page>
```
### RankHoras.xaml.cs
```c#
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
```
### RankHoras.xaml
```xaml
<Page x:Class="NoteSystem.RankHoras"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
      xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
      xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
      xmlns:local="clr-namespace:NoteSystem"
      mc:Ignorable="d" 
      Title="RankFunc"
      Width="936" 
      MinHeight="250" MaxHeight="800">


    <Border Background="white" BorderBrush="Black" BorderThickness="1" CornerRadius="8,8,8,8">
        <Grid Margin="0,0,0,10">

            <Border
                Background="#FF252525"
                CornerRadius="5"
                Margin="10"
                BorderThickness="2"
                BorderBrush="Black">

                <StackPanel Orientation="Vertical">

                    <Label
                        x:Name="titulo"
                        d:Content="Título"
                        Foreground="White"
                        FontSize="22"
                        FontWeight="DemiBold"
                        FontFamily="Arial"
                        HorizontalAlignment="Center"/>

                    <Rectangle
                        Fill="#FF252525"
                        Width="Auto"
                        Height="3">

                        <Rectangle.Effect>
                            <DropShadowEffect
                                BlurRadius="3"
                                ShadowDepth="2.2"
                                Color="Black" 
                                Direction="270"/>
                        </Rectangle.Effect>

                    </Rectangle>

                    <Grid x:Name="grafico" VerticalAlignment="Top" Margin="5" d:Height="300">
                        <Grid.RowDefinitions>
                            <RowDefinition Height="Auto"/>
                            <RowDefinition Height="Auto"/>
                        </Grid.RowDefinitions>
                    </Grid>

                </StackPanel>
            </Border>
        </Grid>
    </Border>
</Page>
```
### MainWindow.xaml.cs [442 - 540]
```c#
private void CmdCalendario()
{
    string sql = string.Format(@"select * from pesquisa_datas_projeto()");
    DataRow[] row = conn.ExecuteCmd(sql).Select();

    List<string> xData = new(),
                 xProj = new();
    foreach(var data in row)
    {
        xData.Add(data["data"].ToString()[..10]);
        xProj.Add(data["nome"].ToString());
    }

    Panel.Children.Clear();
    Panel.Children.Add(
        new Frame
        {
            Content = new Calendario(xData.ToArray(), xProj.ToArray())
        }
    );
}

private void ViewCalendarioTasks(object sender, RoutedEventArgs e)
{
    Panel.Children.Clear();
    CmdCalendario();
}

private void CmdRankFunc()
{
    string sql = string.Format(@"select * from pesquisa_rank_func('{0}')",texto);
    DataRow[] row = conn.ExecuteCmd(sql).Select();

    List<string> Nome = new();
    List<double> Horas = new();

    foreach (var data in row)
    {
        Nome.Add(string.Format("{0}\n{1}",
            data["pnome"].ToString(), 
            data["unome"].ToString()));

        Horas.Add(double.Parse(data["horas"].ToString()));
    }
    string xProj = texto.Equals(string.Empty) ? 
        "Rank de Horas dos Funcionarios" :
        row[0]["proj"].ToString();

    Panel.Children.Clear();
    Panel.Children.Add(
        new Frame
        {
            Content = new RankHoras(
                xProj,
                Nome.ToArray(),
                Horas.ToArray())
        }
    );
}

private void ViewRankFunc(object sender, RoutedEventArgs e)
{
    function = "rank";

    Panel.Children.Clear();
    CmdRankFunc();
}

private void CmdRankProj()
{
    string sql = string.Format(@"select * from pesquisa_rank_proj()");
    DataRow[] row = conn.ExecuteCmd(sql).Select();

    List<string> xProj = new();
    List<double> Horas = new();

    foreach (var data in row)
    {
        xProj.Add(data["nome"].ToString());
        Horas.Add(double.Parse(data["horas"].ToString()));
    }

    Panel.Children.Clear();
    Panel.Children.Add(
        new Frame
        {
            Content = new RankHoras(
                "Rank de Horas dos Projetos",
                xProj.ToArray(),
                Horas.ToArray())
        }
    );
} 

private void ViewRankProj(object sender, RoutedEventArgs e)
{
    Panel.Children.Clear();
    CmdRankProj();
}
```
### BarChart.xaml.cs
```c#
public partial class BarChart : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private void NotifyPropertyChanged(string info) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));

    private double _value;
    public double Value
    {
        get { return _value; }
        set
        {
            _value = value;
            UpdateBarHeight();
            NotifyPropertyChanged("Value");
        }
    }

    private double maxValue;
    public double MaxValue
    {
        get { return maxValue; }
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
        get { return barHeight; }
        private set
        {
            barHeight = value;
            NotifyPropertyChanged("BarHeight");
        }
    }

    private Brush color;
    public Brush Color
    {
        get { return color; }
        set
        {
            color = value;
            NotifyPropertyChanged("Color");
        }
    }

    private void UpdateBarHeight()
    {
        BarHeight = _value < 100 ? _value * 8 : _value;
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
```
### BarChart.xaml
```xaml
<UserControl x:Class="NoteSystem.BarChart"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
    
    mc:Ignorable="d" 
    MinWidth="20"
    Width="auto"
    Loaded="UserControl_Loaded">

    <Grid SizeChanged="Grid_SizeChange">

        <Border 
            x:Name="border" 
            Background="{Binding Color}" 
            VerticalAlignment="Bottom" 
            Height="{Binding BarHeight}"
            CornerRadius="5,5,0,0"
            BorderThickness="1"
            BorderBrush="Black"
            
            d:Height="50"
            d:Background="YellowGreen"/>

        <TextBlock
            Text="{Binding Value}"
            VerticalAlignment="Bottom"
            HorizontalAlignment="Center"
            Background="GhostWhite"
            Padding="1,0"
            Margin="3"
            FontSize="10"
            FontWeight="Bold"
            
            d:Text="0.0" />
    </Grid>
</UserControl>
```
### Banco de Dados.sql
#### [199 - 216]
```sql
create or replace function pesquisa_datas_projeto () 
returns table (
	nome varchar,
	data timestamp
) language plpgsql
as $$
begin
	return query 
		select
			projeto_info.nome,
			projeto.iniciado as a
		from projeto 
			inner join projeto_info
				on projeto.id_info = projeto_info.id_info
		
		order by a asc;

end $$;
```
#### [485 - 533]
```sql
create or replace function pesquisa_rank_func (xproj varchar) 
returns table (
	pnome varchar,
	unome varchar,
	proj varchar,
	horas numeric
) language plpgsql
as $$
begin
	return query 
		select 
			funcionarios.primeiro_nome pnome, 
			funcionarios.ultimo_nome unome,
			projeto_info.nome nome, 
			sum(projeto_info.horas) thoras 
		from funcionarios
			inner join projeto
				on funcionarios.id_func = projeto.id_func
			inner join projeto_info
				on projeto_info.id_info = projeto.id_info
		where 
			projeto_info.horas is not null
		and 
			nome ilike concat('%',xproj,'%')
		group by pnome, unome, nome
		order by 
		(
			case when xproj not like '' 
			then 
				nome
			else null
			end 
		) asc, thoras desc
		limit 10;
end $$;


create or replace function pesquisa_rank_proj () 
returns table (
	nome varchar,
	horas numeric
) language plpgsql
as $$
begin
	return query 
		select * from pesquisa_horas_projeto('')
		order by horas desc
		limit 8;
end $$;
```
## Gifs Demonstrativos do Sistema :movie_camera:

<p align="center">
    <img width="625.6" height="351.4" src="https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/blob/Sprint_Rec/gif/insert-proj.gif"><br>
    <img width="625.6" height="351.4" src="https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/blob/Sprint_Rec/gif/func.gif"><br>
    <img width="625.6" height="351.4" src="https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/blob/Sprint_Rec/gif/horas_proj-ano.gif"><br>
    <img width="625.6" height="351.4" src="https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/blob/Sprint_Rec/gif/horas-mes.gif"><br>
    <img width="625.6" height="351.4" src="https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/blob/Sprint_Rec/gif/dedica%C3%A7%C3%A3o.gif"><br>
    <img width="625.6" height="351.4" src="https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/blob/Sprint_Rec/gif/status-task.gif"><br>
    
    
</p>

## Burndown :chart_with_downwards_trend:
<p align="center">
  <img width="800" height="343" src="http://placeimg.com/800/343/arch">
</p>

## Links

[![](https://img.shields.io/badge/Figma-150485?style=for-the-badge&logo=figma&logoColor=white&labelColor=F24E1E)](https://www.figma.com/file/JNHBHcGorHxBcx50m3nuGg/GSW-projeto-2 "Wireframes iniciais")
[![](https://img.shields.io/badge/Fonte-007ACC?style=for-the-badge&logo=visual%20studio%20code&logoColor=white&labelColor=#007ACC)](https://github.com/Leo0256/Equipe_Lider-Projeto_GSW/tree/sistema "Código Fonte")











