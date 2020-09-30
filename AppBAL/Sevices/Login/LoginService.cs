using AppModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AppDAL.DBRepository;
using AppDAL.DBModels;
using AutoMapper;

namespace AppBAL.Sevices.Login
{
    public interface ILoginService
    {
        Task<CommonResponce> ValidateUser(string UserID, string Password);
    }
    public class LoginService : ILoginService
    {         
        private readonly IAppUserRepository _DBUserRepository;
        private readonly IMapper _mapper;
        public LoginService(IAppUserRepository DBUserRepository, IMapper mapper)
        {
            _DBUserRepository = DBUserRepository;
            _mapper = mapper;
        }
        public async Task<CommonResponce> ValidateUser(string UserID, string Password)
        {
            bool isValid = false;
            LoginUser UserInfo = null;
            var oUser = await _DBUserRepository.GetUserByUserID(UserID).ConfigureAwait(false);

            if (oUser != null)
            {
                if (oUser.Password.Equals(Password) && oUser.IsActive.Equals(1))
                    isValid = true;

                UserInfo = _mapper.Map<LoginUser>(oUser);
            }
            CommonResponce result = new CommonResponce
            {
                Stat = isValid,
                StatusMsg = "",
                StatusObj = UserInfo
            };
            return result;
        }
    }
}
