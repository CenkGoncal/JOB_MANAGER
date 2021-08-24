using System.Collections.Generic;
using System.Linq;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Concrete
{
    public class TitleManager : ITitleService
    {
        private TitleDal _dal;
        
        public TitleManager(TitleDal titleDal)
        {
            _dal = titleDal;
        }

        public ShowState AddorUpdate(TITLES param)
        {
            return _dal.AddorUpdate(param, f => f.TITLE_ID == param.TITLE_ID);
        }

        public ShowState Delete(TITLES param)
        {
            return _dal.Delete(param);
        }

        public TITLES Get(TITLES param)
        {
            return _dal.GetAll(f => f.TITLE_ID == param.TITLE_ID).FirstOrDefault();
        }

        public List<TitleExtented> GetAll()
        {
            return _dal.GetAll2();
        }

        public List<TitleExtented> GetTitleCompany(int CompanyId)
        {
            return _dal.GetAll2(w => w.IS_CANCELED == false && (w.COMPANY_ID == -1 || w.COMPANY_ID == CompanyId)).ToList();
        }
    }
}
