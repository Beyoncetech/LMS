using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
using AppModel.BusinessModel.Master;
using AppUtility.AppIO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppBAL.Sevices.Master
{
    public interface IStudentService
    {
        Task<List<StudentBM>> GetAllStudents(int RowCount, string AppRootPath);        
        Task<CommonResponce> GetStudentByStudentId(int StudentID);
        Task<CommonResponce> GetStudentByRegNo(int RegNo);
        Task<CommonResponce> GetStudentByEmailID(string EmailID);
        Task<CommonResponce> Insert(Student StudentToInsert);
        CommonResponce Update(Student StudentToUpdate);
        CommonResponce Delete(Student StudentToDelete);
    }
    public class StudentService:IStudentService
    {
        private readonly IStudentRepository _DBStudentRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Tblmstudent> _commonRepository;
        private readonly IDirectoryFileService _AppDirectoryFileService;
        public StudentService(IStudentRepository DBStudentRepository, IMapper mapper,ICommonRepository<Tblmstudent> CommonRepository, IDirectoryFileService AppDirectoryFileService)
        {
            _DBStudentRepository = DBStudentRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
            _AppDirectoryFileService = AppDirectoryFileService;
        }

        public async Task<List<StudentBM>> GetAllStudents(int RowCount, String AppRootPath)
        {
            List<StudentBM> result = new List<StudentBM>();
            var oStudents = await _DBStudentRepository.GetAllStudents(RowCount).ConfigureAwait(false);

            if (oStudents != null && oStudents.Count > 0)
            {
                foreach (var item in oStudents)
                {
                    result.Add(new StudentBM
                    {
                        Id = item.Id,
                        UserAvatar = _AppDirectoryFileService.GetAppUserAvatarPath(AppRootPath, item.RegNo.ToString(), "M"),
                        Name = item.Name,
                        RegNo = item.RegNo,
                        Address = item.Address,
                        ContactNo = item.ContactNo,
                        Email = item.Email,
                        StandardId = item.StandardId,
                        Action = item.Id.ToString(),
                        CreatedOn = item.CreatedOn,
                        CreatedBy = item.CreatedBy
                    });
                };
            }
            return result;
        }
        
        public async Task<CommonResponce> GetStudentByStudentId(int StudentID)
        {
            bool isValid = true;
            var oStudent = await _DBStudentRepository.GetStudentByStudentId(StudentID);
            if (oStudent == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Student Id"), StatusObj = oStudent };
            return result;
        }
        public async Task<CommonResponce> GetStudentByRegNo(int RegNo)
        {
            bool isValid = true;
            var oStudent = await _DBStudentRepository.GetStudentByRegNo(RegNo);
            if (oStudent == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Student Registration No"), StatusObj = oStudent };
            return result;
        }
        public async Task<CommonResponce> GetStudentByEmailID(string EmailID)
        {
            bool isValid = true;
            var oStudent = await _DBStudentRepository.GetStudentByEmailID(EmailID);
            if (oStudent == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Email ID"), StatusObj = oStudent };
            return result;
        }

        #region INSERT/ UPDATE/ DELETE 
        public async Task<CommonResponce> Insert(Student StudentToInsert)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                isValid = await _commonRepository.Insert(_mapper.Map<Tblmstudent>(StudentToInsert));
                result.Stat = isValid;
                result.StatusMsg = "Student added successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to add new student"; }
            return result;
        }

        public CommonResponce Update(Student StudentToUpdate)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                _commonRepository.Update(_mapper.Map<Tblmstudent>(StudentToUpdate));
                result.Stat = true;
                result.StatusMsg = "Student information updated successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to update student information"; }
            return result;
        }

        public CommonResponce Delete(Student StudentToDelete)
        {
            CommonResponce result = new CommonResponce();
            bool isValid = false;
            try
            {
                _commonRepository.Delete(_mapper.Map<Tblmstudent>(StudentToDelete));
                result.Stat = true;
                result.StatusMsg = "Student inforamtion deleted";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to delete student information"; }
            return result;
        }
        #endregion INSERT/ UPDATE/ DELETE 
    }
}
