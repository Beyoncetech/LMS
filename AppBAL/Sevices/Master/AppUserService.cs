using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
using AppModel.ViewModel;
using AppUtility.AppEncription;
using AppUtility.AppIO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppBAL.Sevices.Master
{
    public interface IAppUserService
    {
        Task<CommonResponce> GetUserProfile(string UserID);
        Task<CommonResponce> UpdateAppUserProfileAsync(UserProfileVM oModel);
        Task<CommonResponce> ChangeProfilePasswordAsync(ChangeProfilePasswordVM oModel);
        Task<List<AppUserBM>> GetAllAppUsers(int RowCount, String AppRootPath);
        Task<AppUserVM> GetAppUserByID(int Id);
        Task<CommonResponce> SaveAppUserAsync(AppUserVM oModel, string ResetContext, DateTime PasswordValidity);
        Task<CommonResponce> DeleteAppUser(long Id);
        Task<CommonResponce> ResetUserPassAsync(UserResetVM oModel);
    }
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _DBUserRepository;
        private readonly IMapper _mapper;
        private readonly IEncriptionService _AppEncription;
        private readonly IDirectoryFileService _AppDirectoryFileService;
        public AppUserService(IAppUserRepository DBUserRepository, IMapper mapper, IEncriptionService AppEncription, IDirectoryFileService AppDirectoryFileService)
        {
            _DBUserRepository = DBUserRepository;
            _mapper = mapper;
            _AppEncription = AppEncription;
            _AppDirectoryFileService = AppDirectoryFileService;
        }
        public async Task<CommonResponce> GetUserProfile(string UserID)
        {
            bool isValid = false;
            UserProfile UserInfo = null;
            var oUser = await _DBUserRepository.GetUserByUserID(UserID).ConfigureAwait(false);

            if (oUser != null)
            {
                UserInfo = _mapper.Map<UserProfile>(oUser);
                isValid = true;
            }
            CommonResponce result = new CommonResponce
            {
                Stat = isValid,
                StatusMsg = "",
                StatusObj = UserInfo
            };
            return result;
        }

        public async Task<CommonResponce> UpdateAppUserProfileAsync(UserProfileVM oModel)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            var oUser = await _DBUserRepository.GetUserByID(oModel.Id).ConfigureAwait(false);
            if (oUser != null)
            {
                oUser.Name = oModel.UserName;
                oUser.Email = oModel.Email;
                oUser.Mobile = oModel.Mobile;
                oUser.Dob = oModel.Dob;

                await _DBUserRepository.Update(oUser).ConfigureAwait(false);
                result.Stat = true;
                result.StatusMsg = "Successfully updated application User";
            }
            else
            {
                result.StatusMsg = "Not a valid User";
            }

            return result;
        }

        public async Task<CommonResponce> ChangeProfilePasswordAsync(ChangeProfilePasswordVM oModel)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            var oUser = await _DBUserRepository.GetUserByID(oModel.Id).ConfigureAwait(false);
            if (oUser != null)
            {
                if (oUser.Password.Equals(_AppEncription.EncriptWithPrivateKey(oModel.OldPassword)))
                {
                    oUser.Password = _AppEncription.EncriptWithPrivateKey(oModel.NewPassword);

                    await _DBUserRepository.Update(oUser).ConfigureAwait(false);
                    result.Stat = true;
                    result.StatusMsg = "Successfully changed User password";
                }
                else
                {
                    result.StatusMsg = "Old Password not Matched";
                }
            }
            else
            {
                result.StatusMsg = "Not a valid User";
            }

            return result;
        }

        public async Task<List<AppUserBM>> GetAllAppUsers(int RowCount, String AppRootPath)
        {
            List<AppUserBM> result = new List<AppUserBM>();
            var oUsers = await _DBUserRepository.GetAllUser(RowCount).ConfigureAwait(false);

            if (oUsers != null && oUsers.Count > 0)
            {
                foreach (var item in oUsers)
                {
                    result.Add(new AppUserBM
                    {
                        Id = item.Id,
                        UserAvatar = _AppDirectoryFileService.GetAppUserAvatarPath(AppRootPath, item.UserId, item.Gender == null ? "M" : item.Gender),
                        Name = item.Name,
                        UserId = item.UserId,
                        UserType = item.UserType,
                        Gender = item.Gender,
                        Email = item.Email,
                        IsActive = item.IsActive,
                        Action = item.Id.ToString()
                    });
                };
            }
            return result;
        }

        public async Task<AppUserVM> GetAppUserByID(int Id)
        {
            AppUserVM result;
            var oUser = await _DBUserRepository.GetUserByID(Id).ConfigureAwait(false);

            if (oUser != null)
            {
                result = _mapper.Map<AppUserVM>(oUser);
            }
            else
            {
                result = null;
            }

            return result;
        }

        public async Task<CommonResponce> SaveAppUserAsync(AppUserVM oModel, string ResetContext, DateTime PasswordValidity)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            Appuser oUser = null;
            int RowEffect = 0;
            if (oModel.Id == 0) // for new user
            {
                oUser = await _DBUserRepository.GetUserByUserID(oModel.UserId).ConfigureAwait(false);
                if (oUser != null)
                    result.StatusMsg = "User ID is already in use.";
                else
                {
                    oUser = new Appuser
                    {
                        Id = oModel.Id,
                        Name = oModel.Name,
                        UserId = oModel.UserId,
                        UserType = oModel.UserType,
                        Gender = oModel.Gender,
                        IsActive = (ulong)(oModel.IsActive ? 1 : 0),
                        Mobile = oModel.Mobile,
                        Dob = oModel.Dob,
                        UserPerm = oModel.UserPerm,
                        Email = oModel.Email,
                        IsPassReset = 1,
                        ResetPassContext = ResetContext,
                        ResetPassValidity = PasswordValidity,
                        Password = "123"
                    };
                    RowEffect = await _DBUserRepository.Insert(oUser).ConfigureAwait(false);
                    if(RowEffect > 0)
                    {
                        result.Stat = true;
                        result.StatusMsg = "Successfully Save User.";
                        result.StatusObj = oUser.Id;
                    }
                    else
                        result.StatusMsg = "Error on saving user.";
                }
            } // for existing user
            else
            {
                bool IsSameUserIDFound = await _DBUserRepository.FindOtherSameUserID(oModel.Id, oModel.UserId).ConfigureAwait(false);
                if (IsSameUserIDFound)
                    result.StatusMsg = "User ID is already in use.";
                else
                {
                    oUser = await _DBUserRepository.GetUserByID(oModel.Id).ConfigureAwait(false);
                    if (oUser != null)
                    {
                        oUser.Name = oModel.Name;
                        oUser.UserId = oModel.UserId;
                        oUser.UserType = oModel.UserType;
                        oUser.Gender = oModel.Gender;
                        oUser.IsActive = (ulong)(oModel.IsActive ? 1 : 0);
                        oUser.Mobile = oModel.Mobile;
                        oUser.Dob = oModel.Dob;
                        oUser.UserPerm = oModel.UserPerm;
                        oUser.Email = oModel.Email;                        

                        await _DBUserRepository.Update(oUser).ConfigureAwait(false);
                        result.Stat = true;
                        result.StatusMsg = "Successfully Save User";
                    }
                    else
                    {
                        result.StatusMsg = "Error on saving user or user not valid";
                    }
                }
            }            

            return result;
        }

        public async Task<CommonResponce> DeleteAppUser(long Id)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "Error on deleting user" };
            result.Stat = await _DBUserRepository.Delete(Id).ConfigureAwait(false);
            if (result.Stat)
                result.StatusMsg = "Successfully deleted the user.";
            return result;
        }

        public async Task<CommonResponce> ResetUserPassAsync(UserResetVM oModel)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            try
            {
                Appuser oUser = await _DBUserRepository.GetUserByPassResetContext(oModel.UserResetContext).ConfigureAwait(false);
                if (oUser != null)
                {
                    if (oUser.ResetPassValidity < DateTime.Now)
                    {
                        result.StatusMsg = "Reset Password Link has expired";
                    }
                    else
                    {
                        if (oUser.IsActive == 1)
                        {
                            oUser.IsPassReset = 0;
                            oUser.ResetPassContext = "";
                            oUser.Password = _AppEncription.EncriptWithPrivateKey(oModel.Password);

                            await _DBUserRepository.Update(oUser).ConfigureAwait(false);

                            result.Stat = true;
                            result.StatusMsg = "Successfully reset user Password";
                        }
                        else
                            result.StatusMsg = "User not valid or expired";
                    }
                }
                else
                {
                    result.StatusMsg = "Not a valid password reset link.";
                }
            }
            catch (Exception)
            {
                result.StatusMsg = "Not a valid password reset link (ex).";
            }
                           
            return result;
        }
    }
}
