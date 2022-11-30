using System;
using System.Text;
using System.Threading.Tasks;
using AsiaCab.BAL;
using AsiaCab.DataTransferLayer;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class BusinessController : ControllerBase
    {
        private Appsetting _appSettings;
        public BusinessController(IOptions<Appsetting> appsettings)
        {
            // Setup appsettings.json Supakit
            _appSettings = appsettings.Value;

        }

        [HttpDelete("DeleteBusiness"), MapToApiVersion("2.0")]
        public async Task<IActionResult> DeleteAnnouncement(Guid businessId)
        {
            try
            {
                if (businessId == null || businessId.ToString() == "string" || !Guid.TryParse(businessId.ToString(), out businessId))
                {
                    return Ok(new ResponseModel { Message = CommonMessage.InvalidPostedData, Status = APIStatus.Error, Data = null });
                }

                var businessBAL = new BusinessBAL();
                await businessBAL.Delete(businessId);
                return Ok(new ResponseModel { Message = CommonMessage.DeletedSuccessfully, Status = APIStatus.Successfull, Data = null });

            }

            catch (System.Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }
        }

        [HttpGet("GetBusinessById"), MapToApiVersion("2.0")]
        public async Task<IActionResult> GetBusinessById(Guid businessId)
        {
            try
            {
                if (businessId == null || businessId.ToString() == "string" || !Guid.TryParse(businessId.ToString(), out businessId))
                {
                    return Ok(new ResponseModel { Message = CommonMessage.InvalidPostedData, Status = APIStatus.Error, Data = null });
                }

                var businessBAL = new BusinessBAL();
                var businessInfo = await businessBAL.GetById(businessId);
                if (businessInfo != null)
                {
                    return Ok(new ResponseModel { Message = CommonMessage.Successfully, Status = APIStatus.Successfull, Data = businessInfo });
                }
                return Ok(new ResponseModel { Message = CommonMessage.NoRecordFound, Status = APIStatus.Successfull, Data = null });

            }

            catch (System.Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }
        }


        [HttpPost("UpdateBusiness"), MapToApiVersion("2.0")]
        public async Task<IActionResult> UpdateBusiness(BusinessUpdateRequest request)
        {
            try
            {
                Guid businessUserId;

                if (request.BusinessUserId == null || request.BusinessUserId.ToString() == "string" ||
                    !Guid.TryParse(request.BusinessUserId.ToString(), out businessUserId))
                {
                    return Ok(new ResponseModel { Message = CommonMessage.InvalidPostedData, Status = APIStatus.Error, Data = null });
                }

                BusinessUserModel model = new BusinessUserModel()
                {   BusinessUserId = request.BusinessUserId,
                    Password = request.Password,
                    CountryCode = request.CountryCode,
                  //  DeleteAnnouncement = request.DeleteAnnouncement,
                    CompanyName = request.CompanyName,
                    BusinessType = request.BusinessType,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    Email = request.Email,
                    //CalculationOfServiceFee = request.CalculationOfServiceFee,
                    //ServiceFee = request.ServiceFee,
                    //ServiceCharge = request.ServiceCharge,
                    MoreDetails = request.MoreDetails,
                    UserName = request.UserName,
                    ActiveFlag = request.ActiveFlag,
                    AdminId = request.AdminId 
                };

                var businessBAL = new BusinessBAL();
                var businessInfo = await businessBAL.Update(model);

                if (businessInfo != null)
                {
                    return Ok(new ResponseModel { Message = CommonMessage.Successfully, Status = APIStatus.Successfull, Data = businessInfo });
                }
                return Ok(new ResponseModel { Message = CommonMessage.NoRecordFound, Status = APIStatus.Successfull, Data = null });

            }

            catch (System.Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }
        }

        [HttpPost("GetAllBusiness"), MapToApiVersion("2.0")]
        public async Task<IActionResult> GetAllBusiness(Guid adminId)
        {
            try
            {
                if (adminId == null || adminId.ToString() == "string" ||
                    !Guid.TryParse(adminId.ToString(), out adminId))
                {
                    return Ok(new ResponseModel { Message = CommonMessage.InvalidPostedData, Status = APIStatus.Error, Data = null });
                }               

                var businessBAL = new BusinessBAL();
                var businessInfo = await businessBAL.GetAll(adminId);

                if (businessInfo != null)
                {
                    return Ok(new ResponseModel { Message = CommonMessage.Successfully, Status = APIStatus.Successfull, Data = businessInfo });
                }
                return Ok(new ResponseModel { Message = CommonMessage.NoRecordFound, Status = APIStatus.Successfull, Data = null });

            }

            catch (System.Exception ex)
            {
                return Ok(new ResponseModel { Message = ex.Message, Status = APIStatus.Error, Data = null });
            }
        }
    }
}
