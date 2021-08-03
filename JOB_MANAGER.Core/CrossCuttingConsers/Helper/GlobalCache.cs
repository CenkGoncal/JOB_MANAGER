using JOB_MANAGER.Core.CrossCuttingConsers;
using JOB_MANAGER.Core.Helper;
using JOB_MANAGER.CrossCuttingConsers;
using JOB_MANAGER.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.Helper
{
    public class GlobalCache  
    {
        private MemoryCacheManager _memoryCache = MemoryCacheManager.instance;

        public List<ParameterExtented> GetCacheParameter()
        {
            return _memoryCache.Get<List<ParameterExtented>>(CacheEnum.parameter.ToString());
        }

        public void CreateCacheParameter(bool isRemoveCache)
        {
            ParameterManager parameter = new ParameterManager(new ParameterDal());
            parameter.GetAll();

            if (isRemoveCache)
                _memoryCache.Remove(CacheEnum.parameter.ToString());

            _memoryCache.Add(CacheEnum.parameter.ToString(), parameter.GetAll());
        }

        public void CreateCache()
        {
            #region paramater
            CreateCacheParameter(false);
            #endregion
        }

        public void CacheRemove()
        {
            foreach(var item in Enum.GetValues(typeof(CacheEnum)))
            {
                _memoryCache.Remove(item.ToString());
            }          
        }
    }

}
