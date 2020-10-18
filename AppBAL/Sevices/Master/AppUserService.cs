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
    }
}
