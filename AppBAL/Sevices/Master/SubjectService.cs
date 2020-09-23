using AppDAL.DBRepository;
using AppModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppBAL.Sevices
{
    public interface ISubjectService
    {
        Task<CommonResponce> GetAllSubjects();
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
    }
}
