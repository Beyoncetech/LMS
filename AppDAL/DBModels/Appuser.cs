using System;
using System.Collections.Generic;

namespace AppDAL.DBModels
{
    public partial class Appuser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public string UserPerm { get; set; }
        public ulong IsPassReset { get; set; }
        public DateTime? ResetPassValidity { get; set; }
        public string ResetPassContext { get; set; }
        public ulong IsActive { get; set; }
        public DateTime? Dob { get; set; }
        public string CustomData { get; set; }
    }
}
