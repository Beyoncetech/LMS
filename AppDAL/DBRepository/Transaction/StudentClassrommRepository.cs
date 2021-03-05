using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDAL.DBRepository
{
    public interface IStudentClassroomRepository
    {        
        Task<List<Tblrstudentclassroom>> GetStudentClassroomByClassRoomRefID(int RefID);
        //Task<List<Tblrstudentclassroom>> GetStudentClassroomBySubjectID(int SubjectID);
        Task<List<Tblrstudentclassroom>> GetStudentClassroomByStudentID(int StudenntID);
    }
    public class StudentClassroomRepository:IStudentClassroomRepository
    {
        private readonly AppDBContext _DBContext;
        public StudentClassroomRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
        }
        public async Task<List<Tblrstudentclassroom>> GetStudentClassroomByClassRoomRefID(int ClassroomRefID)
        {
            var oStudentClassroom =await _DBContext.Tblrstudentclassroom.Where(c => c.ClassRoomId.Equals(ClassroomRefID)).ToListAsync();
            return oStudentClassroom;
        }

        public async  Task<List<Tblrstudentclassroom>> GetStudentClassroomByStudentID(int StudentID)
        {
            var oStudentClassroom = await _DBContext.Tblrstudentclassroom.Where(c => c.StudentId.Equals(StudentID)).ToListAsync();
            return oStudentClassroom;
        }
    }
}
