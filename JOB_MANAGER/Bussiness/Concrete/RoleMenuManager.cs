﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.ComplexType;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;
using Newtonsoft.Json;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class RoleMenuManager : IService<ROLE_MENU, RoleMenuExtented>
    {
        private RoleMenuDal _dal;

        public RoleMenuManager(RoleMenuDal roleMenuDal)
        {
            _dal = roleMenuDal;
        }

        public ShowState AddorUpdate(ROLE_MENU param)
        {
            return _dal.AddorUpdate(param, f => f.ROLE_ID == param.ROLE_ID);
        }

        public ShowState Delete(ROLE_MENU param)
        {
            return _dal.Delete(param);
        }

        public ROLE_MENU Get(ROLE_MENU param)
        {
            return _dal.GetAll(f => f.ROLE_ID == param.ROLE_ID).FirstOrDefault();
        }

        public List<RoleMenuExtented> GetAll()
        {
            return _dal.GetAll2();
        }

        public List<MenuRolesDto> GetMenuByRole(int roleId)
        {
            var roleMenu = _dal.GetAll2(f => f.ROLE_ID == roleId).FirstOrDefault();
            if (string.IsNullOrEmpty(roleMenu.MENUS_STR))
                return null;

            return JsonConvert.DeserializeObject<List<MenuRolesDto>>(roleMenu.MENUS_STR);
        }

        public List<RoleMenuExtented> GetAllByEmployee()
        {
            return _dal.GetAllByLoginEmployee();
        }
    }
}
