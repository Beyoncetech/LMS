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
        public IActionResult UnAuthorised401()
        {
            return View(_BaseViewModel);
        }
    }
}