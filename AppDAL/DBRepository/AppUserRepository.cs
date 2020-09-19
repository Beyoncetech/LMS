using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.DBRepository
{
    public interface IAppUserRepository
    {
        Task<Appuser> GetUserByUserID(string UserID);
    }
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDBContext _DBContext;        
        public AppUserRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;            
        }
        public async Task<Appuser> GetUserByUserID(string UserID)
        {
            var oUser = await _DBContext.Appuser.Where(x => x.UserName.Equals(UserID)).FirstOrDefaultAsync();

            return oUser;            
        }
    }
}
