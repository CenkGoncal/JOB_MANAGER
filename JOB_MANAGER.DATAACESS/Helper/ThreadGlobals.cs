using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using JOB_MANAGER.DATAACESS.Models;

namespace JOB_MANAGER.DATAACESS.Helper
{
    public static class ThreadGlobals
    {
        public static ThreadLocal<UserInfoDto> UserAuthInfo = new ThreadLocal<UserInfoDto>();
    }
}
