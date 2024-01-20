using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;

namespace Nyp3rCalender
{
    public class Event
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
        public Color Color { get; set; }
        public bool IsWholeDay { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public TimeSpan TravelTime { get; set; }
        public string? Repeat { get; set; }
        public bool RepeatDoesntEnd { get; set; }
        public DateOnly EndRepeatDate { get; set; }
        public string? AlertBefore { get; set; }

        public Event(string? name, string? location, string? description, Color color, bool isWholeDay, string? repeat, bool repeatDoesntEnd, string? aleartBefore)
        {
            Name = name;
            Location = location;
            Description = description;
            Color = color;
            IsWholeDay = isWholeDay;
            //StartDateTime = startDateTime;
            //EndDateTime = endDateTime;
            //TravelTime = travelTime;
            Repeat = repeat;
            RepeatDoesntEnd = repeatDoesntEnd;
            //EndRepeatDate = endRepeatDate;
            AlertBefore = aleartBefore;
        }

        public static void Save(Event ev)
        {
            string filePath = "savedEvents.json";
            string json = JsonConvert.SerializeObject(ev);

            try
            {
                // Ensure the directory exists
                string directoryPath = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                // Write JSON to file
                File.WriteAllText(filePath, json);
                Console.WriteLine("Object written to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }
}
