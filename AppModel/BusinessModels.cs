using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel
{
    public class LoginUser
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }        
        public string UserPerm { get; set; }        
        public ulong IsActive { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }
    }

    public class UserProfile
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserType { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public DateTime? Dob { get; set; }
        public string Gender { get; set; }        
    }

    public class ActivitylogBM
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
