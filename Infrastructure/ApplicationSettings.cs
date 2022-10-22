using System;
using Microsoft.Extensions.Configuration;

namespace Infrastructure
{
    public class ApplicationSettings : IApplicationSettings
    {
        private readonly IConfiguration _configuration;
        public ApplicationSettings(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString
        {
            get
            {
                return _configuration.GetConnectionString("DefaultConnection");
            }
        }
        public AppSettings AppSettings
        {
            get
            {
                return new AppSettings
                {
                    VietlottApi = "https://api.vietlott.vn/services/?securitycode=vietlotcmc&jsondata={Command:\"{{command}}\",JsonData:\"{\\\"drawId\\\": \\\"{{drawId}}\\\", \\\"pageSize\\\": {{pageSize}}, \\\"segment\\\": {{segment}}, \\\"topN\\\": {{topN}}}\"}"
                };
            }
        }
    }
}

