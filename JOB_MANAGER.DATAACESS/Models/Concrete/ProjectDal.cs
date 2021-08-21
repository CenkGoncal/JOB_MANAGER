using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JOB_MANAGER.DATAACESS.Helper;

namespace JOB_MANAGER.DATAACESS.Models
{
    public class ProjectDal : EntityRepositoryBase<PROJECTS, JOB_MANAGER_DBEntities, ProjectExtented>
    {
        public override void SaveControls(PROJECTS entity, ShowState showState)
        {
            PROJECTS control = context.PROJECTS.Where(w => w.PROJECT_ID == entity.PROJECT_ID).FirstOrDefault();

            if (control == null)
            {

                if (context.PROJECTS.Any(x => x.PROJECT_NAME.Equals(entity.PROJECT_NAME) && x.IS_CANCELED == false && x.USE_AS_TEMPLATE == false && x.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId))
                {
                    showState.isError = true;
                    showState.ErrorMessage = "Project Name already exists.";
                }
                if (context.PROJECTS.Any(x => x.CLIENT_PO_NBR.Equals(entity.CLIENT_PO_NBR) && x.IS_CANCELED == false && x.USE_AS_TEMPLATE == false && x.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId))
                {
                    showState.isError = true;
                    showState.ErrorMessage = "PO # already exists.";
                }

            }
            else
            {

                if (context.PROJECTS.Any(x => x.PROJECT_NAME.Equals(entity.PROJECT_NAME) && x.PROJECT_ID != entity.PROJECT_ID && x.USE_AS_TEMPLATE == false && x.IS_CANCELED == false && x.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId))
                {
                    showState.isError = true;
                    showState.ErrorMessage = "Project Name already exists.";
                }
                if (context.PROJECTS.Any(x => x.CLIENT_PO_NBR.Equals(entity.CLIENT_PO_NBR) && x.PROJECT_ID != entity.PROJECT_ID && x.USE_AS_TEMPLATE == false && x.IS_CANCELED == false && x.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId))
                {
                    showState.isError = true;
                    showState.ErrorMessage = "PO # already exists.";
                }

            }

            int DefEventSize = GlobalTools.GetParamIntVal("START_YEAR_MODEL", ThreadGlobals.UserAuthInfo.Value.CompanyId);
            if (DefEventSize < 0)
                DefEventSize = 30;


            DateTime EndTime = entity.INSTALLATION_DATE.Value.AddMinutes(DefEventSize);
            var Isholiday = (from H in context.HOLIDAYS
                             where (H.START_DATE >= entity.INSTALLATION_DATE && H.END_DATE <= entity.INSTALLATION_DATE) ||
                                   (H.START_DATE >= EndTime && H.END_DATE <= EndTime)
                             select H).ToList();
            if (Isholiday != null)
            {
                showState.isError = true;
                showState.ErrorMessage = "The Start or End day coincides with the holiday.";
            }

        }

        public override void SaveHelper_ModifyBeforeSave(PROJECTS param, PROJECTS dbitem)
        {
            if (dbitem == null)
            {
                param.MODIFIED_DATE = param.CREATION_DATE = DateTime.Now;
                param.CREATED_BY = param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;

                param.PROJECT_ACTUAL_VAL = 0;
                param.PROJECT_ESTIMATED_VAL = 0;
                param.EVENT_SIZE = 30;

                int lastjOBID = context.PROJECTS.Where(w => w.IS_CANCELED == false).OrderByDescending(o => o.PROJECT_ID).Select(s => s.PROJECT_ID).FirstOrDefault();
                param.PROJECT_NUMBER = string.Format("J0B-{0}", lastjOBID + 1);
            }
            else
            {
                param.MODIFIED_DATE = dbitem.MODIFIED_DATE;
                param.CREATION_DATE = dbitem.CREATION_DATE;
                param.PROJECT_ID = dbitem.PROJECT_ID;
                param.CREATED_BY = dbitem.CREATED_BY;

                param.MODIFIED_DATE = DateTime.Now;
                param.UPDATED_BY = ThreadGlobals.UserAuthInfo.Value.UserId;
            }
        }

