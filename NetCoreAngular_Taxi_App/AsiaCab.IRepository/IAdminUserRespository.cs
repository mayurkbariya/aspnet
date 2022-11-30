using AsiaCab.EnityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.IRepository
{
    public interface IAdminUserRespository : IGenericRepository<AdminUser>
    {
        AdminUser GetByUserName(AdminUser adminuser);
        AdminUser AdminUserLogin(AdminUser adminuser);
    }
}
