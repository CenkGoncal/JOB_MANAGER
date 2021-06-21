using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.Models.Abstact
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