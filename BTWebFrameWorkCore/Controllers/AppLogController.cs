using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBAL.Sevices.AppCore;
using AppModel;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BTWebAppFrameWorkCore.Controllers
{
    [Authorize(Policy = "AllowAuthUsers")]
    public class AppLogController : BaseController
    {
        private readonly IAppLogService _AppLogService;

        public AppLogController(IAppLogService LogService)
        {
            _AppLogService = LogService;            
        }

        #region activity Log
        public async Task<IActionResult> ActivityLog()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Activity Log", ActionUrl = "/AppLog/ActivityLog" } });

            BaseViewModel VModel = null;          

            
            var result = await _AppLogService.GetAllActivityLog();

            var TempVModel = new ActivityLogVM();
            TempVModel.ActivityLogInfo = new AppGridModel<ActivitylogBM>();
            TempVModel.ActivityLogInfo.Rows = result;

            VModel = await GetViewModel(TempVModel);
                        
            return View(VModel);
        }
        #endregion
    }
}
