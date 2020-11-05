using AppModel.BusinessModel.Master;

namespace AppModel.ViewModel
{
    public class TeacherVM:BaseViewModel
    {
        public AppGridModel<TeacherBM> TeacherInfo { get; set; }
    }
    public class TeacherProfileVM:BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string EducationalQualification { get; set; }
    }

}
