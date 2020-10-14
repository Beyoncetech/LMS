using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel.ViewModel
{
    public class AppBreadCrumb
    {
        public string ActionUrl { get; set; }
        public string Name { get; set; }
    }
    public class UserMessageInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Message { get; set; }
        public string TimeRef { get; set; }
        public string UserAvatar { get; set; }
    }

    public class UserActivityInfo
    {
        public string TotalActivity { get; set; }
        public List<UserMessageInfo> MsgItems { get; set; }
    }

    public class UserNotificationItem
    {
        public string NotifyType { get; set; }
        public string Message { get; set; }
        public string TimeRef { get; set; }
    }
    public class UserNotificationInfo
    {
        public string TotalNotification { get; set; }
        public List<UserNotificationItem> MsgItems { get; set; }
    }
    public class BaseViewModel
    {
        public string ProjectName { get; set; }
        public string PageTitle { get; set; }
        public string BUserID { get; set; }
        public string BUserName { get; set; }
        public string BUserType { get; set; }
        public string BUserGender { get; set; }
        public string BUserImgPath { get; set; }
        public List<AppBreadCrumb> BreadCrumbItems { get; set; }
        public UserActivityInfo UserActivityMsg { get; set; }
        public UserNotificationInfo UserNotification { get; set; }

        public void CopyToBase(BaseViewModel BaseObj)
        {
            ProjectName = BaseObj.ProjectName;
            PageTitle = BaseObj.PageTitle;
            BUserID = BaseObj.BUserID;
            BUserName = BaseObj.BUserName;
            BUserType = BaseObj.BUserType;
            BUserGender = BaseObj.BUserGender;
            BUserImgPath = BaseObj.BUserImgPath;
            BreadCrumbItems = BaseObj.BreadCrumbItems;
            UserActivityMsg = BaseObj.UserActivityMsg;
            UserNotification = BaseObj.UserNotification;
        }
    }
    public class AppGridModel<T> where T : class
    {
        public string TableID { get; set; }
        public List<T> Rows { get; set; }
        public string PageSizeOption { get; set; }
        public bool ShowHeaderOnFooter { get; set; }
        public Type GetMyType
        {
            get
            {
                return typeof(T);
            }
        }

        public AppGridModel()
        {
            TableID = "appHtmlGrid1";
            Rows = new List<T>();
            PageSizeOption = "10;25;50;100";
            ShowHeaderOnFooter = false;
        }
    }
    public class AppGridModelInfo
    {
        public Boolean IsInlineEdit { get; set; }
        public Boolean IsVisible { get; set; }
        public string HeaderText { get; set; }
        public string PropertyName { get; set; }
        public GridColumnType Type { get; set; }
        public int ColumnOrder { get; set; }
        public int ColumnWidth { get; set; }
        public string ColumnFormat { get; set; }
    }
}
