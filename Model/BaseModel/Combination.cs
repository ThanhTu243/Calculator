using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModel
{
    public class Combination
    {
        public int IdCombination { get; set; }
        public string CodeCombination { get; set; }
        public string NameCombination { get; set; }
        public DateTime CreatedDay { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDay { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedDay { get; set; }
        public string DeletedBy { get; set; }
        public int IsDeleted { get; set; }

    }
}
