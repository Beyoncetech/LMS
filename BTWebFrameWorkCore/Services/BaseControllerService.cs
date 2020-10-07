using AppModel;
using AppUtility.AppIO;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BTWebAppFrameWorkCore.Services
{
    public interface IBaseControllerService
    {
        string GetAppRootPath();
        string GetUserAvatarPath(string FileName);
        AppSettingsConfiguration AppSettingConfigaration { get; }
        IDirectoryFileService DirectoryFileService { get; }
    }

    public class BaseControllerService : IBaseControllerService
    {
        private readonly AppSettingsConfiguration _AppSettingConfig;
        private readonly IWebHostEnvironment _HostingEnvironment;
        private readonly IDirectoryFileService _DirectoryFileService;
        public BaseControllerService(IDirectoryFileService DirectoryFileService, IWebHostEnvironment HostingEnvironment,
            AppSettingsConfiguration AppSettingConfig)
        {
            _DirectoryFileService = DirectoryFileService;
            _HostingEnvironment = HostingEnvironment;
            _AppSettingConfig = AppSettingConfig;
        }

        public string GetAppRootPath()
        {
            return _HostingEnvironment.WebRootPath;
        }

        public string GetUserAvatarPath(string FileName)
        {
            string UserAvatarPath = "~/assets/img/AppUser/BlankUser.jpg";
            try
            {
                string UsrImgPath = Path.Combine(_HostingEnvironment.WebRootPath, "AppFileRepo\\UserAvatar");                
                UsrImgPath = string.Format("{0}\\{1}", UsrImgPath, FileName);
                if (File.Exists(UsrImgPath))
                {
                    UsrImgPath = string.Format("~/AppFileRepo/UserAvatar/{0}?r={1}", FileName, DateTime.Now.Ticks.ToString());
                    return UsrImgPath;
                }
                else
                    return UserAvatarPath;
            }
            catch (Exception)
            {
                return UserAvatarPath;
            }
        }       

        public AppSettingsConfiguration AppSettingConfigaration
        {
            get
            {
                return _AppSettingConfig;
            }
        }

        public IDirectoryFileService DirectoryFileService
        {
            get
            {
                return _DirectoryFileService;
            }
        }

    }
}
