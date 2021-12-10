using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficialProject1.Models
{
    public class GlobalVariables
    {
        public static string user_logined
        {
            get
            {
                return (string)HttpContext.Current.Application["user_logined"];
            }
            set
            {
                HttpContext.Current.Application["user_logined"] = value;
            }
        }
        public static int is_logined
        {
            get
            {
                return (int)HttpContext.Current.Application["is_logined"];
            }
            set
            {
                HttpContext.Current.Application["is_logined"] = value;
            }
        }
    }
}