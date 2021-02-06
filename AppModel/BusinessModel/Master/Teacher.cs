using System;

namespace AppModel
{
    public class Teacher
    {
        public int Id { get; set; }
        public string RegNo { get; set; }
        public string Name { get; set; }
        public string LoginId { get; set; }
        public long? LoginUserId { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string EducationalQualification { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatdBy { get; set; }
    }
}
