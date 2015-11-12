using GameProject.Entities.Models.Tanks.Components;

namespace GameProject.Entities.Models.Tanks
{
    public class Tank : EntityBase, ITank
    {
        public ITankBody Body { get; set; }
        public ITankTower Tower { get; set; }
        public ITankArmor Armor { get; set; }

        protected override void OnUpdate(float deltaTime)
        {
            Body.Update(deltaTime);
            Tower.Update(deltaTime);
            //Armor.Update(deltaTime);
        }

        protected override void OnDestroy()
        {
            Body.Destroy();
            //Armor.Dispose();
        }
    }
}
