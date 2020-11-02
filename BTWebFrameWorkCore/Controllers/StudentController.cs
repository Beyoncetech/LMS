using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

        public StudentController(IStudentService StudentService)
        {
            _StudentService = StudentService;
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

            return View("~/Views/Master/Students.cshtml",VModel);
        }

        #endregion

        #region ADD STUDENT
        public async Task<IActionResult> StudentProfile()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Student", ActionUrl = "/Student/StudentProfile" } });

            BaseViewModel VModel = null;
            // CommonResponce CR=await _StudentService.GetStudentByStudentId(StudentID);
            //if(CR.Stat)
           // {
                /*
                Student StudentInfo =(Student)CR.StatusObj;
                var TempVModel = new StudentProfileVM
                {
                    Id=StudentInfo.Id,
                    Name=StudentInfo.Name,
                    RegNo=StudentInfo.RegNo,
                    Address=StudentInfo.Address,
                    ContactNo=StudentInfo.ContactNo,
                    Email=StudentInfo.Email
                };
                */
                var TempVModel = new StudentProfileVM();
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
            if (ModelState.IsValid)
            {
                var result = await _StudentService.InsertStudentProfile(model);
                //await GetBaseService().AddActivity(ActivityType.Update, model.UserID, model.UserName, "User Profile", "Updated user profile");
                if (result.Stat == true)
                    return Json(new { stat = true, msg = "Student Profile Inserted",rtnUrl="/Student/Students" });
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

                Student StudentInfo = (Student)CR.StatusObj;
                var TempVModel = new StudentProfileVM
                {
                    Id = StudentInfo.Id,
                    Name = StudentInfo.Name,
                    RegNo = StudentInfo.RegNo,
                    Address = StudentInfo.Address,
                    ContactNo = StudentInfo.ContactNo,
                    Email = StudentInfo.Email
                };

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
    }
}
