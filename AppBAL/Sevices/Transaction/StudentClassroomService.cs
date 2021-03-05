using AppModel;
using System;
using System.Collections.Generic;
using AppDAL.DBRepository;
using System.Threading.Tasks;
using AutoMapper;
using AppDAL.DBModels;
using AppModel.BusinessModel.Master;

namespace AppBAL.Sevices.Transaction
{
    public interface IStudentClassroomService
    {
        Task<CommonResponce> Insert(StudentClassroom StudentClassroomToInsert);
        CommonResponce Update(StudentClassroom StudentClassroomToInsert);
        CommonResponce Delete(StudentClassroom StudentClassroomToInsert);
    }
    public class StudentClassroomService:IStudentClassroomService
    {
        private readonly IStudentClassroomRepository _DBStudentClassroomRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Tblrstudentclassroom> _commonRepository;
        public StudentClassroomService(IStudentClassroomRepository DBStudentClassroomRepository, IMapper mapper, ICommonRepository<Tblrstudentclassroom> CommonRepository)
        {
            _DBStudentClassroomRepository = DBStudentClassroomRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
        }

        public async Task<CommonResponce> Insert(StudentClassroom StudentClassroomToInsert)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                isValid = await _commonRepository.Insert(_mapper.Map<Tblrstudentclassroom>(StudentClassroomToInsert));
                result.Stat = isValid;
                result.StatusMsg = "Student assigned to Classroom successfully";
            }
            catch(Exception ex){ result.Stat = isValid; result.StatusMsg = ex.Message+" Failed to assign Student to Classroom"; }
            return result;
        }
        public CommonResponce Update(StudentClassroom StudentClassroomToUpdate)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                _commonRepository.Update(_mapper.Map<Tblrstudentclassroom>(StudentClassroomToUpdate));
                result.Stat = true;
                result.StatusMsg = "Student assignment to Classroom updated successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to update assign Student to Classroom"; }
            return result;
        }
        public CommonResponce Delete(StudentClassroom StudentClassroomToDelete)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                _commonRepository.Delete(_mapper.Map<Tblrstudentclassroom>(StudentClassroomToDelete));
                result.Stat = true;
                result.StatusMsg = "Student assignment to Classroom deleted successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to delete Student assignment to Classroom"; }
            return result;
        }
    }
}
