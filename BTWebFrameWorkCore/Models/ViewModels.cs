using AppModel.ViewModel;
using System.ComponentModel.DataAnnotations;

namespace BTWebAppFrameWorkCore.Models
{

    public class UserContactInfo : BaseViewModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string UserEmail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Description { get; set; }
    }
    
}
