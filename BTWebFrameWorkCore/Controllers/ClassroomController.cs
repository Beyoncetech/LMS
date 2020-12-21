using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBAL.Sevices.Master;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace BTWebAppFrameWorkCore.Controllers
{
    public class ClassroomController : BaseController
    {
        private readonly IClassroomService _ClassroomService;
        public ClassroomController(IClassroomService ClassrommService)
        {
            _ClassroomService = ClassrommService;
        }
        #region LIST
        public async Task<IActionResult> Classrooms()
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Classroom", ActionUrl = "/Classroom/Classrooms" } });

            BaseViewModel VModel = null;

            var result = await _ClassroomService.GetAllClassrooms(500, GetBaseService().GetAppRootPath());

            var TempVModel = new ClassroomVM();
            TempVModel.ClassroomInfo = new AppGridModel<ClassroomBM>();
            TempVModel.ClassroomInfo.Rows = result;

            VModel = await GetViewModel(TempVModel);

            return View("~/Views/Master/Classrooms.cshtml", VModel);
        }
        #endregion LIST
    }
}
