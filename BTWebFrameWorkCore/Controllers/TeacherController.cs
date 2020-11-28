using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBAL.Sevices.Master;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _TeacherService;
        private readonly IAppUserService _AppUserService;
        public TeacherController(ITeacherService TeacherService,IAppUserService AppUserService)
        {
            _TeacherService = TeacherService;
            _AppUserService = AppUserService;
    }

        #region TEACHER LIST
        public async Task<IActionResult> Teachers()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Teacher", ActionUrl = "/Teacher/Teachers" } });

            BaseViewModel VModel = null;

           var result = await _TeacherService.GetAllTeachers(500,GetBaseService().GetAppRootPath());

            var TempVModel = new TeacherVM();
            TempVModel.TeacherInfo= new AppGridModel<TeacherBM>();
            TempVModel.TeacherInfo.Rows = result;

            VModel = await GetViewModel(TempVModel);

            return View("~/Views/Master/Teachers.cshtml",VModel);
        }
        #endregion TEACHER LIST

        #region ADD TEACHER
        public async Task<IActionResult> TeacherProfile()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Teacher", ActionUrl = "/Teacher/TeacherProfile" } });

            BaseViewModel VModel = null;
            var TempVModel = new TeacherProfileVM();
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
            return View("~/Views/Master/TeacherProfile.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> TeacherProfile(TeacherProfileVM model)
        {
            StringBuilder rtnMsg = new StringBuilder();
                
            if (ModelState.IsValid)
            {
                var result = await _TeacherService.InsertTeacherProfile(model);
                
                if (result.Stat == true)
                {
                    rtnMsg.Append("Teacher Profile Inserted.");
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
                        rtnMsg.Append(" Failed to add Teacher Login ID.");
                    else
                    {
                        var CurrentUserInfo = GetLoginUserInfo();
                        await GetBaseService().AddActivity(ActivityType.Update, CurrentUserInfo.UserID, CurrentUserInfo.UserName, "Teacher Profile", "Inserted Teacher profile");
                    }
                    return Json(new { stat = true, msg = rtnMsg.ToString(), rtnUrl = "/Teacher/Teachers" });
            }
            else
                return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
                return Json(new { stat = false, msg = "Invalid Teacher Profile" });
        }

        #endregion ADD TEACHER

        #region UPDATE TEACHER
        public async Task<IActionResult> UpdateTeacherProfile(int TeacherID)
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Teacher", ActionUrl = "/Teacher/UpdateTeacherProfile" } });

            BaseViewModel VModel = null;
            CommonResponce CR = await _TeacherService.GetTeacherByTeacherId(TeacherID);
            if (CR.Stat)
            {

                Teacher TeacherInfo = (Teacher)CR.StatusObj;
                var TempVModel = new TeacherProfileVM
                {
                    Id = TeacherInfo.Id,
                    Name = TeacherInfo.Name,
                    Address = TeacherInfo.Address,
                    ContactNo = TeacherInfo.ContactNo,
                    Email = TeacherInfo.Email,
                    EducationalQualification=TeacherInfo.EducationalQualification
                };
                
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
            return View("~/Views/Master/UpdateTeacherProfile.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateTeacherProfile(TeacherProfileVM model)
        {
           // if (ModelState.IsValid)
            //{
                var result = await _TeacherService.UpdateTeacherProfile(model);
                //await GetBaseService().AddActivity(ActivityType.Update, model.UserID, model.UserName, "User Profile", "Updated user profile");
                if (result.Stat == true)
                    return Json(new { stat = true, msg = "Teacher Profile Updated", rtnUrl = "/Teacher/Teachers" });
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
          //  }
            //else
              //  return Json(new { stat = false, msg = "Invalid Teacher Profile" });
        }
        #endregion UPDATE TEACHER

        #region DELETE TEACHER
        [HttpPost]
        public async Task<JsonResult> DeleteTeacher(int TeacherId)
        {
            var result = await _TeacherService.DeleteTeacherProfile(TeacherId);
            if (result.Stat == true)
            {
                var CurrentUserInfo = GetLoginUserInfo();
                await GetBaseService().AddActivity(ActivityType.Delete, CurrentUserInfo.UserID, CurrentUserInfo.UserName,
                                                                                              "Delete Teacher", string.Format("Delete Teacher"));
            }
            return Json(new { stat = result.Stat, msg = result.StatusMsg });
        }


        [HttpPost]
        public async Task<IActionResult> ReloadSTeacher()
        {
            var result = await _TeacherService.GetAllTeachers(500, GetBaseService().GetAppRootPath());
            var TempModel = new AppGridModel<TeacherBM>();
            TempModel.Rows = result;
            return PartialView("_HTMLTable", TempModel);
        }
        #endregion DELETE TEACHER
    }
}
