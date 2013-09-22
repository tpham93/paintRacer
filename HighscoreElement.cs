using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace paintRacer
{
    class HighscoreElement : IComparable<HighscoreElement>
    {
        private TimeSpan time;
        private string name;

        public string Name
        {
            get { return name; }
        }

        public HighscoreElement(string highscoreElementData)
        {
            Regex rx = new Regex(@"name=(?<name>.+) time=(?<time>.+)");
            Match match = rx.Match(highscoreElementData);
            name = (match.Groups["name"].Value);
            int milliseconds = Convert.ToInt32((match.Groups["time"].Value));
            time = TimeSpan.FromMilliseconds(milliseconds);
        }
        public HighscoreElement(string name, TimeSpan time)
        {
            this.time = time;
            this.name = name;
        }
        public override string ToString(){
            return "name=" + name + " time=" + (int)time.TotalMilliseconds;
        }

        public string getFormattedTimeString()
        {
            return string.Format("{0:00}:{1:00}:{2:00}", (int)time.TotalMinutes, time.Seconds, time.Milliseconds);
        }

        public int CompareTo(HighscoreElement other)
        {
            return (int)(this.time.TotalMilliseconds - other.time.TotalMilliseconds);
        }
    }
}
