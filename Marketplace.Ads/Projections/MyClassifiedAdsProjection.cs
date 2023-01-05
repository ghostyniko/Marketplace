using Raven.Client.Documents.Session;
using Marketplace.RavenDb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Marketplace.Ads.Projections.ReadModels;
using static Marketplace.Ads.Messages.Ads.Events;

namespace Marketplace.Ads.Projections
{
    public static class MyClassifiedAdsProjection
    {
        public static Func<Task> GetHandler(
            IAsyncDocumentSession session,
            object @event)
        {
            Func<Guid, string> getDbId = MyClassifiedAds.GetDatabaseId;

#pragma warning disable CS8603 // Possible null reference return.
            return @event switch
            {
                V1.ClassifiedAdCreated e =>
                    () => CreateOrUpdate(e.OwnerId,
                        myAds => myAds.MyAds.Add(
                            new MyClassifiedAds.MyAd { Id = e.Id }
                        ),
                        () => new MyClassifiedAds
                        {
                            Id = getDbId(e.OwnerId),
                            MyAds = new List<MyClassifiedAds.MyAd>()
                        }),
                V1.ClassifiedAdTitleChanged e =>
                    () => UpdateOneAd(e.OwnerId, e.Id,
                        myAd => myAd.Title = e.Title),
                V1.ClassifiedAdTextUpdated e =>
                    () => UpdateOneAd(e.OwnerId, e.Id,
                        myAd => myAd.Description = e.AdText),
                V1.ClassifiedAdPriceUpdated e =>
                    () => UpdateOneAd(e.OwnerId, e.Id,
                        myAd => myAd.Price = e.Price),
                V1.PictureAddedToAClassifiedAd e =>
                    () => UpdateOneAd(e.OwnerId, e.ClassifiedAdId,
                        myAd => myAd.PhotoUrls.Add(e.Url)),
                V1.ClassifiedAdDeleted e =>
                    () => Update(e.OwnerId,
                        myAd => myAd.MyAds
                            .RemoveAll(x => x.Id == e.Id)),
                _ => (Func<Task>)null
            };
#pragma warning restore CS8603 // Possible null reference return.

            Task CreateOrUpdate(
                Guid id,
                Action<MyClassifiedAds> update,
                Func<MyClassifiedAds> create
            )
                => session.UpsertItem(getDbId(id), update, create);

            Task Update(Guid id,
                Action<MyClassifiedAds> update)
                => session.Update(getDbId(id), update);

            Task UpdateOneAd(Guid id, Guid adId,
                Action<MyClassifiedAds.MyAd> update)
                => Update(id, myAds =>
                {
                    var ad = myAds.MyAds
                        .FirstOrDefault(x => x.Id == adId);
                    if (ad != null) update(ad);
                });
        }
    }
}
