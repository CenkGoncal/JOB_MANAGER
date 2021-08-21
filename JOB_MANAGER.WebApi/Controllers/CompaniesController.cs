using System.Collections.Generic;
using System.Web.Http;
using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.DATAACESS.Models;


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
