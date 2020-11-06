using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
using AppUtility.AppMessage;
using AppUtility.Extension;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppBAL.Sevices.AppCore
{
    public interface IAppJobService
    {
        Task AddJobOnQueue(AppJobBM oJob);
        Task AddNewUserCreateEmailJob(long UserID, string UserName, string UserEmail, string AppRoot);
    }
    public class AppJobService : IAppJobService
    {
        private readonly IAppJobRepository _DBRepository;
        private readonly IMapper _mapper;
        private readonly IMailTemplateService _MailTemplate;
        public AppJobService(IAppJobRepository DBRepository, IMapper mapper, IMailTemplateService MailTemplate)
        {
            _DBRepository = DBRepository;
            _mapper = mapper;
            _MailTemplate = MailTemplate;
        }
        public async Task AddJobOnQueue(AppJobBM oJob)
        {
            Mjob DBJob = new Mjob
            {
                JobId = oJob.JobId,
                Command = oJob.Command,
                CommandData = oJob.CommandData,
                RefNo = oJob.RefNo,
                Priority = oJob.Priority,
                CreatedOn = oJob.CreatedOn,
                CreatedBy = oJob.CreatedBy,
                Status = oJob.Status,
                ValidFrom = oJob.ValidFrom,
                ValidTo = oJob.ValidTo
            };

            await _DBRepository.Insert(DBJob).ConfigureAwait(false);            
        }

        public async Task AddNewUserCreateEmailJob(long UserID, string UserName, string UserEmail, string AppRoot)
        {
            string ResetContext = Guid.NewGuid().ToString().Replace("-", ""); //store the reset context ID
            ScheduleEmailInfoBM result = new ScheduleEmailInfoBM();
            result.To = new string[] { UserEmail };
            result.Subject = string.Format("User: [{0}] has been created.", UserName);
            string TempContext = string.Format("{0}/Account/ResetUserPass/{1}", AppRoot, ResetContext);
            string CompLogoLink = string.Format("{0}/img/Comp_logo.png", AppRoot);
            result.MailBody = _MailTemplate.NewLoginUserTemplate(UserName, CompLogoLink, TempContext, "Beyoncetech Team", "1 day");
           
            Mjob DBJob = new Mjob
            {                
                Command = "ScheduleMail",
                CommandData = result.ToXMLString(),
                RefNo = Guid.NewGuid().ToString(),
                Priority = "1",
                Status = 0,
                CreatedOn = DateTime.Now,
                CreatedBy = UserID,                
                ValidFrom = DateTime.Now,
                ValidTo = DateTime.Now.AddDays(1)
            };

            await _DBRepository.Insert(DBJob).ConfigureAwait(false);
        }
    }
}
