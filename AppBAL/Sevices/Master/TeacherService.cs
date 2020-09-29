using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppDAL.DBRepository.Master;
using AppModel;
using AutoMapper;

namespace AppBAL.Sevices.Master
{
    public interface ITeacherService
    {
        Task<CommonResponce> GetAllTeachers();
        Task<CommonResponce> GetTeacherByTeacherId(int TeacherID);        
        Task<CommonResponce> GetTeacherByEmailID(string EmailID);
        Task<CommonResponce> Insert(Teacher TeacherToInsert);
        CommonResponce Update(Teacher TeacherToUpdate);
        CommonResponce Delete(Teacher TeacherToDelete);
    }
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _DBTeacherRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Tblmteacher> _commonRepository;
        public TeacherService(ITeacherRepository DBTeacherRepository, IMapper mapper, ICommonRepository<Tblmteacher> CommonRepository)
        {
            _DBTeacherRepository = DBTeacherRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
        }

        public async Task<CommonResponce> GetAllTeachers()
        {
            var AllTeachers = await _DBTeacherRepository.GetAllTeachers();
            CommonResponce result = new CommonResponce { Stat = true, StatusMsg = "", StatusObj = AllTeachers };
            return result;
        }
        public async Task<CommonResponce> GetTeacherByTeacherId(int TeacherID)
        {
            bool isValid = true;
            var oTeacher = await _DBTeacherRepository.GetTeacherByTeacherId(TeacherID);
            if (oTeacher == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Teacher Id"), StatusObj = oTeacher };
            return result;
        }

        public async Task<CommonResponce> GetTeacherByEmailID(string EmailID)
        {
            bool isValid = true;
            var oTeacher = await _DBTeacherRepository.GetTeacherByEmailID(EmailID);
            if (oTeacher == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Eamil id"), StatusObj = oTeacher };
            return result;

        }

        #region INSERT/ UPDATE/ DELETE 
        public async Task<CommonResponce> Insert(Teacher TeacherToInsert)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                isValid = await _commonRepository.Insert(_mapper.Map<Tblmteacher>(TeacherToInsert));
                result.Stat = isValid;
                result.StatusMsg = "Teacher added successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to add new teacher"; }
            return result;
        }
        public CommonResponce Update(Teacher TeacherToUpdate)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                _commonRepository.Update(_mapper.Map<Tblmteacher>(TeacherToUpdate));
                result.Stat = true;
                result.StatusMsg = "Teacher information updated successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to update teacher information"; }
            return result;
        }
        public CommonResponce Delete(Teacher TeacherToDelete)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                _commonRepository.Delete(_mapper.Map<Tblmteacher>(TeacherToDelete));
                result.Stat = true;
                result.StatusMsg = "Teacher inforamtion deleted";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to delete teacher information"; }
            return result;
        }
        #endregion INSERT/ UPDATE/ DELETE 
    }
}
