using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Objects
{
    public class KeySprite : BaseGameObject
    {
        private const int CellWidth = 128;
        private const int CellHeight = 442;

        private Rectangle _sourceRectangle;

        private bool _isPressed = false;

        public bool NoteProduced = false;

        public KeySprite(Texture2D texture)
        {
            _texture = texture;
            _sourceRectangle = new Rectangle(0, 0, CellWidth, CellHeight);
        }

        public void Pressed()
        {
            _isPressed = true;
        }

        public void Update(GameTime gameTime)
        {
            if (_isPressed)
            {
                _isPressed = false;
                _sourceRectangle = new Rectangle(128, 0, CellWidth, CellHeight);
            }
            else
            {
                _sourceRectangle = new Rectangle(0, 0, CellWidth, CellHeight);
                NoteProduced = false;
            }
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            var destinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, CellWidth, CellHeight);
            var sourceRectangle = _sourceRectangle;

            spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
        }
    }
}
