﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGameTopDownShooter
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch, GameTime gameTime);
    }
}