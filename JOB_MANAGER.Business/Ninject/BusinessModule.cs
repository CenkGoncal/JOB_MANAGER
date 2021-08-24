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
            Bind<IEmployeeService>().To<EmployeeManager>().InSingletonScope();
            Bind<EmployeeDal>().To<EmployeeDal>().InSingletonScope();
            
            Bind<ICompanyService>().To<CompanyManager>().InSingletonScope();
            Bind<CompanyDal>().To<CompanyDal>().InSingletonScope();

            Bind<IContractTypeService>().To<ContractTypeManager>().InSingletonScope();
            Bind<ContractTypeDal>().To<ContractTypeDal>().InSingletonScope();

            Bind<IRoleService>().To<RoleManager>().InSingletonScope();
            Bind<RoleDal>().To<RoleDal>().InSingletonScope();

            Bind<IContractTypeService>().To<ContractTypeManager>().InSingletonScope();
            Bind<RoleDal>().To<RoleDal>().InSingletonScope();

            Bind<ITitleService>().To<TitleManager>().InSingletonScope();
            Bind<TitleDal>().To<TitleDal>().InSingletonScope();

            Bind<IDepartmentService>().To<DepartmentManager>().InSingletonScope();
            Bind<DepartmentDal>().To<DepartmentDal>().InSingletonScope();

            Bind<ICountyService>().To<CountyManager>().InSingletonScope();
            Bind<CountyDal>().To<CountyDal>().InSingletonScope();

            Bind<IStateService>().To<StateManager>().InSingletonScope();
            Bind<StateDal>().To<StateDal>().InSingletonScope();

            Bind<ICityService>().To<CityManager>().InSingletonScope();
            Bind<CitiesDal>().To<CitiesDal>().InSingletonScope();

            Bind<IStreetService>().To<StreetManager>().InSingletonScope();
            Bind<StreetDal>().To<StreetDal>().InSingletonScope();

            Bind<IParametrerService>().To<ParameterManager>().InSingletonScope();
            Bind<ParameterDal>().To<ParameterDal>().InSingletonScope();

            Bind<ICompanyTypeService>().To<CompanyTypeManager>().InSingletonScope();
            Bind<CompanyTypeDal>().To<CompanyTypeDal>().InSingletonScope();

            Bind<IVehicleBodyServices>().To<VehicleBodyManager>().InSingletonScope();
            Bind<VehicleBodyDal>().To<VehicleBodyDal>().InSingletonScope();

            Bind<IVehicleMakeService>().To<VehicleMakeManager>().InSingletonScope();
            Bind<VehicleMakeDal>().To<VehicleMakeDal>().InSingletonScope();

            Bind<IStatusService>().To<StatusManager>().InSingletonScope();
            Bind<StatusDal>().To<StatusDal>().InSingletonScope();

            Bind<IRoleMenuService>().To<RoleMenuManager>().InSingletonScope();
            Bind<RoleMenuDal>().To<RoleMenuDal>().InSingletonScope();

            
        }
    }
}
