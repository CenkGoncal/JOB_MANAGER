using JOB_MANAGER.DATAACESS.CrossCuttingConsers;
using JOB_MANAGER.DATAACESS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JOB_MANAGER.DataAcess.Helper
{
    public class GlobalCache  
    {
        private MemoryCacheManager _memoryCache = MemoryCacheManager.instance;

        public UserInfo AddUserInfo(string username)
        {
            if (!_memoryCache.isExists(CacheEnum.userinfo.ToString()))
            {
                UserInfo userInfo = new UserInfo();
                userInfo.CreateUserInfo(username);
                _memoryCache.Add(CacheEnum.userinfo.ToString(), userInfo);                

                return userInfo;
            }
            else
            {
                return GetUserInfo();
            }
        }

        public UserInfo GetUserInfo()
        {            
            UserInfo userInfo = null;

            if (_memoryCache.isExists(CacheEnum.userinfo.ToString()))
            {
                userInfo = _memoryCache.Get<UserInfo>(CacheEnum.userinfo.ToString());
            }

            return userInfo;
        }

        public List<ParameterExtented> GetCacheParameter()
        {
            return _memoryCache.Get<List<ParameterExtented>>(CacheEnum.parameter.ToString());
        }

        public void CreateCacheParameter(bool isRemoveCache)
        {
            ParameterDal parameter = new ParameterDal();
            parameter.GetAll2();

            if (isRemoveCache)
                _memoryCache.Remove(CacheEnum.parameter.ToString());

            _memoryCache.Add(CacheEnum.parameter.ToString(), parameter.GetAll2());
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

    public class UserInfo
    {
        private JOB_MANAGER_DBEntities db; 
        
        public int UserId;
        public string UserName;
        public int CompanyId;
        public UserInfo()
        {
            db = new JOB_MANAGER_DBEntities();
        }

        public void CreateUserInfo(string username)
        {
            var emp = db.EMPLOYEES.Where(w => w.SYSTEM_USERNAME == username).FirstOrDefault();
            if(emp != null)
            {
                UserId = emp.EMP_ID;
                UserName = emp.SYSTEM_USERNAME;
                CompanyId = emp.COMPANY_ID;
            }
        }
    }
}
