using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.CrossCuttingConsers.Loging.Log4net.Loggers
{
    public class DatabaseLogger : LoggerService
    {
        public DatabaseLogger() : base(LogManager.GetLogger("DatabaseLogger"))
        {
        }
    }
}
