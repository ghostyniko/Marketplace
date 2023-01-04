
using Marketplace.Domain.UserProfile;
using Marketplace.Infrastructure;
using Raven.Client.Documents.Session;
using static Marketplace.UserProfile.ReadModels;

namespace Marketplace.Projections
{
    public class UserDetailsProjection :RavenDbProjection<UserDetails>
    {
        public UserDetailsProjection(Func<IAsyncDocumentSession> getSession) : base(getSession)
        {
        }

        //List<UserDetails> _items;
        //public UserDetailsProjection(
        //List<UserDetails> items)
        //{
        //    _items = items;
        //}

        public override Task Project(object @event)
        {
            switch (@event)
            {
                case Events.UserRegistered e:
                    Create(() =>
                   Task.FromResult(
                       new UserDetails
                       {
                           Id = e.UserId.ToString(),
                           DisplayName = e.DisplayName
                       }
                       ));
                    break;

                case Events.UserDisplayNameUpdated e:
                    UpdateOne(e.UserId,userDetail=>userDetail.DisplayName = e.DisplayName);
                    break;
                
            }
            return Task.CompletedTask;
        }
        
    }
}
