﻿using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDAL.DBRepository
{
    public interface ITeacherRepository
    {
        Task<List<Tblmteacher>> GetAllTeachers(int RowCount);
        Task<Tblmteacher> GetTeacherByTeacherId(int TeacherID);
        Task<Tblmteacher> GetTeacherByRegNo(string RegNo);
        Task<Tblmteacher> GetTeacherByEmailID(string EmailID);
    }
    public class TeacherRepository:ITeacherRepository
    {
        private readonly AppDBContext _DBContext;
        public TeacherRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
        }
        public async Task<List<Tblmteacher>> GetAllTeachers(int RowCount)
        {
            var result = await _DBContext.Tblmteacher.OrderBy(o => o.Name).Take(RowCount).ToListAsync();
            return result;
        }

        public async Task<Tblmteacher> GetTeacherByTeacherId(int TeacherID)
        {
            var oTeacher = await _DBContext.Tblmteacher.Where(s => s.Id.Equals(TeacherID)).FirstOrDefaultAsync();
            return oTeacher;
        }

        public async Task<Tblmteacher> GetTeacherByRegNo(string RegNo)
        {
            var oTeacher = await _DBContext.Tblmteacher.Where(s => s.RegNo.Equals(RegNo)).FirstOrDefaultAsync();
            return oTeacher;
        }
        public async Task<Tblmteacher> GetTeacherByEmailID(string EmailID)
        {
            var oTeacher = await _DBContext.Tblmteacher.Where(s => s.Email.Equals(EmailID)).FirstOrDefaultAsync();
            return oTeacher;
        }
    }
}
