using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Mjob
    {
        public long JobId { get; set; }
        public string RefNo { get; set; }
        public string Command { get; set; }
        public string CommandData { get; set; }
        public string Priority { get; set; }
        public DateTime? CreatedOn { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? FinishedOn { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMsg { get; set; }
        public sbyte? Status { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
    }
}
