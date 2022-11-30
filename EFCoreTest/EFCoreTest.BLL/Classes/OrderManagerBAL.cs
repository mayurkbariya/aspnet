using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EFCoreTest.Common.Models;
using EFCoreTest.DAL.EntityModels;
using EFCoreTest.DAL.IRepository;
using EFCoreTest.BLL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EFCoreTest.BLL.Classes
{
    public class OrderManagerBAL : IOrderManagerBAL
    {
        IOrderRepository orderRepository;

        public OrderManagerBAL(IOrderRepository _orderRepository)
        {
            orderRepository = _orderRepository;
        }

        public async Task<BusinessResult<int>> CreateOrder(Orders ordersRec)
        {
            BusinessResult<int> result = new BusinessResult<int>();
            try
            {
                if (ordersRec != null)
                {
                    var rst = await orderRepository.CreateOrder(ordersRec);
                    result.Status = Common.Enums.ReturnStatus.OK;
                    result.Message = "Order addedd successfully";
                    result.Data = rst;
                }
                else
                {
                    result.Status = Common.Enums.ReturnStatus.BadRequest;
                    result.Message = "Requested Data should not be null";
                }
            }
            catch (Exception ex)
            {
                result.Status = Common.Enums.ReturnStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<BusinessResult<int>> UpdateOrder(Orders ordersRec)
        {
            BusinessResult<int> result = new BusinessResult<int>();
            try
            {
                if (ordersRec != null)
                {
                    var rst = await orderRepository.UpdateOrder(ordersRec);
                    result.Status = Common.Enums.ReturnStatus.OK;
                    result.Message = "Order updated successfully";
                    result.Data = rst;
                }
                else
                {
                    result.Status = Common.Enums.ReturnStatus.BadRequest;
                    result.Message = "Requested Data should not be null";
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                result.Status = Common.Enums.ReturnStatus.Error;
                result.Message = "The record you are working on has been modified by another user.Changes you have made have not been saved, please reload and resubmit.";
                // Re-Setting values to the new values updated by the other user
                #region 
                //foreach (var entry in ex.Entries)
                //{
                //    if (entry.Entity is Orders)
                //    {
                //        var proposedValues = entry.CurrentValues;
                //        var databaseValues = entry.GetDatabaseValues();

                //        foreach (var property in proposedValues.Properties)
                //        {
                //            var proposedValue = proposedValues[property];
                //            var databaseValue = databaseValues[property];
                //        }

                //        entry.OriginalValues.SetValues(databaseValues);
                //        result.Status = Common.Enums.ReturnStatus.Error;
                //        result.Message = "The record you are working on has been modified by another user.Changes you have made have not been saved, please reload and resubmit.";
                //    }
                //    else
                //    {
                //        result.Status = Common.Enums.ReturnStatus.Error;
                //        result.Message = "Don't know how to handle concurrency conflicts for "
                //            + entry.Metadata.Name;
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
                result.Status = Common.Enums.ReturnStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        public BusinessResult<Orders> GetCustomerOrder(int orderId)
        {
            BusinessResult<Orders> result = new BusinessResult<Orders>();
            try
            {
                var queryResult = orderRepository.GetCustomerOrder(orderId);
                if (queryResult != null)
                {
                    result.Data = queryResult;
                    result.Status = Common.Enums.ReturnStatus.OK;
                }
                else
                {
                    result.Status = Common.Enums.ReturnStatus.DataNotFound;
                    result.Message = "No order found for requested customer.";
                }
            }
            catch (Exception ex)
            {
                result.Status = Common.Enums.ReturnStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
