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
    public class StandardMasterController : BaseController
    {
       private readonly IStandardMasterService _StandardMasterService;
        public StandardMasterController(IStandardMasterService StandardMasterService)
        {
            _StandardMasterService = StandardMasterService;
        }

        public async Task<IActionResult> StandardMasters()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "StandardMaster", ActionUrl = "/StandardMaster/StandardMasters" } });

            BaseViewModel VModel = null;

            var result = await _StandardMasterService.GetAllStandards(500, GetBaseService().GetAppRootPath());

            var TempVModel = new StandardVM();
            TempVModel.StandardMasterInfo= new AppGridModel<StandardMasterBM>();
            TempVModel.StandardMasterInfo.Rows = result;

            VModel = await GetViewModel(TempVModel);

            return View("~/Views/Master/StandardMasters.cshtml", VModel);
        }

        #region ADD STANDARD
        public async Task<IActionResult> AddStandard()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Standard", ActionUrl = "/StandardMaster/AddStandard" } });
            BaseViewModel VModel = null;
            var TempVModel = new StandardMasterVM();            
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
            return View("~/Views/Master/AddStandard.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> AddStandard(StandardMasterVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _StandardMasterService.InsertStandard(model);
                if (result.Stat == true)
                {
                    var CurrentUserInfo = GetLoginUserInfo();
                    await GetBaseService().AddActivity(ActivityType.Create, CurrentUserInfo.UserID, CurrentUserInfo.UserName, "Standard Master", "Add Standard");
                    return Json(new { stat = true, msg = "Standard Inserted", rtnUrl = "/StandardMaster/StandardMasters" });
                }
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
                return Json(new { stat = false, msg = "Invalid Standard" });
        }
        #endregion ADD STANDARD

        #region UPDATE STANDARD
        public async Task<IActionResult> UpdateStandard(int Id)
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Student", ActionUrl = "/StandardMaster/UpdateStandard"} });

            BaseViewModel VModel = null;
            CommonResponce CR = await _StandardMasterService.GetStandardByStandardId(Id);
            if (CR.Stat)
            {
                StandardMaster oStandardMaster = (StandardMaster)CR.StatusObj;
                var TempVModel = new StandardMasterVM
                {
                    Id = oStandardMaster.Id,
                    Name = oStandardMaster.Name
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
            return View("~/Views/Master/UpdateStandard.cshtml", VModel);
        }

        [HttpPost]
        public async Task<JsonResult> UpdateStandard(StandardMasterVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await _StandardMasterService.UpdateStandard(model);
                //await GetBaseService().AddActivity(ActivityType.Update, model.UserID, model.UserName, "User Profile", "Updated user profile");
                if (result.Stat == true)
                {
                    var CurrentUserInfo = GetLoginUserInfo();
                    await GetBaseService().AddActivity(ActivityType.Update, CurrentUserInfo.UserID, CurrentUserInfo.UserName, "Standard Master", "Update Standard");
                    return Json(new { stat = true, msg = "Standard Updated", rtnUrl = "/StandardMaster/StandardMasters" });
                }
                else
                    return Json(new { stat = false, msg = result.StatusMsg });
            }
            else
                return Json(new { stat = false, msg = "Invalid Standard" });
        }
        #endregion UPDATE STANDARD

        #region DELETE STANDARD
        [HttpPost]
        public async Task<JsonResult> DeleteStandard(int Id)
        {
            var oStandard = await _StandardMasterService.GetStandardByStandardId(Id).ConfigureAwait(false);
            if (oStandard != null)
            {
                var result = await _StandardMasterService.DeleteStandard(Id).ConfigureAwait(false);
                if(result.Stat)
                {
                    var CurrentUserInfo = GetLoginUserInfo();
                    await GetBaseService().AddActivity(ActivityType.Create, CurrentUserInfo.UserID, CurrentUserInfo.UserName, "Delete Standard", string.Format("Deleted Standard"));
                }
                return Json(new { stat = result.Stat, msg = result.StatusMsg });
            }
            else
                return Json(new {stat=false,msg="Not a valid Standard" });
        }

        [HttpPost]
        public async Task<IActionResult> ReloadStandards()
        {
            var result = await _StandardMasterService.GetAllStandards(500, GetBaseService().GetAppRootPath());
            var TempModel = new AppGridModel<StandardMasterBM>();
            TempModel.Rows = result;
            return PartialView("_HTMLTable", TempModel);
        }
        #endregion DELETE STANDARD

    }
}
