using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Appsetting
    {
        public int Id { get; set; }
        public string AppKey { get; set; }
        public string AppVal { get; set; }
    }
}
