using JOB_MANAGER.DataAcess.Helper;
using JOB_MANAGER.DATAACESS.Models;
using JOB_MANAGER.DATAACESS.Models.ComplexType;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;

namespace JOB_MANAGER.DATAACESS.Helper
{
    public class GlobalTools        
    {
        private static JOB_MANAGER_DBEntities db;
        
        public GlobalTools()
        {
            db = new JOB_MANAGER_DBEntities();            
        }

        #region Encryption/Decryption
        public static string EncryptSystemString(string anStr,bool EmailMode = false)
        {
            if (string.IsNullOrEmpty(anStr as string))
                return string.Empty;

            string passPhase = EmailMode == false ? "R@rC@b1N3tPa55w0rdT03ncryp72019" : "r&Rc@BiN3T#@55w0rdtO3ncyp7m@1l";
            anStr = SecurityProject.EncryptionDecryption.EncryptString(anStr, passPhase);
            return anStr;
        }
        public static string DecryptSystemString(string anStr, bool EmailMode = false)
        {
            if (string.IsNullOrEmpty(anStr as string))
                return string.Empty;

            string passPhase = EmailMode == false ? "R@rC@b1N3tPa55w0rdT03ncryp72019" : "r&Rc@BiN3T#@55w0rdtO3ncyp7m@1l";
            anStr = SecurityProject.EncryptionDecryption.DecryptString(anStr, passPhase);
            return anStr;
        }
        #endregion

        public static DataTable YearTable(int companyID)
        {
            var _YearTable = new DataTable("YOM");
            int startYear = GetParamIntVal("START_YEAR_MODEL",companyID);
            int currentYear = DateTime.Today.Year;
            _YearTable.Columns.Add("YOM_ID");
            _YearTable.Columns.Add("YOM_VALUE");
            for (int i = currentYear; i >= startYear; i--)
                _YearTable.Rows.Add(new Object[] { i, i });
            _YearTable.AcceptChanges();
            return _YearTable;
        }
        public static DataView GetYOMLookUp(int companyID)
        {
            DataTable listOfYOM = new DataTable();
            listOfYOM.Columns.Add("YOM_ID");
            listOfYOM.Columns.Add("YOM_VALUE");
            listOfYOM.Rows.Add(new object[] { -1, "Select Year of Make..." });
            var currentTable = YearTable(companyID);
            //listOfYOA.Rows.Add(ConfigurationData.YOATable.Rows);
            for (int i = 0; i < currentTable.Rows.Count; i++)
                listOfYOM.Rows.Add(new object[] { currentTable.Rows[i][0], currentTable.Rows[i][1] });

            return listOfYOM.AsDataView();
        }

        public static string GetParamStrVal(string ParamName,int CompanyID)
        {
            GlobalCache paramCache = new GlobalCache();
            string paramStr = paramCache.GetCacheParameter().Where(w => w.PARAM_NAME == ParamName).Select(s => s.PARAM_STR).FirstOrDefault();
            
            if (CompanyID > 0)
            {
                string paramStr1 = paramCache.GetCacheParameter().Where(w => w.PARAM_NAME == ParamName && w.COMPANY_ID == CompanyID).Select(s => s.PARAM_STR).FirstOrDefault();
                if (!string.IsNullOrEmpty(paramStr1))
                    paramStr = paramStr1;
            }

            return paramStr;
        }

        public static int GetParamIntVal(string ParamName, int CompanyID)
        {
            GlobalCache paramCache = new GlobalCache();

            int intval = -1;
            var paramint = paramCache.GetCacheParameter().Where(w => w.PARAM_NAME == ParamName).Select(s => s.PARAM_INT).FirstOrDefault();
            if (paramint != null)
                intval = paramint.Value;

            if (CompanyID > 0)
            {
                paramint = (paramCache.GetCacheParameter().Where(w => w.PARAM_NAME == ParamName && w.COMPANY_ID == CompanyID).Select(s => s.PARAM_INT).FirstOrDefault());
                if (paramint != null)
                    intval = paramint.Value;
            }

            return intval;
        }

