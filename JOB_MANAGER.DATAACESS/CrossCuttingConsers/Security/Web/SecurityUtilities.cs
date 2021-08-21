using System.Web.Security;
using JOB_MANAGER.DATAACESS.Models;
using Newtonsoft.Json;

namespace JOB_MANAGER.DATAACESS.CrossCuttingConsers
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
