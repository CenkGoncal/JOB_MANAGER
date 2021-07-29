using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using JOB_MANAGER.Models;
using JOB_MANAGER.Models.ComplexType;

namespace JOB_MANAGER.CrossCuttingConsers.Security
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
