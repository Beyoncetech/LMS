using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppBAL.Sevices
{
    public interface ISubjectService
    {
        Task<List<SubjectBM>> GetAllSubjects(int RowCount);
        Task<SubjectBM> GetSubjectBySubjectId(int SubjectID);
        Task<CommonResponce> GetSubjectBySubjectName(string SubjectName);
        Task<CommonResponce> InsertSubject(SubjectMasterVM SubjectToInsert);
        Task<CommonResponce> UpdateSubject(SubjectMasterVM SubjectToUpdate);
        Task<CommonResponce> DeleteSubject(int SubjectId);
    }

    public class SubjectService:ISubjectService
    {
        private readonly ISubjectRepository _DBSubjectRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Tblmsubject> _commonRepository;
        
        public SubjectService(ISubjectRepository DBSubjectRepository,IMapper mapper,ICommonRepository<Tblmsubject> CommonRepository)
        {
            _DBSubjectRepository = DBSubjectRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
        }

        public async Task<List<SubjectBM>> GetAllSubjects(int RowCount)
        {
            List<SubjectBM> result = new List<SubjectBM>();
            var oSubject = await _DBSubjectRepository.GetAllSubjects(RowCount).ConfigureAwait(false);

            if (oSubject != null && oSubject.Count > 0)
            {
                foreach (var item in oSubject)
                {
                    result.Add(new SubjectBM
                    {
                        Id = item.Id,                       
                        Name = item.Name,
                        Action = item.Id.ToString(),
                        CreatedOn = item.CreatedOn,
                        CreatedBy = item.CreatedBy
                    });
                };
            }
            return result;
        }
        public async Task<SubjectBM> GetSubjectBySubjectId(int SubjectID)
        {
            bool isValid = true;
            SubjectBM SubjectInfo = null;
            var oSubject = await _DBSubjectRepository.GetSubjectBySubjectId(SubjectID);
            if (oSubject != null)
                SubjectInfo = _mapper.Map<SubjectBM>(oSubject);           
            return SubjectInfo;
        }

        public async Task<CommonResponce> GetSubjectBySubjectName(string SubjectName)
        {
            bool isValid = true;
            var oSubject = await _DBSubjectRepository.GetSubjectBySubjectName(SubjectName);
            if (oSubject == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Subject Name"), StatusObj = oSubject };
            return result;
        }

        public async Task<CommonResponce> InsertSubject(SubjectMasterVM SubjectToInsert)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            bool isValid = false;
            try
            {
                Tblmsubject oSubject = new Tblmsubject
                {
                    Name = SubjectToInsert.Name,
                };
                //isValid = await _commonRepository.Insert(_mapper.Map<Tblmstudent>(StudentToInsert));
                isValid = await _commonRepository.Insert(oSubject);
                result.Stat = isValid;
                result.StatusMsg = "Subject added successfully";
            }
            catch(Exception ex) { result.Stat = isValid; result.StatusMsg = "Failed to add new Subject"; }
            return result;
        }

        public async Task<CommonResponce> UpdateSubject(SubjectMasterVM oSubjectToUpdate)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            bool isValid = false;
            try
            {
                var oSubject = await _DBSubjectRepository.GetSubjectBySubjectId(oSubjectToUpdate.Id).ConfigureAwait(false);
                if (oSubject != null)
                {
                    oSubject.Name = oSubjectToUpdate.Name;
                    _commonRepository.Update(oSubject);
                    result.Stat = true;
                    result.StatusMsg = "Subject information updated successfully";
                }
                else      // subject not found      
                    result.StatusMsg = "Not a valid Subject";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to update subject information"; }
            return result;
        }

        public async Task<CommonResponce> DeleteSubject(int SubjectId)
        {
            CommonResponce result = new CommonResponce() { Stat = false, StatusMsg = "Error in deleting Subject" };
            try
            {
                var oSubject = await _DBSubjectRepository.GetSubjectBySubjectId(SubjectId).ConfigureAwait(false); // get subject details from db
                if (oSubject != null)  // subject found
                {
                    _commonRepository.Delete(oSubject);
                    result.Stat = true;
                    result.StatusMsg = "Subject deleted successfully";
                }
                else
                    result.StatusMsg = "Not a valid Subject";
            }
            catch { result.StatusMsg = "Failed to delete Subject"; }
            return result;
        }
    }
}
