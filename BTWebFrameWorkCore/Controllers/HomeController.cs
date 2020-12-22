using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BTWebAppFrameWorkCore.Models;
using AppUtility.AppModels;
using AppModel.ViewModel;
using AppBAL.Sevices.AppCore;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender _EmailSender;        

        public HomeController(ILogger<HomeController> logger, IEmailSender EmailSender)
        {
            _logger = logger;
            _EmailSender = EmailSender;            
        }

        public async Task<IActionResult> Index()
        {
            throw new Exception("this is demo exception");
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" } , 
                                    new { Name = "Dashboard", ActionUrl = "#" } });
            var VModel = await GetViewModel<CommonVM>();
            return View(VModel);
        }

        public async Task<IActionResult> Index2()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" } ,
                                    new { Name = "Dashboard 2", ActionUrl = "#" } });
            var VModel = await GetViewModel<CommonVM>();
            return View(VModel);
        }

        public async Task<IActionResult> Index3()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" } ,
                                    new { Name = "Dashboard 3", ActionUrl = "#" } });
            var VModel = await GetViewModel<CommonVM>();
            return View(VModel);
        }

        public async Task<IActionResult> Widgets()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" } ,
                                    new { Name = "Widgets", ActionUrl = "#" } });
            var VModel = await GetViewModel<CommonVM>();
            return View(VModel);
        }

        public async Task<IActionResult> Form1()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" } ,
                                    new { Name = "Form 1", ActionUrl = "#" } });
            var VModel = await GetViewModel<CommonVM>();
            return View(VModel);
        }
       

        public async Task<IActionResult> Modals()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" } ,
                                    new { Name = "Modals", ActionUrl = "#" } });
            var VModel = await GetViewModel<CommonVM>();
            return View(VModel);
        }

        public async Task<IActionResult> Tables()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" } ,
                                    new { Name = "Tables", ActionUrl = "#" } });
            var VModel = await GetViewModel<CommonVM>();
            return View(VModel);
        }

        public IActionResult OurServices()
        {
            
            return View(_BaseViewModel);
        }

        public IActionResult OurProducts()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" } ,
                                    new { Name = "FAQ", ActionUrl = "#" } });
            return View(_BaseViewModel);
        }

        public IActionResult ContactUs()
        {
            var VModel = GetViewModel<UserContactInfo>();
            //BaseViewModel model = new UserContactInfo();
            return View(VModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(UserContactInfo model)
        {            
            if(ModelState.IsValid)
            {
                var message = new EmailMessage(new string[] { model.UserEmail }, "Thanks for your query at BEYONCETECH", string.Format("Hi {0},\n Thanks for showing interests at BEYONCETECH. \n\n Our team member will get back to you shortly. ", model.UserName), true, null);
                await _EmailSender.SendEmailAsync(message);

                string LastError = _EmailSender.GetLastError();

                if(string.IsNullOrEmpty(LastError))
                {
                    message = new EmailMessage(new string[] { "info@beyoncetechsolutions.com", "debabrata.beyoncetech@gmail.com", "tuhin.beyoncetech@gmail.com" }, string.Format("[{0}] Query at BEYONCETECH - {1}", model.UserName, model.Subject), string.Format("Hi, \n User Message: \n {0} \n User Email: \n {1}", model.Description, model.UserEmail), true, null);                    
                    await _EmailSender.SendEmailAsync(message);
                    return Json(new { Stat = true, Msg = "Successfully send your query" });                    
                }
                else
                {
                    return Json(new { Stat = false, Msg = LastError });
                }                
            }
            else
            {
                return Json(new { Stat = false, Msg = "Data is Not Valid" });
            }           
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShortContactUs(BaseViewModel model)
        {
            //if (!string.IsNullOrEmpty(model.BaseEmail))
            //{
            //    var message = new EmailMessage(new string[] { model.BaseEmail }, "Thanks for your interests at BEYONCETECH", "Hi,\n Thanks for showing interests at BEYONCETECH. \n\n Our team member will get back to you shortly. ", true, null);
            //    await _EmailSender.SendEmailAsync(message);

            //    string LastError = _EmailSender.GetLastError();

            //    if (string.IsNullOrEmpty(LastError))
            //    {
            //        message = new EmailMessage(new string[] { "info@beyoncetechsolutions.com", "debabrata.beyoncetech@gmail.com", "tuhin.beyoncetech@gmail.com" }, "BEYONCETECH - User interests", string.Format("Hi,\n User showing interests at BEYONCETECH. \n\n User Email : {0}. ", model.BaseEmail), true, null);                    
            //        await _EmailSender.SendEmailAsync(message);
            //        return Json(new { Stat = true, Msg = "Successfully submit your email" });
            //    }
            //    else
            //    {
            //        return Json(new { Stat = false, Msg = LastError });
            //    }
            //}
            //else
            //{
                return Json(new { Stat = false, Msg = "Model is Not Valid" });
            //}

        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Chat()
        {
            return View(_BaseViewModel);
        }
    }
}