        public static byte[] GetParamByteVal(string ParamName, int CompanyID)
        {
            GlobalCache paramCache = new GlobalCache();
            
            var param = paramCache.GetCacheParameter().ToList();
            byte[] byteArray = null;
            foreach (var item in param)
            {
                if(item.PARAM_NAME.Contains(ParamName))
                {
                    if (CompanyID > 0 && item.COMPANY_ID == CompanyID)
                        byteArray = item.PARAM_IMG;
                    else
                        byteArray = item.PARAM_IMG;
                }
            }
                
            return byteArray;
        }

        public static string SendEmail(string SenderMail,string CCMail, string Subject,string Body, List<string> ekler, int EmpID)
        {
            string message = string.Empty;

            JOB_MANAGER_DBEntities db = new JOB_MANAGER_DBEntities();    

            var query = (from e in db.EMPLOYEES
                         where e.EMP_ID == EmpID
                         select new
                         {
                             EmailUsername = e.EMAIL_USERNAME,
                             EmailPassword = e.EMAIL_PASSWORD,
                             CompanyID = e.COMPANY_ID,
                             EmpName = e.FIRST_NAME + SqlConstants.stringWhiteSpace + e.LAST_NAME,
                             Signature = e.SHOW_SIGNATURE ? e.SIGNATURE : SqlConstants.stringEmpty
                         }).FirstOrDefault();

            if (query == null)
                return "Employee not found";


            SmtpClient smtp = new SmtpClient(); // smtp nesnesi oluşturuyoruz
         
            smtp.Host = GetParamStrVal("MAIL_HOST", query.CompanyID);//"smtp-mail.outlook.com"; // Mail sunucusu adresi
            smtp.Port = GetParamIntVal("MAIL_PORT", query.CompanyID); // Outlook için 587
            smtp.EnableSsl = true; // Sunucu SSL kullanıyorsa True olacak
                                   // mail adresimizin kullanıcı adı ve parolasını yazıyoruz
            smtp.Credentials = new System.Net.NetworkCredential(query.EmailUsername, query.EmailPassword);

            // eposta adında bir mail nesnesi oluştur
            MailMessage eposta = new MailMessage();
            // Giden mailde görünecek e-posta adresi ve isim email adresi smtp ile aynı olmayınca hata veriyor.
            eposta.From = new MailAddress(query.EmailUsername, query.EmpName);
            // Mail gönderilecek kişi(ler). Eğer birden fazla kişiye gidecekse, kişiler arasına virgül koyulacak
            eposta.To.Add(SenderMail/*"isim1@mail1.com, isim2@mail2.com"*/);

            if(!string.IsNullOrEmpty(CCMail))
                eposta.CC.Add(CCMail/*"ism3@mail3.com, isim4@mail4.com"*/); // Bilgi maili gönerilecek kişileri CC özelliğine ekle
            //eposta.Bcc.Add("isim5@mail5.com"); // Gizli alıcıları bcc özelliğine ekle
            eposta.Subject = Subject; // Mail konusunu subject özelliğine ekle
            eposta.Body = Body; // mesaj içeriğini body özelliğine ekle

            if (!string.IsNullOrEmpty(query.Signature))
                eposta.Body += string.Format(" {0}", query.Signature);
            
            // ekleri dosya yolu ile birlikte bir string dizisinde tutuyoruz
            //string[] ekler = { "c:\\ek1.png", "c:\\ek2.pdf", "c:\\ek3.docx" };
            // Ardından ekleri foreach döngüsü ile Attachments özelliğine ekliyoruz
            if(ekler.Count > 0)
            {
                foreach (string ekle in ekler)
                {
                    eposta.Attachments.Add(new Attachment(@ekle)); // ekleri Attachments özelliğine ekle
                }
            }

            try
            { // Hata kontrolü
                smtp.Send(eposta); // eposta nesnesini smtp.Send fonksiyonu ile gönder
                message = "Sended Email";
            }
            catch(Exception ex){ // Hata oluştuysa oluşan hatayı ex değişkenine aktar
                message="Have a Error: " + ex.Message; // Hatayı kullanıcıya bildir
            }

            return message;
        }

