using AppModel.BusinessModel.Master;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel.ViewModel
{
   public class ClassroomVM:BaseViewModel
    {
        public AppGridModel<ClassroomBM> ClassroomInfo { get; set; }
    }
}
