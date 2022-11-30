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
using ticketing_api.Infrastructure;

namespace ticketing_api.Services
{
    public class WellService
    {
        private readonly ApplicationDbContext _context;
        private readonly ApplicationSieveProcessor _sieveProcessor;

        public WellService(ApplicationDbContext context, ISieveProcessor sieveProcessor)
        {
            _context = context;
            _sieveProcessor = (ApplicationSieveProcessor)sieveProcessor;
        }

        public virtual async Task<PagingResults<WellView>> GetWellViewAsync(SieveModel sieveModel)
        {
            var query = _context.Well.Join(_context.RigLocation,
                               well => well.RigLocationId,
                               rigLocation => rigLocation.Id,
                               (well, rigLocation) => new WellView
                               {
                                   Id = well.Id,
                                   Name = well.Name,
                                   RigLocationId = rigLocation,
                                   Direction = well.Direction,
                                   IsVisible = well.IsVisible,
                                   IsEnabled = well.IsEnabled,
                                   IsDeleted = well.IsDeleted
                               }).AsQueryable();

            var data = await _sieveProcessor.GetPagingDataAsync(sieveModel, query);
            return data;
        }

        public WellView PostWell(Well well)
        {
            WellView wellView = _context.Well.Join(_context.RigLocation,
                               well1 => well.RigLocationId,
                               rigLocation => rigLocation.Id,
                               (well1, rigLocation) => new WellView
                               {
                                   Id = well.Id,
                                   Name = well.Name,
                                   RigLocationId = rigLocation,
                                   Direction = well.Direction,
                                   IsVisible = well.IsVisible,
                                   IsEnabled = well.IsEnabled,
                                   IsDeleted = well.IsDeleted
                               }).FirstOrDefault(w => w.Id == well.Id);

            return wellView;
        }
    }
}


