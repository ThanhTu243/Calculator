using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModel
{
    public class GroupMajor
    {
        public int IdGroupMajor { get; set; }
        public string NameGroupMajor { get; set; }
        public DateTime CreatedDay { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDay { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedDay { get; set; }
        public string DeletedBy { get; set; }
        public int IsDeleted { get; set; }
    }
}
