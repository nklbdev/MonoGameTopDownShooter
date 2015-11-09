using System;
using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public class Tank : Entity, ITank
    {
        public ITankBody Body { get; private set; }
        public ITankTower Tower { get; private set; }
        public ITankArmor Armor { get; private set; }

        public Vector2 ControlColumnPosition { get; set; }
        public event Action<ITank, DeathReason> Died;

        public Tank(ITankBody body, ITankTower tower)
        {
            Body = body;
            Tower = tower;
        }

        public override void Update(GameTime gameTime)
        {
            var elapsedSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
            Body.Update(elapsedSeconds);
            Tower.Update(elapsedSeconds);
            //Armor.Update(elapsedSeconds);
        }

        public override void Dispose()
        {
            Body.Dispose();
            Tower.Dispose();
            //Armor.Dispose();
            base.Dispose();
        }
    }
}
