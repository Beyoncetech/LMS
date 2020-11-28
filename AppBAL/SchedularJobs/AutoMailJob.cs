using AppDAL.DBRepository;
using AppUtility.AppSchedular;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppBAL.SchedularJobs
{
    public class AutoMailJob : AppJob
    {
        private readonly IAppJobRepository _DBRepository;

        public AutoMailJob(IAppJobRepository DBRepository)
        {
            _DBRepository = DBRepository;
        }

        /// <summary>
        /// Get the Job Name, which reflects the class name.
        /// </summary>
        /// <returns>The class Name.</returns>
        public override string GetName()
        {
            return "Auto Mail Job";
        }

        /// <summary>
        /// Execute the Job itself. Just print a message.
        /// </summary>
        public override void DoJob()
        {
            ////**write code for auto mail
            var oMailJobs = _DBRepository.GetAllMailJob().Result;
            if (oMailJobs != null && oMailJobs.Count > 0)
            {
                foreach (var item in oMailJobs)
                {
                    try
                    {

                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
            //string TID = "374D9B86-824F-4910-8665-268FE617C028"; // tenant id is fixed for this application
            //List<ScheduleEmailJobInfo> MailJobs = null;
            //using (AppCommonRepository oManager = new AppCommonRepository(TID))
            //{
            //    MailJobs = oManager.GetProjectNotificationEmailJob();
            //    //create obj for email
            //    if (MailJobs.Count() > 0)
            //    {
            //        var MailManager = AppEmailGenerator.Create(TID);
            //        foreach (var item in MailJobs)
            //        {
            //            try
            //            {
            //                MailManager.IsHtmlBody(true)
            //                .To(item.MailInfo.To)
            //                .Subject(item.MailInfo.Subject)
            //                .MailBody(item.MailInfo.MailBody)
            //                .Send();

            //                oManager.UpdateAppJob(item.JobID, false, "");
            //            }
            //            catch (Exception ex)
            //            {
            //                oManager.UpdateAppJob(item.JobID, true, ex.Message);
            //                LogError(ex);
            //            }

            //        }
            //    }
            //}
        }

        /// <summary>
        /// Determines this job is repeatable.
        /// </summary>
        /// <returns>Returns true because this job is repeatable.</returns>
        public override bool IsRepeatable()
        {
            return true;
        }

        public override bool IsActive()
        {
            return true;
        }

        /// <summary>
        /// Determines that this job is to be executed again after
        /// 1 sec.
        /// </summary>
        /// <returns>1 sec, which is the interval this job is to be
        /// executed repeatadly.</returns>
        public override int GetRepetitionIntervalTime()
        {
            return 30000;
        }

        public override void WriteLog(int MsgType, string Msg)
        {
            string tempmsg = Msg;
        }

        public override string ErrorLogFileName()
        {
            //return the error file path
            return string.Empty;
        }
    }
}
