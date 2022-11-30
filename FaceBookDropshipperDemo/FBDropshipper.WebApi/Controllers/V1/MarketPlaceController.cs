using System.Threading;
using System.Threading.Tasks;
using FBDropshipper.Application.MarketPlaces.Commands.CreateMarketPlace;
using FBDropshipper.Application.MarketPlaces.Commands.DeleteMarketPlace;
using FBDropshipper.Application.MarketPlaces.Commands.UpdateMarketPlace;
using FBDropshipper.Application.MarketPlaces.Queries.GetMarketPlaces;
using FBDropshipper.Domain.Constant;
using FBDropshipper.WebApi.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1
{
    public class MarketPlaceController : BaseController
    {
        ///<summary>
        /// Get MarketPlace
        ///</summary>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpGet]
        public async Task<GetMarketPlacesResponseModel> GetMarketPlace([FromQuery] GetMarketPlacesRequestModel model, CancellationToken token)
        {
            return await Mediator.Send(model, token);
        }




        ///<summary>
        /// Create MarketPlace
        ///</summary>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpPost]
        public async Task<CreateMarketPlaceResponseModel> CreateMarketPlace(CreateMarketPlaceRequestModel model, CancellationToken token)
        {
            return await Mediator.Send(model, token);
        }


        ///<summary>
        /// Update MarketPlace
        ///</summary>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpPut("{id}")]
        public async Task<UpdateMarketPlaceResponseModel> UpdateMarketPlace([FromRoute] int id, UpdateMarketPlaceRequestModel model,
            CancellationToken token)
        {
            model.Id = id;
            return await Mediator.Send(model, token);
        }


        ///<summary>
        /// Delete MarketPlace
        ///</summary>
        [Authorize(Roles = RoleNames.TeamLeader)]
        [HttpDelete("{id}")]
        public async Task<DeleteMarketPlaceResponseModel> DeleteMarketPlace([FromRoute] int id, CancellationToken token)
        {
            var model = new DeleteMarketPlaceRequestModel {Id = id};
            return await Mediator.Send(model, token);
        }
        
    }
}