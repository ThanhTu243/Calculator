using System;
using System.Collections.Generic;
using System.Text;

namespace Model.ViewModel
{
    public class ViewMajor
    {
        public int IdMajor { get; set; }
        public string CodeMajor { get; set; }
        public string NameMajor { get; set; }
        public bool Hot { get; set; }
        public string Slug { get; set; }
        public int IdGroupMajor { get; set; }
    }
}
