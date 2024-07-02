using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace KeyboardCommander.Objects
{
    public class KeySprite : BaseGameObject
    {
        public KeySprite(Texture2D texture)
        {
            _texture = texture;
        }

        //public void Update(GameTime gameTime)
        //{
        //    if (_currentAnimation != null)
        //    {
        //        _currentAnimation.Update(gameTime);
        //    }
        //}

        //public override void Render(SpriteBatch spriteBatch)
        //{
        //    var destinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, AnimationCellWidth, AnimationCellHeight);
        //    var sourceRectangle = _idleRectangle;
        //    if (_currentAnimation != null)
        //    {
        //        var currentFrame = _currentAnimation.CurrentFrame;
        //        if (currentFrame != null)
        //        {
        //            sourceRectangle = currentFrame.SourceRectangle;
        //        }
        //    }

        //    spriteBatch.Draw(_texture, destinationRectangle, sourceRectangle, Color.White);
        //}
    }
}
