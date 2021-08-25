using JOB_MANAGER.Business;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.Business.Ninject;
using JOB_MANAGER.DATAACESS.CrossCuttingConsers;
using JOB_MANAGER.DATAACESS.Helper;
using JOB_MANAGER.DATAACESS.Models;
using JOB_MANAGER.Models;
using System;
using System.Collections.Generic;
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
            ICountyService countyManager = InstanceFactory.GetInstance<ICountyService>();
            IStateService stateManager = InstanceFactory.GetInstance<IStateService>();
            ICityService cityManager = InstanceFactory.GetInstance<ICityService>();
            IContractTypeService companyTypeManager = InstanceFactory.GetInstance<IContractTypeService>();


            ViewBag.COUNTRIES_LIST = new SelectList(countyManager.GetAll(), "COUNTRY_ID", "COUNTRY_NAME");
            ViewBag.STATES_LIST = new SelectList(stateManager.GetAll().Where(w=>w.IS_CANCELED == false).ToList(), "STATE_ID", "STATE_NAME");
            ViewBag.CITIES_LIST = new SelectList(cityManager.GetAll().Where(w => w.IS_CANCELED == false ), "CITY_ID", "CITY_NAME");
            ViewBag.COMPANY_TYPE_LIST = new SelectList(companyTypeManager.GetAll().Where(w => w.IS_CANCELED == false), "COMPANY_TYPE_ID", "COMPANY_TYPE_NAME");                        

            return View();
        }

        public JsonResult GetCompany()
        {
            ICompanyService companyManager = InstanceFactory.GetInstance<ICompanyService>();

            return Json(new { Getlist = companyManager.GetAll().Where(w=>w.IS_CANCELED == false && 
                                        w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCompany(COMPANY param)
        {
            ICompanyService companyManager = InstanceFactory.GetInstance<ICompanyService>();
            var control = companyManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult GetStateByCompanyID(int CountryID)
        {
            IStateService stateManager = InstanceFactory.GetInstance<IStateService>();
            return Json(new { StateList = stateManager.GetAll().Where(w=>w.COUNTRY_ID == CountryID).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCityCompanyID(int StateID)
        {
            ICityService cityManager = InstanceFactory.GetInstance<ICityService>();
            return Json(new { CityList = cityManager.GetAll().Where(w=>w.STATE_ID == StateID).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateCompanyImg(int CompanyID, HttpPostedFileBase Image, int Remove)
        {

            ICompanyService companyManager = InstanceFactory.GetInstance<ICompanyService>();
            var control = companyManager.UpdateCompanyImg(CompanyID,Image,Remove);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        #endregion

        #region Vehicles
        [AuthorityControl("Vehicles")]
        public ActionResult Vehicles()
        {

            IEmployeeService employeeManager = InstanceFactory.GetInstance<IEmployeeService>();
            IVehicleBodyServices vehicleModelManager = InstanceFactory.GetInstance<IVehicleBodyServices>();
            IVehicleMakeService vehicleMakeManager = InstanceFactory.GetInstance<IVehicleMakeService>();
            IVehicleBodyServices vehicleBodyManager = InstanceFactory.GetInstance<IVehicleBodyServices>(); 
            IStatusService statusManager = InstanceFactory.GetInstance<IStatusService>();


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
            IVehicleService vehicleManager = InstanceFactory.GetInstance<IVehicleService>();

            return Json(new { Getlist = vehicleManager.GetAll().Where(w=>w.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetVecModelList(int VecMakesID,int VecBodysID)
        {            
            IVehicleModelService vehicleModelManager = InstanceFactory.GetInstance<IVehicleModelService>();

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
            IVehicleService vehicleManager = InstanceFactory.GetInstance<IVehicleService>();
            var control = vehicleManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage});
        }

        [HttpPost]
        public ActionResult DeleteVehicle(int VehicleID)
        {
            IVehicleService vehicleManager = InstanceFactory.GetInstance<IVehicleService>();
            var control = vehicleManager.Delete(new VEHICLES() { VEHICLE_ID = VehicleID });

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Supplier
        [AuthorityControl("Supplier")]
        public ActionResult Supplier()
        {

            ICountyService countyManager = InstanceFactory.GetInstance<ICountyService>();
            IStatusService statusManager = InstanceFactory.GetInstance<IStatusService>();
            IStateService stateManager = InstanceFactory.GetInstance<IStateService>();
            ICityService cityManager = InstanceFactory.GetInstance<ICityService>();

            ViewBag.COUNTRIES_LIST = new SelectList(countyManager.GetAll().Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
            ViewBag.STATES_LIST = new SelectList(stateManager.GetAll().Where(w => w.IS_CANCELED == false), "STATE_ID", "STATE_NAME");
            ViewBag.CITIES_LIST = new SelectList(cityManager.GetAll().Where(w => w.IS_CANCELED == false), "CITY_ID", "CITY_NAME");
            ViewBag.STATUS_LIST = new SelectList(statusManager.GetAllByType((int)StatusType.Supplier), "STATUS_ID", "STATUS_NAME");

            return View();
        }

        [HttpGet]
        public ActionResult GetSupplierList()
        {
            ISuplierService suplierManager = InstanceFactory.GetInstance<ISuplierService>();

            return Json(new { Getlist = suplierManager.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateSupplier(SUPPLIERS param)
        {
            ISuplierService suplierManager = InstanceFactory.GetInstance<ISuplierService>();
            var control = suplierManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage});
        }

        [HttpPost]
        public ActionResult DeleteSupplier(int SupplierID)
        {
            ISuplierService suplierManager = InstanceFactory.GetInstance<ISuplierService>();
            var control = suplierManager.Delete(new SUPPLIERS() { SUPPLIER_ID = SupplierID });

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Supervisor Areas
        [AuthorityControl("SupervisorAreas")]
        public ActionResult SupervisorAreas()
        {

            ICountyService countyManager = InstanceFactory.GetInstance<ICountyService>();
            IStatusService statusManager = InstanceFactory.GetInstance<IStatusService>();
            ICityService cityManager = InstanceFactory.GetInstance<ICityService>();
            IStateService stateManager = InstanceFactory.GetInstance<IStateService>();

            EmployeeManager employeeManager = new EmployeeManager(new EmployeeDal());

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

            ISupervisorAreasService supervisorAreasManager = InstanceFactory.GetInstance<ISupervisorAreasService>();

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

            ISupervisorAreasService supervisorAreasManager = InstanceFactory.GetInstance<ISupervisorAreasService>();

            return Json(new { Getlist = supervisorAreasManager.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddSuppervisorArea(int SuperVizorID, List<int> CityIDs)
        {
            ISupervisorAreasService supervisorAreasManager = InstanceFactory.GetInstance<ISupervisorAreasService>();
            var control = supervisorAreasManager.AddSuppervisorArea(SuperVizorID, CityIDs);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public ActionResult DeleteSuppervisorArea(int SuperVizorID, int CityID)
        {
            ISupervisorAreasService supervisorAreasManager = InstanceFactory.GetInstance<ISupervisorAreasService>();
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
            IHolidayService holidayManager = InstanceFactory.GetInstance<IHolidayService>(); 
            
            return Json(new { Getlist = holidayManager.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrUpdateHoliday(HOLIDAYS param)
        {
            IHolidayService holidayManager = InstanceFactory.GetInstance<IHolidayService>();
            var control = holidayManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        public JsonResult RemoveHoliday(HOLIDAYS param)
        {
            IHolidayService holidayManager = InstanceFactory.GetInstance<IHolidayService>();
            var control = holidayManager.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        #endregion
    }
}
