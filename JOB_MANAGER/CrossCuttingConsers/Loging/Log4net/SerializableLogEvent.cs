using log4net.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.CrossCuttingConsers.Loging.Log4net
{
    public class SerializableLogEvent
    {
        private LoggingEvent loggingEvent;

        public SerializableLogEvent(LoggingEvent loggingEvent)
        {
            this.loggingEvent = loggingEvent;
        }

        public string Username => loggingEvent.UserName;
        public object MessageObject => loggingEvent.MessageObject;

    }
}