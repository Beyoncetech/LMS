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
    public interface IAppSettingRepository
    {
        void InitDBContext();
        List<Appsetting> GetSettingByKeySync(string Key);
        Task<List<Appsetting>> GetSettingByKey(string Key);        
        Task<int> InsertSetting(string Key, string Value);
        Task<int> UpdateSetting(Appsetting entity);
        Task<int> DeleteSettingByKey(string Key);

    }
    public class AppSettingRepository : IAppSettingRepository
    {
        private AppDBContext _DBContext;
        private DbSet<Appsetting> entities;
        public AppSettingRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
            entities = _DBContext.Set<Appsetting>();
        }
        public void InitDBContext()
        {
            _DBContext = new AppDBContext();
        }
        public List<Appsetting> GetSettingByKeySync(string Key)
        {
            var oSetting = _DBContext.Appsetting.Where(x => x.AppKey.Equals(Key))
                .ToList();

            return oSetting;
        }
        public async Task<List<Appsetting>> GetSettingByKey(string Key)
        {
            var oSetting = await _DBContext.Appsetting.Where(x => x.AppKey.Equals(Key))                
                .ToListAsync();

            return oSetting;
        }
        

        public async Task<int> InsertSetting(string Key, string Value)
        {
            int result = 0;
            if (string.IsNullOrEmpty(Key)) throw new ArgumentNullException("entity");

            Appsetting DBEntity = new Appsetting
            {
                Id = 0,
                AppKey = Key,
                AppVal = Value
            };

            entities.Add(DBEntity);
            result = await _DBContext.SaveChangesAsync();
            return result;
        }

        public async Task<int> UpdateSetting(Appsetting entity)
        {
            int result = 0;
            if (entity == null) throw new ArgumentNullException("entity");

            entities.Attach(entity);
            _DBContext.Entry(entity).State = EntityState.Modified;
            result = await _DBContext.SaveChangesAsync();
                       
            return result;
        }

        public async Task<int> DeleteSettingByKey(string Key)
        {
            int NoOfRowEff;
            string TempQuery = string.Format("Delete from Appsetting where AppKey = '{0}';", Key);
            NoOfRowEff = await _DBContext.Database.ExecuteSqlRawAsync(TempQuery);

            return NoOfRowEff;
        }
    }
}
