using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace GameProject.Infrastructure
{
    public class SplashScreen : IScreen
    {
        private readonly TimeSpan _timeout;
        private TimeSpan _time;

        public SplashScreen(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        public void Update(GameTime gameTime)
        {
            if (_time < _timeout)
            {
                _time += gameTime.ElapsedGameTime;
                if (_time >= _timeout)
                    Trace.WriteLine("Switch screen to main menu screen");
            }
        }

        public void Draw(GameTime gameTime)
        {
            Trace.WriteLine("Draw splash screen");
        }
    }
}