        public static List<DocumentDto> GetDocumentLookUp(int TypeID, int UniqID, int? PhaseID, int? TaskID, int? NoteID)
        {            
            var query = db.DOC_VIEW.Where(w => w.UNIQ_ID == UniqID && w.DOC_KIND == TypeID).OrderBy(w => w.DOC_ID).ToList();

            if (PhaseID.HasValue && PhaseID.Value > 0)
                query = query.Where(w => w.PHASE_ID == PhaseID).ToList();

            if (TaskID.HasValue && TaskID.Value > 0)
                query = query.Where(w => w.TASK_ID == TaskID).ToList();

            if (NoteID.HasValue && NoteID.Value > 0)
                query = query.Where(w => w.NOTE_ID == NoteID).ToList();

            List<DocumentDto> DocumentList = new List<DocumentDto>();
            foreach (var doc in query)
            {
                DocumentDto item = new DocumentDto();
                item.DOC_ID = doc.DOC_ID;
                item.DOC_NAME = doc.DOC_NAME;
                item.DOC_PATH = doc.DOC_PATH;
                item.DOC_KIND = doc.DOC_KIND.Value;
                item.UNIQ_ID = doc.UNIQ_ID;
                item.DOCUMENT_TYPE_ID = doc.DOCUMENT_TYPE_ID;

                if (doc.PHASE_ID.HasValue)
                {
                    item.PHASE_ID = doc.PHASE_ID.Value;
                    item.PHASE_NAME = doc.PHASE_NAME;
                    item.DOC_FOLDER_NAME = "Phase";
                }

                if (doc.TASK_ID.HasValue)
                {
                    item.TASK_ID = doc.TASK_ID.Value;
                    item.TASK_NAME = doc.PROJECT_TASK_NAME;
                    item.DOC_FOLDER_NAME = "Task";
                }

                if (doc.NOTE_ID.HasValue)
                {
                    item.NOTE_ID = doc.NOTE_ID.Value;
                    item.NOTE_NAME = doc.NOTE_DESC;
                    item.DOC_FOLDER_NAME = "Note";
                }

                item.FONT_AWESOME_ICON = doc.FONT_AWESOME_ICON;
                item.DOCUMENT_TYPE_NAME = doc.DOCUMENT_TYPE_NAME;

                DocumentList.Add(item);
            }

            return DocumentList;
        }

        public static List<NoteDto> GetNoteLookUp(int TypeID, int UniqID, int? PhaseID, int? TaskID, int? NoteID)
        {
            JOB_MANAGER_DBEntities db = new JOB_MANAGER_DBEntities();
            var query = db.NOTE_VIEW.Where(w => w.UNIQ_ID == UniqID && w.NOTE_KIND_ID == TypeID).OrderBy(w=>w.NOTE_ID).ToList();

            if (PhaseID.HasValue && PhaseID.Value > 0)
                query = query.Where(w => w.PHASE_ID == PhaseID.Value).ToList();

            if (TaskID.HasValue && TaskID.Value >0)
                query = query.Where(w => w.TASK_ID == TaskID.Value).ToList();

            if (NoteID.HasValue && NoteID.Value > 0)
                query = query.Where(w => w.NOTE_ID == NoteID.Value).ToList();

            List<NoteDto> NoteList = new List<NoteDto>();
            foreach (var item in query)
            {
                NoteDto Note = new NoteDto();
                Note.NOTE_ID = item.NOTE_ID;
                Note.NOTE_KIND_ID = item.NOTE_KIND_ID;
                Note.UNIQ_ID = item.UNIQ_ID;
                Note.NOTE_TYPE_ID = item.NOTE_TYPE_ID;
                Note.NOTE_TYPE_NAME = item.NOTE_TYPE_NAME;
                if (item.PHASE_ID.HasValue)
                {
                    Note.PHASE_ID = item.PHASE_ID.Value;
                    Note.PHASE_NAME = item.PHASE_NAME;
                }
                else
                    Note.PHASE_NAME = string.Empty;

                if (item.TASK_ID.HasValue)
                {
                    Note.TASK_ID = item.TASK_ID.Value;
                    Note.TASK_NAME = item.TASK_NAME;
                }
                else
                    Note.TASK_NAME = string.Empty;

                Note.NOTE_SUBJECT = item.NOTE_SUBJECT;
                Note.NOTE_DESC = item.NOTE_DESC;
                if (item.SET_REMINDER.HasValue)
                    Note.SET_REMINDER = item.SET_REMINDER.Value;

                if (item.REMINDER_DATE.HasValue)
                {
                    Note.REMINDER_DATE = item.REMINDER_DATE.Value.ToString("yyyy-MM-dd");
                    Note.REMINDER_TIME = item.REMINDER_DATE.Value.ToString("hh:mm");
                }

                if (string.IsNullOrEmpty(item.ASSINGED_MEMBERS) == false)
                {
                    Note.ASSINGED_MEMBERS_ARR = JsonConvert.DeserializeObject<List<int>>(item.ASSINGED_MEMBERS);
                }
                
                if (string.IsNullOrEmpty(item.TO_REMINDER) == false)
                {
                    Note.TO_REMIND_ARR = JsonConvert.DeserializeObject<List<int>>(item.TO_REMINDER);
                }

                NoteList.Add(Note);
            }

            return NoteList;
        }

