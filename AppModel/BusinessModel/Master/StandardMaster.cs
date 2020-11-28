using System;

namespace AppModel
{
    public class StandardMaster
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
