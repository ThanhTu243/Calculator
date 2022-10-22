using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using   Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Ultilities
{
    public interface ICacheHelper
    {
        Task<T> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T data, TimeSpan timeSpan);
        Task DeleteAsync(string key);
    }
    public class CacheHelper : ICacheHelper
    {
        private readonly IDistributedCache _distributedCache;
        private readonly bool _redisActive = true;


        public CacheHelper(IConfiguration configuration, IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<T> GetAsync<T>(string key)
        {
            try
            {
                if (_redisActive)
                {
                    var result = await _distributedCache.GetStringAsync(key);
                    if (!string.IsNullOrEmpty(result))
                    {
                        return JsonConvert.DeserializeObject<T>(result);
                    }
                    return default;
                }

                return default;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task SetAsync<T>(string key, T data, TimeSpan timeSpan)
        {
            try
            {
                if (_redisActive)
                {
                    string stringData = JsonConvert.SerializeObject(data);
                    await _distributedCache.SetStringAsync(key, stringData,
                        new DistributedCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTimeOffset.Now.Add(timeSpan)
                        });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task DeleteAsync(string key)
        {
            try
            {
                if (_redisActive)
                {
                    await _distributedCache.RemoveAsync(key);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
