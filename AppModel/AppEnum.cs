using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel
{
    public enum ActivityType
    {
        Create = 1,
        Update = 2,
        Delete = 3,
        Other = 4
    }
    public enum ErrorType
    {
        Error = 1,
        Exception = 2,
        Warning = 3,
        Info = 4
    }
}
