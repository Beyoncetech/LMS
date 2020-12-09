using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using AppModel.BusinessModel.Master;
using AppUtility.AppIO;
using AppModel.ViewModel;

namespace AppBAL.Sevices.Master
{
    public interface ITeacherService
    {
        Task<List<TeacherBM>> GetAllTeachers(int RowCount,string AppRootPath);
        Task<CommonResponce> GetTeacherByTeacherId(int TeacherID);        
        Task<CommonResponce> GetTeacherByEmailID(string EmailID);
        Task<CommonResponce> InsertTeacherProfile(TeacherProfileVM TeacherToInsert);
        Task<CommonResponce> UpdateTeacherProfile(TeacherProfileVM TeacherToUpdate);
       Task< CommonResponce> DeleteTeacherProfile(int TeacherId);
    }
    public class TeacherService : ITeacherService
    {
        private readonly ITeacherRepository _DBTeacherRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Tblmteacher> _commonRepository;
        private readonly IDirectoryFileService _AppDirectoryFileService;
        public TeacherService(ITeacherRepository DBTeacherRepository, IMapper mapper, ICommonRepository<Tblmteacher> CommonRepository, IDirectoryFileService AppDirectoryFileService)
        {
            _DBTeacherRepository = DBTeacherRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
            _AppDirectoryFileService = AppDirectoryFileService;
        }

        public async Task<List<TeacherBM>> GetAllTeachers(int RowCount,string AppRootPath)
        {
            List<TeacherBM> result = new List<TeacherBM>();
            var oTeachers = await _DBTeacherRepository.GetAllTeachers(RowCount).ConfigureAwait(false);

            if (oTeachers != null && oTeachers.Count > 0)
            {
                foreach (var item in oTeachers)
                {
                    result.Add(new TeacherBM
                    {
                        Id = item.Id,
                        UserAvatar = _AppDirectoryFileService.GetAppUserAvatarPath(AppRootPath, item.Id.ToString(), "M"),
                        Name = item.Name,                        
                        Address = item.Address,
                        ContactNo = item.ContactNo,
                        Email = item.Email,
                        EducationalQualification = item.EducationalQualification,
                        Action = item.Id.ToString(),
                        CreatedOn = item.CreatedOn,                        
                    });
                };
            }
            return result;
        }
        public async Task<CommonResponce> GetTeacherByTeacherId(int TeacherID)
        {
            bool isValid = true;
            Teacher TeacherProfile = null;
            var oTeacher= await _DBTeacherRepository.GetTeacherByTeacherId(TeacherID);
            if (oTeacher != null)
            {
                TeacherProfile = _mapper.Map<Teacher>(oTeacher);
                isValid = true;
            }
            else
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Teacher Id"), StatusObj = TeacherProfile };
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
        public async Task<CommonResponce> InsertTeacherProfile(TeacherProfileVM TeacherToInsert)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            bool isValid = false;
            try
            {
                Tblmteacher oTeacher = new Tblmteacher
                {                    
                    Name = TeacherToInsert.Name,
                    Address = TeacherToInsert.Address,
                    Email = TeacherToInsert.Email,
                    ContactNo = TeacherToInsert.ContactNo,
                    EducationalQualification = TeacherToInsert.EducationalQualification,
                    LoginUserId=TeacherToInsert.LoginUserId
                };
                //isValid = await _commonRepository.Insert(_mapper.Map<Tblmstudent>(StudentToInsert));
                isValid = await _commonRepository.Insert(oTeacher);

                result.Stat = isValid;
                result.StatusMsg = "Teacher added successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to add new teacher"; }
            return result;
        }
        public async Task<CommonResponce> UpdateTeacherProfile(TeacherProfileVM TeacherToUpdate)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            bool isValid = false;
            try
            {
                var oTeacher = await _DBTeacherRepository.GetTeacherByTeacherId(TeacherToUpdate.Id).ConfigureAwait(false);
                if (oTeacher != null)
                {
                    oTeacher.Name = TeacherToUpdate.Name;
                    oTeacher.Address = TeacherToUpdate.Address;
                    oTeacher.Email = TeacherToUpdate.Email;                    
                    oTeacher.ContactNo = TeacherToUpdate.ContactNo;
                    oTeacher.EducationalQualification = TeacherToUpdate.EducationalQualification;
                    _commonRepository.Update(oTeacher);
                    result.Stat = true;
                    result.StatusMsg = "Teacher information updated successfully";
                }
                else      // teacher not found      
                    result.StatusMsg = "Not a valid Teacher";               
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to update teacher information"; }
            return result;
        }
        public async Task<CommonResponce> DeleteTeacherProfile(int TeacherId)
        {
            CommonResponce result = new CommonResponce() { Stat = false, StatusMsg = "Error in deleting Student" };
            try
            {
                var oTeacher = await _DBTeacherRepository.GetTeacherByTeacherId(TeacherId).ConfigureAwait(false); // get teacher details from db
                if (oTeacher != null)  // Teacher found
                {
                    _commonRepository.Delete(oTeacher);
                    result.Stat = true;
                    result.StatusMsg = "Teacher deleted successfully";
                }
                else
                    result.StatusMsg = "Not a valid Teacher";
            }
            catch { result.StatusMsg = "Failed to delete Teacher"; }
            return result;
        }
        #endregion INSERT/ UPDATE/ DELETE 
    }
}
