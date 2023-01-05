using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.WebApi
{
    public static class ControllerBaseExtensions
    {
        public static async Task<ActionResult> HandleCommand(
            this ControllerBase _,
            Task handler)
        {
            try
            {
                await handler;
                return new OkResult();
            }
            catch (Exception e)
            {
                return new BadRequestObjectResult(
                    new
                    {
                        error = e.Message,
                        stackTrace = e.StackTrace
                    }
                );
            }
        }
    }
}
