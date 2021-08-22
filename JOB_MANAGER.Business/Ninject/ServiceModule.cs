using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.DATAACESS.CrossCuttingConsers.Utilities.Common;
using Ninject.Modules;

namespace JOB_MANAGER.Business.Ninject
{
    //WCF servisi üzerinden web mvc(JOB_MANAGER) çalıştırmak için yazıldı buda bize ister busines üzerinden dll den ister servis üzerinden çalışmamızı sağlıcak
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ICompanyService>().ToConstant(WcfPrpxy<ICompanyService>.CreateChanel());
        }
    }
}
