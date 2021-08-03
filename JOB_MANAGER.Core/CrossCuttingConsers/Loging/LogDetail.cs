using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.CrossCuttingConsers.Loging
{
    public class LogDetail
    {
        public string Fullname { get; set; }
        public string MethodName { get; set; }
        public List<LogParameter> Parameters { get; set; }
        public string Message { get; set; }
    }
}