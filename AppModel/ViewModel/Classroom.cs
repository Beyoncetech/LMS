using AppModel.BusinessModel.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AppModel.ViewModel
{
   public class ClassroomVM:BaseViewModel
    {
        public AppGridModel<ClassroomBM> ClassroomInfo { get; set; }
    }

    public class ClassRoomDetailsVM : BaseViewModel
    {
        public long Id { get; set; }
        [Required(ErrorMessage = "Class reference Id is required")]
        public string RefId { get; set; }
        [Required]
        public string Name { get; set; }        
        public string Description { get; set; }
        [Required(ErrorMessage = "Select a Class subject is required")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Select a Class standard is required")]
        public int StandardId { get; set; }        
        public int? ClassActivationThreshold { get; set; }
        [Required(ErrorMessage = "Select a time schedule for the class")]
        public ClassSchedule Scheduler { get; set; }

        [Required(ErrorMessage = "Select a Teacher for Clas room")]
        public List<ClassTeacher> Teachers { get; set; }

        [Required(ErrorMessage = "Select student for Clas room")]
        public List<ClassStudent> Students { get; set; }

        public List<AppSelectListItem> Subjects { get; set; }
        public List<AppSelectListItem> Standards { get; set; }
    }

    public class ClassTeacher
    {
        public int Id { get; set; }        
        public string Name { get; set; }
        public string Quification { get; set; }
        public string Avatar { get; set; }
    }

    public class ClassStudent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Standard { get; set; }
        public string Avatar { get; set; }
    }

    public class ClassSchedule
    {
        // pattern properties
        // FrequencyType: DAILY/WEEKLY/MONTHLY
        [Required(ErrorMessage = "Select FrequencyType")]
        public string FrequencyType { get; set; }
        // DaysOFWeek: for WEEKLY FrequencyType, List the Day name like MON/TUE/WED etc
        public string[] DaysOFWeek { get; set; }
        // MonthsName: for MONTHLY FrequencyType, List the Month name like JAN/MAR/APR etc
        public int Day { get; set; }
        public string[] MonthsName { get; set; }
        // range properties
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }
}
