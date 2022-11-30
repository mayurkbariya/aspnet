using AsiaCab.DataTransferLayer;
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
    public class AdminUserBAL
    {
        private readonly AsiaCabWAContext _dbContext;
        private readonly IAdminUserRespository _adminUserRepository;

        public AdminUserBAL()
        {
            _dbContext = new AsiaCabWAContext();
            _adminUserRepository = new AdminUserRespository(_dbContext);
        }

        public async Task<AdminUserResponseModel> Create(AdminUserModel request)
        {
            try
            {
                 
                CryptographyClass tempCryptography = new CryptographyClass()
                {

                    Text = string.Format("{0}{1}", request.AdminName.ToLower().Trim(), request.Password.Trim()),
                    SALT = Encoding.UTF8.GetBytes(request.Password.Trim()),
                    PlainText = Encoding.UTF8.GetBytes(request.Password.Trim())

                };
                var encryptedPassword = await Hash.Encrypt(tempCryptography);
                var adminUser = new AdminUser()
                {
                    Password = encryptedPassword,
                    Compare = encryptedPassword,
                    SaltAes = tempCryptography.PlainText,
                    SaltHash = tempCryptography.SALT,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    CountryCode = request.CountryCode,
                    Email = request.Email,
                    Description = request.Description,
                    AdminName = request.AdminName,
                    CreateDate = DateTime.Now,
                    ActiveFlag = true,
                    AdminType = request.AdminType
                };
                var adminUserAlredayExist =  _adminUserRepository.GetByUserName(adminUser);

                if (adminUserAlredayExist != null)
                {
                    adminUser = adminUserAlredayExist;
                }
                else
                {
                    await _adminUserRepository.Create(adminUser);
                }

                return new AdminUserResponseModel()
                {
                    Id = adminUser.Id,
                    AdminName = adminUser.AdminName,
                    AdminType = adminUser.AdminType,
                    Address = adminUser.Address,
                    ActiveFlag = adminUser.ActiveFlag,
                    CountryCode = adminUser.CountryCode,
                    Description = adminUser.Description,
                    Email = adminUser.Email,
                    PhoneNumber = adminUser.PhoneNumber,
                    Password = adminUser.Password
                };

            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
