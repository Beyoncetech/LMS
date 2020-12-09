using AppModel.BusinessModel.Master;
using System.ComponentModel.DataAnnotations;

namespace AppModel.ViewModel
{
    public class TeacherVM:BaseViewModel
    {
        public AppGridModel<TeacherBM> TeacherInfo { get; set; }
    }
    public class TeacherProfileVM:BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Name cannot be blank")]
        public string Name { get; set; }
        public string Address { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [MaxLength(10, ErrorMessage = "Mobile Number cannot be greater than 10 digit")]
        [MinLength(10, ErrorMessage = "Mobile Number cannot be less than 10 digit")]
        public string ContactNo { get; set; }
        [Required(ErrorMessage ="Login Id cannot be blank")]
        public string LoginId { get; set; }
        public long? LoginUserId { get; set; }
        [Required(ErrorMessage ="Field cannot be blank")]
        [EmailAddress(ErrorMessage ="Invalid Email address")]
        public string Email { get; set; }
        public string EducationalQualification { get; set; }
        public string TeacherImgPath { get; set; }
        public FileUploadInfo AttachTeacherImage { get; set; }        
    }

}
