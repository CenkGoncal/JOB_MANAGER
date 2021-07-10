﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class TitleManager : IService<TITLES, TitleExtented>
    {
        private TitleDal _dal;
        
        public TitleManager(TitleDal titleDal)
        {
            _dal = titleDal;
        }

        public ShowState AddorUpdate(TITLES param)
        {
            return _dal.AddorUpdate(param, f => f.TITLE_ID == param.TITLE_ID);
        }

        public ShowState Delete(TITLES param)
        {
            return _dal.Delete(param);
        }

        public TITLES Get(TITLES param)
        {
            return _dal.GetAll(f => f.TITLE_ID == param.TITLE_ID).FirstOrDefault();
        }

        public List<TitleExtented> GetAll()
        {
            return _dal.GetAll2();
        }
    }
}