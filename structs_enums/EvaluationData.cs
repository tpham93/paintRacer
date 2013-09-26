using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paintRacer
{
    struct EvaluationData
    {
        public HighscoreData highscore;
        public Player player;
        public TimeSpan time;

        public EvaluationData(HighscoreData highscore, Player player, TimeSpan time)
        {
            this.highscore = highscore;
            this.player = player;
            this.time = time;
        }
    }
}
