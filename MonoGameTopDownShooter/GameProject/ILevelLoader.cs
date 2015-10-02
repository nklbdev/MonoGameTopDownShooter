namespace GameProject
{
    public interface ILevelLoader
    {
        IMyComponent LoadLevel(string resourceName);
    }
}