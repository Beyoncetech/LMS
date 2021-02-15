using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel
{
    public class CommonResponce
    {
        public Boolean Stat { get; set; }
        public string StatusMsg { get; set; }
        public dynamic StatusObj { get; set; }
    }

    public class SiteMapInfo
    {
        public string ID { get; set; }
        public string Group { get; set; }
        public string Parent { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string AccessType { get; set; }
        public string ActionUrl { get; set; }
        public string ActionPerm { get; set; }
        public int MenuOrder { get; set; }

        public string IconLeft { get; set; }
        public string BadgeText { get; set; }
    }

    public class AppSettingsConfiguration
    {
        public string ProjectName { get; set; }
        public string AppRootHost { get; set; }
    }

    public sealed class AppConfigSingletonRepository
    {
        private static AppSettingsConfiguration _AppConfig = null;        
        private static readonly object padlock = new object();

        
        public static AppSettingsConfiguration AppConfig
        {
            get
            {
                lock (padlock)
                {                    
                    return _AppConfig;
                }
            }
        }

        public static void UpdateConfiguration(AppSettingsConfiguration AppConfig)
        {
            lock (padlock)
            {
                if (_AppConfig == null)
                {
                    _AppConfig = AppConfig;
                }               
            }

        }
    }

    public class AppKeyValueInfo
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class FileUploadInfo
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileType { get; set; }
        public string FileContentsBase64 { get; set; }
    }

    public class LoginUserInfo
    {
        public string ID { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string UserType { get; set; }
        public string UserPerm { get; set; }
        public string UserGender { get; set; }
    }

    public class AppSelectListItem
    {        
        public string Value { get; set; }
        public string Text { get; set; }        
    }
    
}
