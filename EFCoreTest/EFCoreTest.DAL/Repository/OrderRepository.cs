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
    public class OrderRepository : IOrderRepository
    {
        WideWorldImportersContext _dbContext = new WideWorldImportersContext();

        public async Task<int> CreateOrder(Orders ordersRec)
        {
            await _dbContext.Orders.AddAsync(ordersRec);
            await _dbContext.SaveChangesAsync();
            return ordersRec.OrderId;
        }

        public async Task<int> UpdateOrder(Orders ordersRec)
        {
            _dbContext.Orders.Update(ordersRec);
            await _dbContext.SaveChangesAsync();
            return ordersRec.OrderId;
        }

        public Orders GetCustomerOrder(int orderId)
        {
            var result = _dbContext.Orders.Where(x => x.OrderId == orderId).FirstOrDefault();
            return result;
        }
    }
}
