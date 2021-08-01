using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.Concrete;
using JOB_MANAGER_BUSSINESS.Abstract;

namespace JOB_MANAGER.Bussiness.Concrete
{
    public class CompanyManager : IService<COMPANY, CompanyExtented>
    {
        private CompanyDal _companyDal;

        public CompanyManager(CompanyDal companyDal)
        {
            _companyDal = companyDal;
        }

        public ShowState AddorUpdate(COMPANY param)
        {
            return _companyDal.AddorUpdate(param, f => f.COMPANY_ID == param.COMPANY_ID);
        }

        public ShowState Delete(COMPANY param)
        {
            return _companyDal.Delete(param);
        }

        public COMPANY Get(COMPANY param)
        {
            return _companyDal.GetAll(f => f.COMPANY_ID == param.COMPANY_ID).FirstOrDefault();
        }

        public List<CompanyExtented> GetAll()
        {
            return _companyDal.GetAll2();
        }

        public ShowState UpdateCompanyImg(int CompanyID, HttpPostedFileBase Image, int Remove)
        {
            COMPANY company = _companyDal.GetAll(f => f.COMPANY_ID == CompanyID).FirstOrDefault();
            ShowState showState = new ShowState();
            if (company != null)
            {
           
                if (Image != null)
                {
                    company.COMPANY_LOGO = new byte[Image.ContentLength];
                    Image.InputStream.Read(company.COMPANY_LOGO, 0, Image.ContentLength);
                    //employee.EMPLOYEE_IMG = GlobalTools.ResizeEmployeeImage(employee.EMPLOYEE_IMG);

                    showState.isError = false;
                    showState.ErrorMessage = "Image Chamged";
                }
                else
                {
                    company.COMPANY_LOGO = null;

                    showState.isError = !(Remove == 1);
                    showState.ErrorMessage = !showState.isError ? "Image Chamged" : "No Image found";
                }

                var state = AddorUpdate(company);
                if(state.isError)
                {
                    showState.isError = state.isError;
                    showState.ErrorMessage = state.ErrorMessage;
                }                              
            }

            return showState;
        }
    }
}
