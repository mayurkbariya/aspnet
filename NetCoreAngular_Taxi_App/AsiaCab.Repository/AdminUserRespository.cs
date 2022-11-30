using AsiaCab.EnityModel;
using AsiaCab.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.Repository
{
    public class AdminUserRespository : GenericRepository<AdminUser>, IAdminUserRespository
    {
        public AdminUserRespository(AsiaCabWAContext dbContext)
             : base(dbContext)
        {
        }
         
        public AdminUser GetByUserName(AdminUser adminuser)
        {
            var adminuserList = GetAll();
            if (adminuserList.Any())
                return adminuserList.Where(x => x.AdminName == adminuser.AdminName && x.ActiveFlag == true).FirstOrDefault();
            return null;
        }

        public AdminUser AdminUserLogin(AdminUser adminuser)
        {
            var adminUserlist = GetAll();
            if (adminUserlist.Any())
                return adminUserlist.Where(x => x.AdminName == adminuser.AdminName && x.ActiveFlag == true).FirstOrDefault();
            return null; ;
        }
    }
}
