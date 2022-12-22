using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents.Session;
using Serilog;
using System.Net;
using ILogger = Serilog.ILogger;

namespace Marketplace.ClassifiedAd
{
    [Route("/ad")]
    public class ClassifiedAdsQueryApi : Controller
    {
        private readonly IAsyncDocumentSession _session;
        public ClassifiedAdsQueryApi(IAsyncDocumentSession session)
        => _session = session;

        private static readonly ILogger _log =
                Serilog.Log.ForContext<ClassifiedAdsQueryApi>();

        [HttpGet]
        [Route("list")]
        public Task<IActionResult> Get(QueryModels.GetPublishedClassifiedAds request)
        {
            return RequestHandler.HandleQuery(()=>_session.Query(request), _log);
           
        }

        [HttpGet]
        [Route("myads")]
        public Task<IActionResult> Get(QueryModels.GetOwnersClassifiedAd request)
        {
            return RequestHandler.HandleQuery(() => _session.Query(request), _log);
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public Task<IActionResult> Get(QueryModels.GetPublicClassifiedAd request)
        {
            return RequestHandler.HandleQuery(() => _session.Query(request), _log);

        }


    }
}
