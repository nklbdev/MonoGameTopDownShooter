using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameProject.Factories
{
    public class BulletFactory
    {
        private readonly Texture2D _deadTexture;

        public BulletFactory(Texture2D deadTexture)
        {
            _deadTexture = deadTexture;
        }

        public Bullet Create(World world, Vector2 position, Vector2 velocity)
        {
            var bullet = new Bullet(world, position, velocity, _deadTexture);
            return bullet;
        }
    }
}