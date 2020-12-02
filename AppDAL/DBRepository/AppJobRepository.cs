using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.DBRepository
{    
    public interface IAppJobRepository
    {
        Task<List<Mjob>> GetAllMailJob();           
        Task Insert(Mjob entity);
        Task Update(Mjob entity);
    }
    public class AppJobRepository : IAppJobRepository
    {
        private readonly AppDBContext _DBContext;
        private DbSet<Mjob> entities;
        public AppJobRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
            entities = _DBContext.Set<Mjob>();
        }
        public async Task<List<Mjob>> GetAllMailJob()
        {
            var oJob = await _DBContext.Mjob.Where(x => x.Status == 0 && x.Command.Equals("ScheduleMail"))
                .OrderBy(o => o.CreatedOn)
                .ToListAsync();

            return  oJob;
        }

        public async Task Insert(Mjob entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
                        
            entities.Add(entity);
            await _DBContext.SaveChangesAsync();
        }

        public async Task Update(Mjob entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            entities.Attach(entity);
            _DBContext.Entry(entity).State = EntityState.Modified;
            await _DBContext.SaveChangesAsync();
        }
    }
}
