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
using System.Media;
using System.Drawing;

namespace Nyp3rCalender
{
    public partial class MainWindow : Window
    {
        private int monthIndex = 0;
        private List<Month> months = new List<Month>();
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
            monthIndex = (monthIndex < months.Count-1) ? monthIndex + 1 : monthIndex = 0;
            RefreshWindow();
        }
        public void PreviousMonth(object sender, RoutedEventArgs e)
        {
            monthIndex = (monthIndex > 0) ? monthIndex - 1 : monthIndex = months.Count - 1;
            RefreshWindow();
        }

        private void RefreshWindow()
        {
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

                Image eventIndicator1 = new Image();
                BitmapImage bitmapEI1 = new BitmapImage();
                bitmapEI1.BeginInit();
                bitmapEI1.UriSource = new Uri(imagePath + "Transparent.png");
                bitmapEI1.EndInit();
                eventIndicator1.Source = bitmapEI1;
                eventIndicator1.Height = 20;
                eventIndicator1.IsHitTestVisible = false;
                Grid.SetColumn(eventIndicator1 , 0);
                Grid.SetRow(eventIndicator1 , 2);

                Image eventIndicator2 = new Image();
                BitmapImage bitmapEI2 = new BitmapImage();
                bitmapEI2.BeginInit();
                bitmapEI2.UriSource = new Uri(imagePath + "Transparent.png");
                bitmapEI2.EndInit();
                eventIndicator2.Source = bitmapEI2;
                eventIndicator2.Height = 20;
                eventIndicator2.IsHitTestVisible = false;
                Grid.SetColumn(eventIndicator2, 1);
                Grid.SetRow(eventIndicator2, 2);

                Image eventIndicator3 = new Image();
                BitmapImage bitmapEI3 = new BitmapImage();
                bitmapEI3.BeginInit();
                bitmapEI3.UriSource = new Uri(imagePath + "Transparent.png");
                bitmapEI3.EndInit();
                eventIndicator3.Source = bitmapEI3;
                eventIndicator3.Height = 20;
                eventIndicator3.IsHitTestVisible = false;
                Grid.SetColumn(eventIndicator3, 2);
                Grid.SetRow(eventIndicator3, 2);

                Button dayButton = new Button();
                dayButton.Background = Brushes.Transparent;
                dayButton.Click += DateClick;
                dayButton.Tag = i + 1;

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
                dayNumber.Text = $"{i + 1}.";

                monthDay.Children.Add(dayButton);
                monthDay.Children.Add(weekDay);
                monthDay.Children.Add(dayNumber);
                monthDay.Children.Add(eventIndicator1);
                monthDay.Children.Add(eventIndicator2);
                monthDay.Children.Add(eventIndicator3);

                Grid.SetColumn(monthDay, c);
                Grid.SetRow(monthDay, r);

                daysGrid.Children.Add(monthDay);
            }
        }

        public void DateClick(object sender, EventArgs e)
        {
            int currentMonthDay = (int)(sender as Button).Tag;
            months[monthIndex].addEvent("Birthday","My 21st Birthday",System.Windows.Media.Color.FromArgb(0,55,55,55),currentMonthDay);
            DateWindow dateWindow = new DateWindow(currentMonthDay, months[monthIndex].Events);
            dateWindow.ShowDialog();
        }
    }
}