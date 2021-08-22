using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.DATAACESS.Models;
using JOB_MANAGER.WebApi.MessageHandler;

namespace JOB_MANAGER.WebApi.Controllers
{
    [Authorize(Roles = "CompanyType")]
    public class CompaniesController : ApiController
    {
        private CompanyManager _companyManager;

        public CompaniesController(CompanyManager companyManager)
        {
            _companyManager = companyManager;
        }


        [HttpGet]
        [ReturnType(typeof(List<CompanyExtented>))]        
        public HttpResponseMessage Get()
        {
            try
            {                
                return Request.CreateResponse<List<CompanyExtented>>(_companyManager.GetAll());
            }
            catch (System.Exception ex)
            {

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }
            
        }
    }
}
