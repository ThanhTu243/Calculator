using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;

namespace Ultilities
{
    public class CheckDatetimeValid
    {
        public DateTime CheckDatetime(string sDate)
        {
            try
            {
                DateTime result;
                result = sDate.Contains("-") ? 
                    DateTime.ParseExact(sDate, "dd-MM-yyyy", CultureInfo.InvariantCulture) 
                    : DateTime.ParseExact(sDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
