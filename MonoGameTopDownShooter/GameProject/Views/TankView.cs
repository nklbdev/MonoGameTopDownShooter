using System;
using GameProject.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameProxies;

namespace GameProject.Views
{
    public class TankView : ViewBase
    {
        private readonly ITank _tank;
        private readonly Texture2D _bodyTexture;
        private readonly Texture2D _towerTexture;
        private readonly Vector2 _bodyTextureCenter;
        private readonly Vector2 _towerTextureCenter;
        private const float _scale = 10;

        public TankView(ITank tank, Texture2D bodyTexture, Texture2D towerTexture)
        {
            if (tank == null)
                throw new ArgumentNullException("tank");
            if (bodyTexture == null)
                throw new ArgumentNullException("bodyTexture");
            _tank = tank;
            _bodyTexture = bodyTexture;
            _towerTexture = towerTexture;
            _tank.Destroyed += TankOnDestroyed;
            _bodyTextureCenter = new Vector2(_bodyTexture.Width, _bodyTexture.Height) / 2;
            _towerTextureCenter = new Vector2(_towerTexture.Width, _towerTexture.Height) / 2;
        }

        private void TankOnDestroyed(IEntity entity)
        {
            if (entity != _tank)
                return;
            Destroy();
        }

        protected override void OnDestroy()
        {
            _tank.Destroyed -= TankOnDestroyed;
        }

        public override void OnRender(IImmutableSpriteBatch spriteBatch, ref Rectangle boundingRectangle)
        {
            spriteBatch.Draw(
                _bodyTexture,
                _tank.Body.Position * _scale, //position
                null, //destinationRectangle
                null, //sourceRectangle
                _bodyTextureCenter, //origin
                _tank.Body.Rotation + (float)Math.PI / 2); //color

            spriteBatch.Draw(
                _towerTexture,
                _tank.Tower.Position * _scale, //position
                null, //destinationRectangle
                null, //sourceRectangle
                _towerTextureCenter, //origin
                _tank.Tower.Rotation + (float)Math.PI / 2); //color
        }
    }
}