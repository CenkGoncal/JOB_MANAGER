using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.Business.Abstract
{
    public interface IDepartmentService : IService<DEPARTMENTS, DepartmentExtented>
    {
        List<DepartmentExtented> GetDepartmentCompany(int CompanyId);
    }
}
