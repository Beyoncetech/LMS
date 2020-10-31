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
        Task<Appuser> GetUserByID(long ID);
        Task Update(Appuser entity);
        Task<List<Appuser>> GetAllUser(int RowCount);
        Task<int> Insert(Appuser entity);
        Task<bool> FindOtherSameUserID(long ID, string UserID);
        Task<bool> Delete(long ID);
    }
    public class AppUserRepository : IAppUserRepository
    {
        private readonly AppDBContext _DBContext;
        private DbSet<Appuser> entities;
        public AppUserRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
            entities = _DBContext.Set<Appuser>();
        }
        public async Task<Appuser> GetUserByUserID(string UserID)
        {
            var oUser = await _DBContext.Appuser.Where(x => x.UserId.Equals(UserID)).FirstOrDefaultAsync();

            return oUser;            
        }
        public async Task<Appuser> GetUserByID(long ID)
        {
            var oUser = await _DBContext.Appuser.Where(x => x.Id.Equals(ID)).FirstOrDefaultAsync();

            return oUser;
        }
        public async Task Update(Appuser entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            entities.Attach(entity);
            _DBContext.Entry(entity).State = EntityState.Modified;
            await _DBContext.SaveChangesAsync();
        }
        public async Task<List<Appuser>> GetAllUser(int RowCount)
        {
            var oActivity = await _DBContext.Appuser
                .OrderByDescending(o => o.Name)
                .Take(RowCount)
                .ToListAsync();

            return oActivity;
        }
        public async Task<int> Insert(Appuser entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
                       

            entities.Add(entity);
            int result = await _DBContext.SaveChangesAsync();
            return result;
        }
        public async Task<bool> FindOtherSameUserID(long ID, string UserID)
        {
            var oUser = await _DBContext.Appuser.Where(x => x.Id != ID && x.UserId.Equals(UserID)).FirstOrDefaultAsync();

            if (oUser != null)
                return true;
            else
                return false;           
        }
        public async Task<bool> Delete(long ID)
        {            
            var Result = await _DBContext.Database.ExecuteSqlRawAsync(string.Format("Delete from Appuser where Id = {0};", ID.ToString()));

            if (Result > 0)
                return true;
            else
                return false;

        }
    }
}
