using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Abstract
{
    public interface ICompanyService : IService<COMPANY, CompanyExtented>
    {
        public ShowState UpdateCompanyImg(int CompanyID, HttpPostedFileBase Image, int Remove);
    }
}
