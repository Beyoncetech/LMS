using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
using AutoMapper;
using System.Threading.Tasks;

namespace AppBAL.Sevices.Master
{
    public interface IStudentService
    {
        Task<CommonResponce> GetAllStudents();
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
        public StudentService(IStudentRepository DBStudentRepository, IMapper mapper,ICommonRepository<Tblmstudent> CommonRepository)
        {
            _DBStudentRepository = DBStudentRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
        }

        public async Task<CommonResponce> GetAllStudents()
        {
            var AllStudents = await _DBStudentRepository.GetAllStudents();
            CommonResponce result = new CommonResponce { Stat = true, StatusMsg = "", StatusObj = AllStudents };
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
