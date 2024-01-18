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
using System.Windows.Shapes;

namespace Nyp3rCalender
{
    /// <summary>
    /// Interaction logic for DateWindow.xaml
    /// </summary>
    public partial class DateWindow : Window
    {
        List<Event> events_ = new List<Event>();
        public DateWindow(int day, List<Event> events)
        {
            events_ = events;
            InitializeComponent();
            Title = "Day " + day.ToString();
            foreach (Event e in events)
            {
                eventPicker.Items.Add(e.Name);
            }


            eventPicker.SelectionChanged += EventPicked;
            
            eventTitle.GotFocus += Title_Focused;
            eventTitle.LostFocus += Title_Unfocused;

            location.GotFocus += Location_Focused;
            location.LostFocus += Location_Unfocused;

            description.GotFocus += Description_Focused;
            description.LostFocus += Description_Unfocused;
            description.TextChanged += Description_Focused;
        }

        public void Title_Focused(object sender, RoutedEventArgs e) 
            => bgTitle.Visibility = Visibility.Hidden;
        public void Title_Unfocused(object sender, RoutedEventArgs e)
        {
            if(eventTitle.Text == "" || eventTitle.Text == null)
            {
                bgTitle.Visibility = Visibility.Visible;
            }
        }
        public void Location_Focused(object sender, RoutedEventArgs e)
            => bgLocation.Visibility = Visibility.Hidden;
        public void Location_Unfocused(object sender, RoutedEventArgs e)
        {
            if (location.Text == "" || location.Text == null)
            {
                bgLocation.Visibility = Visibility.Visible;
            }
        }
        public void Description_Focused(object sender, RoutedEventArgs e)
            => bgDescription.Visibility = Visibility.Hidden;
        public void Description_Unfocused(object sender, RoutedEventArgs e)
        {
            string descriptionText = new TextRange(description.Document.ContentStart, description.Document.ContentEnd).Text;

            if (descriptionText == "" || descriptionText == null)
            {
                bgDescription.Visibility = Visibility.Visible;
            }
        }

        public void EventPicked(object sender, RoutedEventArgs e)
        {
            string pickedEventName = (sender as ComboBox).SelectedItem.ToString();

            Event pickedEvent = events_.FirstOrDefault(e => e.Name == pickedEventName);

            description.Document.Blocks.Clear();
            TextRange textRange = new TextRange(description.Document.ContentStart, description.Document.ContentEnd);
            textRange.Text = pickedEvent.Description;
        }
    }
}
