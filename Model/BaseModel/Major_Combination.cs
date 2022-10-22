using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModel
{
    public class Major_Combination
    {
        public int IdMajor_Combination { get; set; }
        public string CodeMajor { get; set; }
        public string CodeCombination { get; set; }
        public DateTime CreatedDay { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedDay { get; set; }
        public string DeletedBy { get; set; }
        public int IsDeleted { get; set; }
    }
}
