using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Tblmteacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public string EducationalQualification { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatdBy { get; set; }
    }
}
