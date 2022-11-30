using AsiaCab.EnityModel;
using AsiaCab.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.Repository
{
    public class AdvanceBookingRpository : GenericRepository<AdvanceBooking>, IAdvanceBookingRpository
    {
        public AdvanceBookingRpository(AsiaCabWAContext dbContext)
             : base(dbContext)
        {
        }
    }
}
