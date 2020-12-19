using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Tblmclassroom
    {
        public int Id { get; set; }
        public string RefId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SubjectId { get; set; }
        public int StandardId { get; set; }
        public int? ClassActivationThreshold { get; set; }
        public string Scheduler { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
