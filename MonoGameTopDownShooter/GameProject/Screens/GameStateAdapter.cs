using Microsoft.Xna.Framework;

namespace GameProject.Infrastructure
{
    public class GameStateAdapter : IScreen
    {
        public IUpdateable UpdateStepUpdateable { get; set; }
        public IUpdateable DrawStepUpdateable { get; set; }
        public IDrawable Drawable { get; set; }

        public void Update(GameTime gameTime)
        {
            if (UpdateStepUpdateable != null)
                UpdateStepUpdateable.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void Draw(GameTime gameTime)
        {
            if (DrawStepUpdateable != null)
                DrawStepUpdateable.Update((float) gameTime.ElapsedGameTime.TotalSeconds);
            if (Drawable != null)
                Drawable.Draw();
        }
    }
}