using System;
using System.Collections.Generic;
using System.Text;

namespace AppModel.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class GridColumnAttribute : Attribute
    {        
        public Boolean IsInlineEdit { get; set; }
        public Boolean IsVisible { get; set; }
        public string HeaderText { get; set; }
        public GridColumnType Type { get; set; }
        public int ColumnOrder { get; set; }
        public int ColumnWidth { get; set; }
        public string ColumnFormat { get; set; }

        public GridColumnAttribute()
        {            
            IsVisible = true;
            IsInlineEdit = false;
            HeaderText = "";
            Type = GridColumnType.Default;
            ColumnOrder = 0;
            ColumnWidth = 0;
            ColumnFormat = "";
        }
    }
}
