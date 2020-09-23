using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel
{
    public class Subject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
