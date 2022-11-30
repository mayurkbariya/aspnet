using EFCoreTest.Common.Models;
using EFCoreTest.DAL.EntityModels;
using System.Threading.Tasks;

namespace EFCoreTest.BLL.Interfaces
{
    public interface ICustomerManagerBAL
    {
        Task<BusinessResult<int>> CreateCustomer(Customers ordersRec);
        Task<BusinessResult<int>> UpdateCustomer(Customers ordersRec);
        BusinessResult<Customers> GetCustomer(int customerId);
    }
}
