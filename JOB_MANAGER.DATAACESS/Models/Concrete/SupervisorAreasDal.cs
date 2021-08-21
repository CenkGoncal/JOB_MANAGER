using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class SupervisorAreasDal : EntityRepositoryBase<SUPERVISOR_AREAS, JOB_MANAGER_DBEntities, SupervisorAreasExtented>
    {
        public override List<SupervisorAreasExtented> GetAll2(Expression<Func<SUPERVISOR_AREAS, bool>> filter = null)
        {
            var _list = filter != null ? context.Set<SUPERVISOR_AREAS>().Where(filter).ToList()
                            : context.Set<SUPERVISOR_AREAS>().ToList();


            var data = (from s in _list

                        where s.IS_CANCELED == false
                        select new SupervisorAreasExtented
                        {
                            EMPLOYEE_ID = s.EMPLOYEE_ID,
                            SUPERVISOR = s.EMPLOYEES.FIRST_NAME + SqlConstants.stringWhiteSpace + s.EMPLOYEES.LAST_NAME,
                            CITY_ID = s.CITY_ID,
                            CITY_NAME = s.CITIES.CITY_NAME,
                            POSTAL_CODE = s.CITIES.POSTAL_CODE
                        }
            ).OrderBy(o => o.SUPERVISOR).ToList();

            return data;
        }

        public override void DeleteControls(SUPERVISOR_AREAS entity, ShowState showState)
        {
            SUPERVISOR_AREAS anSupplier = context.SUPERVISOR_AREAS.Where(w => w.EMPLOYEE_ID == entity.EMPLOYEE_ID && w.CITY_ID == entity.CITY_ID).FirstOrDefault();
            if (anSupplier == null)
            {
                showState.isError = true;
                showState.ErrorMessage = "Suppervisor Area not found";
            }
        }

        public override void SaveHelper_ModifyBeforeSave(SUPERVISOR_AREAS param, SUPERVISOR_AREAS dbitem)
        {
            if (dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
            else
            {
                param.MODIFIED_DATE = dbitem.MODIFIED_DATE;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.EMPLOYEE_ID = dbitem.EMPLOYEE_ID;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public List<int> GetlistNotHaveCity(int SuperVizorID, List<int> CityIDs)
        {
            var insert = (from C in context.CITIES
                          where !context.SUPERVISOR_AREAS.Any(w => w.CITY_ID == C.CITY_ID && w.EMPLOYEE_ID == SuperVizorID && w.IS_CANCELED == false) &&
                               CityIDs.Contains(C.CITY_ID)
                          select new
                          {
                               C
                          }).ToList();

            return insert.Select(s=>s.C.CITY_ID).ToList();
        }
    }
}
