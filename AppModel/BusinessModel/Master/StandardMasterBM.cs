using AppModel.CustomAttributes;
using System;

namespace AppModel.BusinessModel.Master
{
    public class StandardMasterBM
    {
        public long Id { get; set; }
        
        [GridColumn(ColumnOrder = 1, HeaderText = "Name")]
        public string Name { get; set; }

        [GridColumn(ColumnOrder = 2, HeaderText = "Action", Type = GridColumnType.Action, ColumnFormat = "Icon=Edit:fa fa-fw fa-address-card;Icon=Delete:fa fa-fw fa-user-times")]
        public string Action { get; set; }        
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }        
    }
}
