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
}
