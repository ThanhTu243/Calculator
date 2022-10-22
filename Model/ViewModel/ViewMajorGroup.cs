using System;
using System.Collections.Generic;
using System.Text;
using Model.BaseModel;

namespace Model.ViewModel
{
    public class ViewMajorGroup
    {
        public int IdGroupMajor { get; set; }
        public string NameGroupMajor { get; set; }
        public IEnumerable<MajorItem> Major { get; set; }
    }
    public class MajorItem
    {
        public string CodeMajor { get; set; }
        public string NameMajor { get; set; }
    }
}
