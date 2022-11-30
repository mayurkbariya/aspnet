using FBDropshipper.Application.Orders.Commands.CreateOrder;
using FBDropshipper.Application.Orders.Commands.DeleteOrder;
using FBDropshipper.Application.Orders.Commands.DeleteOrderBulk;
using FBDropshipper.Application.Orders.Commands.UpdateOrder;
using FBDropshipper.Application.Orders.Queries.GetOrderById;
using FBDropshipper.Application.Orders.Queries.GetOrders;
using FBDropshipper.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FBDropshipper.WebApi.Controllers.V1;

public class OrderController : BaseController
{
    /// <summary>
    /// Get Orders
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Orders.View)]
    [HttpGet]
    public async Task<GetOrdersResponseModel> GetOrder([FromQuery] GetOrdersRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Get Order By Id
    /// </summary>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Orders.View)]
    [HttpGet("{id:int}")]
    public async Task<GetOrderByIdResponseModel> GetOrderById([FromRoute] int id)
    {
        var model = new GetOrderByIdRequestModel()
        {
            Id = id
        };
        return await Mediator.Send(model);
    }
  

    /// <summary>
    /// Create Order
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Orders.Insert)]
    [HttpPost]
    public async Task<CreateOrderResponseModel> CreateOrder(CreateOrderRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Delete Order
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Orders.Delete)]
    [HttpDelete("{id:int}")]
    public async Task<DeleteOrderResponseModel> DeleteOrder([FromRoute] int id)
    {
        var model = new DeleteOrderRequestModel()
        {
            Id = id,
        };
        return await Mediator.Send(model);
    }
    
    /// <summary>
    /// Delete Orders in Bulk
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Orders.Delete)]
    [HttpPost("delete/bulk")]
    public async Task<DeleteOrderBulkResponseModel> DeleteOrder(DeleteOrderBulkRequestModel model)
    {
        return await Mediator.Send(model);
    }

    /// <summary>
    /// Update Order
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [Authorize(Policy = AppPolicy.Orders.Update)]
    [HttpPut("{id:int}")]
    public async Task<UpdateOrderResponseModel> UpdateOrder([FromRoute] int id, UpdateOrderRequestModel model)
    {
        model.Id = id;
        return await Mediator.Send(model);
    }
}