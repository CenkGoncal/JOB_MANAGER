using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JOB_MANAGER.Bussiness.Concrete;
using JOB_MANAGER.Models;


namespace JOB_MANAGER.WebApi.Controllers
{
    public class CompaniesController : ApiController
    {
        private CompanyManager _companyManager;

        public CompaniesController(CompanyManager companyManager)
        {
            _companyManager = companyManager;
        }

        public List<CompanyExtented> Get()
        {
            return _companyManager.GetAll();
        }
    }
}
