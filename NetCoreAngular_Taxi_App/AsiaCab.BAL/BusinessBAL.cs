using AsiaCab.DataTransferLayer;
using AsiaCab.EnityModel;
using AsiaCab.IRepository;
using AsiaCab.Repository;
using AsiaCab_WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiaCab.BAL
{
    public class BusinessBAL
    {
        private readonly AsiaCabWAContext _dbContext;
        private readonly IBusinessRepository _businessRepository;

        public BusinessBAL()
        {
            _dbContext = new AsiaCabWAContext();
            _businessRepository = new BusinessRepository(_dbContext);
        }
        #region Business User
        public async Task<BusinessUserModel> Create(BusinessUserModel request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.UserName))
                {
                    request.UserName = string.IsNullOrEmpty(request.Email) ? request.PhoneNumber : request.Email;
                }
                CryptographyClass tempCryptography = new CryptographyClass()
                {

                    Text = string.Format("{0}{1}", request.UserName.ToLower().Trim(), request.Password.Trim()),
                    SALT = Encoding.UTF8.GetBytes(request.Password.Trim()),
                    PlainText = Encoding.UTF8.GetBytes(request.Password.Trim())

                };
                var encryptedPassword = await Hash.Encrypt(tempCryptography);
                var userBusinessuser = new BusinessUser()
                {
                    Password = encryptedPassword,
                    Compare = encryptedPassword,
                    SaltAes = tempCryptography.PlainText,
                    SaltHash = tempCryptography.SALT,
                    CompanyName = request.CompanyName,
                    BusinessType = request.BusinessType,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    CountryCode = request.CountryCode,
                    Email = request.Email,
                    //CalculationOfServiceFee = request.CalculationOfServiceFee,
                    //ServiceFee = request.ServiceFee,
                    //ServiceCharge = request.ServiceCharge,
                    MoreDetails = request.MoreDetails,
                    UserName = request.UserName,
                    CreateDate = DateTime.Now,
                    ActiveFlag = true,
                    AdminId = request.AdminId
                };
                var userAlredayExist = await _businessRepository.GetByUserName(userBusinessuser);

                if (userAlredayExist != null)
                {
                    userBusinessuser = userAlredayExist;
                }
                else
                {
                    await _businessRepository.Create(userBusinessuser);
                }

                return new BusinessUserModel()
                {
                    BusinessUserId = userBusinessuser.BusinessUserId,
                    CompanyName = userBusinessuser.CompanyName,
                    BusinessType = userBusinessuser.BusinessType,
                    Address = userBusinessuser.Address,
                    PhoneNumber = userBusinessuser.PhoneNumber,
                    CountryCode = userBusinessuser.CountryCode,
                    Email = userBusinessuser.Email,
                    //CalculationOfServiceFee = userBusinessuser.CalculationOfServiceFee,
                    //ServiceFee = userBusinessuser.ServiceFee,
                    //ServiceCharge = userBusinessuser.ServiceCharge,
                    MoreDetails = userBusinessuser.MoreDetails,
                    UserName = request.UserName,
                    CreateDate = userBusinessuser.CreateDate,
                    ActiveFlag = userBusinessuser.ActiveFlag,
                    //DeleteAnnouncement = userBusinessuser.DeleteAnnouncement,
                    Password = userBusinessuser.Password,
                    AdminId = userBusinessuser.AdminId
                };

            }
            catch (Exception ex)
            {

                throw new Exception("Error occured in create : Details : " + ex.Message);
            }

        }

        public async Task Delete(Guid businessId)
        {
            try
            {
                await _businessRepository.Delete(businessId);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in deleting business : Details : " + ex.Message);
            }
        }

        public async Task<BusinessUserModel> GetById(Guid moderatorId)
        {
            try
            {
                var userBusiness = await _businessRepository.GetByGuidIdAsync(moderatorId);
                if (userBusiness != null)
                {
                    return new BusinessUserModel()
                    {
                        BusinessUserId = userBusiness.BusinessUserId,
                        CompanyName = userBusiness.CompanyName,
                        BusinessType = userBusiness.BusinessType,
                        Address = userBusiness.Address,
                        PhoneNumber = userBusiness.PhoneNumber,
                        CountryCode = userBusiness.CountryCode,
                        Email = userBusiness.Email,
                        //CalculationOfServiceFee = userBusiness.CalculationOfServiceFee,
                        //ServiceFee = userBusiness.ServiceFee,
                        //ServiceCharge = userBusiness.ServiceCharge,
                        MoreDetails = userBusiness.MoreDetails,
                        UserName = userBusiness.UserName,
                        CreateDate = userBusiness.CreateDate,
                        ActiveFlag = userBusiness.ActiveFlag,
                        //  DeleteAnnouncement = userBusiness.DeleteAnnouncement,
                        Password = userBusiness.Password,
                        AdminId = userBusiness.AdminId
                    };
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Error in getting moderator details : Details : " + ex.Message);
            }
        }

        public async Task<BusinessUserModel> Update(BusinessUserModel request)
        {
            try
            {
                var businessUserInfo = await _businessRepository.GetByGuidIdAsync(request.BusinessUserId);
                var encryptedPassword = string.Empty;

                if (!string.IsNullOrEmpty(request.UserName) && request.UserName != "string" &&
                    !string.IsNullOrEmpty(request.Password) && request.Password != "string")

                {
                    string userName = request.UserName;
                    string password = request.Password;
                    CryptographyClass tempCryptography = new CryptographyClass()
                    {

                        Text = string.Format("{0}{1}", userName.ToLower().Trim(), password.Trim()),
                        SALT = Encoding.UTF8.GetBytes(password.Trim()),
                        PlainText = Encoding.UTF8.GetBytes(password.Trim())

                    };
                    encryptedPassword = await Hash.Encrypt(tempCryptography);
                }

                if (businessUserInfo != null)
                {
                    businessUserInfo.CompanyName = !string.IsNullOrEmpty(request.CompanyName) && request.CompanyName != "string" ? request.CompanyName : businessUserInfo.CompanyName;
                    businessUserInfo.BusinessType = request.BusinessType.HasValue ? request.BusinessType.Value : businessUserInfo.BusinessType;
                    businessUserInfo.Address = !string.IsNullOrEmpty(request.Address) && request.Address != "string" ? request.Address : businessUserInfo.Address;
                    businessUserInfo.Email = !string.IsNullOrEmpty(request.Email) && request.Email != "string" ? request.Email : businessUserInfo.Email;
                    businessUserInfo.PhoneNumber = !string.IsNullOrEmpty(request.PhoneNumber) && request.PhoneNumber != "string" ? request.PhoneNumber : businessUserInfo.PhoneNumber;
                    businessUserInfo.CountryCode = !string.IsNullOrEmpty(request.CountryCode) && request.CountryCode != "string" ? request.CountryCode : businessUserInfo.CountryCode;
                    //  businessUserInfo.CalculationOfServiceFee = request.CalculationOfServiceFee.HasValue ? request.CalculationOfServiceFee.Value : businessUserInfo.CalculationOfServiceFee;
                    //  businessUserInfo.ServiceFee = request.ServiceFee.HasValue ? request.ServiceFee.Value : businessUserInfo.ServiceFee;
                    //  businessUserInfo.ServiceCharge = request.ServiceCharge.HasValue ? request.ServiceCharge.Value : businessUserInfo.ServiceCharge;
                    businessUserInfo.MoreDetails = !string.IsNullOrEmpty(request.MoreDetails) && request.MoreDetails != "string" ? request.MoreDetails : businessUserInfo.MoreDetails;
                    //   businessUserInfo.DeleteAnnouncement = !string.IsNullOrEmpty(request.DeleteAnnouncement) && request.DeleteAnnouncement != "string" ? request.DeleteAnnouncement : businessUserInfo.DeleteAnnouncement;
                    businessUserInfo.UserName = !string.IsNullOrEmpty(request.UserName) && request.UserName != "string" ? request.UserName : businessUserInfo.UserName;
                    businessUserInfo.Password = !string.IsNullOrEmpty(encryptedPassword) ? encryptedPassword : businessUserInfo.Password;
                    businessUserInfo.AdminId = request.AdminId.HasValue && request.AdminId.Value.ToString() != "string" ? request.AdminId : businessUserInfo.AdminId;
                    businessUserInfo.ActiveFlag = request.ActiveFlag;
                    await _businessRepository.Update(businessUserInfo.BusinessUserId, businessUserInfo);

                    return new BusinessUserModel()
                    {
                        BusinessUserId = businessUserInfo.BusinessUserId,
                        CompanyName = businessUserInfo.CompanyName,
                        BusinessType = businessUserInfo.BusinessType,
                        Address = businessUserInfo.Address,
                        PhoneNumber = businessUserInfo.PhoneNumber,
                        CountryCode = businessUserInfo.CountryCode,
                        Email = businessUserInfo.Email,
                        //CalculationOfServiceFee = businessUserInfo.CalculationOfServiceFee,
                        //ServiceFee = businessUserInfo.ServiceFee,
                        //ServiceCharge = businessUserInfo.ServiceCharge,
                        MoreDetails = businessUserInfo.MoreDetails,
                        UserName = businessUserInfo.UserName,
                        CreateDate = businessUserInfo.CreateDate,
                        ActiveFlag = businessUserInfo.ActiveFlag,
                        //   DeleteAnnouncement = businessUserInfo.DeleteAnnouncement,
                        Password = businessUserInfo.Password,
                        AdminId = businessUserInfo.AdminId
                    };
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Error occured in updating Business user : Details :" + ex.Message);
            }
        }

        public async Task<List<BusinessUserModel>> GetAll(Guid adminId)
        {
            try
            {
                var result = await _businessRepository.GetAllByAdminId(adminId);
                List<BusinessUserModel> userList = new List<BusinessUserModel>();

                if (result != null && result.Count > 0)
                {
                    foreach (var item in result)
                    {
                        userList.Add(
                               new BusinessUserModel()
                               {
                                   BusinessUserId = item.BusinessUserId,
                                   Password = item.Password,
                                   CountryCode = item.CountryCode,
                                   //DeleteAnnouncement = item.DeleteAnnouncement,
                                   CompanyName = item.CompanyName,
                                   Address = item.Address,
                                   PhoneNumber = item.PhoneNumber,
                                   Email = item.Email,
                                   //CalculationOfServiceFee = item.CalculationOfServiceFee,
                                   //ServiceFee = item.ServiceFee,
                                   //ServiceCharge = item.ServiceCharge,
                                   MoreDetails = item.MoreDetails,
                                   UserName = item.UserName,
                                   ActiveFlag = item.ActiveFlag,
                                   AdminId = item.AdminId,
                                   BusinessType = item.BusinessType,
                                   CreateDate = item.CreateDate
                               }
                                );
                    }
                    return userList;
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Company Business User
        public async Task<CompanyBusinessUserModel> Create(CompanyBusinessUseRequest request)
        {
            try
            {

                if (string.IsNullOrEmpty(request.UserName))
                {
                    request.UserName = string.IsNullOrEmpty(request.Email) ? request.PhoneNumber : request.Email;
                }
                CryptographyClass tempCryptography = new CryptographyClass()
                {

                    Text = string.Format("{0}{1}", request.UserName.ToLower().Trim(), request.Password.Trim()),
                    SALT = Encoding.UTF8.GetBytes(request.Password.Trim()),
                    PlainText = Encoding.UTF8.GetBytes(request.Password.Trim())

                };
                var encryptedPassword = await Hash.Encrypt(tempCryptography);
                var userBusinessuser = new BusinessUser()
                {
                    Password = encryptedPassword,
                    Compare = encryptedPassword,
                    SaltAes = tempCryptography.PlainText,
                    SaltHash = tempCryptography.SALT,
                    CompanyName = request.CompanyName,
                    BusinessType = request.BusinessType,
                    Address = request.Address,
                    PhoneNumber = request.PhoneNumber,
                    CountryCode = request.CountryCode,
                    Email = request.Email,
                    MoreDetails = request.MoreDetails,
                    UserName = request.UserName,
                    CreateDate = DateTime.Now,
                    ActiveFlag = true,
                    AdminId = request.AdminId,
                    CompanyId = request.CompanyId,
                    IsDefault = request.IsDefault

                };
                var userAlredayExist = await _businessRepository.GetByUserName(userBusinessuser);

                if (userAlredayExist != null)
                {
                    userBusinessuser = userAlredayExist;
                }
                else
                {
                    await _businessRepository.Create(userBusinessuser);
                }

                return new CompanyBusinessUserModel()
                {
                    BusinessUserId = userBusinessuser.BusinessUserId,
                    CompanyName = userBusinessuser.CompanyName,
                    BusinessType = userBusinessuser.BusinessType,
                    Address = userBusinessuser.Address,
                    PhoneNumber = userBusinessuser.PhoneNumber,
                    CountryCode = userBusinessuser.CountryCode,
                    Email = userBusinessuser.Email,
                    MoreDetails = userBusinessuser.MoreDetails,
                    UserName = request.UserName,
                    CreateDate = userBusinessuser.CreateDate,
                    ActiveFlag = userBusinessuser.ActiveFlag,
                    Password = userBusinessuser.Password,
                    AdminId = userBusinessuser.AdminId,
                    CompanyId = userBusinessuser.CompanyId,
                    IsDefault = request.IsDefault
                };

            }
            catch (Exception ex)
            {

                throw new Exception("Error occured in create : Details : " + ex.Message);
            }

        }

        public async Task DeleteCompanyBusinessUser(Guid businessId)
        {
            try
            {
                var businessUserInfo = await _businessRepository.GetByGuidIdAsync(businessId);
                businessUserInfo.ActiveFlag = false;
                await _businessRepository.Update(businessId, businessUserInfo);
            }
            catch (Exception ex)
            {

                throw new Exception("Error in deleting business : Details : " + ex.Message);
            }
        }

        public async Task<CompanyBusinessUserModel> GetCompanyBusinessUserById(Guid moderatorId)
        {
            try
            {
                var userBusiness = await _businessRepository.GetByGuidIdAsync(moderatorId);
                if (userBusiness != null)
                {
                    return new CompanyBusinessUserModel()
                    {
                        BusinessUserId = userBusiness.BusinessUserId,
                        CompanyName = userBusiness.CompanyName,
                        BusinessType = userBusiness.BusinessType,
                        Address = userBusiness.Address,
                        PhoneNumber = userBusiness.PhoneNumber,
                        CountryCode = userBusiness.CountryCode,
                        Email = userBusiness.Email,
                        MoreDetails = userBusiness.MoreDetails,
                        UserName = userBusiness.UserName,
                        CreateDate = userBusiness.CreateDate,
                        ActiveFlag = userBusiness.ActiveFlag,
                        Password = userBusiness.Password,
                        AdminId = userBusiness.AdminId,
                        CompanyId = userBusiness.CompanyId
                    };
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Error in getting moderator details : Details : " + ex.Message);
            }
        }

        public async Task<CompanyBusinessUserModel> Update(CompanyBusinessUserModel request)
        {
            try
            {
                var businessUserInfo = await _businessRepository.GetByGuidIdAsync(request.BusinessUserId);
                var encryptedPassword = string.Empty;

                if (!string.IsNullOrEmpty(request.UserName) && request.UserName != "string" &&
                    !string.IsNullOrEmpty(request.Password) && request.Password != "string")

                {
                    string userName = request.UserName;
                    string password = request.Password;
                    CryptographyClass tempCryptography = new CryptographyClass()
                    {

                        Text = string.Format("{0}{1}", userName.ToLower().Trim(), password.Trim()),
                        SALT = Encoding.UTF8.GetBytes(password.Trim()),
                        PlainText = Encoding.UTF8.GetBytes(password.Trim())

                    };
                    encryptedPassword = await Hash.Encrypt(tempCryptography);
                }

                if (businessUserInfo != null)
                {
                    businessUserInfo.CompanyName = !string.IsNullOrEmpty(request.CompanyName) && request.CompanyName != "string" ? request.CompanyName : businessUserInfo.CompanyName;
                    businessUserInfo.BusinessType = request.BusinessType.HasValue ? request.BusinessType.Value : businessUserInfo.BusinessType;
                    businessUserInfo.Address = !string.IsNullOrEmpty(request.Address) && request.Address != "string" ? request.Address : businessUserInfo.Address;
                    businessUserInfo.Email = !string.IsNullOrEmpty(request.Email) && request.Email != "string" ? request.Email : businessUserInfo.Email;
                    businessUserInfo.PhoneNumber = !string.IsNullOrEmpty(request.PhoneNumber) && request.PhoneNumber != "string" ? request.PhoneNumber : businessUserInfo.PhoneNumber;
                    businessUserInfo.CountryCode = !string.IsNullOrEmpty(request.CountryCode) && request.CountryCode != "string" ? request.CountryCode : businessUserInfo.CountryCode;
                    businessUserInfo.MoreDetails = !string.IsNullOrEmpty(request.MoreDetails) && request.MoreDetails != "string" ? request.MoreDetails : businessUserInfo.MoreDetails;
                    businessUserInfo.UserName = !string.IsNullOrEmpty(request.UserName) && request.UserName != "string" ? request.UserName : businessUserInfo.UserName;
                    businessUserInfo.Password = !string.IsNullOrEmpty(encryptedPassword) ? encryptedPassword : businessUserInfo.Password;
                    businessUserInfo.AdminId = request.AdminId.HasValue && request.AdminId.Value.ToString() != "string" ? request.AdminId : businessUserInfo.AdminId;
                    businessUserInfo.ActiveFlag = request.ActiveFlag;
                    await _businessRepository.Update(businessUserInfo.BusinessUserId, businessUserInfo);

                    return new CompanyBusinessUserModel()
                    {
                        BusinessUserId = businessUserInfo.BusinessUserId,
                        CompanyName = businessUserInfo.CompanyName,
                        BusinessType = businessUserInfo.BusinessType,
                        Address = businessUserInfo.Address,
                        PhoneNumber = businessUserInfo.PhoneNumber,
                        CountryCode = businessUserInfo.CountryCode,
                        Email = businessUserInfo.Email,
                        MoreDetails = businessUserInfo.MoreDetails,
                        UserName = businessUserInfo.UserName,
                        CreateDate = businessUserInfo.CreateDate,
                        ActiveFlag = businessUserInfo.ActiveFlag,
                        Password = businessUserInfo.Password,
                        AdminId = businessUserInfo.AdminId,
                        CompanyId = businessUserInfo.CompanyId,
                        IsDefault = businessUserInfo.IsDefault
                    };
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("Error occured in updating Business user : Details :" + ex.Message);
            }
        }

        public async Task<List<CompanyBusinessUserModel>> GetAllCompanyBusinessUserByAdmin(Guid adminId)
        {
            try
            {
                var result = await _businessRepository.GetAllByAdminId(adminId);
                if (result != null && result.Any())
                {
                    return result.ToList().Select(item =>
                    {
                        return new CompanyBusinessUserModel()
                        {
                            BusinessUserId = item.BusinessUserId,
                            Password = item.Password,
                            CountryCode = item.CountryCode,
                            CompanyName = item.CompanyName,
                            Address = item.Address,
                            PhoneNumber = item.PhoneNumber,
                            Email = item.Email,
                            MoreDetails = item.MoreDetails,
                            UserName = item.UserName,
                            ActiveFlag = item.ActiveFlag,
                            AdminId = item.AdminId,
                            BusinessType = item.BusinessType,
                            CreateDate = item.CreateDate,
                            CompanyId = item.CompanyId,
                            IsDefault = item.IsDefault
                        };
                    }).ToList();

                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<CompanyBusinessUserModel>> GetAllCompanyBusinessUserByCompany(Guid companyId)
        {
            try
            {
                var result = await _businessRepository.GetAllByCompanyId(companyId);
                if (result != null && result.Any())
                {
                    return result.ToList().Select(item =>
                    {
                        return new CompanyBusinessUserModel()
                        {
                            BusinessUserId = item.BusinessUserId,
                            Password = item.Password,
                            CountryCode = item.CountryCode,
                            CompanyName = item.CompanyName,
                            Address = item.Address,
                            PhoneNumber = item.PhoneNumber,
                            Email = item.Email,
                            MoreDetails = item.MoreDetails,
                            UserName = item.UserName,
                            ActiveFlag = item.ActiveFlag,
                            AdminId = item.AdminId,
                            BusinessType = item.BusinessType,
                            CreateDate = item.CreateDate,
                            CompanyId = item.CompanyId,
                            IsDefault = item.IsDefault
                        };
                    }).ToList();

                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CompanyBusinessUserModel> GetAllBusiness()
        {
            try
            {
                var result = _businessRepository.GetAll();
                if (result != null && result.Any())
                {
                    result = result.Where(x => x.ActiveFlag == true);
                    if (result != null && result.Any())
                    {
                        return result.ToList().Select(item =>
                        {
                            return new CompanyBusinessUserModel()
                            {
                                BusinessUserId = item.BusinessUserId,
                                Password = item.Password,
                                CountryCode = item.CountryCode,
                                CompanyName = item.CompanyName,
                                Address = item.Address,
                                PhoneNumber = item.PhoneNumber,                               
                                Email = item.Email,
                                MoreDetails = item.MoreDetails,
                                UserName = item.UserName,
                                ActiveFlag = item.ActiveFlag,
                                AdminId = item.AdminId,
                                BusinessType = item.BusinessType,
                                CreateDate = item.CreateDate,
                                CompanyId = item.CompanyId,
                                IsDefault = item.IsDefault
                            };
                        }).ToList();
                    }
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

    }
}
