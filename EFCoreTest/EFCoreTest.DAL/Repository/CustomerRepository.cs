using EFCoreTest.Common.Models;
using EFCoreTest.DAL.EntityModels;
using EFCoreTest.DAL.IRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTest.DAL.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        WideWorldImportersContext _dbContext = new WideWorldImportersContext();

        public async Task<int> CreateCustomer(Customers customerRec)
        {
            await _dbContext.Customers.AddAsync(customerRec);
            await _dbContext.SaveChangesAsync();
            return customerRec.CustomerId;
        }

        public async Task<int> UpdateCustomer(Customers customerRec)
        {
            _dbContext.Customers.Update(customerRec);
            await _dbContext.SaveChangesAsync();
            return customerRec.CustomerId;
        }

        public Customers GetCustomer(int customerId)
        {
            var result = _dbContext.Customers.Where(x => x.CustomerId == customerId).FirstOrDefault();
            return result;
        }
    }
}
