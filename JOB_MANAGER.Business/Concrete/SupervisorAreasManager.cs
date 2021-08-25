using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class SupervisorAreasManager : ISupervisorAreasService
    {
        private SupervisorAreasDal _supervisorAreasDal;
        public SupervisorAreasManager(SupervisorAreasDal supervisorAreasDal)
        {
             _supervisorAreasDal = supervisorAreasDal;
        }

        public ShowState AddorUpdate(SUPERVISOR_AREAS param)
        {
            return _supervisorAreasDal.AddorUpdate(param, f => f.EMPLOYEE_ID == param.EMPLOYEE_ID && f.CITY_ID == param.CITY_ID);
        }

        public ShowState Delete(SUPERVISOR_AREAS param)
        {
            return _supervisorAreasDal.Delete(param);
        }

        public SUPERVISOR_AREAS Get(SUPERVISOR_AREAS param)
        {
            return _supervisorAreasDal.GetAll(f => f.EMPLOYEE_ID == param.EMPLOYEE_ID && f.CITY_ID == param.CITY_ID).FirstOrDefault();
        }

        public List<SupervisorAreasExtented> GetAll()
        {
            return _supervisorAreasDal.GetAll2();
        }

        public ShowState AddSuppervisorArea(int SuperVizorID, List<int> CityIDs)
        {

            var supervisorarea = _supervisorAreasDal.GetAll(w => w.EMPLOYEE_ID == SuperVizorID && !CityIDs.Contains(w.CITY_ID));                        
            if (supervisorarea != null)
            {
                foreach (var item in supervisorarea)
                {
                    item.IS_CANCELED = true;
                    AddorUpdate(item);
                }
            }

            supervisorarea = _supervisorAreasDal.GetAll(w => w.EMPLOYEE_ID == SuperVizorID && CityIDs.Contains(w.CITY_ID));
            if (supervisorarea != null)
            {
                foreach (var item in supervisorarea)
                {
                    item.IS_CANCELED = false;
                    AddorUpdate(item);
                }
            }

         

            foreach (var item in _supervisorAreasDal.GetlistNotHaveCity(SuperVizorID,CityIDs) )
            {
                SUPERVISOR_AREAS param = new SUPERVISOR_AREAS();
                
                param.CITY_ID = item;
                param.EMPLOYEE_ID = SuperVizorID;
                AddorUpdate(param);
            }

            return new ShowState() { isError = false, ErrorMessage = string.Empty };
        }
    }
}
