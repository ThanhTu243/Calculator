using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModel
{
    public class University
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public int Province { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDay { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedDay { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime DeletedDay { get; set; }
        public string DeletedBy { get; set; }
        public int IsDeleted { get; set; }
    }
}
