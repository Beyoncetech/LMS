﻿using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDAL.DBRepository
{
    public interface IClassroomRepository
    {
        Task<List<Tblmclassroom>> GetAllClassrooms();
        Task<Tblmclassroom> GetClassroomByClassroomId(int ClassroomID);
        Task<Tblmclassroom> GetClassroomByRefID(string RefID);
        Task<List<Tblmclassroom>> GetClassroomBySubjectID(int SubjectID);
        Task<List<Tblmclassroom>> GetClassroomByStandardID(int StandardID);
    }
    public class ClassrommRepository:IClassroomRepository
    {
        private readonly AppDBContext _DBContext;
        public ClassrommRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
        }
        public async Task<List<Tblmclassroom>> GetAllClassrooms()
        {
            var result = await _DBContext.Tblmclassroom.ToListAsync();
            return result;
        }
        public async  Task<Tblmclassroom> GetClassroomByClassroomId(int ClassroomID)
        {
            var oClassroom = await _DBContext.Tblmclassroom.Where(c => c.Id.Equals(ClassroomID)).FirstOrDefaultAsync();
            return oClassroom;
        }
        public async  Task<Tblmclassroom> GetClassroomByRefID(string RefID)
        {
            var oClassroom = await _DBContext.Tblmclassroom.Where(c => c.RefId.Equals(RefID)).FirstOrDefaultAsync();
            return oClassroom;
        }
        public async Task<List<Tblmclassroom>> GetClassroomBySubjectID(int SubjectID)
        {
            var oClassroom = await _DBContext.Tblmclassroom.Where(c => c.SubjectId.Equals(SubjectID)).ToListAsync();
            return oClassroom;
        }
        public async  Task<List<Tblmclassroom>> GetClassroomByStandardID(int StandardID)
        {
            var oClassroom = await _DBContext.Tblmclassroom.Where(c => c.StandardId.Equals(StandardID)).ToListAsync();
            return oClassroom;
        }
    }
}