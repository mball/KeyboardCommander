using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Objects
{
    public class NoteSprite : BaseGameObject
    {
        private const float NOTE_SPEED = 10.0f;

        public NoteSprite(Texture2D texture)
        {
            _texture = texture;
        }

        public void MoveUp(GameTime gameTime)
        {
            Position = new Vector2(Position.X, Position.Y - NOTE_SPEED);
        }
    }
}
