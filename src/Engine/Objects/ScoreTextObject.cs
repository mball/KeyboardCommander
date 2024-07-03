using System.Numerics;
using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Engine.Objects
{
    public class ScoreTextObject : BaseTextObject
    {
        public BigInteger Score
        {
            get => BigInteger.Parse(Text);
            set => Text = value.ToString();
        }
        
        public ScoreTextObject(SpriteFont font) : base(font)
        {
        }
    }
}
