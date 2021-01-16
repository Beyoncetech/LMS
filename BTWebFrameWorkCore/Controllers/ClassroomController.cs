using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AppBAL.Sevices.Master;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        #region classroom details
        public async Task<IActionResult> ClassRoom(int id = 0)
        {
            CreateBreadCrumb(new[] {new { Name = "Home", ActionUrl = "#" },
                                    new { Name = "Classroom", ActionUrl = "/Classroom/Classroom" } });

            BaseViewModel VModel = null;

            //var result = await _ClassroomService.GetAllClassrooms(500, GetBaseService().GetAppRootPath());

            var TempVModel = new ClassRoomDetailsVM();
            // get value from service layer
            TempVModel.Subjects = new List<AppSelectListItem>
            {
                new AppSelectListItem { Value = "1", Text = "Math" },
                new AppSelectListItem { Value = "2", Text = "Hindi" },
                new AppSelectListItem { Value = "3", Text = "Biology"  },
            };

            TempVModel.Standards = new List<AppSelectListItem>
            {
                new AppSelectListItem { Value = "1", Text = "Class V" },
                new AppSelectListItem { Value = "2", Text = "Class VI" },
                new AppSelectListItem { Value = "3", Text = "XII"  },
            };
            TempVModel.Scheduler = new ClassSchedule();

            VModel = await GetViewModel(TempVModel);

            return View(VModel);
        }
        #endregion LIST
    }
}