        public static List<MembersDto> GetMembersLookUp(int ProjectID,int? PhaseID,int? TaskID)
        {            
            var query = db.PROJECT_MEMBER_VIEW.Where(w => w.PROJECT_ID == ProjectID).OrderBy(w => w.PROJECT_MEMBER_ID).ToList();

            if (PhaseID.HasValue && PhaseID.Value > 0)
                query = query.Where(w => w.PROJECT_PHASE_ID == PhaseID.Value).ToList();

            if (TaskID.HasValue && TaskID.Value > 0)
                query = query.Where(w => w.TASK_ID == TaskID.Value).ToList();

            List<MembersDto> Memberlist = new  List<MembersDto>();
            foreach(var item in query)
            {
                MembersDto member = new MembersDto();
                member.PROJECT_ID = item.PROJECT_ID;
                member.EMP_ID = item.EMPLOYEE_ID;
                member.EMP_NAME = item.EMP_NAME;
                member.EMP_IMG = item.EMP_IMG;
                
                if(item.SUPERVISOR_ID.HasValue && item.SUPERVISOR_ID.Value > 0)
                {
                    member.SUPERVISOR_ID = item.SUPERVISOR_ID.Value;
                    member.EMP_NAME = item.SUP_NAME;
                }
                                
                member.IS_SUPERVISOR = item.IS_SUPERVISOR;
                member.PROJECT_MEMBER_ID = item.PROJECT_MEMBER_ID;
                member.TITLE_ID = item.TITLE_ID;
                member.TITLE_NAME = item.TITLE_NAME;
                if (item.PROJECT_PHASE_ID.HasValue && item.PROJECT_PHASE_ID > 0)
                {
                    member.PHASE_ID = item.PROJECT_PHASE_ID.Value;
                    member.PHASE_NAME = item.PHASE_NAME;
                }
                else
                    member.PHASE_NAME = string.Empty;

                if (item.TASK_ID.HasValue && item.TASK_ID > 0)
                {
                    member.TASK_ID = item.TASK_ID.Value;
                    member.TASK_NAME = item.TASK_NAME;
                }
                else
                    member.TASK_NAME = string.Empty;

                Memberlist.Add(member);
            }

            return Memberlist;
        }

