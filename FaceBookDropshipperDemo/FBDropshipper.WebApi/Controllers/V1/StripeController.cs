using System.Threading.Tasks;
using FBDropshipper.Application.Stripe.Commands.StripeWebHook;
using FBDropshipper.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class StripeController : BaseController
    {
        
        [HttpPost("webHook")]
        public async Task<StripeWebHookResponseModel> StripeWebHook()
        {
            Request.EnableBuffering();
            var json = await Request.Body.ReadString();
            var model = new StripeWebHookRequestModel()
            {
                Json = json
            };
            return await Mediator.Send(model);
        }
        
    }
}