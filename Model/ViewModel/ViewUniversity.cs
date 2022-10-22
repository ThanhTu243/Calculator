using System;
using System.Collections.Generic;
using System.Text;
using Model.BaseModel;
using Newtonsoft.Json;

namespace Model.ViewModel
{
    public class ViewUniversity
    {
        [JsonProperty(PropertyName = "IdUniversity")]
        public int IdUniversity { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int IdType { get; set; }
        public string NameType { get; set; }
        public  int IdProvince { get; set; }
        public string NameProvince { get; set; }
    }
}
