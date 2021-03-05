using AppBAL.Sevices;
using AppBAL.Sevices.AppCore;
using AppBAL.Sevices.Master;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class StudentController : BaseController
    {
        private readonly IStudentService _StudentService;
        private readonly IStandardMasterService _StandardMasterService;
        private readonly IAppUserService _AppUserService;
        private readonly IAppJobService _JobService;

        public StudentController(IStudentService StudentService, IStandardMasterService StandardMasterService, IAppUserService AppUserService,IAppJobService JobService)
        {
            _StudentService = StudentService;
            _StandardMasterService = StandardMasterService;
            _AppUserService = AppUserService;
            _JobService = JobService;
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
            List<StandardMasterBM> oAllStandards = await _StandardMasterService.GetAllStandards(500); // get all standards
            BaseViewModel VModel = null;
            var TempVModel = new StudentProfileVM();
            TempVModel.AllStandards.AddRange(oAllStandards); // populate the list

            //}
            //*****get user avtar************
            
            string UsrImgPath = string.Format("{0}\\{1}.{2}", Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\StudentAvatar"), TempVModel.RegNo, "jpg");
            if (System.IO.File.Exists(UsrImgPath))
            {
                UsrImgPath = string.Format("~/AppFileRepo/StudentAvatar/{0}.{1}", TempVModel.RegNo, "jpg");
            }
            else
            {
                //if (CurrentUserInfo.UserGender.Equals("M"))
                    UsrImgPath = "~/img/avatar5.png";
                //else
                //    UsrImgPath = "~/img/avatar3.png";
            }
            TempVModel.StudentImgPath = UsrImgPath;
            TempVModel.AttachStudentImage = new FileUploadInfo();

            //*******************************
            VModel = await GetViewModel(TempVModel);
            return View("~/Views/Master/StudentProfile.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> StudentProfile(StudentProfileVM model)
        {
            CommonResponce result = null;
            StringBuilder rtnMsg = new StringBuilder();
            if (ModelState.IsValid)
            {
                result = await _StudentService.CheckDataValidation(model, true);
                if (!result.Stat)// validation failed
                    return Json(new { stat = false, msg = result.StatusMsg });
                result = await _AppUserService.GetUserProfile(model.LoginId); // check same login id
                if (result.Stat)// login id exists
                    return Json(new { stat = false, msg = "Login Id already in use" });
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
                    return Json(new { stat = false, msg =result.StatusMsg});
                else
                {
                    model.LoginUserId = result.StatusObj;
                    result = await _StudentService.InsertStudentProfile(model);

                    if (result.Stat == true)
                    {
                        rtnMsg.Append("Student Profile Inserted.");
                        //save the user picture
                        if (model.AttachStudentImage.FileSize > 0)
                        {
                            string StudentImgPath = Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\StudentAvatar");
                            if (GetBaseService().DirectoryFileService.CreateDirectoryIfNotExist(StudentImgPath))
                            {
                                StudentImgPath = string.Format("{0}\\{1}.{2}", StudentImgPath, model.RegNo, "jpg");
                                if (GetBaseService().DirectoryFileService.CreateFileFromBase64String(model.AttachStudentImage.FileContentsBase64, StudentImgPath))
                                {
                                    model.StudentImgPath = string.Format("~/AppFileRepo/StudentAvatar/{0}.{1}?r={2}", model.RegNo, "jpg", DateTime.Now.Ticks.ToString());
                                    model.BUserImgPath = model.StudentImgPath; // update the Student avatar
                                }
                            }
                        }
                        var CurrentUserInfo = GetLoginUserInfo();// get current user
                        await GetBaseService().AddActivity(ActivityType.Update, CurrentUserInfo.UserID, CurrentUserInfo.UserName, "Student Profile", "Inserted Student profile");
                        await _JobService.AddNewUserCreateEmailJob(Convert.ToInt64(CurrentUserInfo.ID), model.Name, model.Email, GetBaseService().GetAppRootPath());
                        return Json(new { stat = true, msg = rtnMsg.ToString(), rtnUrl = "/Student/Students" });
                    }
                    else
                        return Json(new { stat = false, msg = result.StatusMsg });
                }
            }
            else
                return Json(new { stat = false, msg = result.StatusMsg });
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
                List<StandardMasterBM> oAllStandards = await _StandardMasterService.GetAllStandards(500);
                Student StudentInfo = (Student)CR.StatusObj;
                //*****get user avtar************

                string UsrImgPath = string.Format("{0}\\{1}.{2}", Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\StudentAvatar"), StudentInfo.RegNo, "jpg");
                if (System.IO.File.Exists(UsrImgPath))
                    UsrImgPath = string.Format("~/AppFileRepo/StudentAvatar/{0}.{1}", StudentInfo.RegNo, "jpg");
                else
                    UsrImgPath = "~/img/avatar5.png";

                var TempVModel = new StudentProfileVM
                {
                    Id = StudentInfo.Id,
                    Name = StudentInfo.Name,
                    RegNo = StudentInfo.RegNo,                    
                    Address = StudentInfo.Address,
                    ContactNo = StudentInfo.ContactNo,
                    Email = StudentInfo.Email,
                    StandardId = StudentInfo.StandardId,
                    StudentImgPath = UsrImgPath,
                    AttachStudentImage = new FileUploadInfo()
                };
                TempVModel.AllStandards.AddRange(oAllStandards);// all standard list
                //var TempVModel = new StudentProfileVM();
                if (StudentInfo.LoginUserId != null)
                {
                    var AppVM = await _AppUserService.GetAppUserByID((int)StudentInfo.LoginUserId);
                    if (AppVM != null)
                        TempVModel.LoginId = AppVM.UserId;
                }
                VModel = await GetViewModel(TempVModel);
            }
            //*******************************
            return View("~/Views/Master/UpdateStudentProfile.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStudentProfile(StudentProfileVM model)
        {
            CommonResponce result = null;
            if (ModelState.IsValid)
            {
                result = await _StudentService.CheckDataValidation(model, false);
                if (!result.Stat)// validation failed
                    return Json(new { stat = false, msg = result.StatusMsg });
                result = await _StudentService.UpdateStudentProfile(model);

                if (result.Stat == true)
                {
                    var CurrentUserInfo = GetLoginUserInfo();// get current user
                    await GetBaseService().AddActivity(ActivityType.Update, CurrentUserInfo.UserID, CurrentUserInfo.UserName, "Student Profile", "Updated Student profile");

                    if (model.AttachStudentImage.FileSize > 0)
                    {
                        string StudentImgPath = Path.Combine(GetBaseService().GetAppRootPath(), "AppFileRepo\\StudentAvatar");
                        if (GetBaseService().DirectoryFileService.CreateDirectoryIfNotExist(StudentImgPath))
                        {
                            StudentImgPath = string.Format("{0}\\{1}.{2}", StudentImgPath, model.RegNo, "jpg");
                            if (GetBaseService().DirectoryFileService.CreateFileFromBase64String(model.AttachStudentImage.FileContentsBase64, StudentImgPath))
                            {
                                model.StudentImgPath = string.Format("~/AppFileRepo/StudentAvatar/{0}.{1}?r={2}", model.RegNo, "jpg", DateTime.Now.Ticks.ToString());
                                model.BUserImgPath = model.StudentImgPath; // update the Student avatar
                            }
                        }
                    }

                    return Json(new { stat = true, msg = "Student Profile Updated", rtnUrl = "/Student/Students" });
                }
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
                return Json(new { stat = false, msg = "Invalid Student Profile" });
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
