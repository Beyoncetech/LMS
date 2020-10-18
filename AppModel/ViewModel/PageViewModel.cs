using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppModel.ViewModel
{
    public class LoginVM : BaseViewModel
    {
        [Required(ErrorMessage = "UserID is required")]
        public string UserName { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = "Password cannot be less than 3 char")]
        public string Password { get; set; }
        public Boolean RememberMe { get; set; }
    }

    public class DashboardVM : BaseViewModel
    {
        public Dictionary<string, string> DashboardData { get; set; }
    }

    public class CommonVM : BaseViewModel
    {
        public Dictionary<string, string> CommonData { get; set; }
    }

    public class UserProfileVM : BaseViewModel
    {
        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string Email { get; set; }
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        //[RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid mobile Number")]
        [MaxLength(10, ErrorMessage = "Mobile Number cannot be greater than 10 digit")]
        [MinLength(10, ErrorMessage = "Mobile Number cannot be less than 10 digit")]
        public string Mobile { get; set; }
        public DateTime? Dob { get; set; }
        public string UserImgPath { get; set; }
        public FileUploadInfo AttachUserImage { get; set; }
    }

    public class ChangeProfilePasswordVM : BaseViewModel
    {
        public long Id { get; set; }        
        public string UserID { get; set; }
        [Required(ErrorMessage = "Old Password is required")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "New Password is required")]
        [StringLength(16, ErrorMessage = "Password be between 3 and 16 characters", MinimumLength = 3)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Confirm New Password is required")]
        [Compare("NewPassword", ErrorMessage = "Confirm New Password does not matched")]
        public string ConfirmNewPassword { get; set; }
    }

    public class ActivityLogVM : BaseViewModel
    {
        public AppGridModel<ActivitylogBM> ActivityLogInfo { get; set; }        
    }

    public class SettingsVM : BaseViewModel
    {
        public string Flag { get; set; }
        public MailSettingBM MailSettings { get; set; }
        public GeneralSettingBM AppGeneralSettings { get; set; }
    }

    public class AppUsersVM : BaseViewModel
    {
        public AppGridModel<AppUserBM> AppUsersInfo { get; set; }
    }
}
