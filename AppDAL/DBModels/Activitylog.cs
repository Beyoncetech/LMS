using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Activitylog
    {
        public long Id { get; set; }
        public sbyte ActivityType { get; set; }
        public DateTime ActivityTime { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Origin { get; set; }
        public string Description { get; set; }
        public bool IsRead { get; set; }
    }
}
