using Marketplace.Modules.ClassifiedAds;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Ads.ClassifiedAds
{
    [ApiController, Route("/ad")]
    public class ClassifiedAdsQueryApi : ControllerBase
    {
        readonly Func<IAsyncDocumentSession> _getSession;

        public ClassifiedAdsQueryApi(Func<IAsyncDocumentSession> getSession)
            => _getSession = getSession;

        [HttpGet]
        public Task<ActionResult<ReadModels.ClassifiedAdDetails>> Get(
            [FromQuery] QueryModels.GetPublicClassifiedAd request)
            => _getSession.RunApiQuery(s => s.Query(request));
    }
}
