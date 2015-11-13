namespace GameProject.Domain
{
    public enum MatchType
    {
        Deathmatch,
        Domination,
        CaptureTheFlag,
        Assault
    }

    public interface IScreenFactory
    {
        IScreen CreateMainMenuScreen();
        //and others
        IScreen CreateMatchScreen(MatchType type, string mapName);
    }
}
