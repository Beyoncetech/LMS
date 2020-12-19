using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Tblrteacherclassroom
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int ClassRoomId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? ClosedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
