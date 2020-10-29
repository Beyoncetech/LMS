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
    }
}
