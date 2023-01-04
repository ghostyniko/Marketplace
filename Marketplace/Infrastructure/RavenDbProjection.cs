using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Serilog;
using System.Linq.Expressions;
using System.Text.Json;

namespace Marketplace.Infrastructure
{
    public abstract class RavenDbProjection<T> : IProjection
    {
        protected RavenDbProjection(
            Func<IAsyncDocumentSession> getSession)
            => GetSession = getSession;

        protected Func<IAsyncDocumentSession> GetSession { get; }
        public abstract Task Project(object @event);
       
        protected Task Create(Func<Task<T>> model)
        => UsingSession(
            async session =>
            {
                Log.Information("Creating a new instance of {type}",typeof(T));
                var createdModel = await model();
                Log.Information(JsonSerializer.Serialize(createdModel));
                await session.StoreAsync(createdModel);
            }

          );

        protected Task UpdateOne(Guid id, Action<T> update)
        => UsingSession(
        session =>
            UpdateItem(session, id, update)
          );

        protected Task UpdateWhere(Expression<Func<T, bool>> where, Action<T> update)
            => UsingSession(
                session => UpdateMultipleItems(session,where, update)
                );
        
        private async Task UpdateItem(IAsyncDocumentSession session, Guid id, Action<T> update)
        {
            var item =await session.LoadAsync<T>(id.ToString());
            if (item == null) return;
            update(item);

        }
        private async Task UpdateMultipleItems(IAsyncDocumentSession session,Expression<Func<T, bool>> query, Action<T> update)
        {
            var items = await session.Query<T>().Where(query).ToListAsync();
            foreach (var item in items)
            {
                update(item);
            }
        }
        private async Task UsingSession(Func<IAsyncDocumentSession,Task> operation)
        {
            var session = GetSession();
            await operation(session);
            await session.SaveChangesAsync();
        }
    }
}
