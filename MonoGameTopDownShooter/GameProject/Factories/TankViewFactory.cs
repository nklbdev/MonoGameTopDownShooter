using System;
using GameProject.Entities.Models.Tanks;
using GameProject.Entities.Views;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Factories
{
    public class TankViewFactory : ITankViewFactory
    {
        private readonly Texture2D _bodyTexture;
        private readonly Texture2D _towerTexture;

        public TankViewFactory(Texture2D bodyTexture, Texture2D towerTexture)
        {
            if (bodyTexture == null)
                throw new ArgumentNullException("bodyTexture");
            if (towerTexture == null)
                throw new ArgumentNullException("towerTexture");
            _bodyTexture = bodyTexture;
            _towerTexture = towerTexture;
        }

        public IView Create(ITank tank)
        {
            return new TankView(tank, _bodyTexture, _towerTexture);
        }
    }
}