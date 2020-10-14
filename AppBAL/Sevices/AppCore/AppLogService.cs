using AppDAL.DBRepository;
using AppModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppBAL.Sevices.AppCore
{
    public interface IAppLogService
    {
        Task<List<ActivitylogBM>> GetAllActivityLog();
    }
    public class AppLogService : IAppLogService
    {
        private readonly IAppActivityLogRepository _DBLogRepository;
        private readonly IMapper _mapper;
        public AppLogService(IAppActivityLogRepository DBLogRepository, IMapper mapper)
        {
            _DBLogRepository = DBLogRepository;
            _mapper = mapper;
        }
        public async Task<List<ActivitylogBM>> GetAllActivityLog()
        {
            List<ActivitylogBM> result = new List<ActivitylogBM>();
            var oActivityLogs = await _DBLogRepository.GetAll(500).ConfigureAwait(false);

            if (oActivityLogs != null && oActivityLogs.Count > 0)
            {
                foreach (var item in oActivityLogs)
                {
                    result.Add(_mapper.Map<ActivitylogBM>(item));
                }                
            }            
            return result;
        }
    }
}
