using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Engine.Objects
{
    public enum TextAlignment
    {
        Left,
        Center,
        Right
    }
    
    public class BaseTextObject : BaseGameObject
    {
        protected SpriteFont _font;
        protected string _text;
        protected TextAlignment _textAlignment = TextAlignment.Left;
        protected Vector2 _origin = Vector2.Zero;

        public BaseTextObject(SpriteFont font)
        {
            _font = font;
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;

                UpdateOrigin();
            }
        }

        public TextAlignment TextAlignment
        {
            get => _textAlignment;
            set
            {
                _textAlignment = value;
                
                UpdateOrigin();
            }
        }
        
        public override void Render(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(_font, Text, _position, Color.White, 0, _origin, 1, SpriteEffects.None, 0);
        }
        
        private void UpdateOrigin()
        {
            _origin = TextAlignment switch
            {
                TextAlignment.Left => Vector2.Zero,
                TextAlignment.Center => new Vector2(_font.MeasureString(Text).X / 2, 0),
                TextAlignment.Right => new Vector2(_font.MeasureString(Text).X, 0),
                _ => _origin
            };
        }
    }
}
