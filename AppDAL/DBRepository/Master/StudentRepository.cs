using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppDAL.DBModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AppDAL.DBRepository
{
    public interface IStudentRepository 
    {
        Task<List<Tblmstudent>> GetAllStudents(int RowCount);
        Task<Tblmstudent> GetStudentByStudentId(int StudentID);
        Task<Tblmstudent> GetStudentByRegNo(int RegNo);
        Task<Tblmstudent> GetStudentByEmailID(string EmailID);
    }
    public class StudentRepository:IStudentRepository
    {
        private readonly AppDBContext _DBContext;
        public StudentRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
        }
        public async Task<List<Tblmstudent>> GetAllStudents(int RowCount)
        {
            var result = await _DBContext.Tblmstudent.OrderBy(o =>o.Name).Take(RowCount).ToListAsync();
            return result;
        }
        public async Task<Tblmstudent> GetStudentByStudentId(int StudentID)
        {
            var oStudent = await _DBContext.Tblmstudent.Where(s => s.Id.Equals(StudentID)).FirstOrDefaultAsync();
            return oStudent;
        }
        public async Task<Tblmstudent> GetStudentByRegNo(int RegNo)
        {
            var oStudent = await _DBContext.Tblmstudent.Where(s => s.RegNo.Equals(RegNo)).FirstOrDefaultAsync();
            return oStudent;
        }
        public async Task<Tblmstudent> GetStudentByEmailID(string EmailID)
        {
            var oStudent = await _DBContext.Tblmstudent.Where(s => s.Email.Equals(EmailID)).FirstOrDefaultAsync();
            return oStudent;
        }

    }
}
