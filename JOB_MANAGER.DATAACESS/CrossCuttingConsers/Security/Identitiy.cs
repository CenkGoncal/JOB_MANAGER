using System.Security.Principal;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.DATAACESS.CrossCuttingConsers
{
    public class Identitiy : IIdentity
    {        
        public string Name { get; set; }
        public string AuthenticationType { get; set; }
        public bool IsAuthenticated { get; set; }
        public bool cenk { get; set; }

        public UserInfoDto UserInfo { get; set; }

    }

}
