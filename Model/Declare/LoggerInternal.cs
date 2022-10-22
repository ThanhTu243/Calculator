using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Declare
{
    public class LoggerInternal
    {
        public string TypeLog { get; set; }
        public string Package { get; set; }
        public string ClassName { get; set; }
        public string Method { get; set; }
        public string CallerMethod { get; set; }
        public string Parameters { get; set; }
        public string Message { get; set; }
        public DateTime DateTime { get; set; }
    }
}
