using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBAL.Sevices.AppCore;
using AppModel.ViewModel;
using AppUtility.AppModels;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class AppErrorController : BaseController
    {
        private readonly IEmailSender _EmailSender;
        public AppErrorController(IEmailSender objEmailSender)
        {            
            _EmailSender = objEmailSender;           
        }
        public async Task<IActionResult> UnAuthorised401()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "UnAuthorised", ActionUrl = "/AppError/UnAuthorised401" } });

            var VModel = await GetViewModel(_BaseViewModel);
            return View(VModel);
        }

        public async Task<IActionResult> UnhandleErrors()
        {
            var model = new AppErrorVM();
            var ex = HttpContext.Features.Get<IExceptionHandlerFeature>();

            model.ErrorCode = ex.Error.GetHashCode().ToString();
            model.ErrorMessage = ex.Error.Message;
            model.ErrorDescription = ex.Error.StackTrace;
            model.TrackTrace = ex.Error.StackTrace;
            model.ErrorSource = ex.Error.Source;
            
            var VModel = await GetViewModel(model);
            return View(VModel);
        }

        [HttpPost]        
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> UnhandleErrors(AppErrorVM model)
        {
            // code to send mail
            await Task.Delay(10).ConfigureAwait(false);
            return Json(new { stat = false, msg = "This feature is not activated." });
        }

        [Route("/AppError/HandleStatusCodeErrors/{code:int}")]
        public async Task<IActionResult> HandleStatusCodeErrors(int code)
        {
            var model = new AppStatusCodeErrorVM();
            
            model.ErrorCode = code.ToString();
           
            var VModel = await GetViewModel(model);
            return View(VModel);
        }
    }
}