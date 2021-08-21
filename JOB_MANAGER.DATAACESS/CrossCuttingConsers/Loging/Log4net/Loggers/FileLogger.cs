using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.DATAACESS.CrossCuttingConsers
{
    public class FileLogger : LoggerService
    {
        public FileLogger() : base(LogManager.GetLogger("JsonFileLogger"))
        {
        }
    }
}
