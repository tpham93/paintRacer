using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace paintRacer
{
    class HighscoreElement
    {
        private TimeSpan time;
        private string name;

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

        public static bool operator <(HighscoreElement h1, HighscoreElement h2)
        {
            return h1.time < h2.time;
        }
        public static bool operator >(HighscoreElement h1, HighscoreElement h2)
        {
            return h1.time > h2.time;
        }
    }
}
