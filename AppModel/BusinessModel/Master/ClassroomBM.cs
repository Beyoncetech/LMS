using AppModel.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel.BusinessModel.Master
{
   public class ClassroomBM
    { 
        public int Id { get; set; }
        public string RefId { get; set; }
        [GridColumn(ColumnOrder = 1, HeaderText = "Name")]
        public string Name { get; set; }
        [GridColumn(ColumnOrder = 2, HeaderText = "Description")]
        public string Description { get; set; }
        [GridColumn(ColumnOrder = 3, HeaderText = "Action", Type = GridColumnType.Action, ColumnFormat = "Icon=Edit:fa fa-fw fa-address-card;Icon=Delete:fa fa-fw fa-user-times")]
        public string Action { get; set; }
        public int SubjectId { get; set; }
        public int StandardId { get; set; }
        public string Scheduler { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

}
