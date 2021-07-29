using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using JOB_MANAGER.Models.ComplexType;
using Newtonsoft.Json;

namespace JOB_MANAGER.CrossCuttingConsers.Security.Web
{
    public class SecurityUtilities
    {
        public Identitiy FormsAuthTicketToIdentity(FormsAuthenticationTicket ticket)
        {
            var identity = new Identitiy
            {
                Name = ticket.Name,
                AuthenticationType = "Forms",
                IsAuthenticated = true,
                UserInfo = JsonConvert.DeserializeObject<UserInfoDto>(ticket.UserData)
            };

            return identity;
        }
    }
}
