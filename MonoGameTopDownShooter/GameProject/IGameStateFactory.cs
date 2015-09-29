using XTiled;

namespace GameProject
{
    public interface IGameStateFactory
    {
        IGameState CreateLevelState(Map map);
    }
}