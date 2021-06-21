using JOB_MANAGER.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace JOB_MANAGER.Models.Concrete
{
    public class TitleDal : EntityRepositoryBase<TITLES, JOB_MANAGER_DBEntities, TitleExtented>
    {
        public JOB_MANAGER_DBEntities db;
        public GlobalTools.UserInfo UserInfo;

        public TitleDal(GlobalTools.UserInfo _userInfo)
        {
            db = new JOB_MANAGER_DBEntities();
            UserInfo = _userInfo;
        }

        public override List<TitleExtented> GetAll2(Expression<Func<TITLES, bool>> filter = null)
        {
            var _titles = filter != null ? db.Set<TITLES>().Where(filter).ToList()
                     : db.Set<TITLES>().ToList();

            var data = (from t in _titles

                        join e in db.EMPLOYEES
                        on t.CREATED_BY equals e.EMP_ID into e_join
                        from e_left in e_join.DefaultIfEmpty()
                        select new TitleExtented
                        {
                            TITLE_ID = t.TITLE_ID,
                            TITLE_NAME = t.TITLE_NAME,
                            IS_CANCELED = t.IS_CANCELED,
                            CREATION_DATE = t.CREATION_DATE != null ?
                                                    t.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (t.CREATION_DATE.Month > 9 ? t.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + t.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    t.CREATION_DATE.Day : null,
                            MODIFIED_DATE = t.MODIFIED_DATE != null ?
                                                    t.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (t.MODIFIED_DATE.Month > 9 ? t.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + t.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    t.MODIFIED_DATE.Day : null,
                            CREATE_BY = e_left.FIRST_NAME + SqlConstants.stringWhiteSpace + e_left.LAST_NAME
                        }
                         ).OrderBy(o => o.TITLE_NAME).ToList();

            return data;
        }

        public override void SaveHelper_ModifyBeforeSave(TITLES param, TITLES dbitem)
        {
            if (dbitem == null)
            {
                param.COMPANY_ID = UserInfo.CompanyId;
                param.CREATED_BY = param.UPDATED_BY = UserInfo.UserId;
                param.CREATION_DATE = param.MODIFIED_DATE = DateTime.Now;
            }
            else
            {
                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = UserInfo.UserId;
            }
            //base.SaveHelper_ModifyBeforeSave(param);
        }

        public override void DeleteControls(TITLES entity, ShowState showState)
        {
            var checkEmployee = db.EMPLOYEES.Where(w => w.TITLE_ID == entity.TITLE_ID).FirstOrDefault();
            if (checkEmployee != null)
            {
                showState.ErrorMessage = "Staff are available to use this titles!! You can change cancelled type.";
                showState.isError = true;
            }

            if (!db.TITLES.Any(w => w.TITLE_ID == entity.TITLE_ID))
            {
                showState.ErrorMessage = "Title not found!!";
                showState.isError = true;
            }
        }
    }
}