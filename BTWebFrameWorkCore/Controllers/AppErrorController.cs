using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppUtility.AppModels;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class AppErrorController : BaseController
    {
        private readonly IEmailSender _EmailSender;
        public AppErrorController(IEmailSender EmailSender)
        {            
            _EmailSender = EmailSender;           
        }
        public async Task<IActionResult> UnAuthorised401()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "UnAuthorised", ActionUrl = "/AppError/UnAuthorised401" } });

            var VModel = await GetViewModel(_BaseViewModel);
            return View(VModel);
        }
    }
}