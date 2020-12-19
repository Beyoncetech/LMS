using AppDAL.DBRepository;
using AppModel;
using AppUtility.Extension;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppBAL.Sevices.AppCore
{
    public interface IAppSettingService
    {
        void InitDBContext();
        MailSettingBM GetMailSettingSync();
        Task<MailSettingBM> GetMailSetting();
        Task<CommonResponce> SaveMailSetting(MailSettingBM oEntity);
        Task<GeneralSettingBM> GetAppGeneralSetting();
        Task<CommonResponce> SaveGeneralSetting(GeneralSettingBM oEntity);
    }
    public class AppSettingService : IAppSettingService
    {
        private readonly IAppSettingRepository _DBSettingRepository;
        private readonly IMapper _mapper;
        public AppSettingService(IAppSettingRepository DBSettingRepository, IMapper mapper)
        {
            _DBSettingRepository = DBSettingRepository;
            _mapper = mapper;
        }
        public void InitDBContext()
        {
            _DBSettingRepository.InitDBContext();
        }
        public MailSettingBM GetMailSettingSync()
        {
            _DBSettingRepository.InitDBContext();
            MailSettingBM result = new MailSettingBM();
            var oSettings = _DBSettingRepository.GetSettingByKeySync("Mail-AppMailSetup");

            if (oSettings != null && oSettings.Count > 0)
            {
                try
                {
                    result = oSettings[0].AppVal.XMLStringToObject<MailSettingBM>();
                }
                catch (Exception)
                {
                    // code to do exception
                }
            }
            return result;
        }
        public async Task<MailSettingBM> GetMailSetting()
        {
            MailSettingBM result = new MailSettingBM();
            var oSettings = await _DBSettingRepository.GetSettingByKey("Mail-AppMailSetup").ConfigureAwait(false);

            if (oSettings != null && oSettings.Count > 0)
            {
                try
                {
                    result = oSettings[0].AppVal.XMLStringToObject<MailSettingBM>();
                }
                catch (Exception)
                {
                    // code to do exception
                }
            }
            return result;
        }

        public async Task<CommonResponce> SaveMailSetting(MailSettingBM oEntity)
        {
            CommonResponce result = new CommonResponce
            {
                Stat = false,
                StatusMsg = "Error on Saving email setting",
                StatusObj = null
            };

            int stat = await _DBSettingRepository.DeleteSettingByKey("Mail-AppMailSetup").ConfigureAwait(false);

            stat = await _DBSettingRepository.InsertSetting("Mail-AppMailSetup", oEntity.ToXMLString()).ConfigureAwait(false);

            if (stat > 0)
            {
                result.Stat = true;
                result.StatusMsg = "Successfully Save email Setings.";
            }

            return result;
        }

        public async Task<GeneralSettingBM> GetAppGeneralSetting()
        {
            GeneralSettingBM result = new GeneralSettingBM();
            var oSettings = await _DBSettingRepository.GetSettingByKey("App-GeneralSetup").ConfigureAwait(false);

            if (oSettings != null && oSettings.Count > 0)
            {
                try
                {
                    result = oSettings[0].AppVal.XMLStringToObject<GeneralSettingBM>();
                }
                catch (Exception)
                {
                    // code to do exception
                }
            }
            return result;
        }

        public async Task<CommonResponce> SaveGeneralSetting(GeneralSettingBM oEntity)
        {
            CommonResponce result = new CommonResponce
            {
                Stat = false,
                StatusMsg = "Error on Saving email setting",
                StatusObj = null
            };

            int stat = await _DBSettingRepository.DeleteSettingByKey("App-GeneralSetup").ConfigureAwait(false);

            stat = await _DBSettingRepository.InsertSetting("App-GeneralSetup", oEntity.ToXMLString()).ConfigureAwait(false);

            if (stat > 0)
            {
                result.Stat = true;
                result.StatusMsg = "Successfully Save Setings.";
            }

            return result;
        }
    }
}
