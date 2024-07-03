using System.Numerics;
using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Objects
{
    public class ScoreText : BaseTextObject
    {
        public BigInteger Score
        {
            get => BigInteger.Parse(Text);
            set => Text = value.ToString();
        }
        
        public ScoreText(SpriteFont font) : base(font)
        {
        }
    }
}
