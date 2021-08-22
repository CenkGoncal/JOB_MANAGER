using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace JOB_MANAGER.DATAACESS.CrossCuttingConsers.Utilities.Common
{
    public class WcfPrpxy<T>
    {
        public static T CreateChanel()
        {
            string baseadress = ConfigurationManager.AppSettings["ServiceAdress"];
            string adress = string.Format(baseadress, typeof(T).Name.Substring(1));

            var binding = new BasicHttpBinding();

            var chanel = new ChannelFactory<T>(binding, adress);

            return chanel.CreateChannel();
        }
    }
}
