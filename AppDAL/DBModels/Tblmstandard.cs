using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Tblmstandard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
