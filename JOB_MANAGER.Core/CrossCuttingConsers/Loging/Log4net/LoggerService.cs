using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.CrossCuttingConsers.Loging.Log4net
{
    public class LoggerService
    {
        private ILog log;

        public LoggerService(ILog log)
        {
            this.log = log;
        }

        public bool IsInfoEnabled => log.IsInfoEnabled;
        public bool IsDebugEnabled => log.IsDebugEnabled;
        public bool IsWarmEnabled => log.IsWarnEnabled;
        public bool IsFatalEnabled => log.IsFatalEnabled; 
        public bool IsErorEnabled => log.IsFatalEnabled;

        public void Info(object logMessage)
        {
            if(IsInfoEnabled)
            {
                log.Info(logMessage);
            }
        }

        public void Debug(object logMessage)
        {
            if (IsDebugEnabled)
            {
                log.Debug(logMessage);
            }
        }

        public void Warn(object logMessage)
        {
            if (IsDebugEnabled)
            {
                log.Warn(logMessage);
            }
        }

        public void Fatal(object logMessage)
        {
            if (IsFatalEnabled)
            {
                log.Fatal(logMessage);
            }
        }

        public void Error(object logMessage)
        {
            if (IsErorEnabled)
            {
                log.Error(logMessage);
            }
        }
    }
}