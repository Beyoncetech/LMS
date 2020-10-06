using AppDAL.DBRepository;
using AppModel;
using AppModel.ViewModel;
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
    }
    public class AppUserService : IAppUserService
    {
        private readonly IAppUserRepository _DBUserRepository;
        private readonly IMapper _mapper;
        public AppUserService(IAppUserRepository DBUserRepository, IMapper mapper)
        {
            _DBUserRepository = DBUserRepository;
            _mapper = mapper;
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
    }
}
