using KeyboardCommander.Engine.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardCommander.States
{
    public class GameplayEvents : BaseGameStateEvent
    {
        public class ButtonClickedEvent : GameplayEvents { }
    }
}
