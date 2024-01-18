using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Nyp3rCalender
{
    public class Event
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public Color Color { get; set; }
        public int Day { get; set; }
        public TimeOnly Time {  get; set; }

        public Event(string? name, string? description, Color color, int day, TimeOnly time)
        {
            Name = name;
            Description = description;
            Color = color;
            Day = day;
            Time = time;
        }
        public Event(string? name, string? description, Color color, int day)
        {
            Name = name;
            Description = description;
            Color = color;
            Day = day;
        }
    }
}
