using AsiaCab.DataTransferLayer;
using AsiaCab.EnityModel;
using AsiaCab.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.Repository
{
    public class AnnouncementRepository : GenericRepository<Announcement>, IAnnouncementRepository
    {
        public AnnouncementRepository(AsiaCabWAContext dbContext)
             : base(dbContext)
        {
        }

        public async Task<Announcement> GetAnnouncement(Announcement announcement)
        {
            var announcementList = GetAll();
            return announcementList.Where(x => x.Topic == announcement.Topic && x.Type == announcement.Type).FirstOrDefault();
        }

        public async Task<List<Announcement>> GetByType(AnnouncementGetByTypeRequest request)
        {
            var announcementList = GetAll();
            return announcementList.Where(x => x.Type == request.Type && x.AdminId == request.AdminUserId).ToList();
        }
    }
}
