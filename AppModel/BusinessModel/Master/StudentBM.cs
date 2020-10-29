using AppModel.CustomAttributes;
using System;

namespace AppModel.BusinessModel.Master
{
    public class StudentBM
    {
        public long Id { get; set; }
        [GridColumn(ColumnOrder = 1, HeaderText = "Avatar", Type = GridColumnType.Image)]
        public string UserAvatar { get; set; }
        [GridColumn(ColumnOrder = 1, HeaderText = "Name")]
        public string Name { get; set; }
        [GridColumn(ColumnOrder = 2, HeaderText = "Reg. No")]
        public Int32 RegNo { get; set; }
        [GridColumn(ColumnOrder = 3, HeaderText = "Address")]
        public string Address { get; set; }
        [GridColumn(ColumnOrder = 4, HeaderText = "Contact No")]
        public string ContactNo { get; set; }
        [GridColumn(ColumnOrder = 5, HeaderText = "Email")]
        public string Email { get; set; }

        [GridColumn(ColumnOrder = 7, HeaderText = "Action", Type = GridColumnType.Action, ColumnFormat = "Icon=Edit:fa fa-fw fa-address-card;Icon=Delete:fa fa-fw fa-user-times")]
        public string Action { get; set; }
        public int StandardId { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
    }
}
