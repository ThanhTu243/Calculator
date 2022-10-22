using Model.Declare;
using Nest;
using Newtonsoft.Json;
using Service.Service;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Service.Implementation
{
    public class ElasticSearchService : IElasticSearchService
    {
        protected readonly IElasticClient _elasticClient;
        public ElasticSearchService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }
        public async void UpErrorToElaticSearch(Exception ex, object obj, [CallerMemberName] string callerName = "")
        {
            var log = new LoggerInternal()
            {
                TypeLog = "Error",
                Package = GetType().Namespace,
                ClassName = GetType().Name,
                Method = MethodBase.GetCurrentMethod().ReflectedType.Name,
                CallerMethod = callerName,
                Parameters = JsonConvert.SerializeObject(obj),
                Message = JsonConvert.SerializeObject(ex),
                DateTime = DateTime.Now
            };
            await _elasticClient.IndexDocumentAsync(log);
        }
    }
}
