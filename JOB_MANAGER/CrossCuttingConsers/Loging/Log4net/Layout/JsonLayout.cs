﻿using log4net.Core;
using log4net.Layout;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.CrossCuttingConsers.Loging.Log4net.Layout
{
    //Log Data tipini tanımlamak için kullanılcak
    public class JsonLayout : LayoutSkeleton
    {
        public override void ActivateOptions()
        {
            
        }

        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            var logevent = new SerializableLogEvent(loggingEvent);
            var json = JsonConvert.SerializeObject(logevent, Formatting.Indented);
            writer.WriteLine(json);
        }
    }
}
