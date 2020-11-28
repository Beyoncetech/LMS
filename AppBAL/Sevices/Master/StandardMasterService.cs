using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AppModel;
using AutoMapper;
using AppDAL.DBRepository;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using AppDAL.DBModels;

namespace AppBAL.Sevices
{
    public interface IStandardMasterService
    {
        Task<List<StandardMasterBM>> GetAllStandards(int RowCount, String AppRootPath);
        Task<CommonResponce> GetStandardByStandardId(int StandardID);
        Task<CommonResponce> GetStandardByStandardName(string StandardName);
        Task<CommonResponce> InsertStandard(StandardMasterVM StandanrdToInsert);
        Task<CommonResponce> UpdateStandard(StandardMasterVM StandardUpdate);
        Task<CommonResponce> DeleteStandard(int Id);
    }

    public class StandardMasterService : IStandardMasterService
    {
        private readonly IStandardMasterRepository _DBStandardMasterRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Tblmstandard> _commonRepository;

        public StandardMasterService(IStandardMasterRepository DBStandardMasterRepository, IMapper mapper, ICommonRepository<Tblmstandard> CommonRepository)
        {
            _DBStandardMasterRepository = DBStandardMasterRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
        }

        public async Task<List<StandardMasterBM>> GetAllStandards(int RowCount, String AppRootPath)
        {
            List<StandardMasterBM> result = new List<StandardMasterBM>();
            var oStandards = await _DBStandardMasterRepository.GetAllStandards(RowCount).ConfigureAwait(false);
            if(oStandards!=null && oStandards.Count>0)
            {
                foreach(var item in oStandards)
                {
                    result.Add(new StandardMasterBM 
                    { 
                        Id=item.Id,
                        Name=item.Name,
                        Action=item.Id.ToString(),
                        CreatedOn=item.CreatedOn,
                        CreatedBy=item.CreatedBy
                    });
                }
            }
            return result;
        }

        public async Task<CommonResponce> GetStandardByStandardId(int StandardID)
        {
            bool isValid = true;
            StandardMaster StdMaster = null;
            var oStandard = await _DBStandardMasterRepository.GetStandardByStandardID(StandardID);
            if (oStandard != null)
            {
                StdMaster = _mapper.Map<StandardMaster>(oStandard);
            }
            else
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Standard Id"), StatusObj = StdMaster };
            return result;
        }

        public async Task<CommonResponce> GetStandardByStandardName(string StandardName)
        {
            bool isValid = true;
            StandardMaster StdMaster = null;
            var oStandard = await _DBStandardMasterRepository.GetStandardByStandardName(StandardName);
            if (oStandard != null)
                StdMaster = _mapper.Map<StandardMaster>(StdMaster);
            else
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Standard Name"), StatusObj = StdMaster };
            return result;
        }

        public async Task<CommonResponce> InsertStandard(StandardMasterVM StandanrdToInsert)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            bool isValid = false;
            try
            {
                Tblmstandard oStandard = new Tblmstandard
                {
                    Name = StandanrdToInsert.Name
                };
                //isValid = await _commonRepository.Insert(_mapper.Map<Tblmstudent>(StudentToInsert));
                isValid = await _commonRepository.Insert(oStandard);
                result.Stat = isValid;
                result.StatusMsg = "Standard added successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to add new Standanrd"; }
            return result;
        }

        public async Task<CommonResponce> UpdateStandard(StandardMasterVM oStandardToUpdate)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            bool isValid = false;
            try
            {
                var oStandard = await _DBStandardMasterRepository.GetStandardByStandardID(oStandardToUpdate.Id).ConfigureAwait(false);
                if (oStandard != null)
                {
                    oStandard.Name = oStandardToUpdate.Name;
                    _commonRepository.Update(oStandard);
                    result.Stat = true;
                    result.StatusMsg = "Standard information updated successfully";
                }
                else      // student not found      
                    result.StatusMsg = "Not a valid Standard";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to update Standard information"; }
            return result;
        }

        public async Task<CommonResponce> DeleteStandard(int Id)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "Error in deleting Standard" };
            try
            {
                var oStandard = await _DBStandardMasterRepository.GetStandardByStandardID(Id).ConfigureAwait(false);
                if (oStandard != null)
                {
                    _commonRepository.Delete(oStandard);
                    result.Stat = true;
                    result.StatusMsg = "Standard deleted successfully";
                }
            }
            catch { }
            return result;
        }
    }
}
