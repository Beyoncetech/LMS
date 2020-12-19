using AppBAL.CoreJobService;
using AppBAL.Sevices.AppCore;
using AppDAL.DBRepository;
using AppModel;
using AppUtility.AppModels;
using AppUtility.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppBAL.SchedularJobs
{
    public class AutoMailJob : AppJob
    {
        private readonly IAppJobRepository _DBRepository;
        private readonly IEmailSender _EmailSender;

        public AutoMailJob(IAppJobRepository DBRepository, IEmailSender objEmailSender)
        {
            _DBRepository = DBRepository;
            _EmailSender = objEmailSender;
        }

        /// <summary>
        /// Get the Job Name, which reflects the class name.
        /// </summary>
        /// <returns>The class Name.</returns>
        public override string GetName()
        {
            return "Auto Mail Job";
        }

        public override void OnBeforeJobExecute()
        {
            _DBRepository.InitDBContext();
            _EmailSender.InitDBContext();
            _EmailSender.SetEmailConfigFromDB();
        }

        /// <summary>
        /// Execute the Job itself. Just print a message.
        /// </summary>
        public override async Task DoJob()
        {
            ////**write code for auto mail            
            var oMailJobs = await _DBRepository.GetAllMailJob().ConfigureAwait(false);
            if (oMailJobs != null && oMailJobs.Count > 0)
            {
                foreach (var item in oMailJobs)
                {
                    try
                    {
                        //check the job expiration validation
                        DateTime CurTimeStamp = DateTime.Now;
                        if(CurTimeStamp >= item.ValidFrom && CurTimeStamp <= item.ValidTo)
                        {
                            var MailInfo = item.CommandData.XMLStringToObject<ScheduleEmailInfoBM>();

                            var message = new EmailMessage(MailInfo.To, MailInfo.Subject, MailInfo.MailBody, true, null);
                            await _EmailSender.SendEmailAsync(message);

                            item.Status = 1;
                            item.FinishedOn = CurTimeStamp;                            

                            await _DBRepository.Update(item).ConfigureAwait(false);
                        }
                        else
                        {
                            item.Status = -1;
                            item.FinishedOn = CurTimeStamp;
                            item.ErrorCode = "E05"; // for job time expiration
                            item.ErrorMsg = "Job is expired";

                            await _DBRepository.Update(item).ConfigureAwait(false);
                        }
                    }
                    catch (Exception)
                    {
                        //throw;
                    }

                }
            }           
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
