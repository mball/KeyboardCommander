using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using System.Threading.Tasks;
using KeyboardCommander.Engine.Input;
using KeyboardCommander.Engine.Objects.Collisions;
using KeyboardCommander.Engine.Objects;
using KeyboardCommander.Engine.Sound;
using KeyboardCommander.Engine.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using KeyboardCommander.Objects;

namespace KeyboardCommander.States
{
    public class GameplayState : BaseGameState
    {
        private const string RightKeyTexture = "Sprites/RightKey";
        private const string LeftKeyTexture = "Sprites/LeftKey";
        private const string CounterFont = "Fonts/Counter";

        private Texture2D _keyTexture;

        public override void LoadContent()
        {
            _keyTexture = LoadTexture(RightKeyTexture);

            var keySprite = new KeySprite(_keyTexture);
            keySprite.Position = new Vector2(600, 280);

            AddGameObject(keySprite);
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    NotifyEvent(new BaseGameStateEvent.GameQuit());
                }

            });
        }

        public override void UpdateGameState(GameTime gameTime)
        {
        }

        //public override void Render(SpriteBatch spriteBatch)
        //{
        //    base.Render(spriteBatch);

        //    if (_gameOver)
        //    {
        //        // Draw black rectangle at 30% transparency
        //        var screenBoxTexture = GetScreenBoxTexture(spriteBatch.GraphicsDevice);
        //        var viewportRectangle = new Rectangle(0, 0, _viewportWidth, _viewportHeight);
        //        spriteBatch.Draw(screenBoxTexture, viewportRectangle, Color.Black * 0.3f);
        //    }
        //}

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }
    }
}
