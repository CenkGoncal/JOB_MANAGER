namespace JOB_MANAGER.DATAACESS.Models
{
    public class Entity
    {
        public interface IEntity
        {

        }

        public interface IViewDto
        {
            string CREATION_DATE { get; set; }
            string MODIFIED_DATE { get; set; }
            string CREATE_BY { get; set; }
        }
    }
}
