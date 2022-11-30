using AsiaCab.EnityModel;
using AsiaCab.IRepository;
using AsiaCab.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.BAL
{
    public class BannerBAL
    {
        private readonly AsiaCabWAContext _dbContext;
        private readonly IBannerRepository _bannerRepository;

        public BannerBAL()
        {
            _dbContext = new AsiaCabWAContext();
            _bannerRepository = new BannerRepository(_dbContext);
        }
        public List<Banner> GetAllBanner()
        {
            try
            {
                var list = new List<Banner>();
                
                //list.Add(new Banner() { Id = Guid.NewGuid().ToString(), ActiveFlag = true, ImageName = "bgForHome.png", ImageUrl = "/images/bgForHome.png" });
                // list.Add(new Banner() { Id = Guid.NewGuid().ToString(), ActiveFlag = true, ImageName = "picturemessag.png", ImageUrl = "/images/picturemessag.png" });
                // list.Add(new Banner() { Id = Guid.NewGuid().ToString(), ActiveFlag = true, ImageName = "picturemessage.png", ImageUrl = "/images/picturemessage.png" });
                 
                //return list;
                return _bannerRepository.GetAll().ToList();

            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
