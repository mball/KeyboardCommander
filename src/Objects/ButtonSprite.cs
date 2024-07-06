using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace KeyboardCommander.Objects
{
    public class ButtonSprite : BaseGameObject
    {
        private const int CellWidth = 915;
        private const int CellHeight = 466;

        private const int ButtonWidth = 400;
        private const int ButtonHeight = 200;

        private Rectangle _sourceRectangle;

        public event EventHandler<States.GameplayEvents.ButtonClickedEvent> OnButtonClick;

        public ButtonSprite(Texture2D texture)
        {
            _texture = texture;
            _sourceRectangle =  new Rectangle(10, 10, CellWidth, CellHeight);
        }

        public string Text { get; set; }
        public SpriteFont Font { get; set; }

        public override void Render(SpriteBatch spriteBatch)
        {
            var destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, ButtonWidth, ButtonHeight);
            var sourceRectangle = _sourceRectangle;

            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);

            if (Font != null)
            {
                var fontPosition = new Vector2(Position.X + ButtonWidth / 2 - Font.MeasureString(Text).X / 2, Position.Y + 20);
                spriteBatch.DrawString(Font, Text, fontPosition, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            }
        }

        public void IsClicked(MouseState mouseState, MouseState previousMouseState)
        {
            var mouseRectangle = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
            var buttonRectangle = new Rectangle((int)Position.X, (int)Position.Y, ButtonWidth, ButtonHeight);

            if (mouseRectangle.Intersects(buttonRectangle))
            {
                if (mouseState.LeftButton == ButtonState.Pressed && previousMouseState.LeftButton == ButtonState.Released)
                {
                    _sourceRectangle = new Rectangle(10, 500, CellWidth, CellHeight);
                }

                if (mouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
                {
                    _sourceRectangle =  new Rectangle(10, 10, CellWidth, CellHeight);
                    OnButtonClick?.Invoke(this, new States.GameplayEvents.ButtonClickedEvent());
                }
            }
        }
    }
}
