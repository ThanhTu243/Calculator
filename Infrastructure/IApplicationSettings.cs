using System;
namespace Infrastructure
{
    public interface IApplicationSettings
    {
        public string ConnectionString
        {
            get;
        }

        public AppSettings AppSettings
        {
            get;
        }
    }
}
