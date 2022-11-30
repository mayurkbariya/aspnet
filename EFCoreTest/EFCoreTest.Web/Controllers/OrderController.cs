using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EFCoreTest.BLL.Interfaces;
using EFCoreTest.DAL.EntityModels;

namespace EFCoreTest.Web.Controllers
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        IOrderManagerBAL orderBAL;
        public OrderController(IOrderManagerBAL _orderBAL)
        {
            orderBAL = _orderBAL;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public async Task<IActionResult> CreateOrder(Orders ordersRec)
        {
            return Ok(await orderBAL.CreateOrder(ordersRec));
        }

        [HttpPost]
        [Route("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(Orders ordersRec)
        {
            return Ok(await orderBAL.UpdateOrder(ordersRec));
        }

        [HttpGet]
        [Route("GetCustomerOrder/{orderId}")]
        public IActionResult GetCustomerOrder(int orderId)
        {
            return Ok(orderBAL.GetCustomerOrder(orderId));
        }

    }
}