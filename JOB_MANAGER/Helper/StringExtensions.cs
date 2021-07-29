using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JOB_MANAGER.Helper
{
    public static class StringExtensions
    {
        public static bool IsEmail(this string input)
        {
            return input.Contains("@");
        }
    }
}
