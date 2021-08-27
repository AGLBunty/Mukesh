using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SMAS.Models
{
    public static class WebConfigurationSettings
    {
        public static String DMSServicesAPI = ConfigurationManager.AppSettings["DMSServicesAPI"];
        public static String DMSToken = ConfigurationManager.AppSettings["DMSToken"];
        public static String Mainurl = ConfigurationManager.AppSettings["Mainurl"];
    }
}