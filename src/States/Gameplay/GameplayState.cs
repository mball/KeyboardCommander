using System;
using System.Collections.Generic;
using System.Reflection;
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
        private int AutoSaveInterval = 60;
        
        private const string RightKeyTexture = "Sprites/RightKeySpriteSheet";
        private const string LeftKeyTexture = "Sprites/LeftKeySpriteSheet";
        private const string NoteA = "Sprites/NoteA@2";
        private const string NoteB = "Sprites/NoteB@2";
        private const string NoteC = "Sprites/NoteC@2";
        private const string CounterFont = "Fonts/Counter";
        private const string GrandHotelFont = "Fonts/GrandHotel";

        private Texture2D _rightKeyTexture;
        private Texture2D _leftKeyTexture;
        private KeySprite _keyOfCSprite;
        private KeySprite _keyOfDSprite;
        private NoteSprite _noteSprite;
        
        private SpriteFont _counterFont;
        private SpriteFont _grandHotelFont;
        
        private StaticText _savingTextObject;
        private BigIntegerText _scoreTextObject;
        
        private List<KeySprite> _keyList = new List<KeySprite>();
        private List<NoteSprite> _noteList = new List<NoteSprite>();
        private List<BaseGameObject> _uiElements = new List<BaseGameObject>();
        
        private PlayerState _playerState = PlayerState.LoadOrCreate();
        private TimeSpan _nextAutoSaveTime = TimeSpan.FromSeconds(5);

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
            
            _counterFont = LoadFont(CounterFont);
            _grandHotelFont = LoadFont(GrandHotelFont);
            
            // The text here does not matter, we just need the size of the text
            var textSize = _counterFont.MeasureString("Version"); 
            
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
            {
                var versionTextObject = new StaticText(_counterFont, $"Keyboard Commander v{version.Major}.{version.Minor}.{version.Build}")
                {
                    Position = new Vector2(5, _viewportHeight - textSize.Y - 5),
                    zIndex = 10000,
                };
                _uiElements.Add(versionTextObject);
            }
            
            _savingTextObject = new StaticText(_counterFont, $"Loading...")
            {
                Position = new Vector2(_viewportWidth / 2, _viewportHeight - textSize.Y - 5),
                TextAlignment = TextAlignment.Center,
                zIndex = 10000,
            };
            _uiElements.Add(_savingTextObject);

            var scoreLabelObject = new StaticText(_grandHotelFont, "Inspiration:")
            {
                Position = new Vector2(_viewportWidth / 2, 5),
                TextAlignment = TextAlignment.Right,
                zIndex = 10000,
            };
            _uiElements.Add(scoreLabelObject);
            
            _scoreTextObject = new BigIntegerText(_grandHotelFont)
            {
                Value = _playerState.Inspiration,
                Position = new Vector2(_viewportWidth / 2, 5),
                TextAlignment = TextAlignment.Left,
                zIndex = 10000,
            };
            _uiElements.Add(_scoreTextObject);
            
            foreach (var uiElement in _uiElements)
            {
                AddGameObject(uiElement);
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

                if (cmd is GameplayInputCommand.KeyOfCPressed cPressed)
                {
                    _keyOfCSprite.Pressed();
                    if (cPressed.IsNewPress)
                    {
                        _playerState.Inspiration += 1;
                        ProduceNote(_keyOfCSprite);
                    }
                }

                if (cmd is GameplayInputCommand.KeyOfDPressed dPressed)
                {
                    _keyOfDSprite.Pressed();
                    if (dPressed.IsNewPress)
                    {
                        _playerState.Inspiration += 1;
                        ProduceNote(_keyOfDSprite);
                    }
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
            
            _scoreTextObject.Value = _playerState.Inspiration;

            _noteList = CleanObjects(_noteList);
        }

        public override void SlowUpdate(GameTime gameTime)
        {
            if (!_playerState.IsSaving)
            {
                _nextAutoSaveTime += gameTime.ElapsedGameTime;
                
                var nextSaveTime = AutoSaveInterval - _nextAutoSaveTime.TotalSeconds;

                if (nextSaveTime >= AutoSaveInterval - 5)
                {
                    _savingTextObject.Text = "Saved!";
                }
                else
                {
                    _savingTextObject.Text = $"Saving in {nextSaveTime % 60:00}";
                }
                
                if (nextSaveTime <= 0.1)
                {
                    _savingTextObject.Text = "Saving...";
                    _playerState.Save();
                    _nextAutoSaveTime = TimeSpan.Zero;
                }
            }
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
