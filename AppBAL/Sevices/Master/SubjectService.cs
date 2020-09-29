using AppDAL.DBRepository;
using AppModel;
using AutoMapper;
using System.Threading.Tasks;

namespace AppBAL.Sevices
{
    public interface ISubjectService
    {
        Task<CommonResponce> GetAllSubjects();
        Task<CommonResponce> GetSubjectBySubjectId(int SubjectID);
        Task<CommonResponce> GetSubjectBySubjectName(string SubjectName);
    }

    public class SubjectService:ISubjectService
    {
        private readonly ISubjectRepository _DBSubjectRepository;
        private readonly IMapper _mapper;
        
        public SubjectService(ISubjectRepository DBSubjectRepository,IMapper mapper)
        {
            _DBSubjectRepository = DBSubjectRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponce> GetAllSubjects()
        {
            var AllSubjects = await _DBSubjectRepository.GetAllSubjects();
            CommonResponce result = new CommonResponce { Stat = true, StatusMsg = "", StatusObj = AllSubjects };
            return result;
        }
        public async Task<CommonResponce> GetSubjectBySubjectId(int SubjectID)
        {
            bool isValid = true;
            var oSubject = await _DBSubjectRepository.GetSubjectBySubjectId(SubjectID);
            if (oSubject == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Subject Id"), StatusObj = oSubject };
            return result;
        }

        public async Task<CommonResponce> GetSubjectBySubjectName(string SubjectName)
        {
            bool isValid = true;
            var oSubject = await _DBSubjectRepository.GetSubjectBySubjectName(SubjectName);
            if (oSubject == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Subject Name"), StatusObj = oSubject };
            return result;
        }
    }
}
