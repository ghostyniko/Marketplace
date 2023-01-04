using EventStore.ClientAPI;
using Marketplace.Infrastructure;
using static Marketplace.Domain.ClassifiedAd.Events;
using static Marketplace.Infrastructure.ClassifiedAdUpcastedEvents;

namespace Marketplace.Projections
{
    public class ClassifiedAdUpcasters : IProjection
    {
        private readonly IEventStoreConnection _connection;
        private readonly Func<Guid, string> _getUserPhoto;

        private const string StreamName = "UpcastedClassifiedAdEvents";

        public ClassifiedAdUpcasters(IEventStoreConnection connection, Func<Guid, string> getUserPhoto)
        {
            _connection = connection;
            _getUserPhoto = getUserPhoto;
        }

        public async Task Project(object @event)
        {
            switch (@event)
            {
                case ClassifiedAddApproved e:
                    var photoUrl = _getUserPhoto(e.OwnerId);
                    var newEvent = new V1.ClassifiedAdPublished
                    {
                        Id = e.Id,
                        OwnerId = e.OwnerId,
                        ApprovedBy = e.ApprovedBy,
                        SellersPhotoUrl = photoUrl,
                    };
                    await _connection.AppendEvents(
                            StreamName,
                            ExpectedVersion.Any,
                            newEvent);
                    break;
            }
            
        }
    }
}
