using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyboardCommander.Engine.Objects
{
    public class StatsObject : BaseTextObject
    {
        public const int ROLLING_SIZE = 60;
        private Queue<float> _rollingFPS = new Queue<float>();

        public float FPS { get; set; }
        public float MinFPS { get; private set; }
        public float MaxFPS { get; private set; }

        public float AverageFPS { get; private set; }
        public bool IsRunningSlowly { get; private set; }
        public int NbUpdateCalled { get; set; }
        public int NbDrawCalled { get; set; }

        public StatsObject(SpriteFont font) : base(font)
        {
            NbUpdateCalled = 0;
            NbDrawCalled = 0;
        }

        public void Update(GameTime gameTime)
        {
            NbUpdateCalled++;
            FPS = 1.0f / (float)gameTime.ElapsedGameTime.TotalSeconds;
            _rollingFPS.Enqueue(FPS);

            if (_rollingFPS.Count > ROLLING_SIZE)
            {
                _rollingFPS.Dequeue();
                var sum = 0.0f;
                MaxFPS = int.MinValue;
                MinFPS = int.MaxValue;
                foreach (var fps in _rollingFPS.ToArray())
                {
                    sum += fps;
                    if (fps > MaxFPS)
                    {
                        MaxFPS = fps;
                    }

                    if (fps < MinFPS)
                    {
                        MinFPS = fps;
                    }
                }
                AverageFPS = sum / _rollingFPS.Count;
            }
            else
            {
                AverageFPS = FPS;
                MinFPS = FPS;
                MaxFPS = FPS;
            }

            Text = $"FPS: {FPS}" + System.Environment.NewLine +
                   $"Min FPS: {MinFPS}" + System.Environment.NewLine +
                   $"Max FPS: {MaxFPS}" + System.Environment.NewLine +
                   $"Avg FPS: {AverageFPS}" + System.Environment.NewLine +
                   $"Running Slowly: {IsRunningSlowly}" + System.Environment.NewLine +
                   $"Nb Updates: {NbUpdateCalled}" + System.Environment.NewLine +
                   $"Nb Draws: {NbDrawCalled}";
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            NbDrawCalled++;
            base.Render(spriteBatch);
        }
    }
}
