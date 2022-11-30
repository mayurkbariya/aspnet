using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsiaCab.BAL;
using AsiaCab.DataTransferLayer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AsiaCab_WebAPI.Controllers
{
    [ApiVersion("2.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [EnableCors("MyAllowSpecificOrigins")]
    public class AdminLoginController : ControllerBase
    {
        private Appsetting _appSettings;
        public AdminLoginController(IOptions<Appsetting> appsettings)
        {

            _appSettings = appsettings.Value;

        }


        [HttpPost("AdminUserLogin"), MapToApiVersion("2.0")]
        public async Task<IActionResult> AdminUserLogin(AdminUserLoginModel model)
        {
            try
            {

                if (string.IsNullOrEmpty(model.AdminName) || model.AdminName == "string" ||
                    string.IsNullOrEmpty(model.Password) || model.Password == "string")
                {
                    return Ok(new ResponseModel { Message = CommonMessage.InvalidPostedData, Status = APIStatus.Error, Data = null });
                }

                var _loginBAL = new LoginBAL();

                var adminUser = await _loginBAL.GetAdminLogin(model);
                if (adminUser != null)
                {
                    //using JWT
                    var adminUserTokenModel = new AdminUserTokenModel() { AdminName = adminUser.AdminName, Id = adminUser.Id, AdminType = adminUser.AdminType.HasValue ? adminUser.AdminType.Value : 0};
                    TokenManager TKmgr = new TokenManager(_appSettings);
                    adminUser.Token = TKmgr.GenAdminUserToken(adminUserTokenModel);
                    if (!string.IsNullOrEmpty(adminUser.Token))
                    {

                        var loginResponse = new AdminUserLoginResponseModel()
                        {
                            Id = adminUser.Id,
                            AdminName = adminUser.AdminName,
                            Email = adminUser.Email,
                            Address = adminUser.Address,
                            CountryCode = adminUser.CountryCode,
                            PhoneNumber = adminUser.PhoneNumber,
                            Token = adminUser.Token,
                            AdminType = adminUser.AdminType,
                            ActiveFlag = adminUser.ActiveFlag,
                            Description = adminUser.Description
                        };

                        return Ok(new ResponseModel { Message = CommonMessage.Successfully, Status = APIStatus.Successfull, Data = loginResponse });
                    }
                    return Ok(new ResponseModel { Message = CommonMessage.NoRecordFound, Status = APIStatus.Error, });

                }
                else
                    return Ok(new ResponseModel { Message = CommonMessage.InvalidPostedData, Status = APIStatus.Error, });
            }
            catch (Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error, });
            }
        }

        
        [HttpPost("AdminUserRegistration"), MapToApiVersion("2.0")]
        public async Task<IActionResult> AdminUserRegistration(AdminUserModel request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email)
                    || string.IsNullOrEmpty(request.PhoneNumber)
                    || request.Email == "string"
                    || request.PhoneNumber == "string"
                    || request.AdminName == "string"
                    || string.IsNullOrEmpty(request.AdminName)
                    || string.IsNullOrEmpty(request.Password))
                {
                    return Ok(new ResponseModel { Message = CommonMessage.InvalidPostedData, Status = APIStatus.Error, Data = null });
                }

                CryptographyClass tempCryptography = new CryptographyClass()
                {

                    Text = string.Format("{0}{1}", request.AdminName.ToLower().Trim(), request.Password.Trim()),
                    SALT = Encoding.UTF8.GetBytes(request.Password.Trim()),
                    PlainText = Encoding.UTF8.GetBytes(request.Password.Trim())

                };

                var encryptedPassword = await Hash.Encrypt(tempCryptography);
                var adminUserBAL = new AdminUserBAL();
                var adminUser = new AdminUserModel()
                {
                    Password = request.Password,
                    CountryCode = request.CountryCode,
                    AdminType = request.AdminType,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    Description = request.Description,
                    AdminName = request.AdminName,
                    ActiveFlag = true,
                };
                var adminUserModelResponse = await adminUserBAL.Create(adminUser);

                //using JWT
                TokenManager TKmgr = new TokenManager(_appSettings);
                var adminUserTokenModel = new AdminUserTokenModel() { AdminName = adminUser.AdminName, Id = adminUserModelResponse.Id, AdminType = adminUser.AdminType.Value };

               string token  = TKmgr.GenAdminUserToken(adminUserTokenModel);

                if (!string.IsNullOrEmpty(token))
                {
                    adminUserModelResponse.Token = token;
                    return Ok(new ResponseModel { Message = CommonMessage.Successfully, Status = APIStatus.Successfull, Data = adminUserModelResponse });
                }
                else
                    return Ok(new ResponseModel { Message = CommonMessage.InvalidPostedData, Status = APIStatus.Error, Data = null });


            }
            catch (Exception ex)
            {

                throw new Exception("Error Details :" + ex.Message);
            }
        }
       
    }
}