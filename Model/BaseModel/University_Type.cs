using System;
using System.Collections.Generic;
using System.Text;

namespace Model.BaseModel
{
    public class University_Type
    {
        public int IdType { get; set; }
        public string NameType { get; set; }
        public DateTime CreatedDay { get; set; }
        public string CreatedBy { get; set; }
        public int IsDeleted { get; set; }
    }
}
