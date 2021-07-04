using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JOB_MANAGER.CrossCuttingConsers
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
