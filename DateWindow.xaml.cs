using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        string startHourM = "";
        string endHourM = "";
        string startMinuteM = "";
        string endMinuteM = "";
        string endDateM = "";
        string travelHourM = "";
        string travelMinuteM = "";
        string endRepeatDateM = "";

        List<Event> events = new List<Event>();
        private int month;
        private int day;
        public DateWindow(int day_, int month_)
        {
            day = day_;
            month = month_;

            List<Event> allEvents = Event.Load();
            if (allEvents != null)
            {
                foreach (Event ev in allEvents)
                {
                    if (ev.StartDateTime.Day == day_ && ev.StartDateTime.Month == month_)
                    {
                        events.Add(ev);
                    }
                } 
            }

            InitializeComponent();

            colorPicker.ItemsSource = ColorNames_Get();

            eventPicker.SelectionChanged += Event_Picked;

            eventTitle.GotFocus += Title_Focused;
            eventTitle.LostFocus += Title_Unfocused;
            eventTitle.TextChanged += Title_Focused;

            location.GotFocus += Location_Focused;
            location.LostFocus += Location_Unfocused;
            location.TextChanged += Location_Focused;

            description.GotFocus += Description_Focused;
            description.LostFocus += Description_Unfocused;
            description.TextChanged += Description_Focused;

            wholeDay.Checked += WholeDay_Checked;
            wholeDay.Unchecked += WholeDay_Unchecked;

            insertSameDay.Click += InsertSameDay_Click;
            endDate.LostFocus += EndDate_Unfocused;

            repeatNever.Checked += NeverEndRepeat_Checked;
            repeatNever.Unchecked += NeverEndRepeat_Unchecked;

            add.Click += Event_Add;
            save.Click += Event_Save;



            Title = "Day " + day.ToString();
            eventPicker.Items.Add("New event");
            foreach (Event e in events)
            {
                eventPicker.Items.Add(e.Name);
            }
            eventPicker.SelectedIndex = (eventPicker.Items.Count >= 2) ? 1 : 0;
            for (int i = 0; i < 24; i++)
            {
                startHour.Items.Add(i.ToString());
                endHour.Items.Add(i.ToString());
                travelHour.Items.Add(i.ToString());
            }
            for (int i = 0; i < 60; i += 5)
            {
                startMinute.Items.Add(i.ToString());
                endMinute.Items.Add(i.ToString());
                travelMinute.Items.Add(i.ToString());
            }

            repeatPicker.Items.Add("Never");
            repeatPicker.Items.Add("Every day");
            repeatPicker.Items.Add("Every week");
            repeatPicker.Items.Add("Every second\nweek");
            repeatPicker.Items.Add("Every month");
            repeatPicker.Items.Add("Every year");

            alert.Items.Add("None");
            alert.Items.Add("When the\nevent begins");
            alert.Items.Add("5 minutes\nbefore");
            alert.Items.Add("10 minutes\nbefore");
            alert.Items.Add("15 minutes\nbefore");
            alert.Items.Add("30 minutes\nbefore");
            alert.Items.Add("1 hour\nbefore");
            alert.Items.Add("2 hour\nbefore");
            alert.Items.Add("1 day\nbefore");
            alert.Items.Add("2 days\nbefore");
            alert.Items.Add("1 week\nbefore");
        }
        private List<string> ColorNames_Get()
        {
            List<string> colorNames = new List<string>();

            // Get all public static properties from the Colors class
            Type colorsType = typeof(Colors);
            PropertyInfo[] colorProperties = colorsType.GetProperties(BindingFlags.Public | BindingFlags.Static);

            // Add each color name to the list
            foreach (var property in colorProperties)
            {
                colorNames.Add(property.Name);
            }

            return colorNames;
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

        public void WholeDay_Checked(object sender, RoutedEventArgs e)
        {
            startHour.IsEnabled = false;
            startMinute.IsEnabled = false;
            endDate.IsEnabled = false;
            endHour.IsEnabled = false;
            endMinute.IsEnabled = false;
            travelHour.IsEnabled = false;
            travelMinute.IsEnabled = false;
            insertSameDay.IsEnabled = false;

            startHourM = startHour.Text;
            endHourM = endHour.Text;
            startMinuteM = startMinute.Text;
            endMinuteM = endMinute.Text;
            endDateM = endDate.Text;
            travelHourM = travelHour.Text;
            travelMinuteM = travelMinute.Text;

            startHour.Text = string.Empty;
            startMinute.Text = string.Empty;
            endHour.Text = string.Empty;
            endMinute.Text = string.Empty;
            endDate.Text = string.Empty;
            travelHour.Text = string.Empty;
            travelMinute.Text = string.Empty;
        }
        public void WholeDay_Unchecked(object sender, RoutedEventArgs e)
        {
            startHour.IsEnabled = true;
            startMinute.IsEnabled = true;
            endDate.IsEnabled = true;
            endHour.IsEnabled = true;
            endMinute.IsEnabled = true;
            travelHour.IsEnabled = true;
            travelMinute.IsEnabled = true;
            if(endDateM == string.Empty) { insertSameDay.IsEnabled = true; } 
            else{ insertSameDay.IsEnabled = (Convert.ToInt16(endDateM[0] + endDateM[1]) != day && Convert.ToInt16(endDateM[3] + endDateM[4]) != month) ? true : false; }

            startHour.Text = startHourM;
            startMinute.Text = startMinuteM;
            endHour.Text = endHourM;
            endMinute.Text = endMinuteM;
            endDate.Text = endDateM;
            travelHour.Text = travelHourM;
            travelMinute.Text = travelMinuteM;

            startHourM = string.Empty;
            startMinuteM = string.Empty;
            endHourM = string.Empty;
            endMinuteM = string.Empty;
            endDateM = string.Empty;
            travelHourM = string.Empty;
            travelMinuteM = string.Empty;

        }

        public void InsertSameDay_Click(object sender, RoutedEventArgs e)//Currently being worked on
        {
            endDate.Text = $"{day}-{month}-2024";
            ((Button)sender).IsEnabled = false;
        }

        public void EndDate_Unfocused(object sender, RoutedEventArgs e)//Currently being worked on
        {
            if(((DatePicker)sender).Text != $"{day}-{month}-2024")
            {
                insertSameDay.IsEnabled = true;
            }
        }

        public void NeverEndRepeat_Checked(object sender, RoutedEventArgs e)
        {
            endRepeatDateM = endRepeatDate.Text;
            endRepeatDate.Text = string.Empty;
            endRepeatDate.IsEnabled = false;
        }

        public void NeverEndRepeat_Unchecked(object sender, RoutedEventArgs e)
        {
            endRepeatDate.IsEnabled = true;
            endRepeatDate.Text = endRepeatDateM;
            endRepeatDateM = string.Empty;
        }

        public void Event_Picked(object sender, RoutedEventArgs e)
        {
            if(((ComboBox)sender).SelectedItem == null) 
            {
                return;
            }

            string? pickedEventName = ((ComboBox)sender).SelectedItem.ToString();
            Event? pickedEvent = events.FirstOrDefault(e => e.Name == pickedEventName);

            if(pickedEvent == null) 
            {
                return;
            }

            description.Document.Blocks.Clear();
            TextRange textRange = new TextRange(description.Document.ContentStart, description.Document.ContentEnd);
            textRange.Text = pickedEvent.Description;

            eventTitle.Text = pickedEvent.Name;
            location.Text = pickedEvent.Location;
            wholeDay.IsChecked = pickedEvent.IsWholeDay;
            if (pickedEvent.IsWholeDay)
            {
                startHour.IsEnabled = false;
                startMinute.IsEnabled = false;
                endDate.IsEnabled = false;
                endHour.IsEnabled = false;
                endMinute.IsEnabled = false;
                travelHour.IsEnabled = false;
                travelMinute.IsEnabled = false;
                insertSameDay.IsEnabled = false;
            }
            else
            {
                startHour.SelectedValue = pickedEvent.StartDateTime.Hour.ToString();
                startMinute.SelectedValue = pickedEvent.StartDateTime.Minute.ToString();
                
                endDate.Text = pickedEvent.EndDateTime.Date.ToString();
                endHour.SelectedValue = pickedEvent.EndDateTime.Hour.ToString();
                endMinute.SelectedValue = pickedEvent.EndDateTime.Minute.ToString();

                travelHour.SelectedValue = pickedEvent.TravelTime.Hours.ToString();
                travelMinute.SelectedValue = pickedEvent.TravelTime.Minutes.ToString();


                repeatPicker.SelectedValue = pickedEvent.Repeat;
                repeatNever.IsChecked = pickedEvent.RepeatDoesntEnd;

                if (pickedEvent.RepeatDoesntEnd)
                {
                    endRepeatDate.IsEnabled = false;
                }
                else
                {
                    endRepeatDate.Text = pickedEvent.EndRepeatDate.ToLongDateString();
                }


                colorPicker.SelectedValue = pickedEvent.Color.Name;
                alert.SelectedValue =  pickedEvent.AlertBefore;
            }
        }

        public void Event_Add(object sender, EventArgs e)
        {
            ClearUIInfo();
            remove.IsEnabled = false;
            eventTitle.Focus();
            eventPicker.SelectedIndex = 0;
        }

        public void Event_Save(object sender, EventArgs e)
        {   //check if it's filled in correctly
            if (!IsFilledIn())
            {
                return;
            }

            //adds a new event if it's on the new event option
            if (eventPicker.SelectedIndex == 0)
            {
                Event newEvent = new(eventTitle.Text, location.Text, new TextRange(description.Document.ContentStart, description.Document.ContentEnd).Text, System.Drawing.Color.FromName(colorPicker.Text), (bool)wholeDay.IsChecked, repeatPicker.Text, (bool)repeatNever.IsChecked, alert.Text);
                if (wholeDay.IsChecked == false)
                {
                    newEvent.StartDateTime = new DateTime(2024, month, day, Convert.ToInt16(startHour.Text), Convert.ToInt16(startMinute.Text), 0);
                    newEvent.EndDateTime = new DateTime(2024, endDate.DisplayDate.Month, endDate.DisplayDate.Day, Convert.ToInt16(endHour.Text), Convert.ToInt16(endMinute.Text), 0);
                    newEvent.TravelTime = new TimeSpan(0, Convert.ToInt16(travelHour.Text), Convert.ToInt16(travelMinute.Text), 0);
                }
                else
                {
                    newEvent.StartDateTime = new DateTime(2024,month,day,0,0,0);
                }
                if (repeatNever.IsChecked == false)
                {
                    newEvent.EndRepeatDate = new DateOnly(2024, endRepeatDate.DisplayDate.Month, endRepeatDate.DisplayDate.Day);
                }
                events.Add(newEvent);
                eventPicker.Items.Add(newEvent.Name);

                eventPicker.SelectedIndex = eventPicker.Items.Count-1;
                Event.Save(newEvent);
                return;
            }

            //saves existing event
            int i = eventPicker.SelectedIndex - 1;
            events[i].Name = eventTitle.Text;
            events[i].Location = location.Text;
            events[i].IsWholeDay = (wholeDay.IsChecked != null) ? (bool)wholeDay.IsChecked : false;
            if (wholeDay.IsChecked == null || !(bool)wholeDay.IsChecked)
            {
                events[i].StartDateTime = new DateTime(2024, month, day, Convert.ToInt16(startHour.Text), Convert.ToInt16(startMinute.Text), 0);
                events[i].EndDateTime = new DateTime(2024, endDate.DisplayDate.Month, endDate.DisplayDate.Day, Convert.ToInt16(endHour.Text), Convert.ToInt16(endMinute.Text), 0);
                events[i].TravelTime = new TimeSpan(0, Convert.ToInt16(travelHour.Text), Convert.ToInt16(travelMinute.Text), 0); 
            }
            events[i].Repeat = repeatPicker.Text;
            events[i].RepeatDoesntEnd = (repeatNever.IsChecked != null) ? (bool)repeatNever.IsChecked : false;
            if (!events[i].RepeatDoesntEnd){events[i].EndRepeatDate = new DateOnly(2024, endRepeatDate.DisplayDate.Month, endRepeatDate.DisplayDate.Day);}            
            events[i].Color = System.Drawing.Color.FromName(colorPicker.Text);
            events[i].AlertBefore = alert.Text;
            events[i].Description = new TextRange(description.Document.ContentStart, description.Document.ContentEnd).Text;
        }

        private void ClearUIInfo()
        {
            eventPicker.SelectedItem = null;
            eventTitle.Text = string.Empty;
            location.Text = string.Empty;
            wholeDay.IsChecked = false;
            startHour.SelectedItem = null;
            startMinute.SelectedItem = null;
            endDate.Text = string.Empty;
            endHour.SelectedItem = null;
            endMinute.SelectedItem = null;
            travelHour.SelectedItem = null;
            travelMinute.SelectedItem = null;
            repeatPicker.SelectedItem = null;
            endRepeatDate.Text = string.Empty;
            repeatNever.IsChecked = true;
            colorPicker.SelectedItem = null;
            alert.SelectedItem = null;
            description.Document.Blocks.Clear();
        }

        private bool IsFilledIn()
        {
            TextRange textRange = new TextRange(description.Document.ContentStart, description.Document.ContentEnd);
            if (eventTitle.Text != string.Empty && repeatPicker.SelectedItem.ToString() != string.Empty && colorPicker.SelectedItem.ToString() != string.Empty && alert.SelectedItem.ToString() != string.Empty && textRange.Text != string.Empty)
            {
                if (wholeDay.IsChecked == true && repeatNever.IsChecked == true)
                {
                    return true;
                }
                else if (wholeDay.IsChecked == false && repeatNever.IsChecked == false)
                {
                    if(startHour.SelectedItem.ToString() != string.Empty && startMinute.SelectedItem.ToString() != string.Empty && endDate.Text != string.Empty && endHour.SelectedItem.ToString() != string.Empty && endMinute.SelectedItem.ToString() != string.Empty && travelHour.SelectedItem.ToString() != string.Empty && travelMinute.SelectedItem.ToString() != string.Empty && endRepeatDate.Text != string.Empty)
                    {
                        return true;
                    }
                }
                else if (wholeDay.IsChecked == true && repeatNever.IsChecked == false)
                {
                    if (endRepeatDate.Text != string.Empty)
                    {
                        return true;
                    }
                }
                else if(wholeDay.IsChecked == false && repeatNever.IsChecked == true)
                {
                    if (startHour.SelectedItem.ToString() != string.Empty && startMinute.SelectedItem.ToString() != string.Empty && endDate.Text != string.Empty && endHour.SelectedItem.ToString() != string.Empty && endMinute.SelectedItem.ToString() != string.Empty && travelHour.SelectedItem.ToString() != string.Empty && travelMinute.SelectedItem.ToString() != string.Empty)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