        public static List<TasksDto> GetTasksLookUp(int ProjectID, int? PhaseID, int? TaskID)
        {            
            var query = db.PROJECT_TASK.Where(w => w.PROJECT_ID == ProjectID && w.IS_CANCELED == false).OrderBy(o=>o.TASK_ORDER).ToList();

            if (PhaseID.HasValue && PhaseID.Value > 0)
                query = query.Where(w => w.PROJECT_PHASE_ID == PhaseID.Value).ToList();

            if (TaskID.HasValue && TaskID.Value > 0)
                query = query.Where(w => w.PROJECT_TASK_ID == TaskID.Value).ToList();

            var query2 = (from Q in query
                          join S in db.STATUS on Q.PROJECT_TASK_STATUS equals S.STATUS_ID
                          join P in db.PROJECT_PHASES on Q.PROJECT_PHASE_ID equals P.PROJECT_PHASE_ID
                          join D in db.DEF_PROJECT_PHASES on P.PHASE_ID equals D.PHASE_ID
                          join PRJ in db.PROJECTS on P.PROJECT_ID equals PRJ.PROJECT_ID
                          from C_LEFT in db.EMPLOYEES.Where(w => w.EMP_ID == PRJ.CLIENT_ID).DefaultIfEmpty()
                          select new { Q, S,D, C_LEFT }).ToList();

            List<TasksDto> TaskList = new List<TasksDto>();
            foreach (var item in query2)
            {
                TasksDto task = new TasksDto();
                task.PROJECT_ID = item.Q.PROJECT_ID;
                task.PROJECT_TASK_ID = item.Q.PROJECT_TASK_ID;                
                task.PHASE_ID = item.Q.PROJECT_PHASE_ID;
                task.PHASE_NAME = item.D.PHASE_NAME;
                task.TASK_ORDER = item.Q.TASK_ORDER;
                task.PROJECT_TASK_NAME = item.Q.PROJECT_TASK_NAME;
                task.STATUS_ID = item.S.STATUS_ID;
                task.STATUS_NAME = item.S.STATUS_NAME;
                task.DISPLAY_CLASS = item.S.DISPLAY_CLASS;
                task.PROJECT_TASK_PROGRESS = item.Q.PROJECT_TASK_PROGRESS;
                if(item.Q.TASK_START_DATE.HasValue)
                    task.TASK_START_DATE = item.Q.TASK_START_DATE.Value.ToString("yyyy-MM-dd"); 
                if (item.Q.TASK_END_DATE.HasValue)
                    task.TASK_END_DATE = item.Q.TASK_END_DATE.Value.ToString("yyyy-MM-dd");
                task.PROJECT_TASK_DESC = item.Q.PROJECT_TASK_DESC;
                if (item.C_LEFT.EMP_ID > 0)
                {
                    task.CLIENT_ID = item.C_LEFT.EMP_ID;
                    task.CLIENT_NAME = item.C_LEFT.FIRST_NAME + SqlConstants.stringWhiteSpace + item.C_LEFT.LAST_NAME;
                }

                task.MODIFIED_DATE = item.Q.MODIFIED_DATE.ToString("yyyy-MM-dd");

                task.MEMBERS = GetMembersLookUp(task.PROJECT_ID, 0, task.PROJECT_TASK_ID);

                TaskList.Add(task);
            }

            return TaskList;
        }

