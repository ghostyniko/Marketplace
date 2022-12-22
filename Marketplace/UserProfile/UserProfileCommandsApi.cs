using Marketplace.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using static Marketplace.UserProfile.Contracts;
using ILogger = Serilog.ILogger;

namespace Marketplace.UserProfile
{
    [Route("/profile")]
    public class UserProfileCommandsApi:Controller
    {
        private readonly UserProfileApplicationService
            _applicationService;

        private static readonly ILogger Log =
                Serilog.Log.ForContext<UserProfileCommandsApi>();

        public UserProfileCommandsApi(
            UserProfileApplicationService applicationService)
                => _applicationService = applicationService;

        [HttpPost]
        public async Task<IActionResult> Post(V1.RegisterUser request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);
        }

        [Route("fullname")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateUserFullName request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);

        }

        [Route("displayname")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateUserDisplayName request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);

        }

        [Route("photo")]
        [HttpPut]
        public async Task<IActionResult> Put(V1.UpdateUserProfilePhoto request)
        {
            return await RequestHandler.HandleRequest(request, _ => _applicationService.Handle(request), Log);

        }

    }
}
