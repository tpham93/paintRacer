using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace paintRacer
{
    class EventManager
    {
        Event[] possibleEvent;

        public EventManager(String filename);
        public void getEvent(short index);
    }
}
