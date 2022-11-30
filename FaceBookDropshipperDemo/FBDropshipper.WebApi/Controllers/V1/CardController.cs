using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.Cards.Commands.UpdateCard;
using FBDropshipper.Application.Cards.Queries.GetCard;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class CardController : BaseController
    {
        ///<summary>
        /// Get Card
        ///</summary>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpGet]
        public async Task<GetCardResponseModel> GetCard(CancellationToken token)
        {
            var model = new GetCardRequestModel()
            {
                
            };
            return await Mediator.Send(model, token);
        }



        ///<summary>
        /// Update Card
        ///</summary>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpPut]
        public async Task<UpdateCardResponseModel> UpdateCard(UpdateCardRequestModel model,
            CancellationToken token)
        {
            return await Mediator.Send(model, token);
        }

    }
}