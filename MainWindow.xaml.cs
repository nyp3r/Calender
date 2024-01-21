using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using System.Media;
using System.Reflection;

namespace Nyp3rCalender
{
    public partial class MainWindow : Window
    {
        private int monthIndex = 0;
        private List<Month> months = new List<Month>();
        private List<Event> events = new List<Event>();
        readonly string[] weekDayAbrevs = { "Mo", "Tu", "We", "Th", "Fr", "Sa", "Su" };
        public MainWindow()
        {
            months.Add(new Month("January", 31, 0));
            months.Add(new Month("February", 29, 3));
            months.Add(new Month("March", 31, 4));
            months.Add(new Month("April", 30, 0));
            months.Add(new Month("May", 31, 2));
            months.Add(new Month("June", 30, 5));
            months.Add(new Month("July", 31, 0));
            months.Add(new Month("August", 31, 3));
            months.Add(new Month("September", 30, 6));
            months.Add(new Month("October", 31, 1));
            months.Add(new Month("November", 30, 4));
            months.Add(new Month("December", 31, 6));

            InitializeComponent();
            RefreshWindow();
        }

        public void NextMonth(object sender, RoutedEventArgs e)
        {
            monthIndex = (monthIndex < months.Count - 1) ? monthIndex + 1 : 0;
            RefreshWindow();
        }
        public void PreviousMonth(object sender, RoutedEventArgs e)
        {
            monthIndex = (monthIndex > 0) ? monthIndex - 1 : months.Count - 1;
            RefreshWindow();
        }

        private void RefreshWindow()
        {
            events = Event.Load();
            string imagePath = "C:\\Users\\BjarkeJJ\\source\\repos\\Nyp3rCalender\\Images\\";
            monthName.Text = months[monthIndex].Name;
            daysGrid.Children.Clear();

            int r = 0;
            int c = months[monthIndex].WeekDayStartIndex;
            for (int i = 0; i < months[monthIndex].DayCount; i++, c++)
            {
                if (c > 6) { r++; c = 0; }

                TextBlock weekDay = new TextBlock();
                weekDay.FontSize = 25;
                weekDay.HorizontalAlignment = HorizontalAlignment.Center;
                weekDay.IsHitTestVisible = false;

                TextBlock dayNumber = new TextBlock();
                dayNumber.FontSize = 40;
                dayNumber.HorizontalAlignment = HorizontalAlignment.Center;
                dayNumber.IsHitTestVisible = false;

                BitmapImage source = new BitmapImage(new Uri(imagePath + "ellipsis.png"));
                Image ellipsis = new Image();
                ellipsis.Height = 20;
                ellipsis.Width = 20;
                ellipsis.Source = source;
                ellipsis.Visibility = Visibility.Hidden;
                Grid.SetRow(ellipsis,2);
                Grid.SetColumn(ellipsis,2);

                int thickness = 1;
                System.Drawing.Color strokeColor = System.Drawing.Color.LightGray;
                Ellipse eventIndicator1 = new Ellipse();
                Ellipse eventIndicator2 = new Ellipse();
                Ellipse eventIndicator3 = new Ellipse();
                Ellipse eventIndicator4 = new Ellipse();
                eventIndicator1.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(strokeColor.A, strokeColor.R, strokeColor.G, strokeColor.B));
                eventIndicator2.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(strokeColor.A, strokeColor.R, strokeColor.G, strokeColor.B));
                eventIndicator3.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(strokeColor.A, strokeColor.R, strokeColor.G, strokeColor.B));
                eventIndicator4.Stroke = new SolidColorBrush(System.Windows.Media.Color.FromArgb(strokeColor.A, strokeColor.R, strokeColor.G, strokeColor.B));
                eventIndicator1.StrokeThickness = thickness;
                eventIndicator2.StrokeThickness = thickness;
                eventIndicator3.StrokeThickness = thickness;
                eventIndicator4.StrokeThickness = thickness;
                eventIndicator1.Margin = new Thickness(1,1,1,1);
                eventIndicator2.Margin = new Thickness(1,1,1,1);
                eventIndicator3.Margin = new Thickness(1,1,1,1);
                eventIndicator4.Margin = new Thickness(1,1,1,1);
                eventIndicator1.IsHitTestVisible = false;
                eventIndicator2.IsHitTestVisible = false;
                eventIndicator3.IsHitTestVisible = false;
                eventIndicator4.IsHitTestVisible = false;
                Grid.SetRow(eventIndicator1 , 0); Grid.SetColumn(eventIndicator1, 0);
                Grid.SetRow(eventIndicator2, 0); Grid.SetColumn(eventIndicator2, 1);
                Grid.SetRow(eventIndicator3, 1); Grid.SetColumn(eventIndicator3, 0);
                Grid.SetRow(eventIndicator4, 1); Grid.SetColumn(eventIndicator4, 1);
                if (events.Count > 0)
                {
                    int evNum = 1;
                    foreach (var ev in events)
                    {
                        int eventDay = i + 1;
                        int eventMonth = monthIndex + 1;
                        if(ev.StartDateTime.Day == eventDay && ev.StartDateTime.Month == eventMonth)
                        {
                            switch (evNum)
                            {
                                case 1:
                                    eventIndicator1.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                                    ev.Color.A, ev.Color.R, ev.Color.G, ev.Color.B));
                                    break;

                                case 2:
                                    eventIndicator2.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                                    ev.Color.A, ev.Color.R, ev.Color.G, ev.Color.B));
                                    break;

