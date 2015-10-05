using Microsoft.Xna.Framework;

namespace GameProject
{
    public interface IEntity : IObservableDisposable
    {
        Ticker Ticker { get; set; }
        void Update(GameTime gameTime);
    }
}