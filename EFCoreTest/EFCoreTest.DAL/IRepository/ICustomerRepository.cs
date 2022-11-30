using EFCoreTest.Common.Models;
using EFCoreTest.DAL.EntityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreTest.DAL.IRepository
{
    public interface ICustomerRepository
    {
          Task<int> CreateCustomer(Customers order);
          Task<int> UpdateCustomer(Customers order);
          Customers GetCustomer(int customerId);
    }
}
