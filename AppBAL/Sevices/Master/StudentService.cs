﻿using AppDAL.DBModels;
using AppDAL.DBRepository;
using AppModel;
using AppModel.BusinessModel.Master;
using AppModel.ViewModel;
using AppUtility.AppIO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppBAL.Sevices.Master
{
    public interface IStudentService
    {
        Task<List<StudentBM>> GetAllStudents(int RowCount, string AppRootPath);        
        Task<CommonResponce> GetStudentByStudentId(int StudentID);
        Task<CommonResponce> GetStudentByRegNo(string RegNo);
        Task<CommonResponce> GetStudentByEmailID(string EmailID);
        Task<CommonResponce> CheckDataValidation(StudentProfileVM StudentToInsert, bool IsAdd);
        Task<CommonResponce> InsertStudentProfile(StudentProfileVM StudentToInsert);
        Task<CommonResponce> UpdateStudentProfile(StudentProfileVM StudentToUpdate);
       Task< CommonResponce> DeleteStudentProfile(int StudentId);
    }
    public class StudentService:IStudentService
    {
        private readonly IStudentRepository _DBStudentRepository;
        private readonly IMapper _mapper;
        private readonly ICommonRepository<Tblmstudent> _commonRepository;
        private readonly IDirectoryFileService _AppDirectoryFileService;
        public StudentService(IStudentRepository DBStudentRepository, IMapper mapper,ICommonRepository<Tblmstudent> CommonRepository, IDirectoryFileService AppDirectoryFileService)
        {
            _DBStudentRepository = DBStudentRepository;
            _mapper = mapper;
            _commonRepository = CommonRepository;
            _AppDirectoryFileService = AppDirectoryFileService;
        }

        public async Task<List<StudentBM>> GetAllStudents(int RowCount, String AppRootPath)
        {
            List<StudentBM> result = new List<StudentBM>();
            var oStudents = await _DBStudentRepository.GetAllStudents(RowCount).ConfigureAwait(false);

            if (oStudents != null && oStudents.Count > 0)
            {
                foreach (var item in oStudents)
                {
                    result.Add(new StudentBM
                    {
                        Id = item.Id,
                        UserAvatar = _AppDirectoryFileService.GetAppUserAvatarPath(AppRootPath, item.RegNo.ToString(), "M"),
                        Name = item.Name,
                        RegNo = item.RegNo,
                        Address = item.Address,
                        ContactNo = item.ContactNo,
                        Email = item.Email,
                        StandardId = item.StandardId,
                        Action = item.Id.ToString(),
                        CreatedOn = item.CreatedOn,
                        CreatedBy = item.CreatedBy
                    });
                };
            }
            return result;
        }
        
        public async Task<CommonResponce> GetStudentByStudentId(int StudentID)
        {
            bool isValid = true;
            Student StudentProfile = null;
            var oStudent = await _DBStudentRepository.GetStudentByStudentId(StudentID);
            if (oStudent != null)
            {
                StudentProfile = _mapper.Map<Student>(oStudent);
                isValid = true;
            }
            else
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Student Id"), StatusObj = StudentProfile };
            return result;           
        }
        public async Task<CommonResponce> GetStudentByRegNo(string RegNo)
        {
            bool isValid = true;
            var oStudent = await _DBStudentRepository.GetStudentByRegNo(RegNo);
            if (oStudent == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Student Registration No"), StatusObj = oStudent };
            return result;
        }
        public async Task<CommonResponce> GetStudentByEmailID(string EmailID)
        {
            bool isValid = true;
            var oStudent = await _DBStudentRepository.GetStudentByEmailID(EmailID);
            if (oStudent == null)
                isValid = false;
            CommonResponce result = new CommonResponce { Stat = isValid, StatusMsg = (isValid ? "" : "Invalid Email ID"), StatusObj = oStudent };
            return result;
        }

        #region INSERT/ UPDATE/ DELETE 
        public async Task<CommonResponce> CheckDataValidation(StudentProfileVM StudentToInsert, bool IsAdd)
        {            
            CommonResponce result = new CommonResponce { Stat = true, StatusMsg = "" };
            Tblmstudent oStudent = null;
            if (IsAdd)  // check validation while adding a new student
            {
                oStudent = await _DBStudentRepository.GetStudentByRegNo(StudentToInsert.RegNo).ConfigureAwait(false);
                if (oStudent != null)
                { result.Stat = false; result.StatusMsg = "Registration No already in use"; }
                else
                {
                    oStudent = await _DBStudentRepository.GetStudentByEmailID(StudentToInsert.Email);
                    if (oStudent != null)
                    { result.Stat = false; result.StatusMsg = "Email Id already in use"; }
                }
            }
            else  // validation while updating
            {
                oStudent = await _DBStudentRepository.GetStudentByRegNo(StudentToInsert.RegNo).ConfigureAwait(false);
                if (oStudent != null)  // got result
                {
                    if (StudentToInsert.Id != oStudent.Id)  // different student with same reg no
                    { result.Stat = false; result.StatusMsg = "Registration No already in use"; }
                    else // same student found check duplicate email id
                    {
                        oStudent = await _DBStudentRepository.GetStudentByEmailID(StudentToInsert.Email);
                        if (oStudent != null)
                        {
                            if (StudentToInsert.Id != oStudent.Id)  // different student with same email id
                                result.Stat = false; result.StatusMsg = "Email Id already in use";
                        }
                    }
                }                
            }
            return result;
        }

        public async Task<CommonResponce> InsertStudentProfile(StudentProfileVM StudentToInsert)
        {
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            CommonResponce DataValidationResult = new CommonResponce { Stat = false, StatusMsg = "" };
            bool isValid = false;
            Tblmstudent oStudent = null;
            try
            {
                oStudent = new Tblmstudent
                {
                    RegNo = StudentToInsert.RegNo,
                    Name = StudentToInsert.Name,
                    Address = StudentToInsert.Address,
                    Email = StudentToInsert.Email,
                    ContactNo = StudentToInsert.ContactNo,
                    StandardId = StudentToInsert.StandardId,
                    LoginUserId = StudentToInsert.LoginUserId
                };
                //isValid = await _commonRepository.Insert(_mapper.Map<Tblmstudent>(StudentToInsert));
                isValid = await _commonRepository.Insert(oStudent);
                result.Stat = isValid;
                result.StatusMsg = "Student added successfully";
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to add new student"; }
            return result;
        }

        public async Task<CommonResponce> UpdateStudentProfile(StudentProfileVM oStudentToUpdate)
        {            
            CommonResponce result = new CommonResponce { Stat = false, StatusMsg = "" };
            bool isValid = false;
            try
            {
                var oStudent = await _DBStudentRepository.GetStudentByStudentId(oStudentToUpdate.Id).ConfigureAwait(false);
                if (oStudent != null)
                {
                    oStudent.RegNo = oStudentToUpdate.RegNo;
                    oStudent.Name = oStudentToUpdate.Name;                    
                    oStudent.Address = oStudentToUpdate.Address;
                    oStudent.Email = oStudentToUpdate.Email;
                    oStudent.ContactNo = oStudentToUpdate.ContactNo;
                    oStudent.StandardId = oStudentToUpdate.StandardId;
                    _commonRepository.Update(oStudent);
                    result.Stat = true;
                    result.StatusMsg = "Student information updated successfully";
                }
                else      // student not found      
                    result.StatusMsg = "Not a valid Student";                
            }
            catch { result.Stat = isValid; result.StatusMsg = "Failed to update student information"; }
            return result;
        }

        public async Task<CommonResponce> DeleteStudentProfile(int StudentId)
        {
            CommonResponce result = new CommonResponce() { Stat = false, StatusMsg = "Error in deleting Student" };
            try
            {
                var oStudent = await _DBStudentRepository.GetStudentByStudentId(StudentId).ConfigureAwait(false); // get subject details from db
                if (oStudent != null)  // Student found
                {
                    _commonRepository.Delete(oStudent);
                    result.Stat = true;
                    result.StatusMsg = "Student deleted successfully";
                }
                else
                    result.StatusMsg = "Not a valid Student";
            }
            catch { result.StatusMsg = "Failed to delete Student"; }
            return result;
        }
        #endregion INSERT/ UPDATE/ DELETE 
    }
}
