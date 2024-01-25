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
using Windows.Devices.PointOfService;

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

                List<Event> dayEvents = new();
                DateTime currentDay = new DateTime(2024,monthIndex+1,i+1);
                if (events != null)
                {
                    foreach (var ev in events)
                    {
                        List<DateTime> daysBetween = GetDaysBetween(ev.StartDateTime,ev.EndDateTime);
                        for (int j = 0; j < daysBetween.Count; j++)
                        {
                            if (daysBetween[j].Day == currentDay.Day && daysBetween[j].Month == currentDay.Month)
                            {
                                dayEvents.Add(ev);
                            }
                        }
                    } 
                }

                TextBlock weekDay = new TextBlock();
                weekDay.FontSize = 25;
                weekDay.HorizontalAlignment = HorizontalAlignment.Center;
                weekDay.IsHitTestVisible = false;

                TextBlock dayNumber = new TextBlock();
                dayNumber.FontSize = 40;
                dayNumber.HorizontalAlignment = HorizontalAlignment.Center;
                dayNumber.IsHitTestVisible = false;

                BitmapImage source = new BitmapImage(new Uri(imagePath + "ellipsisU.png"));
                System.Windows.Controls.Image ellipsis = new();
                ellipsis.Height = 20;
                ellipsis.Width = 20;
                ellipsis.Source = source;
                ellipsis.Visibility = Visibility.Hidden;
                ellipsis.VerticalAlignment = VerticalAlignment.Top;
                ellipsis.IsHitTestVisible = false;
                Grid.SetRow(ellipsis,2);
                Grid.SetColumn(ellipsis,2);

                int thickness = 1;
                System.Drawing.Color strokeColor = System.Drawing.Color.LightGray;
                Ellipse eventIndicator1 = new Ellipse();
                Ellipse eventIndicator2 = new Ellipse();
                Ellipse eventIndicator3 = new Ellipse();
                Ellipse eventIndicator4 = new Ellipse();
                eventIndicator1.Stroke = new SolidColorBrush(
                    System.Windows.Media.Color.FromArgb(
                    strokeColor.A, strokeColor.R, strokeColor.G, strokeColor.B));
                eventIndicator2.Stroke = new SolidColorBrush(
                    System.Windows.Media.Color.FromArgb(
                    strokeColor.A, strokeColor.R, strokeColor.G, strokeColor.B));
                eventIndicator3.Stroke = new SolidColorBrush(
                    System.Windows.Media.Color.FromArgb(
                    strokeColor.A, strokeColor.R, strokeColor.G, strokeColor.B));
                eventIndicator4.Stroke = new SolidColorBrush(
                    System.Windows.Media.Color.FromArgb(
                    strokeColor.A, strokeColor.R, strokeColor.G, strokeColor.B));
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
                int s = 15;
                eventIndicator1.Width = s; eventIndicator1.Height = s;
                eventIndicator2.Width = s; eventIndicator2.Height = s;
                eventIndicator3.Width = s; eventIndicator3.Height = s;
                eventIndicator4.Width = s; eventIndicator4.Height = s;
                eventIndicator1.HorizontalAlignment = HorizontalAlignment.Right;
                eventIndicator2.HorizontalAlignment = HorizontalAlignment.Left;
                eventIndicator3.HorizontalAlignment = HorizontalAlignment.Right;
                eventIndicator4.HorizontalAlignment = HorizontalAlignment.Left;
                Grid.SetRow(eventIndicator1 , 0); Grid.SetColumn(eventIndicator1, 0);
                Grid.SetRow(eventIndicator2, 0); Grid.SetColumn(eventIndicator2, 1);
                Grid.SetRow(eventIndicator3, 1); Grid.SetColumn(eventIndicator3, 0);
                Grid.SetRow(eventIndicator4, 1); Grid.SetColumn(eventIndicator4, 1);
                if (dayEvents.Count > 0)
                {
                    for (int j = 0; j < dayEvents.Count; j++)
                    {
                        if (dayEvents[j].dateType == Event.DateType.start)
                        {
                            switch (j)
                            {
                                case 0:
                                    eventIndicator1.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                                    dayEvents[j].Color.A, dayEvents[j].Color.R, dayEvents[j].Color.G, dayEvents[j].Color.B));
                                    break;

                                case 1:
                                    eventIndicator2.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                                    dayEvents[j].Color.A, dayEvents[j].Color.R, dayEvents[j].Color.G, dayEvents[j].Color.B));
                                    break;

                                case 2:
                                    eventIndicator3.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                                    dayEvents[j].Color.A, dayEvents[j].Color.R, dayEvents[j].Color.G, dayEvents[j].Color.B));
                                    break;

                                case 3:
                                    eventIndicator4.Fill = new SolidColorBrush(System.Windows.Media.Color.FromArgb(
                                    dayEvents[j].Color.A, dayEvents[j].Color.R, dayEvents[j].Color.G, dayEvents[j].Color.B));
                                    break;

                                default:
                                    ellipsis.Visibility = Visibility.Visible;
                                    break;
                            } 
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
                dayButton.Background = System.Windows.Media.Brushes.Transparent;
                dayButton.Tag = i + 1;
                dayButton.Click += DateClick;
                dayButton.Content = dayEvents.Count;


                //__________________________________________________________Event Strips_________________________________________________________
                Grid strip1 = new Grid();
                Grid strip2 = new Grid();
                Grid strip3 = new Grid();
                if (dayEvents != null && dayEvents.Count > 0)
                {
                    for (int j = 0; j < dayEvents.Count; j++)
                    {
                        if (dayEvents[j].StartDateTime.Day == currentDay.Day && dayEvents[j].StartDateTime.Month == currentDay.Month)
                        {
                            dayEvents[j].dateType = Event.DateType.start;
                        }
                        else if (dayEvents[j].EndDateTime.Day == currentDay.Day && dayEvents[j].EndDateTime.Month == currentDay.Month)
                        {
                            dayEvents[j].dateType = Event.DateType.end;
                        }
                        else
                        {
                            dayEvents[j].dateType = Event.DateType.middle;
                        }
                    }

                    int numOfGridStrips = 6;
                    for (int å = 0; å < numOfGridStrips; å++)
                    {
                        strip1.RowDefinitions.Add(new RowDefinition());
                        strip2.RowDefinitions.Add(new RowDefinition());
                        strip3.RowDefinitions.Add(new RowDefinition());
                    }
                    Grid.SetColumn(strip1, 0);
                    Grid.SetRow   (strip1, 2);
                    Grid.SetColumn(strip2, 1);
                    Grid.SetRow   (strip2, 2);
                    Grid.SetColumn(strip3, 2);
                    Grid.SetRow   (strip3, 2);

                    List<Border> stripColoring1 = new();
                    List<Border> stripColoring2 = new();
                    List<Border> stripColoring3 = new();

                    int numOfStrips = dayEvents.Count;
                    if (dayEvents.Count > numOfGridStrips-1)
                    {
                        numOfStrips = 5;
                    }
                    for (int j = 0; j < numOfStrips; j++)
                    {
                        System.Drawing.Color color = System.Drawing.Color.Transparent;
                        Border border1 = new();
                        Border border2 = new();
                        Border border3 = new();
                        border1.BorderThickness = new Thickness(0);
                        border1.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                        border2.BorderThickness = new Thickness(0);
                        border2.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                        border3.BorderThickness = new Thickness(0);
                        border3.Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                        stripColoring1.Add(border1);
                        stripColoring2.Add(border2);
                        stripColoring3.Add(border3);
                        int reverseOrder = -(j - 5);
                        Grid.SetRow(stripColoring1[j], reverseOrder);
                        Grid.SetRow(stripColoring2[j], reverseOrder);
                        Grid.SetRow(stripColoring3[j], reverseOrder);
                        color = dayEvents[j].Color;
                        if (dayEvents[j].dateType == Event.DateType.start)
                        {
                            stripColoring3[j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                        }
                        else if (dayEvents[j].dateType == Event.DateType.end)
                        {
                            stripColoring1[j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                        }
                        else
                        {
                            stripColoring1[j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                            stripColoring2[j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                            stripColoring3[j].Background = new SolidColorBrush(System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B));
                        }

                        stripColoring1[j].IsHitTestVisible = false;
                        stripColoring2[j].IsHitTestVisible = false;
                        stripColoring3[j].IsHitTestVisible = false;

                        strip1.Children.Add(stripColoring1[j]);
                        strip2.Children.Add(stripColoring2[j]);
                        strip3.Children.Add(stripColoring3[j]);
                    }
                }
                //‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾Event Strips‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾‾


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
                monthDay.Children.Add(strip1);
                monthDay.Children.Add(strip2);
                monthDay.Children.Add(strip3);

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

        private List<DateTime> GetDaysBetween(DateTime startDate, DateTime endDate)
        {
            List<DateTime> daysBetween = new List<DateTime>();

            // Include the start date
            DateTime currentDate = startDate;

            while (currentDate <= endDate)
            {
                daysBetween.Add(currentDate);
                currentDate = currentDate.AddDays(1);
            }

            return daysBetween;
        }
    }
}