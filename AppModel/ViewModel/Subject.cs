using AppModel.BusinessModel.Master;
using System.ComponentModel.DataAnnotations;

namespace AppModel.ViewModel
{
    public class SubjectVM:BaseViewModel
    {
        public AppGridModel<SubjectBM> SubjectInfo { get; set; }
    }

    public class SubjectMasterVM:BaseViewModel
    {
        public int Id { get; set; }    
        [Required(ErrorMessage ="Subject Name cannot be blank")]
        public string Name { get; set; }
    }
}
