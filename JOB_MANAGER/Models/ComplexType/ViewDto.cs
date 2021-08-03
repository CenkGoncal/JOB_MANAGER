
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using static JOB_MANAGER.Models.Abstact.Entity;

namespace JOB_MANAGER.Models
{ 
    public class EmployeeDto: EMPLOYEES
    {
        public string EMP_NAME { get; set; }
        public string EMP_TITLE { get; set; }
        public string EMP_CONTRACT { get; set; }
        public string EMP_DEPARTMENT { get; set; }
        public string EMP_EMAIL { get; set; }
        public string HIRED_DATE_STR { get; set; }
        public string EMP_STATUS { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public string EMP_ROLE { get; set; }
        public int EMP_COMPANY_ID { get; set; }
    }
    public class QuonteDto : QUOTES
    {
        public string CLIENT_NAME { get; set; }
        public string PROJECT_TYPE_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public string STATUS_NAME { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string STATE_NAME { get; set; }
        public string CITY_NAME { get; set; }
        public string QUOTE_START_DATE_STR { get; set; }
        public string QUOTE_EXPIRY_DATE_STR { get; set; }
        public string MATERIAL_NAME { get; set; }
        public string FLOOR_TYPE_NAME { get; set; }
        public string STREET_NAME { get; set; }
        public string CREATION_DATE_STR { get; set; }
        public string MODIFIED_DATE_STR { get; set; }
    }