        public static List<PhasesDto> GetPhasesLookUp(int ProjectID, int? PhaseID)
        {            
            var query = db.PROJECT_PHASES.Where(w => w.PROJECT_ID == ProjectID && w.IS_CANCELED == false).OrderBy(o=>o.PHASE_ORDER).ToList();

            if (PhaseID.HasValue && PhaseID.Value > 0)
                query = query.Where(w => w.PROJECT_PHASE_ID == PhaseID.Value).ToList();

            var query2 = (from Q in query
                          join PRJ in db.PROJECTS on Q.PROJECT_ID equals PRJ.PROJECT_ID
                          join DP in db.DEF_PROJECT_PHASES on Q.PHASE_ID equals DP.PHASE_ID
                          join S in db.STATUS on Q.PROJECT_PHASE_STATUS equals S.STATUS_ID
                          from C_LEFT in db.EMPLOYEES.Where(w => w.EMP_ID == PRJ.CLIENT_ID).DefaultIfEmpty()
                          from S_LEFT in db.EMPLOYEES.Where(w => w.EMP_ID == Q.SUPERVISOR_ID).DefaultIfEmpty()
                          select new { Q, PRJ, S, C_LEFT, S_LEFT, DP }).ToList();

            List<PhasesDto> PhaseList = new List<PhasesDto>();
            foreach(var item in query2)
            {
                PhasesDto phase = new PhasesDto();
                phase.PROJECT_ID = item.Q.PROJECT_ID;
                phase.PHASE_ORDER = item.Q.PHASE_ORDER;
                phase.PROJECT_PHASE_ID = item.Q.PROJECT_PHASE_ID;
                if(item.Q.PHASE_START_DATE.HasValue)
                   phase.PHASE_START_DATE = item.Q.PHASE_START_DATE.Value.ToString("yyyy-MM-dd");
                if (item.Q.PHASE_END_DATE.HasValue)
                    phase.PHASE_END_DATE = item.Q.PHASE_END_DATE.Value.ToString("yyyy-MM-dd");
                                
                phase.PHASE_NAME = item.DP.PHASE_NAME;
                phase.PROJECT_PHASE_PROGRESS = item.Q.PROJECT_PHASE_PROGRESS;
                phase.STATUS_ID = item.Q.PROJECT_PHASE_STATUS;
                phase.STATUS_NAME = item.S.STATUS_NAME;
                phase.DISPLAY_CLASS = item.S.DISPLAY_CLASS;
                if (item.Q.SUPERVISOR_ID.HasValue)
                {
                    phase.SUPERVISOR_ID = item.Q.SUPERVISOR_ID.Value;
                    phase.SUP_NAME = item.S_LEFT.FIRST_NAME + SqlConstants.stringWhiteSpace + item.S_LEFT.LAST_NAME;
                }
                phase.PROJECT_PHASE_DESC = item.Q.PROJECT_PHASE_DESC;
                if(item.C_LEFT.EMP_ID >0)
                {
                    phase.CLIENT_ID = item.C_LEFT.EMP_ID;
                    phase.CLIENT_NAME = item.C_LEFT.FIRST_NAME + SqlConstants.stringWhiteSpace + item.C_LEFT.LAST_NAME;
                }

                phase.MODIFIED_DATE = item.Q.MODIFIED_DATE.ToString("yyyy-MM-dd");

                PhaseList.Add(phase);
            }

            return PhaseList;
        }

