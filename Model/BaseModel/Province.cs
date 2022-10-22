using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModel
{
    public class Province
    {
        public int IdProvince { get; set; }
        public string NameProvince { get; set; }
        public DateTime CreatedDay { get; set; }
        public string CreatedBy { get; set; }
        public int IsDeleted { get; set; }
        public int OrderBy { get; set; }
    }
}
