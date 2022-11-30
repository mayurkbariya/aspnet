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
    public class CustomerManagerBAL : ICustomerManagerBAL
    {
        ICustomerRepository customerRepository;

        public CustomerManagerBAL(ICustomerRepository _customerRepository)
        {
            customerRepository = _customerRepository;
        }

        public async Task<BusinessResult<int>> CreateCustomer(Customers customerRec)
        {
            BusinessResult<int> result = new BusinessResult<int>();
            try
            {
                if (customerRec != null)
                {
                    var rst = await customerRepository.CreateCustomer(customerRec);
                    result.Status = Common.Enums.ReturnStatus.OK;
                    result.Message = "Customer addedd successfully";
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

        public async Task<BusinessResult<int>> UpdateCustomer(Customers customerRec)
        {
            BusinessResult<int> result = new BusinessResult<int>();
            try
            {
                if (customerRec != null)
                {
                    var rst = await customerRepository.UpdateCustomer(customerRec);
                    result.Status = Common.Enums.ReturnStatus.OK;
                    result.Message = "Customer updated successfully";
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
                foreach (var entry in ex.Entries)
                {
                    if (entry.Entity is Customers)
                    {
                        var proposedValues = entry.CurrentValues;
                        var databaseValues = entry.GetDatabaseValues();

                        foreach (var property in proposedValues.Properties)
                        {
                            var proposedValue = proposedValues[property];
                            var databaseValue = databaseValues[property];
                        }

                        entry.OriginalValues.SetValues(databaseValues);
                        result.Status = Common.Enums.ReturnStatus.Error;
                        result.Message = "The record you are working on has been modified by another user.Changes you have made have not been saved, please reload and resubmit.";
                    }
                    else
                    {
                        result.Status = Common.Enums.ReturnStatus.Error;
                        result.Message = "Don't know how to handle concurrency conflicts for "
                            + entry.Metadata.Name;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Status = Common.Enums.ReturnStatus.Error;
                result.Message = ex.Message;
            }
            return result;
        }

        public BusinessResult<Customers> GetCustomer(int customerId)
        {
            BusinessResult<Customers> result = new BusinessResult<Customers>();
            try
            {
                var queryResult = customerRepository.GetCustomer(customerId);
                if (queryResult != null)
                {
                    result.Data = queryResult;
                    result.Status = Common.Enums.ReturnStatus.OK;
                }
                else
                {
                    result.Status = Common.Enums.ReturnStatus.DataNotFound;
                    result.Message = "No requested customer found.";
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
