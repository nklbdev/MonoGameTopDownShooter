namespace GameProject
{
    public interface IMyDrawer : IMyDrawable
    {
        void AddDrawable(IMyDrawable drawable);
        void RemoveDrawable(IMyDrawable drawable);
    }
}