using System;
using Microsoft.Xna.Framework;

namespace GameProject
{
    public class Ticker
    {
        public event Action<GameTime> Ticked;

        public void Tick(GameTime gameTime)
        {
            if (Ticked != null)
                Ticked(gameTime);
        }
    }
}