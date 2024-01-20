using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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

        public void addEvent(string? name, string? location, string? description, Color color, bool isWholeDay, string? repeat, bool repeatDoesntEnd, string? aleartBefore)
        {
            Events.Add(new Event(name,location,description,color,isWholeDay,repeat,repeatDoesntEnd,aleartBefore));
        }
    }
}
