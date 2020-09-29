using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppModel;
using AutoMapper;
using AppDAL.DBRepository;

namespace AppBAL.Sevices
{
    public interface IStandardMasterService
    {
        Task<CommonResponce> GetAllStandards();
        Task<CommonResponce> GetStandardByStandardId(int StandardID);
        Task<CommonResponce> GetStandardByStandardName(string StandardName);
    }

    public class StandardMasterService : IStandardMasterService
    {
        private readonly IStandardMasterRepository _DBStandardMasterRepository;
        private readonly IMapper _mapper;

        public StandardMasterService(IStandardMasterRepository DBStandardMasterRepository, IMapper mapper)
        {
            _DBStandardMasterRepository = DBStandardMasterRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponce> GetAllStandards()
        {
            var AllStandards = await _DBStandardMasterRepository.GetAllStandards();
            CommonResponce result = new CommonResponce { Stat = true, StatusMsg = "", StatusObj = AllStandards };
            return result;
        }

        public async Task<CommonResponce> GetStandardByStandardId(int StandardID)
        {
            bool isValid = true;
            var oStandard = await _DBStandardMasterRepository.GetStandardByStandardID(StandardID);
            if (oStandard == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Standard Id"), StatusObj = oStandard };
            return result;
        }
        
        public async Task<CommonResponce> GetStandardByStandardName(string StandardName) 
        {
            bool isValid = true;
            var oStandard = await _DBStandardMasterRepository.GetStandardByStandardName(StandardName);
            if (oStandard == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Standard Name"), StatusObj = oStandard };
            return result;
        }
    }
}
