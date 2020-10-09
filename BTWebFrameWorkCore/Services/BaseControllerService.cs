using AppDAL.DBRepository;
using AppModel;
using AppModel.ViewModel;
using AppUtility.AppIO;
using AppUtility.Extension;
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
        Task<UserActivityInfo> GetUnreadUserActivity(int ReturnRowCount);
        Task AddActivity(ActivityType ActvType, string UserID, string UserName, string Origin, string Description);
        AppSettingsConfiguration AppSettingConfigaration { get; }
        IDirectoryFileService DirectoryFileService { get; }
        Task<bool> MarkAllUnreadActivityAsRead();
    }

    public class BaseControllerService : IBaseControllerService
    {
        private readonly AppSettingsConfiguration _AppSettingConfig;
        private readonly IWebHostEnvironment _HostingEnvironment;
        private readonly IDirectoryFileService _DirectoryFileService;
        private readonly IAppActivityLogRepository _AppActivityLogRepository;
        public BaseControllerService(IDirectoryFileService DirectoryFileService, IWebHostEnvironment HostingEnvironment,
            AppSettingsConfiguration AppSettingConfig, IAppActivityLogRepository AppActivityLogRepository)
        {
            _DirectoryFileService = DirectoryFileService;
            _HostingEnvironment = HostingEnvironment;
            _AppSettingConfig = AppSettingConfig;
            _AppActivityLogRepository = AppActivityLogRepository;
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

        public async Task<UserActivityInfo> GetUnreadUserActivity(int ReturnRowCount)
        {
            UserActivityInfo ActivityInfo = new UserActivityInfo();
            ActivityInfo.TotalActivity = "0";
            ActivityInfo.MsgItems = new List<UserMessageInfo>();
            var oActivity = await _AppActivityLogRepository.GetAllUnRead().ConfigureAwait(false);

            if (oActivity != null && oActivity.Count > 0)
            {
                ActivityInfo.TotalActivity = oActivity.Count.ToString();
                for (int i = 0; i < oActivity.Count; i++)
                {
                    if (i == ReturnRowCount)
                        break;

                    ActivityInfo.MsgItems.Add(new UserMessageInfo
                    {
                        ID = oActivity[i].Id.ToString(),
                        Name = oActivity[i].UserName,
                        Message = oActivity[i].Description,
                        TimeRef = oActivity[i].ActivityTime.GetRelativeTimeByUTC(),
                        UserAvatar = GetUserAvatarPath(string.Format("{0}.{1}", oActivity[i].UserId, "jpg"))
                    });
                }

            }
            return ActivityInfo;
        }

        public async Task<bool> MarkAllUnreadActivityAsRead()
        {
            return await _AppActivityLogRepository.MarkAllActivityAsRead().ConfigureAwait(false);
        }

        public async Task AddActivity(ActivityType ActvType, string UserID, string UserName, string Origin, string Description)
        {
            ActivitylogBM entity = new ActivitylogBM
            {
                Id = 0,
                ActivityType = (sbyte)ActvType,
                ActivityTime = DateTime.UtcNow,
                Description = Description,
                UserId = UserID,
                UserName = UserName,
                Origin = Origin,
                IsRead = false
            };
            await _AppActivityLogRepository.InsertActivity(entity);
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
