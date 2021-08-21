
namespace JOB_MANAGER.DATAACESS.CrossCuttingConsers
{
    public interface ICacheManager
    {        
        T Get<T>(string key);
        void Add(string key, object data);
        bool isExists(string key);
        void Remove(string key);
        void Clear();
    }
}
