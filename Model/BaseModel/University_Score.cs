using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModel
{
    public class University_Score
    {
        public int IdSocre { get; set; }
        public string CodeMajor { get; set; }
        public string NameMajor { get; set; }
        public string CodeCombination { get; set; }
        public double Score { get; set; }
        public string Description { get; set; }
        public string CodeUniversity { get; set; }
        public string Year { get; set; }
        public DateTime CreatedDay { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDay { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedDay { get; set; }
        public string DeletedBy { get; set; }
        public int IsDeleted { get; set; }
    }
}
