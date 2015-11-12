using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public class Tank : EntityBase, ITank
    {
        public ITankBody Body { get; set; }
        public ITankTower Tower { get; set; }
        public ITankArmor Armor { get; set; }

        public Vector2 ControlColumnPosition { get; set; }

        protected override void OnUpdate(float deltaTime)
        {
            Body.Update(deltaTime);
            Tower.Update(deltaTime);
            //Armor.Update(elapsedSeconds);
        }

        protected override void OnDestroy()
        {
            Body.Destroy();
            Tower.Destroy();
            //Armor.Dispose();
        }
    }
}
