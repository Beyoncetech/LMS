﻿using AppModel.CustomAttributes;
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
        [GridColumn(ColumnOrder = 1, ColumnWidth = 110, HeaderText = "Activity Type", Type = GridColumnType.Icon, ColumnFormat = "1:fas fa-plus;2:fas fa-edit;3:fas fa-trash")]
        public sbyte ActivityType { get; set; }
        [GridColumn(ColumnOrder = 5, HeaderText = "Activity Time", ColumnFormat = "dd-MMM-yyyy HH:mm:ss")]
        public DateTime ActivityTime { get; set; }
        public string UserId { get; set; }
        [GridColumn(ColumnOrder = 3, HeaderText = "User Name")]
        public string UserName { get; set; }
        [GridColumn(ColumnOrder = 2, HeaderText = "Origin")]
        public string Origin { get; set; }
        [GridColumn(ColumnOrder = 4, HeaderText = "Description")]
        public string Description { get; set; }
        [GridColumn(ColumnOrder = 6, HeaderText = "IsRead", Type = GridColumnType.CheckBox)]
        public bool IsRead { get; set; }
    }
}
