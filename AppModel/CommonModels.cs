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
        public string IconRight { get; set; }
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
}
