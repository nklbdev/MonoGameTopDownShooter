using Microsoft.Xna.Framework;

namespace GameProject.Entities
{
    public class Tank : NewEntityBase, ITank
    {
        public ITankBody Body { get; private set; }
        public ITankTower Tower { get; private set; }
        public ITankArmor Armor { get; private set; }

        public Vector2 ControlColumnPosition { get; set; }

        public Tank(ITankBody body, ITankTower tower)
        {
            Body = body;
            Tower = tower;
        }

        public override void OnUpdate(float deltaTime)
        {
            Body.Update(deltaTime);
            Tower.Update(deltaTime);
            //Armor.Update(elapsedSeconds);
        }

        public override void OnDestroy()
        {
            Body.Destroy();
            Tower.Destroy();
            //Armor.Dispose();
        }
    }
}
