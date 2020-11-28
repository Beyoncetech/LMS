using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBAL.Sevices;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class SubjectController : BaseController
    {
        private readonly ISubjectService _SubjectService;
        public SubjectController(ISubjectService SubjectService)
        {
            _SubjectService = SubjectService;
        }

        #region SUBJECT LIST      
        public async Task<IActionResult> Subjects()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Subject", ActionUrl = "/Subject/Subjects" } });

            BaseViewModel VModel = null;

            var result = await _SubjectService.GetAllSubjects(500, GetBaseService().GetAppRootPath());

            var TempVModel = new SubjectVM();
            TempVModel.SubjectInfo = new AppGridModel<SubjectBM>();
            TempVModel.SubjectInfo.Rows = result;

            VModel = await GetViewModel(TempVModel);

            return View("~/Views/Master/Subjects.cshtml", VModel);
        }

        #endregion  SUBJECT LIST

        #region ADD SUBJECT
        public async Task<IActionResult> AddSubject()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Subject", ActionUrl = "/Subject/AddSubject" } });
            BaseViewModel VModel = null;
            var TempVModel = new SubjectMasterVM();            
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
            return View("~/Views/Master/AddSubject.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddSubject(SubjectMasterVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _SubjectService.InsertSubject(model);
                //await GetBaseService().AddActivity(ActivityType.Update, model.UserID, model.UserName, "User Profile", "Updated user profile");
                if (result.Stat == true)
                    return Json(new { stat = true, msg = "Subject Inserted", rtnUrl = "/Subject/Subjects" });
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
                return Json(new { stat = false, msg = "Invalid Subject" });
        }
        #endregion ADD SUBJECT

        #region UPDATE SUBJECT
        public async Task<IActionResult> UpdateSubject(int SubjectID)
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Subject", ActionUrl = "/Subject/UpdateSubject" } });

            BaseViewModel VModel = null;
            SubjectBM oSubjectBM = await _SubjectService.GetSubjectBySubjectId(SubjectID);
            if (oSubjectBM!=null)
            {                
                var TempVModel = new SubjectMasterVM
                {
                    Id = oSubjectBM.Id,
                    Name = oSubjectBM.Name,
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
            return View("~/Views/Master/UpdateSubject.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateSubject(SubjectMasterVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _SubjectService.UpdateSubject(model);
                //await GetBaseService().AddActivity(ActivityType.Update, model.UserID, model.UserName, "User Profile", "Updated user profile");
                if (result.Stat == true)
                    return Json(new { stat = true, msg = "Subject Updated", rtnUrl = "/Subject/Subjects" });
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
                return Json(new { stat = false, msg = "Invalid Subject" });
        }
        #endregion UPDATE SUBJECT

        #region DELETE SUBJECT
        [HttpPost]
        public async Task<JsonResult> DeleteSubject(int SubjectId)
        {
            var result = await _SubjectService.DeleteSubject(SubjectId);
            if (result.Stat == true)
            {
                var CurrentUserInfo = GetLoginUserInfo();
                await GetBaseService().AddActivity(ActivityType.Delete, CurrentUserInfo.UserID, CurrentUserInfo.UserName,
                                                                                              "Delete Subject", string.Format("Delete Sujbect"));               
            }
            return Json(new { stat = result.Stat, msg = result.StatusMsg });
        }
        #endregion DELETE SUBJECT

        [HttpPost]
        public async Task<IActionResult> ReloadSubjects()
        {
            var result = await _SubjectService.GetAllSubjects(500, GetBaseService().GetAppRootPath());
            var TempModel = new AppGridModel<SubjectBM>();
            TempModel.Rows = result;
            return PartialView("_HTMLTable", TempModel);
        }
    }
}
