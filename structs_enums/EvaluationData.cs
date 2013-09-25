using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paintRacer
{
    struct EvaluationData
    {
        public HighscoreData highscore;
        public string playerName;
        public TimeSpan time;

        public EvaluationData(HighscoreData highscore, string playerName, TimeSpan time)
        {
            this.highscore = highscore;
            this.playerName = playerName;
            this.time = time;
        }
    }
}
