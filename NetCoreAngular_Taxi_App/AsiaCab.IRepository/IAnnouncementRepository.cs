using AsiaCab.DataTransferLayer;
using AsiaCab.EnityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.IRepository
{
    public interface IAnnouncementRepository : IGenericRepository<Announcement>
    {
        Task<Announcement> GetAnnouncement(Announcement announcement);
        Task<List<Announcement>> GetByType(AnnouncementGetByTypeRequest request);
    }
}
