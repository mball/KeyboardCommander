using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Objects
{
    public class StaticText : BaseTextObject
    {
        public StaticText(SpriteFont font, string text) : base(font)
        {
            Text = text;
        }
    }
}
