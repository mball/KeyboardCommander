﻿using System;
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
        private const string RightKeyTexture = "Sprites/RightKeySpriteSheet";
        private const string LeftKeyTexture = "Sprites/LeftKeySpriteSheet";
        private const string NoteA = "Sprites/NoteA@2";
        private const string NoteB = "Sprites/NoteB@2";
        private const string NoteC = "Sprites/NoteC@2";
        private const string CounterFont = "Fonts/Counter";
        private const string GrandHotelFont = "Fonts/GrandHotel";

        private const string PanelTexture = "Sprites/Panel";
        private const string ContainerSpriteSheet = "Sprites/Containers";

        private Texture2D _rightKeyTexture;
        private Texture2D _leftKeyTexture;
        private KeySprite _keyOfCSprite;
        private KeySprite _keyOfDSprite;
        private NoteSprite _noteSprite;
        
        private SpriteFont _counterFont;
        private SpriteFont _grandHotelFont;
        
        private ScoreText _scoreTextObject;
        
        private PanelSprite _panelSprite;

        private List<KeySprite> _keyList = new List<KeySprite>();
        private List<NoteSprite> _noteList = new List<NoteSprite>();
        private List<BaseGameObject> _uiElements = new List<BaseGameObject>();
        private List<ButtonSprite> _upgradeButtonList = new List<ButtonSprite>();
        
        private PlayerState _playerState = new() { Score = 0 };

        public override void LoadContent()
        {
            _counterFont = LoadFont(CounterFont);
            _grandHotelFont = LoadFont(GrandHotelFont);

            var upgradeButton = new ButtonSprite(LoadTexture("Sprites/Containers"));
            upgradeButton.Position = new Vector2(1325, 20);
            upgradeButton.zIndex = 101;
            upgradeButton.OnButtonClick += _upgrade_onUpgrade1Click;
            upgradeButton.Font = _grandHotelFont;
            upgradeButton.Text = "Upgrade 1";

            AddGameObject(upgradeButton);
            _upgradeButtonList.Add(upgradeButton);

            var upgradeButton2 = new ButtonSprite(LoadTexture("Sprites/Containers"));
            upgradeButton2.Position = new Vector2(1325, 220);
            upgradeButton2.zIndex = 101;
            upgradeButton2.OnButtonClick += _upgrade_onUpgrade2Click;
            upgradeButton2.Font = _grandHotelFont;
            upgradeButton2.Text = "Upgrade 2";
            AddGameObject(upgradeButton2);
            _upgradeButtonList.Add(upgradeButton2);

            var upgradeButton3 = new ButtonSprite(LoadTexture("Sprites/Containers"));
            upgradeButton3.Position = new Vector2(1325, 420);
            upgradeButton3.zIndex = 101;
            AddGameObject(upgradeButton3);
            _upgradeButtonList.Add(upgradeButton3);

            var upgradeButton4 = new ButtonSprite(LoadTexture("Sprites/Containers"));
            upgradeButton4.Position = new Vector2(1325, 620);
            upgradeButton4.zIndex = 101;
            AddGameObject(upgradeButton4);
            _upgradeButtonList.Add(upgradeButton4);

            var upgradeButton5 = new ButtonSprite(LoadTexture("Sprites/Containers"));
            upgradeButton5.Position = new Vector2(1325, 820);
            upgradeButton5.zIndex = 101;
            AddGameObject(upgradeButton5);
            _upgradeButtonList.Add(upgradeButton5);

            var upgradeButton6 = new ButtonSprite(LoadTexture("Sprites/Containers"));
            upgradeButton6.Position = new Vector2(1325, 1020);
            upgradeButton6.zIndex = 101;
            AddGameObject(upgradeButton6);
            _upgradeButtonList.Add(upgradeButton6);

            var upgradeButton7 = new ButtonSprite(LoadTexture("Sprites/Containers"));
            upgradeButton7.Position = new Vector2(1325, 1220);
            upgradeButton7.zIndex = 101;
            AddGameObject(upgradeButton7);
            _upgradeButtonList.Add(upgradeButton7);

            _panelSprite = new PanelSprite(LoadTexture(PanelTexture));
//            _panelSprite.SetUpgradeList(ref _upgradeButtonList);
            AddGameObject(_panelSprite);

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
            
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version != null)
            {
                // The text here does not matter, we just need the size of the text
                var textSize = _counterFont.MeasureString("Version"); 
                
                var versionTextObject = new StaticText(_counterFont, $"Keyboard Commander v{version.Major}.{version.Minor}.{version.Build}")
                {
                    Position = new Vector2(5, _viewportHeight - textSize.Y - 5),
                    zIndex = 10000,
                };
                _uiElements.Add(versionTextObject);
            }

            var scoreLabelObject = new StaticText(_grandHotelFont, "Inspiration:")
            {
                Position = new Vector2(_viewportWidth / 2, 5),
                TextAlignment = TextAlignment.Right,
                zIndex = 10000,
            };
            _uiElements.Add(scoreLabelObject);
            
            _scoreTextObject = new ScoreText(_grandHotelFont)
            {
                Score = _playerState.Score,
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
                        _playerState.Score += 1;
                        ProduceNote(_keyOfCSprite);
                    }
                }

                if (cmd is GameplayInputCommand.KeyOfDPressed dPressed)
                {
                    _keyOfDSprite.Pressed();
                    if (dPressed.IsNewPress)
                    {
                        _playerState.Score += 1;
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
            
            _scoreTextObject.Score = _playerState.Score;

            _panelSprite.Update(_upgradeButtonList);

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

        private void _upgrade_onUpgrade1Click(object sender, GameplayEvents.ButtonClickedEvent e)
        {
            _playerState.Score += 10;
        }

        private void _upgrade_onUpgrade2Click(object sender, GameplayEvents.ButtonClickedEvent e)
        {
            _playerState.Score += 10000;
        }
    }
}
