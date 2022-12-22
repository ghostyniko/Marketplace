using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Queries;
using Raven.Client.Documents.Session;
using static Marketplace.ClassifiedAd.ReadModels;
using static Marketplace.Domain.ClassifiedAd.ClassifiedAdd;

namespace Marketplace.ClassifiedAd
{
    public static class Queries
    {
        public static Task<List<ClassifiedAdListItem>> Query(this IAsyncDocumentSession session,
            QueryModels.GetPublishedClassifiedAds query)
        {
            return session.Query<Domain.ClassifiedAd.ClassifiedAdd>()
                .Where(x => x.State == ClassifiedAdState.Active)
                .Select(x => new ClassifiedAdListItem
                {
                    ClassifiedAdId = x.Id.Value,
                    CurrencyCode = x.Price.Currency.CurrencyCode,
                    Price = x.Price.Amount,
                    Title = x.Title.Value,
                })
                .PagedList(query.Page, query.PageSize);
        }

        public static Task<List<ClassifiedAdListItem>> Query(this IAsyncDocumentSession session,
           QueryModels.GetOwnersClassifiedAd query)
        {
            return session.Query<Domain.ClassifiedAd.ClassifiedAdd>()
                .Where(x => x.OwnerId.Value == query.OwnerId)
                .Select(x => new ClassifiedAdListItem
                {
                    ClassifiedAdId = x.Id.Value,
                    CurrencyCode = x.Price.Currency.CurrencyCode,
                    Price = x.Price.Amount,
                    Title = x.Title.Value,
                })
                .PagedList(query.Page, query.PageSize);
        }

        public static Task<ClassifiedAdDetails> Query(this IAsyncDocumentSession session,
           QueryModels.GetPublicClassifiedAd query)
        {
            return (from ad in session.Query<Domain.ClassifiedAd.ClassifiedAdd>()
             where ad.Id.Value == query.ClassifiedAdId
             let user = RavenQuery
                .Load<Domain.UserProfile.UserProfile>(
                        "Userprofile/" + ad.OwnerId.Value
                        )
             select new ClassifiedAdDetails
             {
                 ClassifiedAdId = ad.Id.Value,
                 CurrencyCode = ad.Price.Currency.CurrencyCode,
                 Description = ad.Text.Value,
                 Price = ad.Price.Amount,
                 SellersDisplayName = user.DisplayName.Value,
                 Title = ad.Title.Value
             }).SingleAsync();
        }

        private static async Task<List<T>> PagedList<T>
            (this IRavenQueryable<T> query, int page, int pageSize)
        {
            return await query.Skip(page*pageSize).Take(pageSize).ToListAsync();
        } 
    }
}
