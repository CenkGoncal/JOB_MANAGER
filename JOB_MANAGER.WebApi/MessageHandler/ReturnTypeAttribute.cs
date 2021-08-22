using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.WebApi.MessageHandler
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public sealed class ReturnTypeAttribute : Attribute
    {
        readonly Type returnType;

        public ReturnTypeAttribute(Type returnType)
        {
            this.returnType = returnType;
        }

        public Type ReturnType
        {
            get { return returnType; }
        }
    }
}
