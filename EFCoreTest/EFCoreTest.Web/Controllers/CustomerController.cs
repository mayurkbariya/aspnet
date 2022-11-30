using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EFCoreTest.BLL.Interfaces;
using EFCoreTest.DAL.EntityModels;

namespace EFCoreTest.Web.Controllers
{
    [Route("api/Customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        ICustomerManagerBAL customerBAL;
        public CustomerController(ICustomerManagerBAL _customerBAL)
        {
            customerBAL = _customerBAL;
        }

        /// <summary>
        /// Create Customer
        /// </summary>
        /// <param name="customerRec"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<IActionResult> CreateCustomer(Customers customerRec)
        {
            return Ok(await customerBAL.CreateCustomer(customerRec));
        }

        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="customerRec"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCustomer")]
        public async Task<IActionResult> UpdateCustomer(Customers customerRec)
        {
            return Ok(await customerBAL.UpdateCustomer(customerRec));
        }

        /// <summary>
        /// Get Customer by CustomerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCustomer/{customerId}")]
        public IActionResult GetCustomer(int customerId)
        {
            return Ok(customerBAL.GetCustomer(customerId));
        }
    }
}