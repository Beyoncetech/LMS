using AppModel;
using System;
using System.Collections.Generic;
using AppDAL.DBRepository;
using System.Threading.Tasks;
using AutoMapper;
using AppDAL.DBModels;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using AppUtility.Extension;

namespace AppBAL.Sevices.Master
{
    public interface IClassroomService
    {
        Task<CommonResponce> GetClassroomVM(ClassRoomDetailsVM TempVModel);
        Task<List<ClassroomBM>> GetAllClassrooms(int RowCount, string AppRootPath);
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
        private readonly ISubjectService _SubjectService;
        private readonly IStandardMasterRepository _DBStandardMasterRepository;
        private readonly ITeacherRepository _DBTeacherRepository;
        private readonly IStudentRepository _DBStudentRepository;
        private readonly ISubjectRepository _DBSubjectRepository;
        public ClassroomService(IClassroomRepository DBClassroomRepository, IMapper mapper, ICommonRepository<Tblmclassroom> CommonRepository, ISubjectRepository SubjectRepository,
             IStandardMasterRepository StandardMasterRepository, ITeacherRepository TeacherRepository, IStudentRepository StudentRepository)
        {
            _DBClassroomRepository = DBClassroomRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
            _DBSubjectRepository = SubjectRepository;
            _DBStandardMasterRepository = StandardMasterRepository;
            _DBTeacherRepository = TeacherRepository;
            _DBStudentRepository = StudentRepository;
        }

        public async Task<List<ClassroomBM>> GetAllClassrooms(int RowCount, string AppRootPath)
        {
            List<ClassroomBM> result = new List<ClassroomBM>();
            var oClassroom = await _DBClassroomRepository.GetAllClassrooms(RowCount).ConfigureAwait(false);

            if (oClassroom != null && oClassroom.Count > 0)
            {
                foreach (var item in oClassroom)
                {
                    result.Add(new ClassroomBM
                    {
                        Id = item.Id,                        
                        Name = item.Name,
                        RefId = item.RefId,
                        Description = item.Description,
                        Scheduler = item.Scheduler,
                        SubjectId = item.SubjectId,
                        StandardId = item.StandardId,
                        Action = item.Id.ToString(),
                        CreatedOn = item.CreatedOn,
                        CreatedBy = item.CreatedBy
                    });
                };
            }
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

        public async Task<CommonResponce> GetClassroomVM(ClassRoomDetailsVM TempVModel)
        {
            bool isValid = true;
            TempVModel.Subjects = new List<AppSelectListItem>();
            var oSubject = await _DBSubjectRepository.GetAllSubjects(500).ConfigureAwait(false);
            if (oSubject != null && oSubject.Count > 0)
            {
                foreach (var item in oSubject)
                {
                    AppSelectListItem AppSItem = new AppSelectListItem { Value = item.Id.ToString(), Text = item.Name };
                    TempVModel.Subjects.Add(AppSItem);
                };
            }
            TempVModel.Standards = new List<AppSelectListItem>();
            var oStandards = await _DBStandardMasterRepository.GetAllStandards(500).ConfigureAwait(false);
            if (oStandards != null && oStandards.Count > 0)
            {
                foreach (var item in oStandards)
                {
                    AppSelectListItem AppSItem = new AppSelectListItem { Value = item.Id.ToString(), Text = item.Name };
                    TempVModel.Standards.Add(AppSItem);
                }
            }
            
            TempVModel.Scheduler = new ClassSchedule();

            TempVModel.AllTeachers = new List<ClassMemberInfo>();
            var oTeachers = await _DBTeacherRepository.GetAllTeachers(500).ConfigureAwait(false);
            if (oTeachers != null && oTeachers.Count > 0)
            {
                foreach (var item in oTeachers)
                {
                    ClassMemberInfo CMI = new ClassMemberInfo { Id = item.Id, RegNo = item.RegNo, Name = item.Name, Description = item.EducationalQualification, Avatar = string.Format("~/AppFileRepo/TeacherAvatar/{0}.{1}?r={2}", item.RegNo, "jpg", DateTime.Now.Ticks.ToString()) };
                    TempVModel.AllTeachers.Add(CMI);
                };
            }
            
            TempVModel.AsignTeacher = new string[1] { "44445555" };
            
            TempVModel.AllStudents = new List<ClassMemberInfo>();
            var oStudents = await _DBStudentRepository.GetAllStudents(500).ConfigureAwait(false);

            if (oStudents != null && oStudents.Count > 0)
            {
                foreach (var item in oStudents)
                {
                    ClassMemberInfo CMI = new ClassMemberInfo { Id = (int)item.Id, RegNo = item.RegNo, Name = item.Name, Description = item.StandardId.ToString(), Avatar = string.Format("~/AppFileRepo/StudentAvatar/{0}.{1}?r={2}", item.RegNo, "jpg", DateTime.Now.Ticks.ToString()) };
                    TempVModel.AllStudents.Add(CMI);
                };
            }
           
            TempVModel.AsignStudent = new string[2] { "6655", "7744" };
            if(TempVModel.Id>0)
            {
                var TempClassRoom = await _DBClassroomRepository.GetClassroomByClassroomId(TempVModel.Id);
                if (TempClassRoom != null)                    
                {                    
                    TempVModel.Description = TempClassRoom.Description;
                    TempVModel.RefId = TempClassRoom.RefId;
                    TempVModel.Name = TempClassRoom.Name;
                    TempVModel.SubjectId = TempClassRoom.SubjectId;
                    TempVModel.StandardId = TempClassRoom.StandardId;
                    TempVModel.Scheduler = TempClassRoom.Scheduler.JSONStringToObject<ClassSchedule>();
                }
            }
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Classroom Id"), StatusObj = null };
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
            catch(Exception ex){ result.Stat = isValid; result.StatusMsg = ex.Message+" Failed to add new Classroom"; }
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
