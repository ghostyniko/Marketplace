using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Modules.ClassifiedAds
{
    public static class Queries
    {
        public static Task<ReadModels.ClassifiedAdDetails> Query(
            this IAsyncDocumentSession session,
            QueryModels.GetPublicClassifiedAd query)
            => session.LoadAsync<ReadModels.ClassifiedAdDetails>(
                query.ClassifiedAdId.ToString()
            );
    }
}
