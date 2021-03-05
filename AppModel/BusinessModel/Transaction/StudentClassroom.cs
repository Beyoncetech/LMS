using System;

namespace AppModel
{
    public class StudentClassroom
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int ClassRoomId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
