using System;
using System.Collections.Generic;
using KeyboardCommander.Engine.Input;
using KeyboardCommander.Engine.Objects;
using KeyboardCommander.Engine.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using KeyboardCommander.Objects;

namespace KeyboardCommander.States
{
    public class GameplayState : BaseGameState
    {
        private const string RightKeyTexture = "Sprites/RightKeySpriteSheet";
        private const string LeftKeyTexture = "Sprites/LeftKeySpriteSheet";
        private const string NoteA = "Sprites/NoteA@2";
        private const string NoteB = "Sprites/NoteB@2";
        private const string NoteC = "Sprites/NoteC@2";
        private const string CounterFont = "Fonts/Counter";

        private Texture2D _rightKeyTexture;
        private Texture2D _leftKeyTexture;
        private KeySprite _keyOfCSprite;
        private KeySprite _keyOfDSprite;
        private NoteSprite _noteSprite;

        private List<KeySprite> _keyList = new List<KeySprite>();
        private List<NoteSprite> _noteList = new List<NoteSprite>();

        public override void LoadContent()
        {
            _rightKeyTexture = LoadTexture(RightKeyTexture);
            _leftKeyTexture = LoadTexture(LeftKeyTexture);

            _keyOfCSprite = new KeySprite(_rightKeyTexture);
            _keyOfCSprite.Position = new Vector2(600, 280);
            _keyList.Add(_keyOfCSprite);

            _keyOfDSprite = new KeySprite(_leftKeyTexture);
            _keyOfDSprite.Position = new Vector2(728, 280);
            _keyList.Add(_keyOfDSprite);


            foreach (var key in _keyList)
            {
                AddGameObject(key);
            }
        }

        public override void HandleInput(GameTime gameTime)
        {
            InputManager.GetCommands(cmd =>
            {
                if (cmd is GameplayInputCommand.GameExit)
                {
                    NotifyEvent(new BaseGameStateEvent.GameQuit());
                }

                if (cmd is GameplayInputCommand.KeyOfCPressed)
                {
                    _keyOfCSprite.Pressed();
                    ProduceNote(_keyOfCSprite);
                }

                if (cmd is GameplayInputCommand.KeyOfDPressed)
                {
                    _keyOfDSprite.Pressed();
                    ProduceNote(_keyOfDSprite);
                }
            });
        }

        public override void UpdateGameState(GameTime gameTime)
        {
            foreach (var _keySprite in _keyList)
            {
                _keySprite.Update(gameTime);
            }

            foreach (var _noteSprite in _noteList)
            {
                _noteSprite.MoveUp(gameTime);
            }

            _noteList = CleanObjects(_noteList);
        }

        private void ProduceNote(KeySprite keySprite)
        {
            if (keySprite.NoteProduced == false)
            {
                var noteSprite = new NoteSprite(LoadTexture(NoteC));
                noteSprite.Position = new Vector2(keySprite.Position.X + 20, keySprite.Position.Y + 30);

                _noteList.Add(noteSprite);
                AddGameObject(noteSprite);

                keySprite.NoteProduced = true;
            }
        }

        protected override void SetInputManager()
        {
            InputManager = new InputManager(new GameplayInputMapper());
        }
        private List<T> CleanObjects<T>(List<T> objectList, Func<T, bool> predicate) where T : BaseGameObject
        {
            List<T> listOfItemsToKeep = new List<T>();
            foreach (T item in objectList)
            {
                var performRemoval = predicate(item);

                if (performRemoval || item.Destroyed)
                {
                    RemoveGameObject(item);
                }
                else
                {
                    listOfItemsToKeep.Add(item);
                }
            }

            return listOfItemsToKeep;
        }
        private List<T> CleanObjects<T>(List<T> objectList) where T : BaseGameObject
        {
            return CleanObjects(objectList, item => item.Position.Y < -200);
        }
    }
}
