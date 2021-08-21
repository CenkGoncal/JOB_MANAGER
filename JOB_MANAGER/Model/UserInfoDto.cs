using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.Models.ComplexType
{
    public class UserInfoDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmpName { get; set; }
        public string Email { get; set; }
        public string EmailPassword { get; set; }
        public int CompanyId { get; set; }
        public string RoleName { get; set; }
        public List<string> MenuRoles { get; set; }

    }
}
