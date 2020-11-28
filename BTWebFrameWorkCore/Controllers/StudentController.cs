using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using AppBAL.Sevices;
using AppBAL.Sevices.Master;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudentService _StudentService;
        private readonly IStandardMasterService _StandardMasterService;
        private readonly IAppUserService _AppUserService;

        public StudentController(IStudentService StudentService, IStandardMasterService StandardMasterService, IAppUserService AppUserService)
        {
            _StudentService = StudentService;
            _StandardMasterService = StandardMasterService;
            _AppUserService = AppUserService;
        }
        #region STUDENT LIST      
        public async Task<IActionResult> Students()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Student", ActionUrl = "/Student/Students" } });

            BaseViewModel VModel = null;

            var result = await _StudentService.GetAllStudents(500, GetBaseService().GetAppRootPath());

            var TempVModel = new StudentVM();
            TempVModel.StudentInfo = new AppGridModel<StudentBM>();
            TempVModel.StudentInfo.Rows = result;

            VModel = await GetViewModel(TempVModel);

            return View("~/Views/Master/Students.cshtml", VModel);
        }

        #endregion

        #region ADD STUDENT
        public async Task<IActionResult> StudentProfile()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Student", ActionUrl = "/Student/StudentProfile" } });
            List<StandardMasterBM> oAllStandards = await _StandardMasterService.GetAllStandards(500, GetBaseService().GetAppRootPath()); // get all standards
            BaseViewModel VModel = null;
            var TempVModel = new StudentProfileVM();
            TempVModel.AllStandards.AddRange(oAllStandards); // populate the list
            VModel = await GetViewModel(TempVModel);
            //}
            //*****get user avtar************
            /*
            string UsrImgPath = string.Format("{0}\\{1}.{2}", Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\UserAvatar"), CurrentUserInfo.UserID, "jpg");
            if (System.IO.File.Exists(UsrImgPath))
            {
                UsrImgPath = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}", CurrentUserInfo.UserID, "jpg");
            }
            else
            {
                if (CurrentUserInfo.UserGender.Equals("M"))
                    UsrImgPath = "~/img/avatar5.png";
                else
                    UsrImgPath = "~/img/avatar3.png";
            }
            */
            //*******************************
            return View("~/Views/Master/StudentProfile.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> StudentProfile(StudentProfileVM model)
        {
            StringBuilder rtnMsg = new StringBuilder();
            if (ModelState.IsValid)
            {
                var result = await _StudentService.InsertStudentProfile(model);

                if (result.Stat == true)
                {
                    rtnMsg.Append("Student Profile Inserted.");
                    AppUserVM oAppUserVM = new AppUserVM(); // add an user in the user table
                    oAppUserVM.Name = model.Name;
                    oAppUserVM.UserId = model.LoginId;
                    oAppUserVM.UserType = "A";// admin
                    oAppUserVM.Email = model.Email;
                    oAppUserVM.Mobile = model.ContactNo;
                    oAppUserVM.IsActive = true;
                    string ResetContext = Guid.NewGuid().ToString().Replace("-", "RP");
                    DateTime PassValidity = DateTime.Now.AddDays(1); //validity for 1 day
                    result = await _AppUserService.SaveAppUserAsync(oAppUserVM, ResetContext, PassValidity).ConfigureAwait(false);
                    if (!result.Stat)// user addition failed
                        rtnMsg.Append(" Failed to add student Login ID.");
                    else
                    {
                        var CurrentUserInfo = GetLoginUserInfo();
                        await GetBaseService().AddActivity(ActivityType.Update, CurrentUserInfo.UserID, CurrentUserInfo.UserName, "Student Profile", "Inserted Student profile");
                    }
                    return Json(new { stat = true, msg = rtnMsg.ToString(), rtnUrl = "/Student/Students" });
                }
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
                return Json(new { stat = false, msg = "Invalid Student Profile" });
        }
        #endregion ADD STUDENT

        #region UPDATE STUDENT
        public async Task<IActionResult> UpdateStudentProfile(int StudentID)
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Student", ActionUrl = "/Student/UpdateStudentProfile" } });

            BaseViewModel VModel = null;
            CommonResponce CR = await _StudentService.GetStudentByStudentId(StudentID);
            if (CR.Stat)
            {
                List<StandardMasterBM> oAllStandards = await _StandardMasterService.GetAllStandards(500, GetBaseService().GetAppRootPath());
                Student StudentInfo = (Student)CR.StatusObj;
                var TempVModel = new StudentProfileVM
                {
                    Id = StudentInfo.Id,
                    Name = StudentInfo.Name,
                    RegNo = StudentInfo.RegNo,
                    Address = StudentInfo.Address,
                    ContactNo = StudentInfo.ContactNo,
                    Email = StudentInfo.Email,
                    StandardId = StudentInfo.StandardId
                };
                TempVModel.AllStandards.AddRange(oAllStandards);// all standard list
                //var TempVModel = new StudentProfileVM();
                VModel = await GetViewModel(TempVModel);
            }
            //*****get user avtar************
            /*
            string UsrImgPath = string.Format("{0}\\{1}.{2}", Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\UserAvatar"), CurrentUserInfo.UserID, "jpg");
            if (System.IO.File.Exists(UsrImgPath))
            {
                UsrImgPath = string.Format("~/AppFileRepo/UserAvatar/{0}.{1}", CurrentUserInfo.UserID, "jpg");
            }
            else
            {
                if (CurrentUserInfo.UserGender.Equals("M"))
                    UsrImgPath = "~/img/avatar5.png";
                else
                    UsrImgPath = "~/img/avatar3.png";
            }
            */
            //*******************************
            return View("~/Views/Master/UpdateStudentProfile.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStudentProfile(StudentProfileVM model)
        {
              if (ModelState.IsValid)
             {
            var result = await _StudentService.UpdateStudentProfile(model);
            //await GetBaseService().AddActivity(ActivityType.Update, model.UserID, model.UserName, "User Profile", "Updated user profile");
            if (result.Stat == true)
                return Json(new { stat = true, msg = "Student Profile Updated", rtnUrl = "/Student/Students" });
            else
                return Json(new { stat = false, msg = result.StatusMsg });
             }
               else
                  return Json(new { stat = false, msg = "Invalid Student Profile"});
        }
        #endregion UPDATE STUDENT

        #region DELETE STUDENT
        [HttpPost]
        public async Task<JsonResult> DeleteStudent(int StudentId)
        {
            var result = await _StudentService.DeleteStudentProfile(StudentId);
            if (result.Stat == true)
            {
                var CurrentUserInfo = GetLoginUserInfo();
                await GetBaseService().AddActivity(ActivityType.Delete, CurrentUserInfo.UserID, CurrentUserInfo.UserName,
                                                                                              "Delete Student", string.Format("Delete Student"));
            }
            return Json(new { stat = result.Stat, msg = result.StatusMsg });
        }


        [HttpPost]
        public async Task<IActionResult> ReloadStudents()
        {
            var result = await _StudentService.GetAllStudents(500, GetBaseService().GetAppRootPath());
            var TempModel = new AppGridModel<StudentBM>();
            TempModel.Rows = result;
            return PartialView("_HTMLTable", TempModel);
        }
        #endregion DELETE STUDENT
    }
}
