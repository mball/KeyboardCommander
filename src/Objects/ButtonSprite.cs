using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public ButtonSprite(Texture2D texture)
        {
            _texture = texture;
            _sourceRectangle =  new Rectangle(10, 10, CellWidth, CellHeight);
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var destinationRectangle = new Rectangle((int)Position.X, (int)Position.Y, ButtonWidth, ButtonHeight);
            var sourceRectangle = _sourceRectangle;

            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
        }

    }
}
