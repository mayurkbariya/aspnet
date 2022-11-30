using EFCoreTest.Common.Models;
using EFCoreTest.DAL.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreTest.BLL.Interfaces
{
    public interface IOrderManagerBAL
    {
        Task<BusinessResult<int>> CreateOrder(Orders ordersRec);
        Task<BusinessResult<int>> UpdateOrder(Orders ordersRec);
        BusinessResult<Orders> GetCustomerOrder(int orderId);
    }
}
