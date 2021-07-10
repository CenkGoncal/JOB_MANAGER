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
               var data = (from vb in db.MATERIALS

                            join e in db.EMPLOYEES
                            on vb.CREATED_BY equals e.EMP_ID into e_join
                            from e_left in e_join.DefaultIfEmpty()

                            select new
                            {
                                MATERIAL_ID = vb.MATERIAL_ID,
                                MATERIAL_NAME = vb.MATERIAL_NAME,
                                IS_CANCELED = vb.IS_CANCELED,
                                CREATION_DATE = vb.CREATION_DATE != null ?
                                                        vb.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                        (vb.CREATION_DATE.Month > 9 ? vb.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + vb.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                        vb.CREATION_DATE.Day : null,
                                MODIFIED_DATE = vb.MODIFIED_DATE != null ?
                                                        vb.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                        (vb.MODIFIED_DATE.Month > 9 ? vb.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + vb.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                        vb.MODIFIED_DATE.Day : null,
                                CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                            }
                        ).OrderBy(o => o.MATERIAL_NAME).ToList();

                return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 3)
            {
                    var data = (from vb in db.PROJECT_TYPES

                                join e in db.EMPLOYEES
                                on vb.CREATED_BY equals e.EMP_ID into e_join
                                from e_left in e_join.DefaultIfEmpty()

                                select new
                                {
                                    PROJECT_TYPE_ID = vb.PROJECT_TYPE_ID,
                                    PROJECT_TYPE_NAME = vb.PROJECT_TYPE_NAME,
                                    PROJECT_TYPE_DESC = vb.PROJECT_TYPE_DESC,
                                    DEFAULT_SUPERVISOR = vb.DEFAULT_SUPERVISOR,                                    
                                    IS_CANCELED = vb.IS_CANCELED,
                                    CREATION_DATE = vb.CREATION_DATE != null ?
                                                            vb.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                            (vb.CREATION_DATE.Month > 9 ? vb.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + vb.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                            vb.CREATION_DATE.Day : null,
                                    MODIFIED_DATE = vb.MODIFIED_DATE != null ?
                                                            vb.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                            (vb.MODIFIED_DATE.Month > 9 ? vb.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + vb.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                            vb.MODIFIED_DATE.Day : null,
                                    CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                                }
                ).OrderBy(o => o.PROJECT_TYPE_NAME).ToList();

                return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
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
                var data = (from N in db.NOTE_TYPES

                            join e in db.EMPLOYEES
                            on N.CREATED_BY equals e.EMP_ID into e_join
                            from e_left in e_join.DefaultIfEmpty()

                            select new
                            {
                                NOTE_TYPE_ID = N.NOTE_TYPE_ID,
                                NOTE_TYPE_NAME = N.NOTE_TYPE_NAME,
                                IS_CANCELED = N.IS_CANCELED,
                                CREATION_DATE = N.CREATION_DATE != null ?
                                                            N.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                            (N.CREATION_DATE.Month > 9 ? N.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + N.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                            N.CREATION_DATE.Day : null,
                                MODIFIED_DATE = N.MODIFIED_DATE != null ?
                                                            N.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                            (N.MODIFIED_DATE.Month > 9 ? N.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + N.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                            N.MODIFIED_DATE.Day : null,
                                CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME

                            }
                ).OrderBy(o => o.NOTE_TYPE_NAME).ToList();

                return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
            }
            else
            if (Type == 6)
            {
                var data = (from D in db.DOCUMENT_TYPES

                            join e in db.EMPLOYEES
                            on D.CREATED_BY equals e.EMP_ID into e_join
                            from e_left in e_join.DefaultIfEmpty()

                            where D.IS_CANCELED == false
                            select new
                            {
                                DOCUMENT_TYPE_ID = D.DOCUMENT_TYPE_ID,
                                DOCUMENT_TYPE_NAME = D.DOCUMENT_TYPE_NAME,
                                DOCUMENT_EXTENSION = D.DOCUMENT_EXTENSION,
                                FONT_AWESOME_ICON = D.FONT_AWESOME_ICON,
                                IS_CANCELED = D.IS_CANCELED,
                                CREATION_DATE = D.CREATION_DATE != null ?
                                                            D.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                            (D.CREATION_DATE.Month > 9 ? D.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + D.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                            D.CREATION_DATE.Day : null,
                                MODIFIED_DATE = D.MODIFIED_DATE != null ?
                                                            D.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                            (D.MODIFIED_DATE.Month > 9 ? D.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + D.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                            D.MODIFIED_DATE.Day : null,
                                CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME

                            }
                ).OrderBy(o => o.DOCUMENT_TYPE_NAME).ToList();

                return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
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
            bool _success = false;
            string _message = string.Empty;

            try
            {
                FLOOR_TYPES control = db.FLOOR_TYPES.Where(w => w.FLOOR_TYPE_ID == param.FLOOR_TYPE_ID).FirstOrDefault();
                var checkModel = db.QUOTES.Where(w => w.FLOOR_TYPE == param.FLOOR_TYPE_ID).FirstOrDefault();
                if (checkModel != null)
                {
                    _message = "Quante is available to use floor Type!! Please try to check cancelled";
                    _success = false;
                }
                if (control != null)
                {
                    db.FLOOR_TYPES.Remove(control);
                    db.SaveChanges();

                    _message = "Operation Successful";
                    _success = true;
                }
                else
                {
                    _message = "Vehicle Body Type not found!!";
                    _success = false;
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult AddOrUpdateMaterial(MATERIALS param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                MATERIALS control = db.MATERIALS.Where(w => w.MATERIAL_ID == param.MATERIAL_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                    if (db.MATERIALS.Any(x => x.MATERIAL_NAME.Equals(param.MATERIAL_NAME) && x.IS_CANCELED == false))
                    {
                        _message = "Material already exists.";
                        _success = false;
                    }

                }
                else
                {
                    if (db.MATERIALS.Any(x => x.MATERIAL_NAME.Equals(param.MATERIAL_NAME) && x.MATERIAL_ID != param.MATERIAL_ID && x.IS_CANCELED == false))
                    {
                        _message = "Material already exists.";
                        _success = false;
                    }
                }

                if (_success == false)
                    return Json(new { success = _success, Message = _message });

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();

                    db.MATERIALS.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.MATERIAL_NAME = param.MATERIAL_NAME;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();
                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;
                    db.MATERIALS.Attach(control);
                    db.Entry(control).Property(x => x.MATERIAL_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();
                }

                _success = true;
                _message = "Operation Successful";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult RemoveMaterial(MATERIALS param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {

                MATERIALS control = db.MATERIALS.Where(w => w.MATERIAL_ID == param.MATERIAL_ID).FirstOrDefault();
                var checkModel = db.QUOTES.Where(w => w.MATERIAL_ID == param.MATERIAL_ID).FirstOrDefault();
                if (checkModel != null)
                {
                    _message = "Quante is available to use material!! Please try to check cancelled";
                    _success = false;
                }
                else
                if (control != null)
                {
                    db.MATERIALS.Remove(control);
                    db.SaveChanges();

                    _message = "Operation Successful";
                    _success = true;
                }
                else
                {
                    _message = "Vehicle Body Type not found!!";
                    _success = false;
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult AddOrUpdateProjectType(PROJECT_TYPES param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                PROJECT_TYPES control = db.PROJECT_TYPES.Where(w => w.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;

                    if (db.PROJECT_TYPES.Any(x => x.PROJECT_TYPE_NAME.Equals(param.PROJECT_TYPE_NAME) && x.IS_CANCELED == false))
                    {
                        _message = "Type already exists.";
                        _success = false;
                    }

                    if (param.IS_DEFAULT)
                    {
                        if (db.PROJECT_TYPES.Any(x => x.IS_DEFAULT == true))
                        {
                            _message = "Only one type can be set as default";
                            _success = false;
                        }
                    }
                }
                else
                {
                    if (db.PROJECT_TYPES.Any(x => x.PROJECT_TYPE_NAME.Equals(param.PROJECT_TYPE_NAME) && x.PROJECT_TYPE_ID != param.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Type already exists.";
                        _success = false;
                    }

                    if (param.IS_DEFAULT)
                    {
                        if (db.PROJECT_TYPES.Any(x => x.IS_DEFAULT == true && x.PROJECT_TYPE_ID != param.PROJECT_TYPE_ID))
                        {
                            _message = "Only one type can be set as default";
                            _success = false;
                        }
                    }
                }

                if(_success == false)
                        return Json(new { success = _success, Message = _message });

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();

                    db.PROJECT_TYPES.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.PROJECT_TYPE_NAME = param.PROJECT_TYPE_NAME;
                    control.PROJECT_TYPE_DESC = param.PROJECT_TYPE_DESC;
                    control.IS_DEFAULT = param.IS_DEFAULT;
                    control.DEFAULT_SUPERVISOR = param.DEFAULT_SUPERVISOR;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();
                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;
                    db.PROJECT_TYPES.Attach(control);
                    db.Entry(control).Property(x => x.PROJECT_TYPE_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TYPE_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.IS_DEFAULT).IsModified = true;
                    db.Entry(control).Property(x => x.DEFAULT_SUPERVISOR).IsModified = true;
                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();
                }

                _success = true;
                _message = "Operation Successful";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult RemoveProjectType(PROJECT_TYPES param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {

                PROJECT_TYPES control = db.PROJECT_TYPES.Where(w => w.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID).FirstOrDefault();
                var checkModel = db.QUOTES.Where(w => w.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID).FirstOrDefault();
                if (checkModel != null)
                {
                    _message = "Quonte is available to use Project Type!! Please try to check cancelled";
                    _success = false;
                }
                else
                if (control != null)
                {
                    db.PROJECT_TYPES.Remove(control);
                    db.SaveChanges();

                    _message = "Operation Successful";
                    _success = true;
                }
                else
                {
                    _message = "Vehicle Body Type not found!!";
                    _success = false;
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }


        [HttpPost]
        public JsonResult AddOrUpdateNoteType(NOTE_TYPES param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                NOTE_TYPES control = db.NOTE_TYPES.Where(w => w.NOTE_TYPE_ID == param.NOTE_TYPE_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;

                    if (db.NOTE_TYPES.Any(x => x.NOTE_TYPE_NAME.Equals(param.NOTE_TYPE_NAME) && x.IS_CANCELED == false))
                    {
                        _message = "Type already exists.";
                        _success = false;
                    }

                }
                else
                {
                    if (db.NOTE_TYPES.Any(x => x.NOTE_TYPE_NAME.Equals(param.NOTE_TYPE_NAME) && x.NOTE_TYPE_ID != param.NOTE_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Type already exists.";
                        _success = false;
                    }
                }

                if(_success == false)
                    return Json(new { success = _success, Message = _message });

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();

                    db.NOTE_TYPES.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.NOTE_TYPE_NAME = param.NOTE_TYPE_NAME;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();
                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;
                    db.NOTE_TYPES.Attach(control);
                    db.Entry(control).Property(x => x.NOTE_TYPE_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();
                }

                _success = true;
                _message = "Operation Successful";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult RemoveNoteType(NOTE_TYPES param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {

                NOTE_TYPES control = db.NOTE_TYPES.Where(w => w.NOTE_TYPE_ID == param.NOTE_TYPE_ID).FirstOrDefault();
                var checkModel = db.NOTES.Where(w => w.NOTE_TYPE_ID == param.NOTE_TYPE_ID).FirstOrDefault();
                if (checkModel != null)
                {
                    _message = "Quonte is available to use Note Type!! Please try to check cancelled";
                    _success = false;
                }
                else
                if (control != null)
                {
                    db.NOTE_TYPES.Remove(control);
                    db.SaveChanges();

                    _message = "Operation Successful";
                    _success = true;
                }
                else
                {
                    _message = "Vehicle Body Type not found!!";
                    _success = false;
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult AddOrUpdateDocumentType(DOCUMENT_TYPES param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                DOCUMENT_TYPES control = db.DOCUMENT_TYPES.Where(w => w.DOCUMENT_TYPE_ID == param.DOCUMENT_TYPE_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                    if (db.DOCUMENT_TYPES.Any(x => x.DOCUMENT_TYPE_NAME.Equals(param.DOCUMENT_TYPE_NAME) && x.IS_CANCELED == false))
                    {
                        _message = "Type already exists.";
                        _success = false;
                    }

                    if (db.DOCUMENT_TYPES.Any(x => x.DOCUMENT_EXTENSION.Equals(param.DOCUMENT_EXTENSION) && x.IS_CANCELED == false))
                    {
                        _message = "Doc ext. already exists.";
                        _success = false;
                    }
                }
                else
                {
                    if (db.DOCUMENT_TYPES.Any(x => x.DOCUMENT_TYPE_NAME.Equals(param.DOCUMENT_TYPE_NAME) && x.DOCUMENT_TYPE_ID != param.DOCUMENT_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Type already exists.";
                        _success = false;
                    }

                    if (db.DOCUMENT_TYPES.Any(x => x.DOCUMENT_EXTENSION.Equals(param.DOCUMENT_EXTENSION) && x.DOCUMENT_TYPE_ID != param.DOCUMENT_TYPE_ID  && x.IS_CANCELED == false))
                    {
                        _message = "Doc ext. already exists.";
                        _success = false;
                    }
                }

                if(_success == false)
                {
                    return Json(new { success = _success, Message = _message });
                }

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();

                    db.DOCUMENT_TYPES.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.DOCUMENT_TYPE_NAME = param.DOCUMENT_TYPE_NAME;
                    control.DOCUMENT_EXTENSION = param.DOCUMENT_EXTENSION;
                    control.FONT_AWESOME_ICON = param.FONT_AWESOME_ICON;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();
                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;
                    db.DOCUMENT_TYPES.Attach(control);
                    db.Entry(control).Property(x => x.DOCUMENT_TYPE_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.DOCUMENT_EXTENSION).IsModified = true;
                    db.Entry(control).Property(x => x.FONT_AWESOME_ICON).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();
                }

                _success = true;
                _message = "Operation Successful";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }


        [HttpPost]
        public JsonResult RemoveDocumentType(DOCUMENT_TYPES param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {                
                var checkModel = db.DOCUMENTS.Where(w => w.DOCUMENT_TYPE_ID == param.DOCUMENT_TYPE_ID).FirstOrDefault();
                var control = db.DOCUMENT_TYPES.Where(w => w.DOCUMENT_TYPE_ID == param.DOCUMENT_TYPE_ID).FirstOrDefault();
                if (checkModel != null)
                {
                    _message = "Quonte or Project are available to use Document Type!! Please try to check cancelled";
                    _success = false;
                }
                else
                if (control != null)
                {
                    db.DOCUMENT_TYPES.Remove(control);
                    db.SaveChanges();

                    _message = "Operation Successful";
                    _success = true;
                }
                else
                {
                    _message = "Document Type not found!!";
                    _success = false;
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
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
            var ProjectTypes = db.PROJECT_TYPES.ToList();
            var ProjectStatus = (from S in db.STATUS
                                 where S.IS_CANCELED == false && S.STATUS_TYPE == (int)StatusType.Project
                                 select new
                                 {
                                     STATUS_ID = S.STATUS_ID,
                                     STATUS_NAME = S.STATUS_NAME
                                 }).ToList();
            int CompanyID = GetCompanyID();
            var Employee = (from E in db.EMPLOYEES
                                 where E.IS_CANCELED == false && E.COMPANY_ID == CompanyID
                            select new
                                 {
                                     EMP_ID = E.EMP_ID,
                                     EMP_NAME = E.FIRST_NAME+ SqlConstants.stringWhiteSpace+E.LAST_NAME
                                 }).ToList();

            List<ProjectTypeWithPhaseTask> PhaseList = new List<ProjectTypeWithPhaseTask>();

            foreach (var item in ProjectTypes)
            {
                ProjectTypeWithPhaseTask Phase = new ProjectTypeWithPhaseTask();

                Phase.PROJECT_TYPE_ID = item.PROJECT_TYPE_ID;
                Phase.PROJECT_TYPE_NAME = item.PROJECT_TYPE_NAME;
                Phase.PHASE = (from PH in db.DEF_PROJECT_PHASES
                               from E in db.EMPLOYEES.Where(w =>w.EMP_ID == PH.CREATED_BY).DefaultIfEmpty()                            
                               where PH.PROJECT_TYPE_ID == item.PROJECT_TYPE_ID
                               select new DefPhasesDto
                               {
                                   PHASE_ID = PH.PHASE_ID,
                                   PHASE_NAME = PH.PHASE_NAME,
                                   PROJECT_TYPE_ID = PH.PROJECT_TYPE_ID,
                                   PHASE_ORDER = PH.PHASE_ORDER,
                                   PHASE_DESC = PH.PHASE_DESC,
                                   PHASE_COLOR = PH.PHASE_COLOR,
                                   WIDGET_COLOR = PH.WIDGET_COLOR,
                                   CORRESPONDANT_STATUS = PH.CORRESPONDANT_STATUS,
                                   IS_CANCELED = PH.IS_CANCELED,
                                   CREATION_DATE = PH.CREATION_DATE != null ?
                                                               PH.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                               (PH.CREATION_DATE.Month > 9 ? PH.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + PH.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                               PH.CREATION_DATE.Day : null,
                                   MODIFIED_DATE = PH.MODIFIED_DATE != null ?
                                                               PH.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                               (PH.MODIFIED_DATE.Month > 9 ? PH.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + PH.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                               PH.MODIFIED_DATE.Day : null,
                                   CREATE_BY = E.FIRST_NAME + SqlConstants.stringWhiteSpace + E.LAST_NAME
                               }
                              ).OrderBy(o => o.PHASE_ORDER).ToList();

                foreach(var phase in Phase.PHASE)
                {
                    phase.TASK = (from T in db.DEF_TASKS
                                  from E in db.EMPLOYEES.Where(w => w.EMP_ID == T.CREATED_BY).DefaultIfEmpty()
                                  where T.PROJECT_TYPE_ID == item.PROJECT_TYPE_ID && T.PHASE_ID == phase.PHASE_ID
                               select new DefTaskDto
                               {
                                   TASK_ID = T.TASK_ID,
                                   TASK_NAME = T.TASK_NAME,
                                   TASK_DESC = T.TASK_DESC,
                                   PROJECT_TYPE_ID = T.PROJECT_TYPE_ID,
                                   PHASE_ID = T.PHASE_ID,
                                   TASK_ORDER = T.TASK_ORDER,
                                   ALLOCATE_SUPERVISOR = T.ALLOCATE_SUPERVISOR,
                                   COMPLETE_ON_UPLOAD = T.COMPLETE_ON_UPLOAD,
                                   IS_CANCELED = T.IS_CANCELED,
                                   CREATION_DATE = T.CREATION_DATE != null ?
                                                               T.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                               (T.CREATION_DATE.Month > 9 ? T.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + T.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                               T.CREATION_DATE.Day : null,
                                   MODIFIED_DATE = T.MODIFIED_DATE != null ?
                                                               T.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                               (T.MODIFIED_DATE.Month > 9 ? T.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + T.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                               T.MODIFIED_DATE.Day : null,
                                   CREATE_BY = E.FIRST_NAME + SqlConstants.stringWhiteSpace + E.LAST_NAME
                               }
                          ).OrderBy(o => o.TASK_NAME).ToList();
                }

                PhaseList.Add(Phase);
            }

            return Json(new { Getlist = PhaseList, ProjectStatus = ProjectStatus, Employee = Employee }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddOrUpdatePhases(DEF_PROJECT_PHASES param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                DEF_PROJECT_PHASES control = db.DEF_PROJECT_PHASES.Where(w => w.PHASE_ID == param.PHASE_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;

                    if (db.DEF_PROJECT_PHASES.Any(x => x.PHASE_NAME.Equals(param.PHASE_NAME) && x.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && x.IS_CANCELED == false ))
                    {
                        _message ="Phase already exists.";
                        _success = false;
                    }
                    if (db.DEF_PROJECT_PHASES.Any(x => x.PHASE_ORDER == param.PHASE_ORDER && x.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && x.IS_CANCELED == false ))
                    {
                        _message = "Order already exists.";
                        _success = false;
                    }
                }
                else
                {
                    if (db.DEF_PROJECT_PHASES.Any(x => x.PHASE_NAME.Equals(param.PHASE_NAME) && x.PHASE_ID != param.PHASE_ID && x.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Phase already exists.";
                        _success = false;
                    }
                    if (db.DEF_PROJECT_PHASES.Any(x => x.PHASE_ORDER == param.PHASE_ORDER && x.PHASE_ID != param.PHASE_ID  && x.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Order already exists.";
                        _success = false;
                    }
                }

                if(_success == false)
                {
                    return Json(new { success = _success, Message = _message });
                }

                param.CORRESPONDANT_STATUS = param.CORRESPONDANT_STATUS == 0 ? null : param.CORRESPONDANT_STATUS;

                if (isNew)
                {
                    int ORDER = db.DEF_PROJECT_PHASES.Where(w => w.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID).Select(s => s.PHASE_ID).ToList().Count() + 1;
                    param.PHASE_ORDER = (short)ORDER;
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();

                    db.DEF_PROJECT_PHASES.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.PHASE_NAME = param.PHASE_NAME;
                    control.PHASE_ORDER = param.PHASE_ORDER;
                    control.PHASE_COLOR = param.PHASE_COLOR;
                    control.PROJECT_TYPE_ID = param.PROJECT_TYPE_ID;
                    control.WIDGET_COLOR = param.WIDGET_COLOR;
                    control.CORRESPONDANT_STATUS = param.CORRESPONDANT_STATUS;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();
                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;
                    db.DEF_PROJECT_PHASES.Attach(control);
                    db.Entry(control).Property(x => x.PHASE_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.PHASE_ORDER).IsModified = true;
                    db.Entry(control).Property(x => x.PHASE_COLOR).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TYPE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.WIDGET_COLOR).IsModified = true;
                    db.Entry(control).Property(x => x.CORRESPONDANT_STATUS).IsModified = true;
                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();
                }

                _success = true;
                _message = "Operation Successful";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }


        [HttpPost]
        public JsonResult RemovePhases(DEF_PROJECT_PHASES param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {
                DEF_PROJECT_PHASES control = db.DEF_PROJECT_PHASES.Where(w => w.PHASE_ID == param.PHASE_ID).FirstOrDefault();
                //var checkModel = db.P.Where(w => w.FLOOR_TYPE == param.FLOOR_TYPE_ID).FirstOrDefault();
                //if (checkModel != null)
                //{
                //    _message = "Quante is available to use floor Type!! Please try to check cancelled";
                //    _success = false;
                //}
                if (control != null)
                {
                    db.DEF_PROJECT_PHASES.Remove(control);
                    db.SaveChanges();

                    _message = "Operation Successful";
                    _success = true;
                }
                else
                {
                    _message = "Vehicle Body Type not found!!";
                    _success = false;
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult AddOrUpdateTask(DEF_TASKS param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                DEF_TASKS control = db.DEF_TASKS.Where(w => w.TASK_ID == param.TASK_ID).FirstOrDefault();

                bool isNew = false;
                if (control == null)
                {
                    isNew = true;
                    if (db.DEF_TASKS.Any(x => x.TASK_NAME.Equals(param.TASK_NAME)  && x.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Task already exists.";
                        _success = false;
                    }
                    if (db.DEF_TASKS.Any(x => x.TASK_ORDER == param.TASK_ORDER && x.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Order already exists.";
                        _success = false;
                    }
                }
                else
                {
                    if (db.DEF_TASKS.Any(x => x.TASK_NAME.Equals(param.TASK_NAME) && x.TASK_ID != param.TASK_ID && x.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Task already exists.";
                        _success = false;
                    }
                    if (db.DEF_TASKS.Any(x => x.TASK_ORDER == param.TASK_ORDER && x.TASK_ID != param.TASK_ID && x.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && x.IS_CANCELED == false))
                    {
                        _message = "Order already exists.";
                        _success = false;
                    }
                }

                if (_success == false)
                {
                    return Json(new { success = _success, Message = _message });
                }

                if (isNew)
                {
                    int ORDER = db.DEF_TASKS.Where(w => w.PROJECT_TYPE_ID == param.PROJECT_TYPE_ID && w.PHASE_ID == param.PHASE_ID).Select(s => s.PHASE_ID).ToList().Count() + 1;
                    param.TASK_ORDER = (short)ORDER;
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();

                    db.DEF_TASKS.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.TASK_NAME = param.TASK_NAME;
                    control.TASK_DESC = param.TASK_DESC;
                    control.TASK_ORDER = param.TASK_ORDER;
                    control.PROJECT_TYPE_ID = param.PROJECT_TYPE_ID;
                    control.PHASE_ID = param.PHASE_ID;
                    control.ALLOCATE_SUPERVISOR = param.ALLOCATE_SUPERVISOR;
                    control.COMPLETE_ON_UPLOAD = param.COMPLETE_ON_UPLOAD;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();
                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;
                    db.DEF_TASKS.Attach(control);
                    db.Entry(control).Property(x => x.TASK_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.TASK_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.TASK_ORDER).IsModified = true;
                    db.Entry(control).Property(x => x.PROJECT_TYPE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.PHASE_ID).IsModified = true;
                    db.Entry(control).Property(x => x.ALLOCATE_SUPERVISOR).IsModified = true;
                    db.Entry(control).Property(x => x.COMPLETE_ON_UPLOAD).IsModified = true;
                    db.Entry(control).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(control).Property(x => x.UPDATED_BY).IsModified = true;
                    //db.Entry(ContractType).Property(x => x.MODIFIED_DATE).IsModified = true;
                    db.SaveChanges();
                }

                _success = true;
                _message = "Operation Successful";
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }

        [HttpPost]
        public JsonResult RemoveTask(DEF_TASKS param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {
                DEF_TASKS control = db.DEF_TASKS.Where(w => w.TASK_ID == param.TASK_ID).FirstOrDefault();
                //var checkModel = db.P.Where(w => w.FLOOR_TYPE == param.FLOOR_TYPE_ID).FirstOrDefault();
                //if (checkModel != null)
                //{
                //    _message = "Quante is available to use floor Type!! Please try to check cancelled";
                //    _success = false;
                //}
                if (control != null)
                {
                    db.DEF_TASKS.Remove(control);
                    db.SaveChanges();

                    _message = "Operation Successful";
                    _success = true;
                }
                else
                {
                    _message = "Vehicle Body Type not found!!";
                    _success = false;
                }
            }
            catch (Exception e)
            {
                _message = e.Message;
                _success = false;
            }

            return Json(new { success = _success, Message = _message });
        }
        #endregion
    }
}
