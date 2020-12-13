using AppModel.BusinessModel.Master;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppModel.ViewModel
{
    public class StudentVM : BaseViewModel
    {
        public AppGridModel<StudentBM> StudentInfo { get; set; }
    }

    public class StudentProfileVM:BaseViewModel
    {
        public StudentProfileVM() { AllStandards = new List<StandardMasterBM>(); }
        public int Id { get; set; }
        [Required(ErrorMessage = "Student Name cannot be blank")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Registration no. cannot be blank")]
        [Range(1,int.MaxValue)]
        public int RegNo { get; set; }
        [Required(ErrorMessage = "Login Id cannot be blank")]
        public string LoginId { get; set; } // student user id for log in   
        public long LoginUserId { get; set; }
        public string Address { get; set; }

        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [MaxLength(10, ErrorMessage = "Mobile Number cannot be greater than 10 digit")]
        [MinLength(10, ErrorMessage = "Mobile Number cannot be less than 10 digit")]
        public string ContactNo { get; set; }

        [Required(ErrorMessage ="Email cannot be blank")]
        [EmailAddress(ErrorMessage ="Invalid Email address")]
        public string Email { get; set; }        
        public int StandardId { get; set; }
        [Required(ErrorMessage ="Select a Standard")]
        public List<StandardMasterBM> AllStandards { get; set; }
        public string StudentImgPath { get; set; }
        public FileUploadInfo AttachStudentImage { get; set; }
    }
}
