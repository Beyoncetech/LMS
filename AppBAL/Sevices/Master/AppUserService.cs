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
            try
            {
                //MUser oDBUser = await GetEntityAsync<MUser>(x => x.ID.Equals(oModel.ID)).ConfigureAwait(false);
                //if (oDBUser == null)
                //    throw new Exception("Invalid User.");

                //oDBUser.FirstName = oModel.Name;
                //oDBUser.Email = oModel.Email;
                //oDBUser.MobNo = oModel.MobNo;
                //oDBUser.DOB = oModel.DOB;
                //oDBUser.HintQuestion = oModel.HintQuestion;
                //oDBUser.HintAnswer = oModel.HintAnswer;

                //Update(oDBUser);

                //Insert(AppActivityResolver.GetUpdateActivity(CurrentUser.UserID, "Profile update", string.Format("Profile {0} has updated", oModel.UserID)));

                //result.Stat = true;
                //result.Msg = "Successfully updated application User";
            }
            catch (Exception ex)
            {
                //Insert(AppErrorLogResolver.GetExceptionErrorLog(CurrentUser.UserID, RepositoryName, ex.Message));
                result.Stat = false;
                result.StatusMsg = ex.Message;
            }

            return result;
        }
    }
}
