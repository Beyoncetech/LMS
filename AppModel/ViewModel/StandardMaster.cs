using AppModel.BusinessModel.Master;
using System;
using System.ComponentModel.DataAnnotations;

namespace AppModel.ViewModel
{
    public class StandardVM : BaseViewModel
    {
        public AppGridModel<StandardMasterBM> StandardMasterInfo { get; set; }
    }

    public class StandardMasterVM : BaseViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Standard Name cannot be blank")]
        public string Name { get; set; }   
    }
}
