using Marketplace.Domain.Shared;
using Marketplace.Domain.UserProfile;
using Marketplace.Infrastructure;
using Raven.Client.Documents.Session;

namespace Marketplace.UserProfile
{
    public class UserProfileRepository : RavenDbRepository<Marketplace.Domain.UserProfile.UserProfile, UserId>,IUserProfileRepository
    {
        public UserProfileRepository(IAsyncDocumentSession session) : base(session, userId=>$"Userprofile/{userId.Value}")
        {
        }
    }
}
