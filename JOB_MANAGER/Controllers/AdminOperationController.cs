using JOB_MANAGER.Bussiness.Concrete;
using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER.Models.Login;
using JOB_MANAGER_BUSSINESS.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace JOB_MANAGER.Controllers
{    
    public class AdminOperationController : BaseController
    {        
        protected UserInfo UserInfo;
        public AdminOperationController()
        {
            UserInfo = this.GetUserInfo();
        }

        #region Contract
        [AuthorityControl("Contract")]
        public ActionResult Contract()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetContractTypeList()
        {
            ContractTypeManager contractType = new ContractTypeManager(new ContractTypeDal(UserInfo));
            return Json(new { Getlist = contractType.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetRolesList()
        {
            RoleManager role = new RoleManager(new RoleDal(UserInfo));

            return Json(new { Getlist = role.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateContractType(CONTRACT_TYPES param)
        {
            ContractTypeManager contractType = new ContractTypeManager(new ContractTypeDal(UserInfo));
            var control = contractType.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveContractType(CONTRACT_TYPES param)
        {
            ContractTypeManager contractType = new ContractTypeManager(new ContractTypeDal(UserInfo));
            var control = contractType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateContractRole(ROLES param)
        {
            RoleManager role = new RoleManager(new RoleDal(UserInfo));
            var control = role.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveRoleType(ROLES param)
        {
            RoleManager role = new RoleManager(new RoleDal(UserInfo));
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
            TitleManager title = new TitleManager(new TitleDal(UserInfo));
            return Json(new { Getlist = title.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateTitle(TITLES param)
        {
            TitleManager title = new TitleManager(new TitleDal(UserInfo));
            var control = title.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage});
        }

        [HttpPost]
        public JsonResult RemoveTitle(TITLES param)
        {
            TitleManager title =new TitleManager(new TitleDal(UserInfo));
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
            DepartmentManager department = new DepartmentManager(new DepartmentDal(UserInfo));
            return Json(new { Getlist = department.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateDepartment(DEPARTMENTS param)
        {
            DepartmentManager department = new DepartmentManager(new DepartmentDal(UserInfo));
            var control = department.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveDepartment(DEPARTMENTS param)
        {
            DepartmentManager department = new DepartmentManager(new DepartmentDal(UserInfo));
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
            CountyManager county = new CountyManager(new CountyDal(UserInfo));
            return Json(new { Getlist = county.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCountry(COUNTRIES param)
        {
            CountyManager county = new CountyManager(new CountyDal(UserInfo));
            var control = county.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveCountries(COUNTRIES param)
        {
            CountyManager county = new CountyManager(new CountyDal(UserInfo));
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
            StateManager state = new StateManager(new StateDal(UserInfo));

            return Json(new { Getlist = state.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateState(STATES param)
        {
            StateManager state = new StateManager(new StateDal(UserInfo));
            var control = state.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveState(STATES param)
        {
            StateManager state = new StateManager( new StateDal(UserInfo));
            var control = state.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetCitiesList()
        {
            CityManager cityManager = new CityManager(new CitiesDal(UserInfo));
            return Json(new { Getlist = cityManager.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCity(CITIES param)
        {
            CityManager cityManager = new CityManager(new CitiesDal(UserInfo));
            var control = cityManager.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveCity(CITIES param)
        {
            CityManager cityManager = new CityManager(new CitiesDal(UserInfo));
            var control = cityManager.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetSteetList()
        {
            StreetManager street = new StreetManager(new StreetDal(UserInfo));
            return Json(new { Getlist = street.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateStreet(STREET param)
        {
            StreetManager street = new StreetManager(new StreetDal(UserInfo));
            var control = street.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }


        [HttpPost]
        public JsonResult RemoveSteet(STREET param)
        {
            StreetManager street = new StreetManager(new StreetDal(UserInfo));
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
            ParameterManager parameter = new ParameterManager(new ParameterDal(UserInfo));
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
            CompanyTypeManager companyType = new CompanyTypeManager(new CompanyTypeDal(UserInfo));
            return Json(new { Getlist = companyType.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCompanyType(COMPANY_TYPES param)
        {
            CompanyTypeManager companyType = new CompanyTypeManager(new CompanyTypeDal(UserInfo));
            var control = companyType.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveCompanyType(COMPANY_TYPES param)
        {
            CompanyTypeManager companyType = new CompanyTypeManager(new CompanyTypeDal(UserInfo));
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
            VehicleBodyManager vehicle = new VehicleBodyManager(new VehicleBodyDal(UserInfo));
            return Json(new { Getlist = vehicle.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateVehiclebody(VEHICLE_BODY_TYPES param)
        {
            VehicleBodyManager vehicle = new VehicleBodyManager(new VehicleBodyDal(UserInfo));
            var control = vehicle.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveVehiclebody(VEHICLE_BODY_TYPES param)
        {
            VehicleBodyManager vehicle = new VehicleBodyManager(new VehicleBodyDal(UserInfo));
            var control = vehicle.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetVehiclemakesList()
        {
            VehicleMakeManager vehicle = new VehicleMakeManager(new VehicleMakeDal(UserInfo));
            return Json(new { Getlist = vehicle.GetAll() }, JsonRequestBehavior.AllowGet);            
        }

        [HttpPost]
        public JsonResult AddOrUpdateVehiclemake(VEHICLE_MAKES param)
        {
            VehicleMakeManager vehicle = new VehicleMakeManager(new VehicleMakeDal(UserInfo));
            var control = vehicle.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveVehiclemake(VEHICLE_MAKES param)
        {
            VehicleMakeManager vehicle = new VehicleMakeManager(new VehicleMakeDal(UserInfo));
            var control = vehicle.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetVehiclemodelsList()
        {
            VehicleModelManager vehicle = new VehicleModelManager( new VehicleModelDal(UserInfo));
            return Json(new { Getlist = vehicle.GetAll() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateVehiclemodels(VEHICLE_MODELS param)
        {
            VehicleModelManager vehicle = new VehicleModelManager(new VehicleModelDal(UserInfo));
            var control = vehicle.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });

        }

        [HttpPost]
        public JsonResult RemoveVehiclemodels(VEHICLE_MODELS param)
        {
            VehicleModelManager vehicle = new VehicleModelManager(new VehicleModelDal(UserInfo));
            var control = vehicle.Delete(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpGet]
        public JsonResult GetStatusList(int StatusType)
        {
            StatusManager status =new StatusManager( new StatusDal(UserInfo));
            return Json(new { Getlist = status.GetAll().Where(w=>w.STATUS_ID == StatusType).ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateStatus(STATUS param)
        {
            StatusManager status = new StatusManager(new StatusDal(UserInfo));
            var control = status.AddorUpdate(param);
            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveStatus(STATUS param)
        {
            StatusManager status = new StatusManager(new StatusDal(UserInfo));
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
            RoleMenuManager roleMenu = new RoleMenuManager(new RoleMenuDal(UserInfo));

            return Json(new { Getlist = roleMenu.GetAll().Distinct().ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveMenuRoles(int RoleID,List<MenuRolesDto> Menus)
        {
            ROLE_MENU chekmenus = new ROLE_MENU();
            chekmenus.ROLE_ID = RoleID;
            chekmenus.MENUS_STR = JsonConvert.SerializeObject(Menus);

            RoleMenuManager roleMenu = new RoleMenuManager(new RoleMenuDal(UserInfo));
            var control = roleMenu.AddorUpdate(chekmenus);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        public JsonResult GetLoginEmpRolesMenus()
        {
            RoleMenuManager roleMenu = new RoleMenuManager(new RoleMenuDal(UserInfo));            
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
                FloorTypeManager floorType = new FloorTypeManager(new FloorTypeDal(UserInfo));
                return Json(new { Getlist = floorType.GetAll() }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 2)
            {
                MateryalManager materyal = new MateryalManager(new MateryalDal(UserInfo));
                return Json(new { Getlist = materyal.GetAll() }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 3)
            {
                ProjectTypeManager projectType = new ProjectTypeManager(new ProjectTypeDal(UserInfo));
                return Json(new { Getlist = projectType.GetAll() }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 4)
            {
                var data = (from vb in db.EMPLOYEES
                            where vb.IS_CANCELED == false && vb.IS_SUPERVISOR == true
                            select new
                            {
                                EMP_ID = vb.EMP_ID,
                                EMP_NAME = vb.FIRST_NAME+SqlConstants.stringWhiteSpace+vb.LAST_NAME,
                            }
                ).OrderBy(o => o.EMP_NAME).ToList();   

                return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 5)
            {
                NoteTypeManager noteType = new NoteTypeManager(new NoteTypeDal(UserInfo));
                return Json(new { Getlist = noteType.GetAll() }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 6)
            {
                DocumentTypeManager documentType = new DocumentTypeManager(new DocumentTypeDal(UserInfo));
                return Json(new { Getlist = documentType.GetAll() }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Getlist = false }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateFloorType(FLOOR_TYPES param)
        {
            FloorTypeManager floorType = new FloorTypeManager(new FloorTypeDal(UserInfo));
            var control = floorType.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveFloorType(FLOOR_TYPES param)
        {
            FloorTypeManager floorType = new FloorTypeManager(new FloorTypeDal(UserInfo));
            var control = floorType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateMaterial(MATERIALS param)
        {
            MateryalManager materyal = new MateryalManager(new MateryalDal(UserInfo));
            var control = materyal.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveMaterial(MATERIALS param)
        {
            MateryalManager materyal = new MateryalManager(new MateryalDal(UserInfo));
            var control = materyal.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateProjectType(PROJECT_TYPES param)
        {
            ProjectTypeManager projectType = new ProjectTypeManager(new ProjectTypeDal(UserInfo));
            var control = projectType.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveProjectType(PROJECT_TYPES param)
        {
            ProjectTypeManager projectType = new ProjectTypeManager(new ProjectTypeDal(UserInfo));
            var control = projectType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }


        [HttpPost]
        public JsonResult AddOrUpdateNoteType(NOTE_TYPES param)
        {
            NoteTypeManager noteType = new NoteTypeManager(new NoteTypeDal(UserInfo));
            var control = noteType.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveNoteType(NOTE_TYPES param)
        {
            NoteTypeManager noteType = new NoteTypeManager(new NoteTypeDal(UserInfo));
            var control = noteType.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateDocumentType(DOCUMENT_TYPES param)
        {
            DocumentTypeManager documentType = new DocumentTypeManager(new DocumentTypeDal(UserInfo));
            var control = documentType.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }


        [HttpPost]
        public JsonResult RemoveDocumentType(DOCUMENT_TYPES param)
        {            
            DocumentTypeManager documentType = new DocumentTypeManager(new DocumentTypeDal(UserInfo));
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
            PhaseManager phase = new PhaseManager(new PhaseDal(UserInfo));
            StatusManager status = new StatusManager(new StatusDal(UserInfo));
            EmployeeManager employee = new EmployeeManager(new EmployeeDal(UserInfo));
            
            return Json(new { Getlist = phase.GetProjectTypeWithPhaseTasks(), 
                              ProjectStatus = status.GetAllByType((int)StatusType.Project), 
                              Employee = employee.GetAllByCompany(UserInfo.CompanyId) }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddOrUpdatePhases(DEF_PROJECT_PHASES param)
        {
            PhaseManager phase = new PhaseManager(new PhaseDal(UserInfo));
            var control = phase.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }


        [HttpPost]
        public JsonResult RemovePhases(DEF_PROJECT_PHASES param)
        {
            PhaseManager phase = new PhaseManager(new PhaseDal(UserInfo));
            var control = phase.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult AddOrUpdateTask(DEF_TASKS param)
        {
            TaskManager task = new TaskManager(new TaskDal(UserInfo));
            var control = task.AddorUpdate(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }

        [HttpPost]
        public JsonResult RemoveTask(DEF_TASKS param)
        {
            TaskManager task = new TaskManager(new TaskDal(UserInfo));
            var control = task.Delete(param);

            return Json(new { success = !control.isError, Message = control.ErrorMessage });
        }
        #endregion
    }
}
