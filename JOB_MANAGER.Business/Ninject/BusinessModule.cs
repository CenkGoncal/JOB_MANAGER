using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.Business.Concrete;
using JOB_MANAGER.DATAACESS.Models;
using Ninject.Modules;

namespace JOB_MANAGER.Business.Ninject
{
    public class BusinessModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ContractTypeDal>().To<ContractTypeDal>().InSingletonScope();
            Bind<IEmployeeService>().To<EmployeeManager>().InSingletonScope();
            Bind<EmployeeDal>().To<EmployeeDal>().InSingletonScope();
            
            Bind<ICompanyService>().To<CompanyManager>().InSingletonScope();
            Bind<CompanyDal>().To<CompanyDal>().InSingletonScope();
        }
    }
}
