using AppModel;
using System;
using System.Collections.Generic;
using AppDAL.DBRepository;
using System.Threading.Tasks;
using AutoMapper;
using AppDAL.DBModels;

namespace AppBAL.Sevices.Master
{
    public interface IClassroomService
    {
        Task<CommonResponce> GetAllClassrooms();
        Task<CommonResponce> GetClassroomByClassroomId(int ClassroomID);
        Task<CommonResponce> GetClassroomByRefID(string RefID);
        Task<CommonResponce> GetClassroomBySubjectID(int SubjectID);
        Task<CommonResponce> GetClassroomByStandardID(int StandardID);
        Task<CommonResponce> Insert(Classroom ClassroomToInsert);
        CommonResponce Update(Classroom ClassroomToInsert);
        CommonResponce Delete(Classroom ClassroomToInsert);
    }
    public class ClassroomService:IClassroomService
    {
        private readonly IClassroomRepository _DBClassroomRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Tblmclassroom> _commonRepository;
        public ClassroomService(IClassroomRepository DBClassroomRepository, IMapper mapper, ICommonRepository<Tblmclassroom> CommonRepository)
        {
            _DBClassroomRepository = DBClassroomRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
        }
        public async Task<CommonResponce> GetAllClassrooms()
        {
            var AllClassrooms = await _DBClassroomRepository.GetAllClassrooms();
            CommonResponce result = new CommonResponce { Stat = true, StatusMsg = "", StatusObj = AllClassrooms };
            return result;
        }
        public async Task<CommonResponce> GetClassroomByClassroomId(int ClassroomID)
        {
            bool isValid = true;
            var oClassroom= await _DBClassroomRepository.GetClassroomByClassroomId(ClassroomID);
            if (oClassroom == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Classroom Id"), StatusObj = oClassroom };
            return result;
        }
        public async Task<CommonResponce> GetClassroomByRefID(string RefID)
        {
            bool isValid = true;
            var oClassroom = await _DBClassroomRepository.GetClassroomByRefID(RefID);
            if (oClassroom == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Reference Id"), StatusObj = oClassroom };
            return result;
        }
        public async Task<CommonResponce> GetClassroomBySubjectID(int SubjectID)
        {
            bool isValid = true;
            var oClassroom = await _DBClassroomRepository.GetClassroomBySubjectID(SubjectID);
            if (oClassroom == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Subject Id"), StatusObj = oClassroom };
            return result;
        }
        public async Task<CommonResponce> GetClassroomByStandardID(int StandardID)
        {
            bool isValid = true;
            var oClassroom = await _DBClassroomRepository.GetClassroomByStandardID(StandardID);
            if (oClassroom == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Standard Id"), StatusObj = oClassroom };
            return result;
        }
        public async Task<CommonResponce> Insert(Classroom ClassroomToInsert)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                isValid = await _commonRepository.Insert(_mapper.Map<Tblmclassroom>(ClassroomToInsert));
                result.Stat = isValid;
                result.StatusMsg = "Classroom added successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to add new Classroom"; }
            return result;
        }
        public CommonResponce Update(Classroom ClassroomToUpdate)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                _commonRepository.Update(_mapper.Map<Tblmclassroom>(ClassroomToUpdate));
                result.Stat = true;
                result.StatusMsg = "Classroom information updated successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to update classroom information"; }
            return result;
        }
        public CommonResponce Delete(Classroom ClassroomToDelete)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                _commonRepository.Delete(_mapper.Map<Tblmclassroom>(ClassroomToDelete));
                result.Stat = true;
                result.StatusMsg = "Classroom deleted successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to delete classroom information"; }
            return result;
        }
    }
}
