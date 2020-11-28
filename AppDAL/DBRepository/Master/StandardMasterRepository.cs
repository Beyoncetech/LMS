using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppDAL.DBRepository
{
    public interface IStandardMasterRepository
    {
        Task<List<Tblmstandard>> GetAllStandards(int RowCount);
        Task<Tblmstandard> GetStandardByStandardID(int StandardID);
        Task<Tblmstandard> GetStandardByStandardName(string StandardName);
    }
    public class StandardMasterRepository:IStandardMasterRepository
    {
        private readonly AppDBContext _DBContext;

        public StandardMasterRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
        }
        public async Task<List<Tblmstandard>> GetAllStandards(int RowCount)
        {
            var result = await _DBContext.Tblmstandard.OrderBy(o=>o.Name).Take(RowCount).ToListAsync();
            return result;
        }

        public async Task<Tblmstandard> GetStandardByStandardID(int StandardID)
        {
            var result = await _DBContext.Tblmstandard.Where(s => s.Id.Equals(StandardID)).FirstOrDefaultAsync();
            return result;
        }

        public async Task<Tblmstandard> GetStandardByStandardName(string StandardName)
        {
            var result = await _DBContext.Tblmstandard.Where(s => s.Name.Equals(StandardName)).FirstOrDefaultAsync();
            return result;
        }
    }
}
