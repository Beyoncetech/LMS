using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AppBAL.Sevices.Master;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class ClassroomController : BaseController
    {
        private readonly IClassroomService _ClassroomService;
        public ClassroomController(IClassroomService ClassrommService)
        {
            _ClassroomService = ClassrommService;
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
            TempVModel.Subjects = new List<AppSelectListItem>
            {
                new AppSelectListItem { Value = "1", Text = "Math" },
                new AppSelectListItem { Value = "2", Text = "Hindi" },
                new AppSelectListItem { Value = "3", Text = "Biology"  },
            };

            TempVModel.Standards = new List<AppSelectListItem>
            {
                new AppSelectListItem { Value = "1", Text = "Class V" },
                new AppSelectListItem { Value = "2", Text = "Class VI" },
                new AppSelectListItem { Value = "3", Text = "XII"  },
            };
            TempVModel.Scheduler = new ClassSchedule();
            TempVModel.AllTeachers = new List<ClassMemberInfo>
            {
                new ClassMemberInfo {Id = 1, RegNo = "111222", Name = "Rohit Sign", Description = "M. Sc (CS)", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())},
                new ClassMemberInfo {Id = 2, RegNo = "44445555", Name = "Kunal Sarma", Description = "B. Sc (CS)", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())},
                new ClassMemberInfo {Id = 3, RegNo = "777888", Name = "Kamal Das", Description = "B com", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())},
                new ClassMemberInfo {Id = 4, RegNo = "6666555", Name = "Molani Saw", Description = "Math", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())},
                new ClassMemberInfo {Id = 5, RegNo = "454545", Name = "Kanchan Paul", Description = "English graduate", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())}
            };
            TempVModel.AsignTeacher = new string[1] { "44445555" };
            TempVModel.AllStudents = new List<ClassMemberInfo>
            {
                new ClassMemberInfo {Id = 1, RegNo = "1144", Name = "Ripan paul", Description = "M. Sc (V Sem)", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())},
                new ClassMemberInfo {Id = 2, RegNo = "6655", Name = "Sunil saw", Description = "B. Sc (1st sem)", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())},
                new ClassMemberInfo {Id = 3, RegNo = "8899", Name = "Mani Das", Description = "B com Pass", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())},
                new ClassMemberInfo {Id = 4, RegNo = "7744", Name = "Prasanta saha", Description = "Math pass", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())},
                new ClassMemberInfo {Id = 5, RegNo = "5656", Name = "Pallav Paul", Description = "English graduate", Avatar = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}?r={2}", "dd", "jpg", DateTime.Now.Ticks.ToString())}
            };
            TempVModel.AsignStudent = new string[2] { "6655", "7744" };
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
                if (model.Id > 0)
                {
                    // write update logic
                    return Json(new { stat = true, msg = "Successfully updated Classroom" });
                }
                else
                {
                    // write save logic
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
