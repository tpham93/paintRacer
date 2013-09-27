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
            return Config.getFormattedTimeString(time);
        }

        public static bool operator ==(HighscoreElement e1, HighscoreElement e2)
        {
            return e1.CompareTo(e2) == 0 && e1.name == e2.name;
        }
        public static bool operator !=(HighscoreElement e1, HighscoreElement e2)
        {
            return !(e1==e2);
        }

        public int CompareTo(HighscoreElement other)
        {
            return (int)(this.time.TotalMilliseconds - other.time.TotalMilliseconds);
        }
    }
}