                                case 3:
                                    eventIndicator3.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                                    ev.Color.A, ev.Color.R, ev.Color.G, ev.Color.B));
                                    break;

                                case 4:
                                    eventIndicator4.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                                    ev.Color.A, ev.Color.R, ev.Color.G, ev.Color.B));
                                    break;

                                case 5:
                                    ellipsis.Visibility = Visibility.Visible;
                                    break;

                                default:
                                    break;
                            }
                            evNum++;
                        } 
                    }
                }

                Grid indicators = new Grid();
                Grid.SetRow(indicators, 1); Grid.SetColumn(indicators, 2);
                for (int å = 0; å < 2; å++)
                {
                    indicators.ColumnDefinitions.Add(new ColumnDefinition());
                    indicators.RowDefinitions.Add(new RowDefinition());
                }
                indicators.Children.Add(eventIndicator1);
                indicators.Children.Add(eventIndicator2);
                indicators.Children.Add(eventIndicator3);
                indicators.Children.Add(eventIndicator4);


                Button dayButton = new Button();
                dayButton.Background = Brushes.Transparent;
                dayButton.Tag = i + 1;
                dayButton.Click += DateClick;

                Grid monthDay = new Grid();
                for (int å = 0; å < 3; å++)
                {
                    monthDay.ColumnDefinitions.Add(new ColumnDefinition());
                    monthDay.RowDefinitions.Add(new RowDefinition());
                }

                Grid.SetColumnSpan(dayNumber, 2);
                Grid.SetRowSpan(dayNumber, 3);
                Grid.SetColumn(weekDay, 2);
                Grid.SetColumnSpan(dayButton, 3);
                Grid.SetRowSpan(dayButton, 3);

                weekDay.Text = weekDayAbrevs[c];
                dayNumber.Text = Convert.ToString((int)dayButton.Tag);

                monthDay.Children.Add(dayButton);
                monthDay.Children.Add(weekDay);
                monthDay.Children.Add(dayNumber);
                monthDay.Children.Add(indicators);
                monthDay.Children.Add(ellipsis);

                Grid.SetColumn(monthDay, c);
                Grid.SetRow(monthDay, r);

                daysGrid.Children.Add(monthDay);
            }
        }

        public void DateClick(object sender, EventArgs e)
        {
            int day = Convert.ToInt16((sender as Button).Tag);
            DateWindow dateWindow = new DateWindow(day, monthIndex + 1);
            dateWindow.Closed += DateWindowClosed;
            dateWindow.ShowDialog();
        }

        public void DateWindowClosed(object sender, EventArgs e)
        {
            RefreshWindow();
        }
    }
}