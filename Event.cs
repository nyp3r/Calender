using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Printing;
using System.Windows.Controls;

namespace Nyp3rCalender
{
    public class Event
    {
        public enum DateType
        {
            start,
            end,
            middle
        }
        public string? Key { get; set; }
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
        public DateType dateType { get; set; }

        public Event(string? name, string? location, string? description, Color color, bool isWholeDay, string? repeat, bool repeatDoesntEnd, string? aleartBefore)
        {
            Name = name;
            Location = location;
            Description = description;
            Color = color;
            IsWholeDay = isWholeDay;
            Repeat = repeat;
            RepeatDoesntEnd = repeatDoesntEnd;
            AlertBefore = aleartBefore;
        }

        public static void Edit(Event givenEvent)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(baseDirectory, "savedEvents.json");

            string existingContent = File.ReadAllText(filePath);

            List<Event> existingEvents = JsonConvert.DeserializeObject<List<Event>>(existingContent) ?? new List<Event>();
            for (int i = 0; i < existingEvents.Count; i++)
            {
                if (existingEvents[i].Key == givenEvent.Key)
                {
                    existingEvents[i] = givenEvent;
                    Popup popup = new();
                    TextBox textBox = new();
                    textBox.Text = givenEvent.EndDateTime.Date.ToString();
                    popup.Child = textBox;
                    popup.IsOpen = true;
                    string updatedEvents = JsonConvert.SerializeObject(existingEvents, Formatting.Indented);
                    File.WriteAllText(filePath, updatedEvents);
                    return;
                }
            }
        }

        public static void Add(Event givenEvent)
        {
            string lowerCaseChars = "abcdefghijklmnopqrstuvwxyz";
            string upperCaseChars = lowerCaseChars.ToUpper();
            string specialChars = "<>-_.:,;'*¨^`´+?\\=}][{)(/&%€¤$#£\"@!æøå";
            string numbers = "1234567890";
            char[] chars = (lowerCaseChars + upperCaseChars + specialChars + numbers).ToCharArray();
            givenEvent.Key = RandomNumberGenerator.GetString(chars, 32);

            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(baseDirectory, "savedEvents.json");

            string existingContent = File.ReadAllText(filePath);

            List<Event> existingEvents = JsonConvert.DeserializeObject<List<Event>>(existingContent) ?? new List<Event>();

            existingEvents.Add(givenEvent);
            string updatedContent = JsonConvert.SerializeObject(existingEvents, Formatting.Indented);
            File.WriteAllText(filePath, updatedContent);
        }

        public static void Remove(Event givenEvent)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string filePath = Path.Combine(baseDirectory, "savedEvents.json");

            string existingContent = File.ReadAllText(filePath);

            List<Event> deserializedEvents = JsonConvert.DeserializeObject<List<Event>>(existingContent) ?? new List<Event>();
            
            deserializedEvents.RemoveAt(deserializedEvents.IndexOf(deserializedEvents.FirstOrDefault(e => e.Key == givenEvent.Key)));
            string serializedEvents = JsonConvert.SerializeObject(deserializedEvents, Formatting.Indented);
            File.WriteAllText(filePath, serializedEvents);
        }

        public static List<Event> Load()
        {
            //get base directory
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            //check if the file exists
            if (!File.Exists(baseDirectory + "savedEvents.json"))
            { // if not, make one
                File.WriteAllText("savedEvents.json",string.Empty);
            }
            string json = File.ReadAllText("savedEvents.json");
            if (string.IsNullOrEmpty(json)) { return null; }
            List<Event> events = JsonConvert.DeserializeObject<List<Event>>(json);
            return events;
        }
    }
}
