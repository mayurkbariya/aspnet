﻿using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ticketing_api.Data;
using ticketing_api.Models;
using ticketing_api.Models.Views;

namespace ticketing_api.Services
{
    public class RigLocationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationSieveProcessor _sieveProcessor;

        public RigLocationService(ApplicationDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = (ApplicationSieveProcessor)sieveProcessor;
        }

        public virtual async Task<PagingResults<RigLocationView>> GetRigLocationViewAsync(SieveModel sieveModel)
        {
            var query = _context.RigLocation.Join(_context.Customer,
                               riglocation => riglocation.CustomerId,
                               customer => customer.Id,
                               (riglocation, customer) => new RigLocationView
                               {
                                   Id = riglocation.Id,
                                   CustomerId = customer,
                                   Name = riglocation.Name,
                                   Note = riglocation.Note,
                                   IsVisible = riglocation.IsVisible,
                                   IsEnabled = riglocation.IsEnabled,
                                   IsDeleted = riglocation.IsDeleted
                               }).AsQueryable();
            var data = await _sieveProcessor.GetPagingDataAsync(sieveModel, query);
            return data;
        }

        public RigLocationView PostRigLocation(RigLocation rigLocation)
        {
            RigLocationView rigLocationView = _context.RigLocation.Join(_context.Customer,
                             riglocation => riglocation.CustomerId,
                             customer => customer.Id,
                             (riglocation, customer) => new RigLocationView
                             {
                                 Id = riglocation.Id,
                                 CustomerId = customer,
                                 Name = riglocation.Name,
                                 Note = riglocation.Note,
                                 IsVisible = riglocation.IsVisible,
                                 IsEnabled = riglocation.IsEnabled,
                                 IsDeleted = riglocation.IsDeleted
                             }).FirstOrDefault(o => o.Id == rigLocation.Id);

            return rigLocationView;
        }
    }
}
