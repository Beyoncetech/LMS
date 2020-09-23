using AppDAL.DBModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppDAL.DBRepository
{
    public interface ISubjectRepository
    {
        Task<List<Tblmsubject>> GetAllSubjects();
        Task<Tblmsubject> GetSubjectBySubjectId(int SubjectID);
        Task<Tblmsubject> GetSubjectBySubjectName(string SubjectName);
    }

    public class SubjectRepository:ISubjectRepository
    {
        private readonly AppDBContext _DBContext;
        public SubjectRepository(AppDBContext DBContext)
        {
            _DBContext = DBContext;
        }

        public async Task<List<Tblmsubject>> GetAllSubjects()
        {
            var result = await _DBContext.Tblmsubject.ToListAsync();
            return result;
        }

        public async Task<Tblmsubject> GetSubjectBySubjectId(int SubjectID)
        {
            var oSubject = await _DBContext.Tblmsubject.Where(s => s.Id.Equals(SubjectID)).FirstOrDefaultAsync();
            return oSubject;
        }

        public async Task<Tblmsubject> GetSubjectBySubjectName(string SubjectName)
        {
            var oSubject = await _DBContext.Tblmsubject.Where(s => s.Name.Equals(SubjectName)).FirstOrDefaultAsync();
            return oSubject;
        }
    }
}