        public static List<QuonteDto> GetQuontesLookUp(int QuanteID,int CompanyID)
        {
            //var CompanyID = GetCompanyID();            
            var data = (from Q in db.QUOTES
                        join C in db.CLIENTS on Q.CLIENT_ID equals C.CLIENT_ID
                        join PT in db.PROJECT_TYPES on Q.PROJECT_TYPE_ID equals PT.PROJECT_TYPE_ID
                        join S in db.STATUS on Q.QUOTE_STATUS equals S.STATUS_ID
                        join CT in db.CITIES on Q.QUOTE_CITY equals CT.CITY_ID
                        join ST in db.STATES on Q.QUOTE_STATE equals ST.STATE_ID
                        join CN in db.COUNTRIES on Q.QUOTE_COUNTRY equals CN.COUNTRY_ID
                        join F in db.FLOOR_TYPES on Q.FLOOR_TYPE equals F.FLOOR_TYPE_ID
                        join M in db.MATERIALS on Q.MATERIAL_ID equals M.MATERIAL_ID
                        join STT in db.STREET on Q.STREET_SUFFIX_ID equals STT.STREET_ID
                        where Q.IS_CANCELED == false && Q.COMPANY_ID == CompanyID
                        select new QuonteDto
                        {
                            QUOTE_ID = Q.QUOTE_ID,
                            QUOTE_NUMBER = Q.QUOTE_NUMBER,
                            CLIENT_ID = Q.CLIENT_ID,
                            CLIENT_NAME = C.CLIENT_NAME,
                            PROJECT_TYPE_ID = Q.PROJECT_TYPE_ID,
                            PROJECT_TYPE_NAME = PT.PROJECT_TYPE_NAME,
                            QUOTE_STATUS = Q.QUOTE_STATUS,
                            QUOTE_NAME = Q.QUOTE_NAME,
                            DISPLAY_CLASS = S.DISPLAY_CLASS,
                            STATUS_NAME = S.STATUS_NAME,
                            ADR_LOT_NBR = Q.ADR_LOT_NBR,
                            ADR_UNIT_NBR = Q.ADR_UNIT_NBR,
                            ADR_STREET_NBR = Q.ADR_STREET_NBR,
                            ADR_STREET_NAME = Q.ADR_STREET_NAME,
                            QUOTE_ADDRESS = Q.QUOTE_ADDRESS,
                            QUOTE_COUNTRY = Q.QUOTE_COUNTRY,
                            COUNTRY_NAME = CN.COUNTRY_NAME,
                            QUOTE_STATE = Q.QUOTE_STATE,
                            STATE_NAME = ST.STATE_NAME,
                            QUOTE_CITY = Q.QUOTE_CITY,
                            CITY_NAME = CT.CITY_NAME,
                            QUOTE_POSTAL_CODE = Q.QUOTE_POSTAL_CODE,
                            QUOTE_AMOUNT = Q.QUOTE_AMOUNT,
                            CABINETS_NBR = Q.CABINETS_NBR,
                            QUOTE_DESC = Q.QUOTE_DESC,
                            QUOTE_START_DATE_STR = Q.QUOTE_START_DATE != null ?
                                                    Q.QUOTE_START_DATE.Year + SqlConstants.stringMinus +
                                                    (Q.QUOTE_START_DATE.Month > 9 ? Q.QUOTE_START_DATE.Month + SqlConstants.stringMinus : "0" + Q.QUOTE_START_DATE.Month + SqlConstants.stringMinus) +
                                                    Q.QUOTE_START_DATE.Day : null,
                            QUOTE_EXPIRY_DATE_STR = Q.QUOTE_EXPIRY_DATE != null ?
                                                    Q.QUOTE_EXPIRY_DATE.Value.Year + SqlConstants.stringMinus +
                                                    (Q.QUOTE_EXPIRY_DATE.Value.Month > 9 ? Q.QUOTE_EXPIRY_DATE.Value.Month + SqlConstants.stringMinus : "0" + Q.QUOTE_EXPIRY_DATE.Value.Month + SqlConstants.stringMinus) +
                                                    Q.QUOTE_EXPIRY_DATE.Value.Day : null,
                            DISPLAY = Q.DISPLAY,
                            FLOOR_TYPE = Q.FLOOR_TYPE,
                            FLOOR_TYPE_NAME = F.FLOOR_TYPE_NAME,
                            MATERIAL_ID = Q.MATERIAL_ID,
                            MATERIAL_NAME = M.MATERIAL_NAME,
                            STREET_SUFFIX_ID = Q.STREET_SUFFIX_ID,
                            STREET_NAME = STT.STREET_NAME,
                            CONTACT_DESC = Q.CONTACT_DESC,
                            USE_AS_TEMPLATE = Q.USE_AS_TEMPLATE,
                            CREATION_DATE_STR = Q.CREATION_DATE != null ?
                                                    Q.CREATION_DATE.Year + SqlConstants.stringMinus +
                                                    (Q.CREATION_DATE.Month > 9 ? Q.CREATION_DATE.Month + SqlConstants.stringMinus : "0" + Q.CREATION_DATE.Month + SqlConstants.stringMinus) +
                                                    Q.CREATION_DATE.Day : null,
                            MODIFIED_DATE_STR = Q.MODIFIED_DATE != null ?
                                                    Q.MODIFIED_DATE.Year + SqlConstants.stringMinus +
                                                    (Q.MODIFIED_DATE.Month > 9 ? Q.MODIFIED_DATE.Month + SqlConstants.stringMinus : "0" + Q.MODIFIED_DATE.Month + SqlConstants.stringMinus) +
                                                    Q.MODIFIED_DATE.Day : null,

                        }
                       ).OrderBy(o => o.CLIENT_NAME).ToList();

            if (QuanteID > 0)
                data = data.Where(w => w.QUOTE_ID == QuanteID).ToList();

            return data;
        }
    }
    
};