        public override void DeleteControls(PROJECTS entity, ShowState showState)
        {
            if (!context.PROJECTS.Any(x => x.PROJECT_ID == entity.PROJECT_ID))
            {
                showState.ErrorMessage = "Project not exists.";
                showState.isError = true;
            }
        }

        public override List<ProjectExtented> GetAll2(Expression<Func<PROJECTS, bool>> filter = null)
        {
            var _projectlist = filter != null ? context.Set<PROJECTS>().Where(filter).ToList()
                : context.Set<PROJECTS>().ToList();

            var data = (from P in _projectlist
                        join C in context.CLIENTS on P.CLIENT_ID equals C.CLIENT_ID
                        join PT in context.PROJECT_TYPES on P.PROJECT_TYPE_ID equals PT.PROJECT_TYPE_ID
                        join S in context.STATUS on P.PROJECT_STATUS equals S.STATUS_ID
                        join CT in context.CITIES on P.PROJECT_CITY equals CT.CITY_ID
                        join ST in context.STATES on P.PROJECT_STATE equals ST.STATE_ID
                        join CN in context.COUNTRIES on P.PROJECT_COUNTRY equals CN.COUNTRY_ID
                        join F in context.FLOOR_TYPES on P.FLOOR_TYPE equals F.FLOOR_TYPE_ID
                        join M in context.MATERIALS on P.MATERIAL_ID equals M.MATERIAL_ID
                        join STT in context.STREET on P.STREET_SUFFIX_ID equals STT.STREET_ID
                        from CS_LEFT in context.CONTACTS.Where(w => w.CONTACT_ID == P.CLIENT_SUPRVISOR).DefaultIfEmpty()
                        from PS_LEFT in context.EMPLOYEES.Where(w => w.EMP_ID == P.PROJECT_SUPERVISOR).DefaultIfEmpty()
                        from Q_LEFT in context.QUOTES.Where(w => w.QUOTE_ID == P.QUOTE_ID).DefaultIfEmpty()
                        where P.IS_CANCELED == false && P.COMPANY_ID == ThreadGlobals.UserAuthInfo.Value.CompanyId
                        select new ProjectExtented
                        {
                            PROJECT_ID = P.PROJECT_ID,
                            QUOTE_ID = Q_LEFT.QUOTE_ID,
                            QUOTE_NUMBER = Q_LEFT.QUOTE_NUMBER,
                            PROJECT_NUMBER = P.PROJECT_NUMBER,
                            CLIENT_ID = P.CLIENT_ID,
                            CLIENT_NAME = C.CLIENT_NAME,
                            CLIENT_PO_NBR = P.CLIENT_PO_NBR,
                            PROJECT_TYPE_ID = P.PROJECT_TYPE_ID,
                            PROJECT_TYPE_NAME = PT.PROJECT_TYPE_NAME,
                            PROJECT_STATUS = P.PROJECT_STATUS,
                            PROJECT_NAME = P.PROJECT_NAME,
                            DISPLAY_CLASS = S.DISPLAY_CLASS,
                            STATUS_NAME = S.STATUS_NAME,
                            ADR_LOT_NBR = P.ADR_LOT_NBR,
                            ADR_UNIT_NBR = P.ADR_UNIT_NBR,
                            ADR_STREET_NBR = P.ADR_STREET_NBR,
                            ADR_STREET_NAME = P.ADR_STREET_NAME,
                            PROJECT_ADDRESS = P.PROJECT_ADDRESS,
                            PROJECT_COUNTRY = P.PROJECT_COUNTRY,
                            COUNTRY_NAME = CN.COUNTRY_NAME,
                            PROJECT_STATE = P.PROJECT_STATE,
                            STATE_NAME = ST.STATE_NAME,
                            PROJECT_CITY = P.PROJECT_CITY,
                            CITY_NAME = CT.CITY_NAME,
                            PROJECT_POSTAL_CODE = P.PROJECT_POSTAL_CODE,
                            PROJECT_AMOUNT = P.PROJECT_AMOUNT,
                            PROJECT_ESTIMATED_VAL = P.PROJECT_ESTIMATED_VAL,
                            PROJECT_ACTUAL_VAL = P.PROJECT_ACTUAL_VAL,
                            CLIENT_SUPRVISOR = P.CLIENT_SUPRVISOR,
                            CLI_SUPERVISOR_NAME = PS_LEFT.FIRST_NAME + SqlConstants.stringWhiteSpace + PS_LEFT.LAST_NAME,
                            PROJECT_SUPERVISOR = P.PROJECT_SUPERVISOR,
                            PRO_SUPERVISOR_NAME = CS_LEFT.CONTACT_FIRST_NAME + SqlConstants.stringWhiteSpace + CS_LEFT.CONTACT_LAST_NAME,
                            PROJECT_PROGRESS = P.PROJECT_PROGRESS,
                            CABINETS_NBR = P.CABINETS_NBR,
                            PROJECT_DESC = P.PROJECT_DESC,
                            INSTALLATION_DATE = P.INSTALLATION_DATE != null ?
                                                    P.INSTALLATION_DATE.Value.Year + SqlConstants.stringMinus +
                                                    (P.INSTALLATION_DATE.Value.Month > 9 ? P.INSTALLATION_DATE.Value.Month + SqlConstants.stringMinus : "0" + P.INSTALLATION_DATE.Value.Month + SqlConstants.stringMinus) +
                                                    P.INSTALLATION_DATE.Value.Day : null,
                            TIME = P.INSTALLATION_DATE != null ? P.INSTALLATION_DATE.Value.Hour + SqlConstants.stringIkiNokta + P.INSTALLATION_DATE.Value.Minute : null,
                            PROJECT_START_DATE = P.PROJECT_START_DATE != null ?
                                                    P.PROJECT_START_DATE.Year + SqlConstants.stringMinus +
                                                    (P.PROJECT_START_DATE.Month > 9 ? P.PROJECT_START_DATE.Month + SqlConstants.stringMinus : "0" + P.PROJECT_START_DATE.Month + SqlConstants.stringMinus) +
                                                    P.PROJECT_START_DATE.Day : null,
                            PROJECT_START_DATE_DT = P.PROJECT_START_DATE,
                            PROJECT_END_DATE = P.PROJECT_END_DATE != null ?
                                                    P.PROJECT_END_DATE.Value.Year + SqlConstants.stringMinus +
                                                    (P.PROJECT_END_DATE.Value.Month > 9 ? P.PROJECT_END_DATE.Value.Month + SqlConstants.stringMinus : "0" + P.PROJECT_END_DATE.Value.Month + SqlConstants.stringMinus) +
                                                    P.PROJECT_END_DATE.Value.Day : null,
                            PROJECT_END_DATE_DT = P.PROJECT_END_DATE,
                            DISPLAY = P.DISPLAY,
                            FLOOR_TYPE = P.FLOOR_TYPE,
                            FLOOR_TYPE_NAME = F.FLOOR_TYPE_NAME,
                            MATERIAL_ID = P.MATERIAL_ID,
                            MATERIAL_NAME = M.MATERIAL_NAME,
                            STREET_SUFFIX_ID = P.STREET_SUFFIX_ID,
                            STREET_NAME = STT.STREET_NAME,
                            USE_AS_TEMPLATE = P.USE_AS_TEMPLATE,
                            CREATION_DATE = P.CREATION_DATE != null ?
                                                    P.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (P.CREATION_DATE.Month > 9 ? P.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + P.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    P.CREATION_DATE.Day : null,
                            MODIFIED_DATE = P.MODIFIED_DATE != null ?
                                                    P.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (P.MODIFIED_DATE.Month > 9 ? P.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + P.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    P.MODIFIED_DATE.Day : null,

                        }
                 ).OrderBy(o => o.CLIENT_NAME).ToList();

            return data;
        }
    }
}
