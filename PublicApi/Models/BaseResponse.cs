using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PublicApi.Models
{
    public class BaseResponse<T> where T : class
    {
        public T data { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public string errorCode { get; set; }
        public BaseResponse(T data)
        {
            this.data = data;
            success = true;
            message = "Success";
            errorCode = "1";
        }
        public BaseResponse(string error, string message)
        {
            data = null;
            success = false;
            this.message = message;
            errorCode = error;
        }
    }
}
