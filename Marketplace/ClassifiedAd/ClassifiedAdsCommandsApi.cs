using System.Threading.Tasks;
using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using static Marketplace.ClassifiedAd.Contracts;
using ILogger = Serilog.ILogger;

namespace Marketplace.ClassifiedAd
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly ClassifiedAdApplicationService
            _applicationService;

        private static readonly ILogger Log =
                Serilog.Log.ForContext<ClassifiedAdsCommandsApi>();

        public ClassifiedAdsCommandsApi(
            ClassifiedAdApplicationService applicationService)
                => _applicationService = applicationService;

        [HttpPost]
        public async Task<IActionResult> Post(V1.Create request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);

        }

        [Route("name")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.SetTitle request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);

        }

        [Route("text")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateText request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);

        }

        [Route("price")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdatePrice request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);

        }

        [Route("requestpublish")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.RequestToPublish request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request),Log);
        }
        [Route("publish")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.Publish request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);
        }

    }
}

