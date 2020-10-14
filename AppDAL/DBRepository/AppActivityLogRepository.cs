using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;
using AppModel;

namespace AppDAL.DBRepository
{
    public interface IAppActivityLogRepository
    {
        Task<List<Activitylog>> GetAllUnRead();
        Task<List<Activitylog>> GetAll(int RowCount);
        Task<bool> MarkAllActivityAsRead();
        Task InsertActivity(ActivitylogBM entity);
    }
    public class AppActivityLogRepository : IAppActivityLogRepository
    {
        private readonly AppDBContext _DBContext;
        private DbSet<Activitylog> entities;
        public AppActivityLogRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
            entities = _DBContext.Set<Activitylog>();
        }
        public async Task<List<Activitylog>> GetAllUnRead()
        {
            var oActivity = await _DBContext.Activitylog.Where(x => x.IsRead == false)
                .OrderByDescending(o => o.ActivityTime)
                .ToListAsync();

            return oActivity;
        }
        public async Task<List<Activitylog>> GetAll(int RowCount)
        {
            var oActivity = await _DBContext.Activitylog
                .OrderByDescending(o => o.ActivityTime)
                .Take(RowCount)
                .ToListAsync();

            return oActivity;
        }
        public async Task<bool> MarkAllActivityAsRead()
        {           
            var Result = await _DBContext.Database.ExecuteSqlRawAsync("Update Activitylog set IsRead = '1' where IsRead = '0';");

            if (Result > 0)
                return true;
            else
                return false;

        }

        public async Task InsertActivity(ActivitylogBM entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            Activitylog DBEntity = new Activitylog
            {
                Id = entity.Id,
                ActivityType = entity.ActivityType,
                ActivityTime = entity.ActivityTime,
                Description = entity.Description,
                UserId = entity.UserId,
                UserName = entity.UserName,
                Origin = entity.Origin,
                IsRead = entity.IsRead
            };

            entities.Add(DBEntity);
            await _DBContext.SaveChangesAsync();
        }
    }
}