    public class PhasesExtented:PROJECT_PHASES
    {
        public List<int> ASSINGED_MEMBERS_ARR { get; set; }
    }    
    public class TaskExtented:PROJECT_TASK
    {
        public List<int> ASSINGED_MEMBERS_ARR { get; set; }
    }
    public class NotesExtented:NOTES
    {
        public List<int> ASSINGED_MEMBERS_ARR { get; set; }
        public List<int> TO_REMIND_ARR { get; set; }
    }
    public class ContactViewDto
    {
        public int CONTACT_ID { get; set; }
        public string CONTACT_FIRST_NAME { get; set; }
        public string CONTACT_LAST_NAME { get; set; }
        public string CONTACT_NAME { get; set; }
        public int CLIENT_ID { get; set; }        
        public string CLIENT_NAME { get; set; }
        public string CONTACT_TITLE { get; set; }
        public int CONTACT_STATUS { get; set; }
        public string STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public int? CONTACT_ROLE { get; set; }
        public string ROLE_NAME { get; set; }
        public string CONTACT_PHONE { get; set; }
        public string CONTACT_PHONE_EX { get; set; }
        public string CONTACT_PHONE_ALL { get; set; }
        public string CONTACT_MOBILE { get; set; }
        public bool IS_PRIMARY { get; set; }
        public string CONTACT_EMAIL { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class ProjectViewDto
    {
        public int PROJECT_ID { get; set; }
        public int? QUOTE_ID { get; set; }
        public string QUOTE_NUMBER { get; set; }
        public string PROJECT_NUMBER { get; set; }
        public int CLIENT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public string CLIENT_PO_NBR { get; set; }
        public int PROJECT_TYPE_ID { get; set; }
        public string PROJECT_TYPE_NAME { get; set; }
        public int PROJECT_STATUS { get; set; }
        public string PROJECT_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public string STATUS_NAME { get; set; }
        public string ADR_LOT_NBR { get; set; }
        public string ADR_UNIT_NBR { get; set; }
        public string ADR_STREET_NBR { get; set; }
        public string ADR_STREET_NAME { get; set; }
        public string PROJECT_ADDRESS { get; set; }
        public int PROJECT_COUNTRY { get; set; }
        public string COUNTRY_NAME { get; set; }
        public int PROJECT_STATE { get; set; }
        public string STATE_NAME { get; set; }
        public int PROJECT_CITY { get; set; }
        public string CITY_NAME { get; set; }
        public string PROJECT_POSTAL_CODE { get; set; }
        public decimal PROJECT_AMOUNT { get; set; }
        public decimal PROJECT_ESTIMATED_VAL { get; set; }
        public decimal PROJECT_ACTUAL_VAL { get; set; }
        public int? CLIENT_SUPRVISOR { get; set; }
        public string CLI_SUPERVISOR_NAME { get; set; }
        public int? PROJECT_SUPERVISOR { get; set; }
        public string PRO_SUPERVISOR_NAME { get; set; }
        public int PROJECT_PROGRESS { get; set; }
        public int CABINETS_NBR { get; set; }
        public string PROJECT_DESC { get; set; }
        public string INSTALLATION_DATE { get; set; }
        public string TIME { get; set; }
        public string PROJECT_START_DATE { get; set; }
        public DateTime PROJECT_START_DATE_DT { get; set; }
        public string PROJECT_END_DATE { get; set; }
        public DateTime? PROJECT_END_DATE_DT { get; set; }
        public bool DISPLAY { get; set; }
        public short FLOOR_TYPE { get; set; }
        public string FLOOR_TYPE_NAME { get; set; }
        public int MATERIAL_ID { get; set; }
        public string MATERIAL_NAME { get; set; }
        public short? STREET_SUFFIX_ID { get; set; }
        public string STREET_NAME { get; set; }
        public bool USE_AS_TEMPLATE { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }

        public ProjectDto ProjectDto { get; set; }
    }
    
    public class ProjectDto
    {
        public List<PhasesDto> Phase { get; set; }
        public List<MembersDto> Members { get; set; }
        public List<TasksDto> Task { get; set; }
        public List<NoteDto> Note { get; set; }
        public List<DocumentDto> Document { get; set; }
    }

    public class MembersDto
    {
        public int PROJECT_ID { get; set; }
        public int PROJECT_MEMBER_ID { get; set; }
        public int EMP_ID { get; set; }
        public string EMP_NAME { get; set; }
        public byte[] EMP_IMG { get; set; }
        public int SUPERVISOR_ID { get; set; }
        public string SUP_NAME { get; set; }
        public bool IS_SUPERVISOR { get; set; }
        public int TITLE_ID { get; set; }
        public string TITLE_NAME { get; set; }
        public int PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
    }
   
    public class PhasesDto
    {
        public int PROJECT_ID { get; set; }
        public int PHASE_ORDER { get; set; }
        public int PROJECT_PHASE_ID { get; set; }
        public string PHASE_START_DATE { get; set; }
        public string PHASE_END_DATE { get; set; }
        public string PHASE_NAME { get; set; }
        public int PROJECT_PHASE_PROGRESS { get; set; }
        public int STATUS_ID { get; set; }
        public string STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public int SUPERVISOR_ID { get; set; }
        public string SUP_NAME { get; set; }        
        public string PROJECT_PHASE_DESC { get; set; }
        public int CLIENT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public string MODIFIED_DATE { get; set; }
        public List<TasksDto> TASKS { get; set; }
        public List<MembersDto> MEMBERS { get; set; }
    }

    public class TasksDto
    {
        public int PROJECT_ID { get; set; }        
        public int PROJECT_TASK_ID { get; set; }        
        public int TASK_ORDER { get; set; }
        public int PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public string PROJECT_TASK_NAME { get; set; }
        public int CLIENT_ID { get; set; }
        public string CLIENT_NAME { get; set; }
        public int STATUS_ID { get; set; }
        public string STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public int PROJECT_TASK_PROGRESS { get; set; }
        public string TASK_START_DATE { get; set; }
        public string TASK_END_DATE { get; set; }        
        public string PROJECT_TASK_DESC { get; set; }
        public string MODIFIED_DATE { get; set; }
        public List<MembersDto> MEMBERS { get; set; }
    }

    public class NoteDto
    {
        public int NOTE_ID { get; set; }
        public int UNIQ_ID { get; set; }
        public int NOTE_KIND_ID { get; set; }
        public int NOTE_TYPE_ID { get; set; }
        public string NOTE_TYPE_NAME { get; set; }
        public string NOTE_DESC { get; set; }
        public string NOTE_SUBJECT { get; set; }
        public int PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public bool SET_REMINDER { get; set; }
        public string REMINDER_DATE { get; set; }
        public string REMINDER_TIME { get; set; }
        public List<int> ASSINGED_MEMBERS_ARR { get; set; }
        public List<int> TO_REMIND_ARR { get; set; }
    }

    public class DocumentDto
    {
        public int DOC_ID { get; set; }
        public int DOC_KIND { get; set; }
        public int UNIQ_ID { get; set; }
        public int DOCUMENT_TYPE_ID { get; set; }
        public string DOCUMENT_TYPE_NAME { get; set; }
        public string DOC_NAME { get; set; }
        public string DOC_PATH { get; set; }
        public string DOC_DESC { get; set; }
        public string DOC_FOLDER_NAME { get; set; }
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public int PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public int NOTE_ID { get; set; }
        public string NOTE_NAME { get; set; }
        public bool IS_CANCELED { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string FONT_AWESOME_ICON { get; set; }
    }

    public class StatusDto
    {
        public int STATUS_ID { get; set; }
        public string STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }

    }

    public class DasboradDto
    {
        public List<JOBSSUMMARY_VIEW> JobSummary { get; set; }
        public List<JOBSPHASESSUMMARY_VIEW> PhaseSummary { get; set; }
    }

    #region defPhases
    public class ProjectTypeWithPhaseTask
    {
        public int PROJECT_TYPE_ID { get; set; }
        public string PROJECT_TYPE_NAME { get; set; }
        public List<DefProjectPhaseExtented> PHASE { get; set; }
    }
    public class DefPhasesDto
    {
        public int PHASE_ID { get; set; }
        public string PHASE_NAME { get; set; }
        public int PROJECT_TYPE_ID { get; set; }
        public int PHASE_ORDER { get; set; }
        public string PHASE_DESC { get; set; }
        public string PHASE_COLOR { get; set; }
        public string WIDGET_COLOR { get; set; }
        public short? CORRESPONDANT_STATUS { get; set; }
        public bool IS_CANCELED { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string CREATE_BY { get; set; }
        public List<DefTaskDto> TASK { get; set; }
    }

    public class DefTaskDto
    {
        public int TASK_ID { get; set; }
        public string TASK_NAME { get; set; }
        public string TASK_DESC { get; set; }
        public int PROJECT_TYPE_ID { get; set; }
        public int PHASE_ID { get; set; }
        public int TASK_ORDER { get; set; }
        public int ALLOCATE_SUPERVISOR { get; set; }
        public bool COMPLETE_ON_UPLOAD { get; set; }
        public bool IS_CANCELED { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string CREATE_BY { get; set; }
    }
    #endregion


    public class Entities:DbContext
    {
        public DbSet<EMPLOYEES> Employee { get; set; }
        public DbSet<QUOTES> Quotes { get; set; }        
    }

    public class ShowState
    {
        public bool isError { get; set; }
        public string ErrorMessage { get; set; }
    }

    public interface IDataResult<out T> : IViewDto
    {
        T Data { get; }
    }

    public class ViewData<T> : IDataResult<T>
    {
        public ViewData(T data)
        {
            Data = data;
        }

        public ViewData()
        {

        }

        public T Data { get; }

        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class ContractTypesExtented : CONTRACT_TYPES, IViewDto
    {
        public string CREATE_BY { get ; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class RolesExtented : ROLES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class TitleExtented : TITLES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }


    public class DepartmentExtented : DEPARTMENTS, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class CountryExtented : COUNTRIES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class StateExtented : STATES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string COUNTRY_NAME { get; set; }
    }

    public class CitiesExtented : CITIES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string STATE_NAME { get; set; }
    }

    public class StreetExtented : STREET, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string COUNTRY_CODE { get; set; }
        public string FLAG_URL { get; set; }
        public string COUNTRY_NAME { get; set; }
        public int COUNTRY_ID { get; set; }
        public bool IS_DEFAULT { get; set; }
    }

    public class ParameterExtented : PARAMETERS, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class CompanyTypesExtented : COMPANY_TYPES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }
    public class VehiclesExtented : VEHICLES, IViewDto
    {
        public string VEHICLE_STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public string DRIVER_NAME { get; set; }
        public string REGISTRATION_EXPIRY { get; set; }
        public string VEHICLE_MAKE_NAME { get; set; }
        public string BODY_TYPE_NAME { get; set; }
        public string VEHICLE_MODEL_NAME { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }       
    }

    public class VehicleBodysExtented : VEHICLE_BODY_TYPES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class VehicleMakeExtented : VEHICLE_MAKES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class VehicleModelExtented : VEHICLE_MODELS, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class StatusExtented : STATUS, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class RoleMenuExtented : ROLE_MENU, IViewDto
    {
        public string ROLE_NAME { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class FloorTypeExtented : FLOOR_TYPES, IViewDto
    {
        public string ROLE_NAME { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class MaterialExtented : MATERIALS, IViewDto
    {        
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class ProjectTypeExtented : PROJECT_TYPES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class NoteTypeExtented : NOTE_TYPES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class DocumentTypeExtented : DOCUMENT_TYPES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class DefProjectPhaseExtented : DEF_PROJECT_PHASES, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public List<DefTaskExtented> TASK { get; set; }
    }

    public class DefTaskExtented : DEF_TASKS, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class EmployeeExtended : EMPLOYEES,IViewDto
    {
        public string EMP_NAME { get; set; }
        public string EMP_TITLE { get; set; }
        public string EMP_CONTRACT { get; set; }
        public string EMP_DEPARTMENT { get; set; }
        public string HIRED_DATE_STR { get; set; }
        public string EMP_STATUS { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class ClientExtented : CLIENTS, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string COMPANY_TYPE_NAME { get; set; }
        public string STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public string COUNTRY_NAME { get; set; }
        public string STATE_NAME { get; set; }
        public string CITY_NAME { get; set; }        
        public string CLIENT_SINCE { get; set; }
    }

    public class ContactsExtented : CONTACTS,IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string CONTACT_NAME { get; set; }
        public string CLIENT_NAME { get; set; }
        public string STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
        public string ROLE_NAME { get; set; }
        public string CONTACT_PHONE_ALL { get; set; }
    }

    public class CompanyExtented : COMPANY, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
    }

    public class SuplierExtended : SUPPLIERS, IViewDto
    {
        public string CREATE_BY { get; set; }
        public string CREATION_DATE { get; set; }
        public string MODIFIED_DATE { get; set; }
        public string SUPPLIER_STATUS_NAME { get; set; }
        public string DISPLAY_CLASS { get; set; }
    }
}
