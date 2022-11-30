using EFCoreTest.Common.Models;
using EFCoreTest.DAL.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreTest.DAL.IRepository
{
    public interface IOrderRepository
    {
          Task<int> CreateOrder(Orders order);
          Task<int> UpdateOrder(Orders order);
          Orders GetCustomerOrder(int customerId);
    }
}
