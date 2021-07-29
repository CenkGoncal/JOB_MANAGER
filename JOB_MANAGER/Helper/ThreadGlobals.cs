using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using JOB_MANAGER.Models.ComplexType;

namespace JOB_MANAGER.Helper
{
    public static class ThreadGlobals
    {
        public static ThreadLocal<UserInfoDto> UserAuthInfo = new ThreadLocal<UserInfoDto>();
    }
}
