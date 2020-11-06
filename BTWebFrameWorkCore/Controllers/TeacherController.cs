using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBAL.Sevices.Master;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _TeacherService;

        public TeacherController(ITeacherService TeacherServce)
        {
            _TeacherService = TeacherServce;
        }
        public async Task<IActionResult> Teachers()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Teacher", ActionUrl = "/Teacher/Teachers" } });

            BaseViewModel VModel = null;

            //var result = await _TeacherService.GetAllTeachers();

            //var TempVModel = new TeacherVM();
            //TempVModel.TeacherInfo= new AppGridModel<TeacherBM>();
            //TempVModel.TeacherInfo.Rows = result;

            //VModel = await GetViewModel(TempVModel);

            return View("~/Views/Master/Teachers.cshtml",VModel);
        }

    }
}
