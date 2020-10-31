using AppModel.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppModel
{
    public class LoginUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserPerm { get; set; }
        public ulong IsActive { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
    }

    public class UserProfile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
    }

    public class ActivitylogBM
    {
        public long Id { get; set; }
        [GridColumn(ColumnOrder = 1, ColumnWidth = 110, HeaderText = "Activity Type", Type = GridColumnType.Icon, ColumnFormat = "1:fas fa-plus;2:fas fa-edit;3:fas fa-trash")]
        public sbyte ActivityType { get; set; }
        [GridColumn(ColumnOrder = 5, HeaderText = "Activity Time", Type = GridColumnType.Date, ColumnFormat = "dd-MMM-yyyy HH:mm:ss")]
        public DateTime ActivityTime { get; set; }
        public string UserId { get; set; }
        [GridColumn(ColumnOrder = 3, HeaderText = "User Name")]
        public string UserName { get; set; }
        [GridColumn(ColumnOrder = 2, HeaderText = "Origin")]
        public string Origin { get; set; }
        [GridColumn(ColumnOrder = 4, HeaderText = "Description")]
        public string Description { get; set; }
        [GridColumn(ColumnOrder = 6, HeaderText = "IsRead", Type = GridColumnType.CheckBox)]
        public bool IsRead { get; set; }
    }

    public class MailSettingBM
    {
        [Required]
        public string FromMailID { get; set; }
        [Required]
        public string SmtpServer { get; set; }
        [Required]
        public string MailUserID { get; set; }
        [Required]
        public string MailPassword { get; set; }
        [Required]
        public string SmtpServerPort { get; set; }        
        [Display(Name = "Mail Password")]
        public string MailPasswordEx
        {
            get
            {
                string TempPass = "";
                if (MailPassword != null && MailPassword.Length > 0)
                    TempPass = new string('*', MailPassword.Length);
                
                return TempPass;
            }
            set
            {
                string TempPass = "";
                if (MailPassword != null && MailPassword.Length > 0)
                    TempPass = new string('*', MailPassword.Length);
                if (!value.Equals(TempPass))
                    MailPassword = value;
            }
        }
    }

    public class GeneralSettingBM
    {
        [Required]
        [Range(0, 720, ErrorMessage = "Please enter valid minute between 0 to 720")]
        public string ClassActiveThresholdTime { get; set; }
        public string SupportMailID { get; set; }
    }

    public class AppUserBM
    {        
        public long Id { get; set; }
        [GridColumn(ColumnOrder = 1, HeaderText = "Avatar", Type = GridColumnType.Image)]
        public string UserAvatar { get; set; }
        [GridColumn(ColumnOrder = 1, HeaderText = "Name")]
        public string Name { get; set; }
        [GridColumn(ColumnOrder = 2, HeaderText = "User ID")]
        public string UserId { get; set; }
        [GridColumn(ColumnOrder = 3, HeaderText = "User Type")]
        public string UserType { get; set; }
        [GridColumn(ColumnOrder = 4, HeaderText = "Gender")]
        public string Gender { get; set; }
        [GridColumn(ColumnOrder = 5, HeaderText = "Email")]
        public string Email { get; set; }
        [GridColumn(ColumnOrder = 6, HeaderText = "Is Active", Type = GridColumnType.CheckBox)]
        public ulong IsActive { get; set; }
        [GridColumn(ColumnOrder = 7, HeaderText = "Action", Type = GridColumnType.Action, ColumnFormat = "Icon=Edit:fa fa-fw fa-address-card;Icon=Delete:fa fa-fw fa-user-times;Icon=Reset Password:fa fa-fw fa-retweet")]
        public string Action { get; set; }        
        public string Mobile { get; set; }        
        public string UserPerm { get; set; }
        public ulong IsPassReset { get; set; }
        public DateTime? ResetPassValidity { get; set; }
        public string ResetPassContext { get; set; }        
        public DateTime? Dob { get; set; }
        public string CustomData { get; set; }        
    }

    public class AppJobBM
    {
        public long JobId { get; set; }
        public string RefNo { get; set; }
        public string Command { get; set; }
        public string CommandData { get; set; }
        public string Priority { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }        
        public sbyte? Status { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
