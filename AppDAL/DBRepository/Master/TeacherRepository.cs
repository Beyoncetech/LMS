using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDAL.DBRepository
{
    public interface ITeacherRepository
    {
        Task<List<Tblmteacher>> GetAllTeachers();
        Task<Tblmteacher> GetTeacherByTeacherId(int TeacherID);        
        Task<Tblmteacher> GetTeacherByEmailID(string EmailID);
    }
    public class TeacherRepository:ITeacherRepository
    {
        private readonly AppDBContext _DBContext;
        public TeacherRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
        }
        public async Task<List<Tblmteacher>> GetAllTeachers()
        {
            var result = await _DBContext.Tblmteacher.ToListAsync();
            return result;
        }
        public async Task<Tblmteacher> GetTeacherByTeacherId(int TeacherID)
        {
            var oTeacher = await _DBContext.Tblmteacher.Where(s => s.Id.Equals(TeacherID)).FirstOrDefaultAsync();
            return oTeacher;
        }
        public async Task<Tblmteacher> GetTeacherByEmailID(string EmailID)
        {
            var oTeacher = await _DBContext.Tblmteacher.Where(s => s.Email.Equals(EmailID)).FirstOrDefaultAsync();
            return oTeacher;
        }
    }
}
