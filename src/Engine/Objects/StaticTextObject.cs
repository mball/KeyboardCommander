using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Engine.Objects
{
    public class StaticTextObject : BaseTextObject
    {
        public StaticTextObject(SpriteFont font, string text) : base(font)
        {
            Text = text;
        }
    }
}
