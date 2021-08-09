using JOB_MANAGER.Bussiness.Concrete;
using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER.Models.Login;
using JOB_MANAGER_BUSSINESS.Concrete;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JOB_MANAGER.Controllers
{
    public class CompanyController : BaseController
    {
        #region CompanyInfo
        [AuthorityControl("Profile")]
        public ActionResult CompanyProfile()
        {
            CountyManager countyManager = new CountyManager(new Models.Concrete.CountyDal());
            StateManager stateManager = new StateManager(new Models.Concrete.StateDal());
            CityManager cityManager = new CityManager(new Models.Concrete.CitiesDal());
            CompanyTypeManager companyTypeManager = new CompanyTypeManager(new Models.Concrete.CompanyTypeDal());


            ViewBag.COUNTRIES_LIST = new SelectList(countyManager.GetAll(), "COUNTRY_ID", "COUNTRY_NAME");
            ViewBag.STATES_LIST = new SelectList(stateManager.GetAll().Where(w=>w.IS_CANCELED == false).ToList(), "STATE_ID", "STATE_NAME");
            ViewBag.CITIES_LIST = new SelectList(cityManager.GetAll().Where(w => w.IS_CANCELED == false ), "CITY_ID", "CITY_NAME");
            ViewBag.COMPANY_TYPE_LIST = new SelectList(companyTypeManager.GetAll().Where(w => w.IS_CANCELED == false), "COMPANY_TYPE_ID", "COMPANY_TYPE_NAME");                        

            return View();
        }

        public JsonResult GetCompany()
        {
            CompanyManager companyManager = new CompanyManager(new Models.Concrete.CompanyDal());   

            return Json(new { Getlist = companyManager.GetAll().Where(w=>w.IS_CANCELED == false && 
                                        w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCompany(COMPANY param)
        {
            CompanyManager companyManager = new CompanyManager(new Models.Concrete.CompanyDal());
            var control = companyManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult GetStateByCompanyID(int CountryID)
        {
            StateManager stateManager = new StateManager(new Models.Concrete.StateDal());

            return Json(new { StateList = stateManager.GetAll().Where(w=>w.COUNTRY_ID == CountryID).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCityCompanyID(int StateID)
        {

            CityManager cityManager = new CityManager(new Models.Concrete.CitiesDal());

            return Json(new { CityList = cityManager.GetAll().Where(w=>w.STATE_ID == StateID).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateCompanyImg(int CompanyID, HttpPostedFileBase Image, int Remove)
        {

            CompanyManager companyManager = new CompanyManager(new Models.Concrete.CompanyDal());
            var control = companyManager.UpdateCompanyImg(CompanyID,Image,Remove);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        #endregion

        #region Vehicles
        [AuthorityControl("Vehicles")]
        public ActionResult Vehicles()
        {

            EmployeeManager employeeManager = new EmployeeManager(new Models.Concrete.EmployeeDal());
            VehicleModelManager vehicleModelManager = new VehicleModelManager(new Models.Concrete.VehicleModelDal());
            VehicleMakeManager vehicleMakeManager = new VehicleMakeManager(new Models.Concrete.VehicleMakeDal());
            VehicleBodyManager vehicleBodyManager = new VehicleBodyManager(new Models.Concrete.VehicleBodyDal());
            StatusManager statusManager = new StatusManager(new Models.Concrete.StatusDal());


            ViewBag.DRIVER_LIST = new SelectList(employeeManager.GetEmployeesByTypes(false,false,true,ThreadGlobals.UserAuthInfo.Value.CompanyId), "EMP_ID", "EMP_NAME");
            ViewBag.VEHICLE_MODELS = new SelectList(vehicleModelManager.GetAll().Where(w => w.IS_CANCELED == false).ToList(), "VEHICLE_MODEL_ID", "VEHICLE_MODEL_NAME");
            ViewBag.STATUS_LIST = new SelectList(statusManager.GetAllByType((int)StatusType.Vehicle), "STATUS_ID", "STATUS_NAME");
            ViewBag.VEHICLE_MAKES = new SelectList(vehicleMakeManager.GetAll().Where(w => w.IS_CANCELED == false), "VEHICLE_MAKE_ID", "VEHICLE_MAKE_NAME");
            ViewBag.VEHICLE_BODYS = new SelectList(vehicleBodyManager.GetAll().Where(w => w.IS_CANCELED == false), "BODY_TYPE_ID", "BODY_TYPE_NAME");                       
            ViewBag.VEHICLE_YEARS = new SelectList(GlobalTools.GetYOMLookUp(ThreadGlobals.UserAuthInfo.Value.CompanyId), "YOM_ID", "YOM_VALUE");

            return View();
        }

        public ActionResult GetVeciclesList()
        {
            VehicleManager vehicleManager = new VehicleManager(new Models.Concrete.VehicleDal());

            return Json(new { Getlist = vehicleManager.GetAll().Where(w=>w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetVecModelList(int VecMakesID,int VecBodysID)
        {
            VehicleModelManager vehicleModelManager = new VehicleModelManager(new Models.Concrete.VehicleModelDal());

            var data = vehicleModelManager.GetAll().Where(w => w.IS_CANCELED == false);

            if(VecBodysID > 0)
            {
                data = data.Where(w => w.BODY_TYPE == VecBodysID);
            }

            if (VecMakesID > 0)
            {
                data = data.Where(w => w.VEHICLE_MAKE_ID == VecMakesID);
            }

            if(data != null)
            {
                var query = (from d in data
                             select new
                             {
                                 VEHICLE_MODEL_ID = d.VEHICLE_MODEL_ID,
                                 VEHICLE_MODEL_NAME = d.VEHICLE_MODEL_NAME
                             }
                            ).ToList();

                return Json(new { ModelList = query }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { ModelList = data }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddOrUpdateVehicle(VEHICLES param)
        {
            VehicleManager vehicleManager = new VehicleManager(new Models.Concrete.VehicleDal());
            var control = vehicleManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage});
        }

        [HttpPost]
        public ActionResult DeleteVehicle(int VehicleID)
        {
            VEHICLES anVehicle = db.VEHICLES.Find(VehicleID);
            if (anVehicle != null)
            {
                try
                {
                    anVehicle.IS_CANCELED = true;
                    anVehicle.UPDATED_BY = GetUserID();
                    db.VEHICLES.Attach(anVehicle);                    

                    db.Entry(anVehicle).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anVehicle).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Vehicle not found" });
        }
        #endregion

        #region Supplier
        [AuthorityControl("Supplier")]
        public ActionResult Supplier()
        {

            CountyManager countyManager = new CountyManager(new Models.Concrete.CountyDal());
            StateManager stateManager = new StateManager(new Models.Concrete.StateDal());
            CityManager cityManager = new CityManager(new Models.Concrete.CitiesDal());
            StatusManager statusManager = new StatusManager(new Models.Concrete.StatusDal());

            ViewBag.COUNTRIES_LIST = new SelectList(countyManager.GetAll().Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
            ViewBag.STATES_LIST = new SelectList(stateManager.GetAll().Where(w => w.IS_CANCELED == false), "STATE_ID", "STATE_NAME");
            ViewBag.CITIES_LIST = new SelectList(cityManager.GetAll().Where(w => w.IS_CANCELED == false), "CITY_ID", "CITY_NAME");
            ViewBag.STATUS_LIST = new SelectList(statusManager.GetAllByType((int)StatusType.Supplier), "STATUS_ID", "STATUS_NAME");

            return View();
        }

        [HttpGet]
        public ActionResult GetSupplierList()
        {
            SuplierManager suplierManager = new SuplierManager(new Models.Concrete.SuplierDal());
            
            return Json(new { Getlist = suplierManager.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateSupplier(SUPPLIERS param)
        {
            SuplierManager suplierManager = new SuplierManager(new Models.Concrete.SuplierDal());
            var control = suplierManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage});
        }

        [HttpPost]
        public ActionResult DeleteSupplier(int SupplierID)
        {
            SuplierManager suplierManager = new SuplierManager(new Models.Concrete.SuplierDal());
            var control = suplierManager.Delete(new SUPPLIERS() { SUPPLIER_ID = SupplierID });

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Supervisor Areas
        [AuthorityControl("SupervisorAreas")]
        public ActionResult SupervisorAreas()
        {

            CountyManager countyManager = new CountyManager(new Models.Concrete.CountyDal());
            StateManager stateManager = new StateManager(new Models.Concrete.StateDal());
            CityManager cityManager = new CityManager(new Models.Concrete.CitiesDal());
            StatusManager statusManager = new StatusManager(new Models.Concrete.StatusDal());

            EmployeeManager employeeManager = new EmployeeManager(new Models.Concrete.EmployeeDal());

            int CompanyID = GetCompanyID();
            ViewBag.SUPERVISOR_LIST = new SelectList(employeeManager.GetEmployeesByTypes(true,false,false,ThreadGlobals.UserAuthInfo.Value.CompanyId).Select(e => new
            {
                e.EMP_ID,
                NAME = e.EMP_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");

            int defaultCountry = countyManager.GetAll().Where(w => w.IS_DEFAULT == true).Select(s => s.COUNTRY_ID).FirstOrDefault();
            if (defaultCountry > 0)
            {
                ViewBag.COUNTRIES_LIST = new SelectList(countyManager.GetAll().Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "COUNTRY_ID", "COUNTRY_NAME");
                ViewBag.STATES_LIST = new SelectList(stateManager.GetAll().Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "STATE_ID", "STATE_NAME");
                ViewBag.CITIES_LIST = new SelectList(cityManager.GetAll().Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "CITY_ID", "CITY_NAME");
            }
            else
            {
                ViewBag.COUNTRIES_LIST = new SelectList(countyManager.GetAll().Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
                ViewBag.STATES_LIST = new SelectList(stateManager.GetAll().Where(w => w.IS_CANCELED == false), "STATE_ID", "STATE_NAME");
                ViewBag.CITIES_LIST = new SelectList(cityManager.GetAll().Where(w => w.IS_CANCELED == false), "CITY_ID", "CITY_NAME");
            }

            return View();
        }

        [HttpGet]
        public JsonResult GetListFilterSupervisors()
        {

            SupervisorAreasManager supervisorAreasManager = new SupervisorAreasManager(new SupervisorAreasDal());

            var dataCity = supervisorAreasManager.GetAll().Select(s => new { CITY_ID = s.CITY_ID, CITY_NAME = s.CITY_NAME }).Distinct().ToList();
            var dataSup = supervisorAreasManager.GetAll().Select(s => new
            {
                EMPLOYEE_ID = s.EMPLOYEE_ID,
                EMPLOYEE_NAME = s.EMPLOYEES.FIRST_NAME + SqlConstants.stringWhiteSpace + s.EMPLOYEES.LAST_NAME
            }).Distinct().ToList();


            return Json(new { City = dataCity, Suplier= dataSup }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSuperVisorList()
        {

            SupervisorAreasManager supervisorAreasManager = new SupervisorAreasManager(new SupervisorAreasDal());         

            return Json(new { Getlist = supervisorAreasManager.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddSuppervisorArea(int SuperVizorID, List<int> CityIDs)
        {
            SupervisorAreasManager supervisorAreasManager = new SupervisorAreasManager(new SupervisorAreasDal());
            var control = supervisorAreasManager.AddSuppervisorArea(SuperVizorID, CityIDs);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public ActionResult DeleteSuppervisorArea(int SuperVizorID, int CityID)
        {
            SupervisorAreasManager supervisorAreasManager = new SupervisorAreasManager(new SupervisorAreasDal());
            var control = supervisorAreasManager.Delete(new SUPERVISOR_AREAS() { EMPLOYEE_ID = SuperVizorID});

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Holiday
        [AuthorityControl("Holidays")]
        public ActionResult Holidays()
        {
            return View();
        }
        

        [HttpGet]
        public JsonResult GetHolidays()
        {
            HolidayManager holidayManager = new HolidayManager(new HolidayDal());
            
            return Json(new { Getlist = holidayManager.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrUpdateHoliday(HOLIDAYS param)
        {
            HolidayManager holidayManager = new HolidayManager(new HolidayDal());
            var control = holidayManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        public JsonResult RemoveHoliday(HOLIDAYS param)
        {
            HolidayManager holidayManager = new HolidayManager(new HolidayDal());
            var control = holidayManager.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        #endregion
    }
}
