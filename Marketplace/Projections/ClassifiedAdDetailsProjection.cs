using Marketplace.ClassifiedAd;
using Marketplace.Domain.ClassifiedAd;
using Marketplace.Infrastructure;
using Raven.Client.Documents.Session;
using Serilog;
using static Marketplace.Infrastructure.ClassifiedAdUpcastedEvents;

namespace Marketplace.Projections
{
    public class ClassifiedAdDetailsProjection : RavenDbProjection<ReadModels.ClassifiedAdDetails>
    {
        private readonly Func<Guid, Task<string>> _getUserDisplayName;

        public ClassifiedAdDetailsProjection(Func<IAsyncDocumentSession> getSession,
            Func<Guid, Task<string>> getUserDisplayName) : base(getSession)
        {
            _getUserDisplayName = getUserDisplayName;   
        }

        public override Task Project(object @event)
        {
            switch (@event)
            {
                case Events.ClassifiedAddCreated e:
                    Log.Information("Creating classifiedAdd");
                    Create(
                       async() => 
                            new ReadModels.ClassifiedAdDetails
                            {
                                Id = e.Id.ToString(),
                                SellerId = e.OwnerId,
                                SellersDisplayName = await _getUserDisplayName(e.OwnerId)
                            }
                         );
                    break;
                case Events.ClassifiedAddTitleChanged e:
                    UpdateOne(e.Id, ad => ad.Title = e.Title);
                    break;
                case Events.ClassifiedAddTextChanged e:
                    UpdateOne(e.Id, ad => ad.Description = e.Text);
                    break;
                case Events.ClassifiedAddPriceChanged e:
                    UpdateOne(e.Id, ad =>
                    {
                        ad.Price = e.Price;
                        ad.CurrencyCode = e.CurrencyCode;
                    });
                    break;
                case Domain.UserProfile.Events.UserDisplayNameUpdated e:
                    UpdateWhere(add => add.SellerId == e.UserId,
                        add => add.SellersDisplayName = e.DisplayName);
                    break;
                case V1.ClassifiedAdPublished e:
                    UpdateOne(e.Id, ad => ad.SellersPhotoUrl = e.SellersPhotoUrl);
                    break;
            }
            return Task.CompletedTask;
        }
        
    }
}
