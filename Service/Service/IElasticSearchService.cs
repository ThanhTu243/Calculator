using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Service.Service
{
    public interface IElasticSearchService
    {
        void UpErrorToElaticSearch(Exception ex, object obj, [CallerMemberName] string callerName = "");
    }
}
