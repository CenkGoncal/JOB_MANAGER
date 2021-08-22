using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using JOB_MANAGER.Business.Abstract;
using JOB_MANAGER.Business.Ninject;
using JOB_MANAGER.Business.ServiceContacts.Wcf;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.WCF
{
    // NOT: "Service1" sınıf adını kodda, svc'de ve yapılandırma dosyasında birlikte değiştirmek için "Yeniden Düzenle" menüsündeki "Yeniden Adlandır" komutunu kullanabilirsiniz.
    // NOT: Bu hizmeti test etmek üzere WCF Test İstemcisi'ni başlatmak için lütfen Çözüm Gezgini'nde Service1.svc'yi veya Service1.svc.cs'yi seçin ve hata ayıklamaya başlayın.
    public class CompayService : ICompanyServiceDetail
    {
        private ICompanyService _companyService = InstanceFactory.GetInstance<ICompanyService>();
        public List<CompanyExtented> GetAll()
        {
            return _companyService.GetAll();
        }
    }
}
