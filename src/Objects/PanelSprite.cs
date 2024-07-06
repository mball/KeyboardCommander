using KeyboardCommander.Engine.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KeyboardCommander.Objects
{
    public class PanelSprite : BaseGameObject
    {
        private const float SPEED = 20.0f;

        private bool _open = false;

        private MouseState _previousMouseState;
        
        public List<ButtonSprite> _upgradeButtonList;

        public PanelSprite(Texture2D texture)
        {
            _texture = texture;
            Position = new Vector2(1255, 0);
            zIndex = 100;
        }

        public void Open(List<ButtonSprite> upgradeButtonList)
        {
            if (Position.X > 800)
            {
                Position = new Vector2(Position.X - SPEED, Position.Y);
                foreach (var button in upgradeButtonList)
                {
                    button.Position = new Vector2(button.Position.X - SPEED, button.Position.Y);
                }
            }
        }

        public void SetUpgradeList(ref List<ButtonSprite> upgradeButtonList)
        {
            _upgradeButtonList = upgradeButtonList;
        }

        public void Close(List<ButtonSprite> upgradeButtonList)
        {
            if (Position.X < 1255)
            {
                Position = new Vector2(Position.X + SPEED, Position.Y);
                foreach (var button in upgradeButtonList)
                {
                    button.Position = new Vector2(button.Position.X + SPEED, button.Position.Y);
                }
            }
        }

        private bool IsMouseInBounds(MouseState mouseState)
        {
            return mouseState.X > Position.X && mouseState.X < Position.X + 50 && mouseState.Y > 310 && mouseState.Y < 405;
        }

        public void Update(List<ButtonSprite> upgradeButtonList)
        {
            if (_open)
            {
                Open(upgradeButtonList);
            }
            else
            {
                Close(upgradeButtonList);
            }

            var mouseState = Mouse.GetState();
            if (mouseState.LeftButton == ButtonState.Pressed && !_open  && _previousMouseState.LeftButton == ButtonState.Released && IsMouseInBounds(mouseState))
            {
                    _open = true;
            }
            else if (mouseState.LeftButton == ButtonState.Pressed && _open && _previousMouseState.LeftButton == ButtonState.Released && IsMouseInBounds(mouseState))
            {
                    _open = false;
            }

            if (mouseState.ScrollWheelValue > _previousMouseState.ScrollWheelValue && upgradeButtonList.First().Position.Y < 10)
            {
                foreach (var button in upgradeButtonList)
                {
                    button.Position = new Vector2(button.Position.X, button.Position.Y + 40);
                }
            }
            else if (mouseState.ScrollWheelValue < _previousMouseState.ScrollWheelValue && upgradeButtonList.Last().Position.Y > 500)
            {
                foreach (var button in upgradeButtonList)
                {
                    button.Position = new Vector2(button.Position.X, button.Position.Y - 40);
                }
            }

            foreach (var upgradeButton in upgradeButtonList)
            {
                upgradeButton.IsClicked(mouseState, _previousMouseState);
            }

            _previousMouseState = mouseState;
        }
    }
}
