using AppModel.BusinessModel.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AppModel.ViewModel
{
    public class ClassroomVM : BaseViewModel
    {
        public AppGridModel<ClassroomBM> ClassroomInfo { get; set; }
    }

    public class ClassRoomDetailsVM : BaseViewModel
    {
        public int Id { get; set; }
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
                        
        public string[] AsignTeacher { get; set; }
        public List<ClassMemberInfo> AllTeachers { get; set; }
                        
        public string[] AsignStudent { get; set; }
        public List<ClassMemberInfo> AllStudents { get; set; }

        public List<AppSelectListItem> Subjects { get; set; }
        public List<AppSelectListItem> Standards { get; set; }

        public string AutoCompleteSearchText { get; set; }
        public string TempClassRefId { get; set; }

        public string ApprootPath { get; set; }
        // custom properties
        public List<ClassMemberInfo> AsignStudentInfo
        {
            get
            {
                List<ClassMemberInfo> result = new List<ClassMemberInfo>();
                if (AllStudents != null && AllStudents.Count > 0)
                {
                    if (AsignStudent != null)
                    {
                        for (int i = 0; i < AsignStudent.Length; i++)
                        {
                            var TempTeacherInfo = AllStudents.Where(x => x.RegNo.Equals(AsignStudent[i])).FirstOrDefault();
                            if (TempTeacherInfo != null)
                                result.Add(TempTeacherInfo);
                        }
                    }
                }
                return result;
            }
        }
        public List<ClassMemberInfo> AsignTeacherInfo
        {
            get
            {
                List<ClassMemberInfo> result = new List<ClassMemberInfo>();
                if (AllTeachers != null && AllTeachers.Count > 0)
                {                    
                    if (AsignTeacher != null)
                    {
                        for (int i = 0; i < AsignTeacher.Length; i++)
                        {
                            var TempTeacherInfo = AllTeachers.Where(x => x.RegNo.Equals(AsignTeacher[i])).FirstOrDefault();
                            if (TempTeacherInfo != null)
                                result.Add(TempTeacherInfo);                            
                        }
                    }                    
                }
                return result;
            }
        }
        public string[] TeacherSearch
        {
            get
            {
                if(AllTeachers != null && AllTeachers.Count > 0)
                {
                    string[] result = new string[AllTeachers.Count];
                    for (int i = 0; i < AllTeachers.Count; i++)
                    {
                        result[i] = string.Format("{0} [{1}]",  AllTeachers[i].Name, AllTeachers[i].RegNo);
                    }
                    return result;
                }
                else
                {
                    string[] result = new string[1] {"No teacher data found"};
                    return result;
                }
            }
        }
        public string[] StudentSearch
        {
            get
            {
                if (AllStudents != null && AllStudents.Count > 0)
                {
                    string[] result = new string[AllStudents.Count];
                    for (int i = 0; i < AllStudents.Count; i++)
                    {
                        result[i] = string.Format("{0} [{1}]", AllStudents[i].Name, AllStudents[i].RegNo);
                    }
                    return result;
                }
                else
                {
                    string[] result = new string[1] { "No student data found" };
                    return result;
                }
            }
        }
    }

    public class ClassMemberInfo
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
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

    public class ClassRoomDashboardInfo
    {
        public long ClassId { get; set; }
        public string ClassRefId { get; set; }
        public string ClassName { get; set; }
        public string ClassTeacherName { get; set; }       
        public string TotalClassStudents { get; set; }
        public string ClassTime { get; set; }
        public string NextClass { get; set; }
        public string RecurringType { get; set; }
        public string Frequency { get; set; }
    }
}
