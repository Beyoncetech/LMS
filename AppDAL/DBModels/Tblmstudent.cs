﻿using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Tblmstudent
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegNo { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public int StandardId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
