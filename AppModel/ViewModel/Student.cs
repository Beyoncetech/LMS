﻿using AppModel.BusinessModel.Master;

namespace AppModel.ViewModel
{
    public class StudentVM : BaseViewModel
    {
        public AppGridModel<StudentBM> StudentInfo { get; set; }
    }

    public class StudentProfileVM:BaseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RegNo { get; set; }
        public string Address { get; set; }
        public string ContactNo { get; set; }
        public string Email { get; set; }
        public int StandardId { get; set; }
    }
}
