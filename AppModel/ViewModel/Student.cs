using AppModel.BusinessModel.Master;

namespace AppModel.ViewModel
{
    public class StudentVM : BaseViewModel
    {
        public AppGridModel<StudentBM> StudentInfo { get; set; }
    }

}
