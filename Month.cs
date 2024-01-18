using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nyp3rCalender
{
    internal class Month
    {
        public string? Name { get; set; }
        public int DayCount { get; set; }
        public int WeekDayStartIndex { get; set; }
        public List<Event> Events { get; set; }

        public Month(string name, int dayCount, int weekDayStartIndex)
        {
            Name = name;
            DayCount = dayCount;
            Events = new List<Event>();
            WeekDayStartIndex = weekDayStartIndex;
        }

        public void addEvent(string name, string description, Color color, int day, TimeOnly time)
        {
            Events.Add(new Event(name,description,color,day,time));
        }
        public void addEvent(string name, string description, Color color, int day)
        {
            Events.Add(new Event(name, description, color, day));
        }
    }
}
