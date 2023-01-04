namespace Marketplace.Infrastructure
{
    public interface IProjection
    {
        Task Project(object @event);
    }
}
