using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.Business.Ninject;
using JOB_MANAGER.DataAcess.Helper;
using JOB_MANAGER.DATAACESS.CrossCuttingConsers;
using JOB_MANAGER.DATAACESS.Helper;
using JOB_MANAGER.DATAACESS.Models;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.ComplexType;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JOB_MANAGER.Controllers
{
    public class AdminOperationController : BaseController
    {              

        #region Contract
        [AuthorityControl("Contract")]
        public ActionResult Contract()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetContractTypeList()
        {
            IContractTypeService contractType = InstanceFactory.GetInstance<IContractTypeService>();

            return Json(new { Getlist = contractType.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRolesList()
        {
            IRoleService role = InstanceFactory.GetInstance<IRoleService>();

            return Json(new { Getlist = role.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateContractType(CONTRACT_TYPES param)
        {
            IContractTypeService contractType = InstanceFactory.GetInstance<IContractTypeService>();
            var control = contractType.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveContractType(CONTRACT_TYPES param)
        {
            ContractTypeManager contractType = new ContractTypeManager(new ContractTypeDal());
            var control = contractType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateContractRole(ROLES param)
        {
            IRoleService role = InstanceFactory.GetInstance<IRoleService>();
            var control = role.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveRoleType(ROLES param)
        {
            IRoleService role = InstanceFactory.GetInstance<IRoleService>();
            var control = role.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Employee
        [AuthorityControl("Employee")]
        public ActionResult Employee()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetTitleList()
        {
            ITitleService title = InstanceFactory.GetInstance<ITitleService>();
            return Json(new { Getlist = title.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateTitle(TITLES param)
        {
            ITitleService title = InstanceFactory.GetInstance<ITitleService>();
            var control = title.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage});
        }

        [HttpPost]
        public JsonResult RemoveTitle(TITLES param)
        {
            ITitleService title = InstanceFactory.GetInstance<ITitleService>();
            var control = title.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Department
        [AuthorityControl("Department")]
        public ActionResult Department()
        {
            return View();
        }
        
        [HttpGet]
        public JsonResult GetDepartmentList()
        {
            IDepartmentService department = InstanceFactory.GetInstance<IDepartmentService>();
            return Json(new { Getlist = department.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateDepartment(DEPARTMENTS param)
        {
            IDepartmentService department = InstanceFactory.GetInstance<IDepartmentService>();
            var control = department.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveDepartment(DEPARTMENTS param)
        {
            IDepartmentService department = InstanceFactory.GetInstance<IDepartmentService>();
            var control = department.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Location
        [AuthorityControl("Location")]
        public ActionResult Location()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCountyiesList()
        {
            ICountyService county = InstanceFactory.GetInstance<ICountyService>();           
            return Json(new { Getlist = county.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCountry(COUNTRIES param)
        {
            ICountyService county = InstanceFactory.GetInstance<ICountyService>();
            var control = county.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveCountries(COUNTRIES param)
        {
            ICountyService county = InstanceFactory.GetInstance<ICountyService>();
            var control = county.Delete(param);

            return Json(new { success = !control.isError, Message = control.isError, GetList = GetDepartmentList() });
        }

        [HttpPost]
        public JsonResult OnLoadWebCountriesSave(List<WebLoadCountries> param)
        {

            foreach(WebLoadCountries country in param)
            {                
                //WebClient webClient = new WebClient();
                //var imageData = webClient.DownloadData(country.FLAG);

                COUNTRIES insert = new COUNTRIES();

                insert.COUNTRY_CODE = country.COUNTRY_CODE;
                insert.COUNTRY_NAME = country.COUNTRY_NAME;
                insert.FLAG_URL = country.FLAG;                

                AddOrUpdateCountry(insert);
            }
           
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetStateList()
        {
            IStateService state = InstanceFactory.GetInstance<IStateService>();
            return Json(new { Getlist = state.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateState(STATES param)
        {
            IStateService state = InstanceFactory.GetInstance<IStateService>();
            var control = state.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveState(STATES param)
        {
            IStateService state = InstanceFactory.GetInstance<IStateService>();
            var control = state.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetCitiesList()
        {            
            ICityService cityManager = InstanceFactory.GetInstance<ICityService>();
            return Json(new { Getlist = cityManager.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCity(CITIES param)
        {
            ICityService cityManager = InstanceFactory.GetInstance<ICityService>();
            var control = cityManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveCity(CITIES param)
        {
            ICityService cityManager = InstanceFactory.GetInstance<ICityService>();
            var control = cityManager.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetSteetList()
        {
            IStreetService street = InstanceFactory.GetInstance<IStreetService>();            
            return Json(new { Getlist = street.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateStreet(STREET param)
        {
            IStreetService street = InstanceFactory.GetInstance<IStreetService>();
            var control = street.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }


        [HttpPost]
        public JsonResult RemoveSteet(STREET param)
        {
            IStreetService street = InstanceFactory.GetInstance<IStreetService>();
            var control = street.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Parameters
        [AuthorityControl("SystemConfiguration")]
        public ActionResult Parameters()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetParameterList()
        {
            //ParameterManager parameter = new ParameterManager(new ParameterDal(UserInfo));
            GlobalCache global = new GlobalCache();
            return Json(new { Getlist = global.GetCacheParameter() }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult AddOrUpdateParameter(PARAMETERS param)
        {            
            IParametrerService parameter = InstanceFactory.GetInstance<IParametrerService>();
            var control = parameter.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region  CompanyType

        [AuthorityControl("CompanyType")]
        public ActionResult CompanyType()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCompanyTypeList()
        {
            ICompanyTypeService companyType = InstanceFactory.GetInstance<ICompanyTypeService>();
            return Json(new { Getlist = companyType.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCompanyType(COMPANY_TYPES param)
        {
            ICompanyTypeService companyType = InstanceFactory.GetInstance<ICompanyTypeService>();
            var control = companyType.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveCompanyType(COMPANY_TYPES param)
        {
            ICompanyTypeService companyType = InstanceFactory.GetInstance<ICompanyTypeService>();
            var control = companyType.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Vehicle
        [AuthorityControl("Vehicle")]
        public ActionResult Vehicle()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetVehiclebody()
        {
            IVehicleBodyServices vehicle = InstanceFactory.GetInstance<IVehicleBodyServices>();            
            return Json(new { Getlist = vehicle.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateVehiclebody(VEHICLE_BODY_TYPES param)
        {
            IVehicleBodyServices vehicle = InstanceFactory.GetInstance<IVehicleBodyServices>();
            var control = vehicle.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveVehiclebody(VEHICLE_BODY_TYPES param)
        {
            IVehicleBodyServices vehicle = InstanceFactory.GetInstance<IVehicleBodyServices>();
            var control = vehicle.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetVehiclemakesList()
        {
            IVehicleMakeService vehicle = InstanceFactory.GetInstance<IVehicleMakeService>();
            return Json(new { Getlist = vehicle.GetAll() }, JsonRequestBehavior.AllowGet);            
        }

        [HttpPost]
        public JsonResult AddOrUpdateVehiclemake(VEHICLE_MAKES param)
        {
            IVehicleMakeService vehicle = InstanceFactory.GetInstance<IVehicleMakeService>();
            var control = vehicle.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveVehiclemake(VEHICLE_MAKES param)
        {
            IVehicleMakeService vehicle = InstanceFactory.GetInstance<IVehicleMakeService>();
            var control = vehicle.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetVehiclemodelsList()
        {
            IVehicleModelService vehicle = InstanceFactory.GetInstance<IVehicleModelService>();
            return Json(new { Getlist = vehicle.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateVehiclemodels(VEHICLE_MODELS param)
        {
            IVehicleModelService vehicle = InstanceFactory.GetInstance<IVehicleModelService>();
            var control = vehicle.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });

        }

        [HttpPost]
        public JsonResult RemoveVehiclemodels(VEHICLE_MODELS param)
        {
            IVehicleModelService vehicle = InstanceFactory.GetInstance<IVehicleModelService>();
            var control = vehicle.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetStatusList(int StatusType)
        {
            IStatusService status = InstanceFactory.GetInstance<IStatusService>();
            return Json(new { Getlist = status.GetAll().Where(w=>w.STATUS_ID == StatusType).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateStatus(STATUS param)
        {
            IStatusService status = InstanceFactory.GetInstance<IStatusService>();
            var control = status.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveStatus(STATUS param)
        {
            IStatusService status = InstanceFactory.GetInstance<IStatusService>();
            var control = status.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region SupplierStatus
        [AuthorityControl("SupplierStatus")]
        public ActionResult SupplierStatus()
        {
            return View();
        }
        #endregion

        #region ClientStatus
        [AuthorityControl("ClientStatus")]
        public ActionResult ClientStatus()
        {
            return View();
        }
        #endregion

        #region menuAuthority
        [AuthorityControl("MenuAuthority")]
        public ActionResult MenuAuthority()
        {
            return View();
        }

        public JsonResult GetRolesMenuList()
        {            
            IRoleMenuService roleMenu = InstanceFactory.GetInstance<IRoleMenuService>();
            return Json(new { Getlist = roleMenu.GetAll().Distinct().ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveMenuRoles(int RoleID,List<MenuRolesDto> Menus)
        {
            ROLE_MENU chekmenus = new ROLE_MENU();
            chekmenus.ROLE_ID = RoleID;
            chekmenus.MENUS_STR = JsonConvert.SerializeObject(Menus);

            IRoleMenuService roleMenu = InstanceFactory.GetInstance<IRoleMenuService>();
            var control = roleMenu.AddorUpdate(chekmenus);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        public JsonResult GetLoginEmpRolesMenus()
        {
            IRoleMenuService roleMenu = InstanceFactory.GetInstance<IRoleMenuService>();
            return Json(new { Getlist = roleMenu.GetAllByEmployee() }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region QuoteAndJob
        [AuthorityControl("QuoteAndJob")]
        public ActionResult QuoteAndJob()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetQuoteAndJobList(int Type)
        {
            if (Type == 1)
            {
                IFloorTypeService floorType = InstanceFactory.GetInstance<IFloorTypeService>();
                return Json(new { Getlist = floorType.GetAll() }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 2)
            {
                IMateryalService materyal = InstanceFactory.GetInstance<IMateryalService>();
                return Json(new { Getlist = materyal.GetAll() }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 3)
            {
                IProjectTypeService projectType = InstanceFactory.GetInstance<IProjectTypeService>();
                return Json(new { Getlist = projectType.GetAll() }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 4)
            {
                IEmployeeService employeeService = InstanceFactory.GetInstance<IEmployeeService>();

                var data = employeeService.GetEmployeesByTypes(true, false, false, ThreadGlobals.UserAuthInfo.Value.CompanyId);

                return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 5)
            {
                INoteTypeService noteType = InstanceFactory.GetInstance<INoteTypeService>();
                return Json(new { Getlist = noteType.GetAll() }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 6)
            {
                IDocumentTypeService documentType = InstanceFactory.GetInstance<IDocumentTypeService>();
                return Json(new { Getlist = documentType.GetAll() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Getlist = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateFloorType(FLOOR_TYPES param)
        {
            IFloorTypeService floorType = InstanceFactory.GetInstance<IFloorTypeService>();
            var control = floorType.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveFloorType(FLOOR_TYPES param)
        {
            IFloorTypeService floorType = InstanceFactory.GetInstance<IFloorTypeService>();
            var control = floorType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateMaterial(MATERIALS param)
        {
            IMateryalService materyal = InstanceFactory.GetInstance<IMateryalService>();
            var control = materyal.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveMaterial(MATERIALS param)
        {
            IMateryalService materyal = InstanceFactory.GetInstance<IMateryalService>();
            var control = materyal.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateProjectType(PROJECT_TYPES param)
        {
            IProjectTypeService projectType = InstanceFactory.GetInstance<IProjectTypeService>();
            var control = projectType.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveProjectType(PROJECT_TYPES param)
        {
            IProjectTypeService projectType = InstanceFactory.GetInstance<IProjectTypeService>();
            var control = projectType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }


        [HttpPost]
        public JsonResult AddOrUpdateNoteType(NOTE_TYPES param)
        {
            INoteTypeService noteType = InstanceFactory.GetInstance<INoteTypeService>();
            var control = noteType.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveNoteType(NOTE_TYPES param)
        {
            INoteTypeService noteType = InstanceFactory.GetInstance<INoteTypeService>();
            var control = noteType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateDocumentType(DOCUMENT_TYPES param)
        {
            IDocumentTypeService documentType = InstanceFactory.GetInstance<IDocumentTypeService>();
            var control = documentType.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }


        [HttpPost]
        public JsonResult RemoveDocumentType(DOCUMENT_TYPES param)
        {
            IDocumentTypeService documentType = InstanceFactory.GetInstance<IDocumentTypeService>();
            var control = documentType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion

        #region Phases
        [AuthorityControl("PhaseandTask")]
        public ActionResult PhaseandTask()
        {
            return View();
        }
             
        [HttpGet]
        public JsonResult DefPhasesTask()
        {
            IPhaseService phase =InstanceFactory.GetInstance<IPhaseService>();
            IStatusService status = InstanceFactory.GetInstance<IStatusService>();
            IEmployeeService employee = InstanceFactory.GetInstance<IEmployeeService>();

            return Json(new { Getlist = phase.GetProjectTypeWithPhaseTasks(), 
                              ProjectStatus = status.GetAllByType((int)StatusType.Project), 
                              Employee = employee.GetAllByCompany(ThreadGlobals.UserAuthInfo.Value.CompanyId) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddOrUpdatePhases(DEF_PROJECT_PHASES param)
        {
            IPhaseService phase = InstanceFactory.GetInstance<IPhaseService>();
            var control = phase.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }


        [HttpPost]
        public JsonResult RemovePhases(DEF_PROJECT_PHASES param)
        {
            IPhaseService phase = InstanceFactory.GetInstance<IPhaseService>();
            var control = phase.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateTask(DEF_TASKS param)
        {
            ITaskService task = InstanceFactory.GetInstance<ITaskService>();
            var control = task.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveTask(DEF_TASKS param)
        {
            ITaskService task = InstanceFactory.GetInstance<ITaskService>();
            var control = task.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion
    }
}
