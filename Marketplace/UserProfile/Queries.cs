using Raven.Client.Documents.Session;
using static Marketplace.UserProfile.ReadModels;

namespace Marketplace.UserProfile
{
    public static class Queries
    {
        public async static Task<UserDetails> GetUserDetails(
            this Func<IAsyncDocumentSession> getSession,
            Guid id
        )
        {
            using var session = getSession();
            return await session.LoadAsync<UserDetails>(id.ToString());
        }
    }
}
