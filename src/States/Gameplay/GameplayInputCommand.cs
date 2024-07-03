using KeyboardCommander.Engine.Input;

namespace KeyboardCommander.States
{
    public class GameplayInputCommand : BaseInputCommand
    {
        public class GameExit : GameplayInputCommand { }

        public class KeyOfCPressed : GameplayInputCommand
        {
            public bool IsNewPress { get; set; }
        }

        public class KeyOfDPressed : GameplayInputCommand
        {
            public bool IsNewPress { get; set; }
        }
    }
}
