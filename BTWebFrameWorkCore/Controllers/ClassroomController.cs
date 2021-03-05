using AppBAL.Sevices;
using AppBAL.Sevices.Master;
using AppBAL.Sevices.Transaction;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using AppUtility.Extension;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class ClassroomController : BaseController
    {
        private readonly IClassroomService _ClassroomService;
        private readonly ISubjectService _SubjectService;
        public readonly IStandardMasterService _StandardMasterService;
        public readonly ITeacherService _TeacherService;
        public readonly IStudentService _StudentService;
        public readonly IStudentClassroomService _StudentClassroomService;


        public ClassroomController(IClassroomService ClassrommService,ISubjectService SubjectService,IStandardMasterService StandardMasterService,
                                    ITeacherService TeacherService,IStudentService StudentService,IStudentClassroomService StudentClassroomService)
        {
            _ClassroomService = ClassrommService;
            _SubjectService = SubjectService;
            _StandardMasterService = StandardMasterService;
            _TeacherService = TeacherService;
            _StudentService = StudentService;
            _StudentClassroomService = StudentClassroomService;
        }
        #region LIST
        public async Task<IActionResult> Classrooms()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Classroom", ActionUrl = "/Classroom/Classrooms" } });

            BaseViewModel VModel = null;

            var result = await _ClassroomService.GetAllClassrooms(500, GetBaseService().GetAppRootPath());

            var TempVModel = new ClassroomVM();
            TempVModel.ClassroomInfo = new AppGridModel<ClassroomBM>();
            TempVModel.ClassroomInfo.Rows = result;

            VModel = await GetViewModel(TempVModel);

            return View("~/Views/Master/Classrooms.cshtml", VModel);
        }
        #endregion LIST

        #region classroom details
        public async Task<IActionResult> ClassRoom(int id = 0)
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Classroom", ActionUrl = "/Classroom/Classroom" } });

            BaseViewModel VModel = null;

            var TempVModel = new ClassRoomDetailsVM();
            TempVModel.Id = id;
            TempVModel.ApprootPath = GetBaseService().GetAppRootPath();
            //set class ref id and remove tempdata if exist
            var tempLoginUser = GetLoginUserInfo();
            if (tempLoginUser != null)            
                TempVModel.TempClassRefId = tempLoginUser.UserID;
            
            string TempTeacherKey = string.Format("{0}_{1}", TempVModel.TempClassRefId, "ClassTeacher");
            if (TempData.ContainsKey(TempTeacherKey))            
                TempData.Remove(TempTeacherKey);
            
            TempTeacherKey = string.Format("{0}_{1}", TempVModel.TempClassRefId, "ClassStudent");
            if (TempData.ContainsKey(TempTeacherKey))            
                TempData.Remove(TempTeacherKey);
            //end of class ref id and remove tempdata if exist
            // get value from service layer  
            //var AllSubjects = await _SubjectService.GetAllSubjects(500, GetBaseService().GetAppRootPath());
            //TempVModel.Subjects = new List<AppSelectListItem>();
            //foreach (SubjectBM subject in AllSubjects)
            //{
            //    AppSelectListItem AppSItem = new AppSelectListItem { Value = subject.Id.ToString(), Text = subject.Name };
            //    TempVModel.Subjects.Add(AppSItem);
            //}
            //var AllStandards= await _StandardMasterService.GetAllStandards(500, GetBaseService().GetAppRootPath());
            //TempVModel.Standards = new List<AppSelectListItem>();
            //foreach (StandardMasterBM standard in AllStandards)
            //{
            //    AppSelectListItem AppSItem = new AppSelectListItem { Value = standard.Id.ToString(), Text = standard.Name };
            //    TempVModel.Standards.Add(AppSItem);
            //}
            // TempVModel.Scheduler = new ClassSchedule();
            //var AllTeachers = await _TeacherService.GetAllTeachers(500, GetBaseService().GetAppRootPath());
            //TempVModel.AllTeachers = new List<ClassMemberInfo>();
            //foreach (TeacherBM teacher in AllTeachers)
            //{
            //    ClassMemberInfo CMI = new ClassMemberInfo { Id = teacher.Id, RegNo = teacher.RegNo, Name = teacher.Name, Description = teacher.EducationalQualification, Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", teacher.RegNo, "jpg", DateTime.Now.Ticks.ToString()) };
            //    TempVModel.AllTeachers.Add(CMI);
            //}
            //TempVModel.AsignTeacher = new string[1] { "44445555" };
            //var AllStudents = await _StudentService.GetAllStudents(500, GetBaseService().GetAppRootPath());
            //TempVModel.AllStudents = new List<ClassMemberInfo>();
            //foreach(StudentBM student in AllStudents)
            //{
            //    ClassMemberInfo CMI = new ClassMemberInfo { Id = (int)student.Id, RegNo = student.RegNo, Name = student.Name, Description = student.StandardId.ToString(), Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", student.RegNo, "jpg", DateTime.Now.Ticks.ToString()) };
            //    TempVModel.AllStudents.Add(CMI);
            //};
            //TempVModel.AsignStudent = new string[2] { "6655", "7744" };
           await _ClassroomService.GetClassroomVM(TempVModel);
            // end of service lyer

            VModel = await GetViewModel(TempVModel);
            
            return View(VModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> ClassRoom(ClassRoomDetailsVM model)
        {
            if (ModelState.IsValid)
            {
                string TempMemberAsignList = string.Empty;
                CommonResponce result = null;
                string TempAsignKey = string.Format("{0}_{1}", model.TempClassRefId, "ClassTeacher");
                if (TempData.ContainsKey(TempAsignKey))
                {
                    TempMemberAsignList = TempData[TempAsignKey].ToString();
                    model.AsignTeacher = TempMemberAsignList.Split(',');
                }
                TempAsignKey = string.Format("{0}_{1}", model.TempClassRefId, "ClassStudent");
                if (TempData.ContainsKey(TempAsignKey))
                {
                    TempMemberAsignList = TempData[TempAsignKey].ToString();
                    model.AsignStudent = TempMemberAsignList.Split(',');
                }

                // write the class save service
                await Task.Delay(5).ConfigureAwait(false);
                Classroom CR = new Classroom();
                CR.RefId = model.RefId;
                CR.Name = model.Name;
                CR.Scheduler =  model.Scheduler.ToJSONString();
                CR.StandardId = model.StandardId;
                CR.SubjectId = model.SubjectId;
                CR.Description = model.Description;
                if (model.Id > 0)
                {
                    // write update logic
                    result = _ClassroomService.Update(CR);
                    return Json(new { stat = true, msg = "Successfully updated Classroom" });
                }
                else
                {
                    // write save logic
                    result =await _ClassroomService.Insert(CR);
                    //result = await _StudentClassroomService.Insert();
                    return Json(new { stat = true, msg = "Successfully save Classroom" });
                }                                
            }
            else
            {
                return Json(new { stat = false, msg = "Invalid Classroom data" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddClassRoomTeacher([FromBody] ClassRoomDetailsVM model)
        {
            await Task.Delay(5).ConfigureAwait(false);
            string TempTeacherAsignList = string.Empty;
            string TempTeacherKey = string.Format("{0}_{1}", model.TempClassRefId, "ClassTeacher");
            if (TempData.ContainsKey(TempTeacherKey))
            {
                TempTeacherAsignList = TempData[TempTeacherKey].ToString();
            }
            else
            {
                TempTeacherAsignList = String.Join(",", model.AsignTeacher);
            }
            string pattern = @"(?<=\[)(.*?)(?=\])";
            Match output = Regex.Match(model.AutoCompleteSearchText, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (output.Success)
            {
                if (!TempTeacherAsignList.Contains(output.Value))
                    TempTeacherAsignList = TempTeacherAsignList + "," + output.Value;
            }
            TempData[TempTeacherKey] = TempTeacherAsignList;
            model.AsignTeacher = TempTeacherAsignList.Split(',');
            return PartialView("_PartialClassRoomTeacher", model.AsignTeacherInfo);

        }
        [HttpPost]
        public async Task<IActionResult>DeleteClassRoomTeacher([FromBody] ClassRoomDetailsVM model)
        {
            await Task.Delay(5).ConfigureAwait(false);
            string TempTeacherAsignList = string.Empty;
            string TempTeacherKey = string.Format("{0}_{1}", model.TempClassRefId, "ClassTeacher");
            if (TempData.ContainsKey(TempTeacherKey))
            {
                TempTeacherAsignList = TempData[TempTeacherKey].ToString();
            }
            else
            {
                TempTeacherAsignList = String.Join(",", model.AsignTeacher);
            }
            if (TempTeacherAsignList.Contains(model.AutoCompleteSearchText))
            {
                var tempRegNos = TempTeacherAsignList.Split(',');
                var listRegNos = new List<string>(tempRegNos);
                listRegNos.Remove(model.AutoCompleteSearchText);
                tempRegNos = listRegNos.ToArray();
                TempTeacherAsignList = String.Join(",", tempRegNos);
            }
            
            TempData[TempTeacherKey] = TempTeacherAsignList;
            model.AsignTeacher = TempTeacherAsignList.Split(',');
            return PartialView("_PartialClassRoomTeacher", model.AsignTeacherInfo);

        }

        [HttpPost]
        public async Task<IActionResult> AddClassRoomStudent([FromBody] ClassRoomDetailsVM model)
        {
            await Task.Delay(5).ConfigureAwait(false);
            string TempStudentAsignList = string.Empty;
            string TempStudentKey = string.Format("{0}_{1}", model.TempClassRefId, "ClassStudent");
            if (TempData.ContainsKey(TempStudentKey))
            {
                TempStudentAsignList = TempData[TempStudentKey].ToString();
            }
            else
            {
                TempStudentAsignList = String.Join(",", model.AsignStudent);
            }
            string pattern = @"(?<=\[)(.*?)(?=\])";
            Match output = Regex.Match(model.AutoCompleteSearchText, pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            if (output.Success)
            {
                if (!TempStudentAsignList.Contains(output.Value))
                    TempStudentAsignList = TempStudentAsignList + "," + output.Value;
            }
            TempData[TempStudentKey] = TempStudentAsignList;
            model.AsignStudent = TempStudentAsignList.Split(',');
            return PartialView("_PartialClassRoomStudent", model.AsignStudentInfo);

        }
        [HttpPost]
        public async Task<IActionResult> DeleteClassRoomStudent([FromBody] ClassRoomDetailsVM model)
        {
            await Task.Delay(5).ConfigureAwait(false);
            string TempStudentAsignList = string.Empty;
            string TempStudentKey = string.Format("{0}_{1}", model.TempClassRefId, "ClassStudent");
            if (TempData.ContainsKey(TempStudentKey))
            {
                TempStudentAsignList = TempData[TempStudentKey].ToString();
            }
            else
            {
                TempStudentAsignList = String.Join(",", model.AsignStudent);
            }
            if (TempStudentAsignList.Contains(model.AutoCompleteSearchText))
            {
                var tempRegNos = TempStudentAsignList.Split(',');
                var listRegNos = new List<string>(tempRegNos);
                listRegNos.Remove(model.AutoCompleteSearchText);
                tempRegNos = listRegNos.ToArray();
                TempStudentAsignList = String.Join(",", tempRegNos);
            }

            TempData[TempStudentKey] = TempStudentAsignList;
            model.AsignStudent = TempStudentAsignList.Split(',');
            return PartialView("_PartialClassRoomStudent", model.AsignStudentInfo);

        }
        #endregion
               
    }
}
