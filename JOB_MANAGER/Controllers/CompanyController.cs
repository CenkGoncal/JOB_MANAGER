using JOB_MANAGER.Helper;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Login;
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
            var CompanyID = GetCompanyID();
            ViewBag.COUNTRIES_LIST = new SelectList(db.COUNTRIES.Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
            ViewBag.STATES_LIST = new SelectList(db.STATES.Where(w => w.IS_CANCELED == false), "STATE_ID", "STATE_NAME");
            ViewBag.CITIES_LIST = new SelectList(db.CITIES.Where(w => w.IS_CANCELED == false ), "CITY_ID", "CITY_NAME");
            ViewBag.COMPANY_TYPE_LIST = new SelectList(db.COMPANY_TYPES.Where(w => w.IS_CANCELED == false), "COMPANY_TYPE_ID", "COMPANY_TYPE_NAME");                        

            return View();
        }

        public JsonResult GetCompany()
        {
            int CompanyID = GetCompanyID();

            var data = (from c in db.COMPANY
                        where c.IS_CANCELED == false && c.COMPANY_ID == CompanyID
                        select new
                        {
                            COMPANY_ID = c.COMPANY_ID,
                            COMPANY_CODE = c.COMPANY_CODE,
                            COMPANY_NAME = c.COMPANY_NAME,
                            COMPANY_TYPE_ID = c.COMPANY_TYPE_ID,
                            COMPANY_COUNTRY = c.COMPANY_COUNTRY,
                            COMPANY_STATE = c.COMPANY_STATE,
                            COMPANY_CITY = c.COMPANY_CITY,
                            COMPANY_ADDRESS = c.COMPANY_ADDRESS,
                            POSTAL_CODE = c.POSTAL_CODE,
                            WEB_URL = c.WEB_URL,
                            COMPANY_ABN = c.COMPANY_ABN,
                            COMPANY_PHONE = c.COMPANY_PHONE,
                            COMPANY_OWNER = c.COMPANY_OWNER,
                            COMPANY_LINKED_IN = c.COMPANY_LINKED_IN,
                            COMPANY_LOGO = c.COMPANY_LOGO,
                            COMPANY_DESC = c.COMPANY_DESC
                        }
                       ).OrderBy(o => o.COMPANY_NAME).FirstOrDefault();       

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateCompany(COMPANY param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {
                COMPANY cmpny = db.COMPANY.Where(w => w.COMPANY_ID == param.COMPANY_ID).FirstOrDefault();

                bool isNew = false;
                if (cmpny == null)
                {
                    isNew = true;
                }

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = GetCompanyID();                    

                    db.COMPANY.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    cmpny.MODIFIED_DATE = DateTime.Now;
                    cmpny.COMPANY_CODE = param.COMPANY_CODE;
                    cmpny.COMPANY_NAME = param.COMPANY_NAME;
                    cmpny.COMPANY_TYPE_ID = param.COMPANY_TYPE_ID;
                    cmpny.COMPANY_COUNTRY = param.COMPANY_COUNTRY;
                    cmpny.COMPANY_STATE = param.COMPANY_STATE;
                    cmpny.COMPANY_CITY = param.COMPANY_CITY;
                    cmpny.COMPANY_ADDRESS = param.COMPANY_ADDRESS;
                    cmpny.POSTAL_CODE = param.POSTAL_CODE;
                    cmpny.WEB_URL = param.WEB_URL;
                    cmpny.COMPANY_ABN = param.COMPANY_ABN;
                    cmpny.COMPANY_PHONE = param.COMPANY_PHONE;
                    cmpny.COMPANY_OWNER = param.COMPANY_OWNER;
                    cmpny.COMPANY_LINKED_IN = param.COMPANY_LINKED_IN;
                    cmpny.COMPANY_DESC = param.COMPANY_DESC;
                    cmpny.UPDATED_BY = GetUserID();
                    cmpny.IS_CANCELED = param.IS_CANCELED;
             
                    db.COMPANY.Attach(cmpny);
                    db.Entry(cmpny).Property(x => x.COMPANY_CODE).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_NAME).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_TYPE_ID).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_COUNTRY).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_STATE).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_CITY).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_ADDRESS).IsModified = true;
                    db.Entry(cmpny).Property(x => x.POSTAL_CODE).IsModified = true;
                    db.Entry(cmpny).Property(x => x.WEB_URL).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_ABN).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_PHONE).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_OWNER).IsModified = true;
                    db.Entry(cmpny).Property(x => x.UPDATED_BY).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_LINKED_IN).IsModified = true;
                    db.Entry(cmpny).Property(x => x.COMPANY_DESC).IsModified = true;
                    db.Entry(cmpny).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(cmpny).Property(x => x.UPDATED_BY).IsModified = true;
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
        public JsonResult GetStateByCompanyID(int CountryID)
        {
            var data = (from s in db.STATES
                        where s.COUNTRY_ID == CountryID
                        select new
                        {
                            STATE_ID = s.STATE_ID,
                            STATE_NAME = s.STATE_NAME
                        }).ToList();
                
                //db.STATES.Where(w => w.COUNTRY_ID == CountryID).Select(s => s.STATE_ID,).ToList();

            return Json(new { StateList = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetCityCompanyID(int StateID)
        {
            //var data = db.CITIES.Where(w => w.STATE_ID == StateID).ToList();

            var data = (from c in db.CITIES
                        where c.STATE_ID == StateID
                        select new
                        {
                            CITY_ID = c.CITY_ID,
                            CITY_NAME = c.CITY_NAME,
                            POSTAL_CODE = c.POSTAL_CODE
                        }).ToList();

            return Json(new { CityList = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateCompanyImg(int CompanyID, HttpPostedFileBase Image, int Remove)
        {
            COMPANY company = db.COMPANY.Find(CompanyID);
            if (company != null)
            {
                try
                {
                    string Message = string.Empty;
                    bool State = false;

                    if (Image != null)
                    {
                        company.COMPANY_LOGO = new byte[Image.ContentLength];
                        Image.InputStream.Read(company.COMPANY_LOGO, 0, Image.ContentLength);
                        //employee.EMPLOYEE_IMG = GlobalTools.ResizeEmployeeImage(employee.EMPLOYEE_IMG);

                        State = true;
                        Message = "Image Chamged";
                    }
                    else
                    {
                        company.COMPANY_LOGO = null;

                        State = Remove == 1;
                        Message = State ? "Image Chamged" : "No Image found";
                    }

                    db.COMPANY.Attach(company);
                    db.Entry(company).Property(x => x.COMPANY_LOGO).IsModified = true;
                    db.SaveChanges();

                    return Json(new { success = State, message = Message });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "No Employee found" });
        }

        #endregion

        #region Vehicles
        [AuthorityControl("Vehicles")]
        public ActionResult Vehicles()
        {
            var CompanyID = GetCompanyID();
            var empList = (from e in db.EMPLOYEES
                       where e.IS_CANCELED == false && e.IS_DRIVER == true
                       select new
                       {
                           EMP_ID = e.EMP_ID,
                           EMP_NAME = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME
                       }
                      ).ToList();

            ViewBag.DRIVER_LIST = new SelectList(empList, "EMP_ID", "EMP_NAME");
            ViewBag.VEHICLE_MODELS = new SelectList(db.VEHICLE_MODELS.Where(w => w.IS_CANCELED == false), "VEHICLE_MODEL_ID", "VEHICLE_MODEL_NAME");
            ViewBag.STATUS_LIST = new SelectList(db.STATUS.Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.Vehicle), "STATUS_ID", "STATUS_NAME");
            ViewBag.VEHICLE_MAKES = new SelectList(db.VEHICLE_MAKES.Where(w => w.IS_CANCELED == false), "VEHICLE_MAKE_ID", "VEHICLE_MAKE_NAME");
            ViewBag.VEHICLE_BODYS = new SelectList(db.VEHICLE_BODY_TYPES.Where(w => w.IS_CANCELED == false), "BODY_TYPE_ID", "BODY_TYPE_NAME");                       
            ViewBag.VEHICLE_YEARS = new SelectList(GlobalTools.GetYOMLookUp(CompanyID), "YOM_ID", "YOM_VALUE");

            return View();
        }

        public ActionResult GetVeciclesList()
        {
            var empID = GetUserID();

            if (empID < 0)
            {
                return RedirectToAction("LockedUser", "Login");
            }

            var CompanyID = GetCompanyID();

            var data = (from v in db.VEHICLES      
                        
                        where v.IS_CANCELED == false && v.COMPANY_ID == CompanyID
                        select new
                        {
                            VEHICLE_ID = v.VEHICLE_ID,
                            VEHICLE_NBR = v.VEHICLE_NBR,
                            VEHICLE_STATUS = v.VEHICLE_STATUS,
                            VEHICLE_STATUS_NAME = v.STATUS.STATUS_NAME,
                            DISPLAY_CLASS = v.STATUS.DISPLAY_CLASS,
                            CURRENT_DRIVER = v.EMPLOYEES.EMP_ID,
                            DRIVER_NAME = v.EMPLOYEES.FIRST_NAME+ SqlConstants.stringWhiteSpace+v.EMPLOYEES.LAST_NAME,
                            REGISTRATION_NUMBER = v.REGISTRATION_NUMBER,
                            REGISTRATION_EXPIRY = v.REGISTRATION_EXPIRY != null ?
                                                    v.REGISTRATION_EXPIRY.Value.Year + SqlConstants.stringMinus +
                                                    (v.REGISTRATION_EXPIRY.Value.Month > 9 ? v.REGISTRATION_EXPIRY.Value.Month + SqlConstants.stringMinus : "0"+ v.REGISTRATION_EXPIRY.Value.Month + SqlConstants.stringMinus) +
                                                    v.REGISTRATION_EXPIRY.Value.Day : null,
                            ASSIGNED_TAG = v.ASSIGNED_TAG,
                            VEHICLE_MAKE_ID = v.VEHICLE_MAKE_ID,
                            VEHICLE_MAKE_NAME = v.VEHICLE_MAKES.VEHICLE_MAKE_NAME,
                            BODY_TYPE_ID = v.BODY_TYPE_ID,
                            BODY_TYPE_NAME = v.VEHICLE_BODY_TYPES.BODY_TYPE_NAME,
                            VEHICLE_MODEL_ID = v.VEHICLE_MODEL_ID,
                            VEHICLE_MODEL_NAME = v.VEHICLE_MODELS.VEHICLE_MODEL_NAME,
                            VEHICLE_YEAR = v.VEHICLE_YEAR,
                            VEHICLE_COLOR = v.VEHICLE_COLOR,
                            VEHICLE_DESC = v.VEHICLE_DESC
                        }
                       ).OrderBy(o => o.VEHICLE_NBR).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetVecModelList(int VecMakesID,int VecBodysID)
        {
            //var data = db.CITIES.Where(w => w.STATE_ID == StateID).ToList();
            var data = db.VEHICLE_MODELS.Where(w => w.IS_CANCELED == false);

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
            bool _success = false;
            string _message = string.Empty;

            try
            {
                VEHICLES vechile = db.VEHICLES.Where(w => w.VEHICLE_ID == param.VEHICLE_ID).FirstOrDefault();

                bool isNew = false;
                if (vechile == null)
                {
                    isNew = true;
                }

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = GetCompanyID();

                    db.VEHICLES.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    vechile.MODIFIED_DATE = DateTime.Now;
                    vechile.VEHICLE_NBR = param.VEHICLE_NBR;
                    vechile.VEHICLE_STATUS = param.VEHICLE_STATUS;
                    vechile.VEHICLE_MODEL_ID = param.VEHICLE_MODEL_ID;
                    vechile.VEHICLE_MAKE_ID = param.VEHICLE_MAKE_ID;
                    vechile.BODY_TYPE_ID = param.BODY_TYPE_ID;
                    vechile.CURRENT_DRIVER = param.CURRENT_DRIVER;
                    vechile.REGISTRATION_NUMBER = param.REGISTRATION_NUMBER;
                    vechile.REGISTRATION_EXPIRY = param.REGISTRATION_EXPIRY;
                    vechile.ASSIGNED_TAG = param.ASSIGNED_TAG;
                    vechile.VEHICLE_DESC = param.VEHICLE_DESC;
                    vechile.VEHICLE_YEAR = param.VEHICLE_YEAR;
                    vechile.VEHICLE_COLOR = param.VEHICLE_COLOR;
                    vechile.UPDATED_BY = GetUserID();                    

                    db.VEHICLES.Attach(vechile);
                    db.Entry(vechile).Property(x => x.VEHICLE_NBR).IsModified = true;
                    db.Entry(vechile).Property(x => x.VEHICLE_STATUS).IsModified = true;
                    db.Entry(vechile).Property(x => x.VEHICLE_MODEL_ID).IsModified = true;
                    db.Entry(vechile).Property(x => x.VEHICLE_MAKE_ID).IsModified = true;
                    db.Entry(vechile).Property(x => x.BODY_TYPE_ID).IsModified = true;
                    db.Entry(vechile).Property(x => x.CURRENT_DRIVER).IsModified = true;
                    db.Entry(vechile).Property(x => x.REGISTRATION_NUMBER).IsModified = true;
                    db.Entry(vechile).Property(x => x.REGISTRATION_EXPIRY).IsModified = true;
                    db.Entry(vechile).Property(x => x.ASSIGNED_TAG).IsModified = true;
                    db.Entry(vechile).Property(x => x.VEHICLE_DESC).IsModified = true;
                    db.Entry(vechile).Property(x => x.VEHICLE_YEAR).IsModified = true;
                    db.Entry(vechile).Property(x => x.VEHICLE_COLOR).IsModified = true;
                    db.Entry(vechile).Property(x => x.UPDATED_BY).IsModified = true;
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
            ViewBag.COUNTRIES_LIST = new SelectList(db.COUNTRIES.Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
            ViewBag.STATES_LIST = new SelectList(db.STATES.Where(w => w.IS_CANCELED == false), "STATE_ID", "STATE_NAME");
            ViewBag.CITIES_LIST = new SelectList(db.CITIES.Where(w => w.IS_CANCELED == false), "CITY_ID", "CITY_NAME");
            ViewBag.STATUS_LIST = new SelectList(db.STATUS.Where(w => w.IS_CANCELED == false && w.STATUS_TYPE == (int)StatusType.Supplier), "STATUS_ID", "STATUS_NAME");

            return View();
        }

        [HttpGet]
        public ActionResult GetSupplierList()
        {
            var empID = GetUserID();
            if (empID < 0)
            {
                return RedirectToAction("LockedUser", "Login");
            }

            var CompanyID = GetCompanyID();

            var data = (from v in db.SUPPLIERS

                        where v.IS_CANCELED == false && v.COMPANY_ID == CompanyID
                        select new
                        {
                            SUPPLIER_ID = v.SUPPLIER_ID,
                            SUPPLIER_NAME = v.SUPPLIER_NAME,
                            SUPPLIER_NUMBER = v.SUPPLIER_NUMBER,
                            SUPPLIER_STATUS_ID = v.SUPPLIER_STATUS_ID,
                            SUPPLIER_STATUS_NAME = v.STATUS.STATUS_NAME,
                            DISPLAY_CLASS = v.STATUS.DISPLAY_CLASS,
                            SUPPLIER_URL = v.SUPPLIER_URL,
                            SUPPLIER_PHONE = v.SUPPLIER_PHONE,
                            SUPPLIER_EMAIL = v.SUPPLIER_EMAIL,
                            SUPPLIER_ADDRESS = v.SUPPLIER_ADDRESS,                            
                            SUPPLIER_COUNTRY = v.SUPPLIER_COUNTRY,
                            SUPPLIER_STATE = v.SUPPLIER_STATE,
                            SUPPLIER_CITY = v.SUPPLIER_CITY,
                            POSTAL_CODE = v.POSTAL_CODE,
                        }
                       ).OrderBy(o => o.SUPPLIER_NAME).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddOrUpdateSupplier(SUPPLIERS param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {
                SUPPLIERS suppliers = db.SUPPLIERS.Where(w => w.SUPPLIER_ID == param.SUPPLIER_ID).FirstOrDefault();

                bool isNew = false;
                if (suppliers == null)
                {
                    isNew = true;
                }

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = GetCompanyID();

                    db.SUPPLIERS.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    suppliers.MODIFIED_DATE = DateTime.Now;
                    suppliers.SUPPLIER_NAME = param.SUPPLIER_NAME;
                    suppliers.SUPPLIER_STATUS_ID = param.SUPPLIER_STATUS_ID;
                    suppliers.SUPPLIER_URL = param.SUPPLIER_URL;
                    suppliers.SUPPLIER_PHONE = param.SUPPLIER_PHONE;
                    suppliers.SUPPLIER_EMAIL = param.SUPPLIER_EMAIL;
                    suppliers.SUPPLIER_ADDRESS = param.SUPPLIER_ADDRESS;
                    suppliers.SUPPLIER_CITY = param.SUPPLIER_CITY;
                    suppliers.POSTAL_CODE = param.POSTAL_CODE;
                    suppliers.SUPPLIER_COUNTRY = param.SUPPLIER_COUNTRY;
                    suppliers.SUPPLIER_STATE = param.SUPPLIER_STATE;
                    suppliers.UPDATED_BY = GetUserID();

                    db.SUPPLIERS.Attach(suppliers);
                    db.Entry(suppliers).Property(x => x.SUPPLIER_NAME).IsModified = true;
                    db.Entry(suppliers).Property(x => x.SUPPLIER_STATUS_ID).IsModified = true;
                    db.Entry(suppliers).Property(x => x.SUPPLIER_URL).IsModified = true;
                    db.Entry(suppliers).Property(x => x.SUPPLIER_PHONE).IsModified = true;
                    db.Entry(suppliers).Property(x => x.SUPPLIER_EMAIL).IsModified = true;
                    db.Entry(suppliers).Property(x => x.SUPPLIER_ADDRESS).IsModified = true;
                    db.Entry(suppliers).Property(x => x.SUPPLIER_CITY).IsModified = true;
                    db.Entry(suppliers).Property(x => x.POSTAL_CODE).IsModified = true;
                    db.Entry(suppliers).Property(x => x.POSTAL_CODE).IsModified = true;
                    db.Entry(suppliers).Property(x => x.SUPPLIER_COUNTRY).IsModified = true;
                    db.Entry(suppliers).Property(x => x.SUPPLIER_STATE).IsModified = true;
                    db.Entry(suppliers).Property(x => x.UPDATED_BY).IsModified = true;
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
        public ActionResult DeleteSupplier(int SupplierID)
        {
            SUPPLIERS anSupplier = db.SUPPLIERS.Find(SupplierID);
            if (anSupplier != null)
            {
                try
                {
                    anSupplier.IS_CANCELED = true;
                    anSupplier.UPDATED_BY = GetUserID();
                    db.SUPPLIERS.Attach(anSupplier);

                    db.Entry(anSupplier).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anSupplier).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Supplier not found" });
        }
        #endregion

        #region Supervisor Areas
        [AuthorityControl("SupervisorAreas")]
        public ActionResult SupervisorAreas()
        {

            int CompanyID = GetCompanyID();
            ViewBag.SUPERVISOR_LIST = new SelectList(db.EMPLOYEES.Where(c => c.IS_SUPERVISOR == true && c.IS_CANCELED == false && c.COMPANY_ID == CompanyID).Select(e => new
            {
                e.EMP_ID,
                NAME = e.FIRST_NAME +SqlConstants.stringWhiteSpace+e.LAST_NAME
            }).OrderBy(e => e.EMP_ID), "EMP_ID", "NAME");

            int defaultCountry = db.COUNTRIES.Where(w => w.IS_DEFAULT == true).Select(s => s.COUNTRY_ID).FirstOrDefault();
            if (defaultCountry > 0)
            {
                ViewBag.COUNTRIES_LIST = new SelectList(db.COUNTRIES.Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "COUNTRY_ID", "COUNTRY_NAME");
                ViewBag.STATES_LIST = new SelectList(db.STATES.Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "STATE_ID", "STATE_NAME");
                ViewBag.CITIES_LIST = new SelectList(db.CITIES.Where(w => w.IS_CANCELED == false && w.COUNTRY_ID == defaultCountry), "CITY_ID", "CITY_NAME");
            }
            else
            {
                ViewBag.COUNTRIES_LIST = new SelectList(db.COUNTRIES.Where(w => w.IS_CANCELED == false), "COUNTRY_ID", "COUNTRY_NAME");
                ViewBag.STATES_LIST = new SelectList(db.STATES.Where(w => w.IS_CANCELED == false), "STATE_ID", "STATE_NAME");
                ViewBag.CITIES_LIST = new SelectList(db.CITIES.Where(w => w.IS_CANCELED == false), "CITY_ID", "CITY_NAME");
            }

            return View();
        }

        [HttpGet]
        public JsonResult GetListFilterSupervisors()
        {
            var dataCity = (from s in db.SUPERVISOR_AREAS

                        where s.IS_CANCELED == false
                        select new
                        {
                            CITY_ID = s.CITY_ID,
                            CITY_NAME = s.CITIES.CITY_NAME
                        }
                  ).OrderBy(o => o.CITY_NAME).Distinct().ToList();

            var dataSup = (from s in db.SUPERVISOR_AREAS

                        where s.IS_CANCELED == false
                        select new
                        {
                            EMPLOYEE_ID = s.EMPLOYEE_ID,
                            EMPLOYEE_NAME = s.EMPLOYEES.FIRST_NAME + SqlConstants.stringWhiteSpace + s.EMPLOYEES.LAST_NAME,
                        }
                ).OrderBy(o => o.EMPLOYEE_NAME).Distinct().ToList();


            return Json(new { City = dataCity, Suplier= dataSup }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetSuperVisorList()
        {
            var data = (from s in db.SUPERVISOR_AREAS

                        where s.IS_CANCELED == false
                        select new
                        {
                            EMPLOYEE_ID = s.EMPLOYEE_ID,
                            SUPERVISOR = s.EMPLOYEES.FIRST_NAME+ SqlConstants.stringWhiteSpace+s.EMPLOYEES.LAST_NAME,
                            CITY_ID = s.CITY_ID,
                            CITY_NAME = s.CITIES.CITY_NAME,
                            POSTAL_CODE = s.CITIES.POSTAL_CODE
                        }
                    ).OrderBy(o => o.SUPERVISOR).ToList();

            return Json(new { Getlist = data }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddSuppervisorArea(int SuperVizorID, List<int> CityIDs)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {

                var supervisorarea = db.SUPERVISOR_AREAS.Where(w => w.EMPLOYEE_ID == SuperVizorID && w.IS_CANCELED == false && !CityIDs.Contains(w.CITY_ID)).ToList();
                if(supervisorarea != null)
                {
                    foreach(var item in supervisorarea)
                    {
                        item.IS_CANCELED = true;
                        item.UPDATED_BY = GetUserID();
                        db.SUPERVISOR_AREAS.Attach(item);

                        db.Entry(item).Property(x => x.IS_CANCELED).IsModified = true;
                        db.Entry(item).Property(x => x.UPDATED_BY).IsModified = true;

                        db.SaveChanges();
                    }
                }

                supervisorarea = db.SUPERVISOR_AREAS.Where(w => w.EMPLOYEE_ID == SuperVizorID && w.IS_CANCELED == false && CityIDs.Contains(w.CITY_ID)).ToList();
                if (supervisorarea != null)
                {
                    foreach (var item in supervisorarea)
                    {
                        item.IS_CANCELED = false;
                        item.UPDATED_BY = GetUserID();
                        db.SUPERVISOR_AREAS.Attach(item);

                        db.Entry(item).Property(x => x.IS_CANCELED).IsModified = true;
                        db.Entry(item).Property(x => x.UPDATED_BY).IsModified = true;

                        db.SaveChanges();
                    }
                }

                var insert = (from C in db.CITIES
                              where !db.SUPERVISOR_AREAS.Any(w => w.CITY_ID == C.CITY_ID && w.EMPLOYEE_ID == SuperVizorID && w.IS_CANCELED == false) &&
                                   CityIDs.Contains(C.CITY_ID)
                              select new
                              {
                                  CITY_ID = C.CITY_ID
                              }).ToList();


                foreach (var item in insert)
                {
                    SUPERVISOR_AREAS param = new SUPERVISOR_AREAS();

                    param.IS_CANCELED = false;
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.CITY_ID = item.CITY_ID;
                    param.EMPLOYEE_ID = SuperVizorID;                    

                    db.SUPERVISOR_AREAS.Add(param);
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
        public ActionResult DeleteSuppervisorArea(int SuperVizorID,int CityID)
        {
            SUPERVISOR_AREAS anSupplier = db.SUPERVISOR_AREAS.Where(w=>  w.EMPLOYEE_ID == SuperVizorID && w.CITY_ID == CityID).FirstOrDefault();
            if (anSupplier != null)
            {
                try
                {
                    anSupplier.IS_CANCELED = true;
                    anSupplier.UPDATED_BY = GetUserID();
                    db.SUPERVISOR_AREAS.Attach(anSupplier);

                    db.Entry(anSupplier).Property(x => x.IS_CANCELED).IsModified = true;
                    db.Entry(anSupplier).Property(x => x.UPDATED_BY).IsModified = true;

                    db.SaveChanges();
                    return Json(new { success = true, message="Operation Succesfull" });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = ex.Message });
                }
            }
            return Json(new { success = false, message = "Suppervisor Area not found" });
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
            var list = (from H in db.HOLIDAYS
                        from E_LEFT in db.EMPLOYEES.Where(w=>w.EMP_ID == H.CREATED_BY).DefaultIfEmpty()
                        
                        select new
                        {
                            HOLIDAY_ID = H.HOLIDAY_ID,
                            HOLIDAY_NAME = H.HOLIDAY_NAME,
                            HOLIDAY_DESC = H.HOLIDAY_DESC,
                            START_DATE =  H.START_DATE != null ?
                                        H.START_DATE.Year + SqlConstants.stringMinus +
                                        (H.START_DATE.Month > 9 ? H.START_DATE.Month + SqlConstants.stringMinus : "0" + H.START_DATE.Month + SqlConstants.stringMinus) +
                                        H.START_DATE.Day : null,
                            END_DATE = H.END_DATE != null ?
                                        H.END_DATE.Year + SqlConstants.stringMinus +
                                        (H.END_DATE.Month > 9 ? H.END_DATE.Month + SqlConstants.stringMinus : "0" + H.END_DATE.Month + SqlConstants.stringMinus) +
                                        H.END_DATE.Day : null,
                            IS_CANCELED = H.IS_CANCELED,
                            CREATION_DATE = H.CREATION_DATE != null ?
                                        H.CREATION_DATE.Year + SqlConstants.stringMinus +
                                        (H.CREATION_DATE.Month > 9 ? H.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + H.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                        H.CREATION_DATE.Day : null,
                            MODIFIED_DATE = H.MODIFIED_DATE != null ?
                                        H.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                        (H.MODIFIED_DATE.Month > 9 ? H.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + H.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                        H.MODIFIED_DATE.Day : null,
                            CREATE_BY = E_LEFT.FIRST_NAME + SqlConstants.stringWhiteSpace + E_LEFT.LAST_NAME
                        }).ToList();

            return Json(new { Getlist = list }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddOrUpdateHoliday(HOLIDAYS param)
        {
            bool _success = true;
            string _message = string.Empty;

            try
            {
                HOLIDAYS control = db.HOLIDAYS.Where(w => w.HOLIDAY_ID == param.HOLIDAY_ID).FirstOrDefault();
                int CompanyID = GetCompanyID();
                bool isNew = false;
                if (control == null)
                {
                    isNew = true;

                    if (db.HOLIDAYS.Any(x => x.HOLIDAY_NAME.Equals(param.HOLIDAY_NAME) && x.IS_CANCELED == false && (x.COMPANY_ID == -1 || x.COMPANY_ID == CompanyID)))
                    {
                        _message = "Holiday already exists.";
                        _success = false;
                    }

                }
                else
                {
                    if (db.HOLIDAYS.Any(x => x.HOLIDAY_NAME.Equals(param.HOLIDAY_NAME) && x.HOLIDAY_ID != param.HOLIDAY_ID && x.IS_CANCELED == false && (x.COMPANY_ID == -1 || x.COMPANY_ID == CompanyID)))
                    {
                        _message = "Holiday already exists.";
                        _success = false;
                    }
                }

                if (_success == false)
                    return Json(new { success = _success, Message = _message });

                if (isNew)
                {
                    param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                    param.CREATED_BY = param.UPDATED_BY = GetUserID();
                    param.COMPANY_ID = CompanyID;

                    db.HOLIDAYS.Add(param);
                    db.SaveChanges();
                }
                else
                {
                    control.MODIFIED_DATE = DateTime.Now;
                    control.HOLIDAY_NAME = param.HOLIDAY_NAME;
                    control.HOLIDAY_DESC = param.HOLIDAY_DESC;
                    control.START_DATE = param.START_DATE;
                    control.END_DATE = param.END_DATE;
                    control.IS_CANCELED = param.IS_CANCELED;
                    control.UPDATED_BY = GetUserID();
                    //ContractType.MODIFIED_DATE = param.MODIFIED_DATE;

                    db.HOLIDAYS.Attach(control);
                    db.Entry(control).Property(x => x.HOLIDAY_NAME).IsModified = true;
                    db.Entry(control).Property(x => x.HOLIDAY_DESC).IsModified = true;
                    db.Entry(control).Property(x => x.START_DATE).IsModified = true;
                    db.Entry(control).Property(x => x.END_DATE).IsModified = true;
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

        public JsonResult RemoveHoliday(HOLIDAYS param)
        {
            bool _success = false;
            string _message = string.Empty;

            try
            {
                HOLIDAYS control = db.HOLIDAYS.Where(w => w.HOLIDAY_ID == param.HOLIDAY_ID).FirstOrDefault();
                if (control != null)
                {
                    db.HOLIDAYS.Remove(control);
                    db.SaveChanges();

                    _success = true;
                    _message = "Operation Successful";
                }
                else
                {
                    _message = "Holiday Not Found not found";
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