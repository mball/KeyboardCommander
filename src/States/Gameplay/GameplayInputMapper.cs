using System.Collections.Generic;
using KeyboardCommander.Engine.Input;
using KeyboardCommander.States;
using Microsoft.Xna.Framework.Input;

namespace KeyboardCommander.States
{
    public class GameplayInputMapper : BaseInputMapper
    {
        public override IEnumerable<BaseInputCommand> GetKeyboardState(KeyboardState state)
        {
            var commands = new List<GameplayInputCommand>();

            if (state.IsKeyDown(Keys.Escape))
            {
                commands.Add(new GameplayInputCommand.GameExit());
            }

            return commands;
        }
    }
}
