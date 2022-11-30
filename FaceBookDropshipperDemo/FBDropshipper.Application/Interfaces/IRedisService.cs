using System;
using System.Threading.Tasks;

namespace FBDropshipper.Application.Interfaces
{
    public interface IRedisService
    {
        T GetObject<T>(string key) where T : class;
        Task<string> GetString(string key);    
        Task<DateTime> GetDateTime(string key);    
        Task<int> GetInt(string key);
        Task<double> GetDouble(string key);    
        Task<bool> SaveObject<T>(string key, T value) where T : class;
        Task<bool> SaveString(string key, string value);    
        Task<bool> SaveDateTime(string key, DateTime value);    
        Task<bool> SaveInt(string key, int value);
        Task<bool> SaveDouble(string key, double value);    
    }
}