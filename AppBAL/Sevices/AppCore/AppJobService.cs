using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
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
    }
    public class AppJobService : IAppJobService
    {
        private readonly IAppJobRepository _DBRepository;
        private readonly IMapper _mapper;
        public AppJobService(IAppJobRepository DBRepository, IMapper mapper)
        {
            _DBRepository = DBRepository;
            _mapper = mapper;
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
    }
}
