using System.Numerics;
using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Objects
{
    public class BigIntegerText : BaseTextObject
    {
        public BigInteger Value
        {
            get => BigInteger.Parse(Text);
            set => Text = value.ToString();
        }
        
        public BigIntegerText(SpriteFont font) : base(font)
        {
        }
    }
}
