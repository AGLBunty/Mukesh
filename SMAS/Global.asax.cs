using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SMAS
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            string path = string.Empty;

            string strDomin = HttpContext.Current.Request.Url.Host.ToString();
            if (!Request.IsSecureConnection && strDomin != "localhost")
            {
                    path = string.Format("https{0}", Request.Url.AbsoluteUri.Substring(4));
                    Response.Redirect(path);
            }
        }


        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (exception.Message != null)
            {
                string action= "HttpError404";

                //switch (httpException.GetHttpCode())
                //{
                //    case 404:
                //        // page not found
                //        action = "HttpError404";
                //        break;
                //    case 500:
                //        // server error
                //        action = "HttpError500";
                //        break;
                //    default:
                //        action = "General";
                //        break;
                //}

                // clear error on server
                Server.ClearError();

                //Response.Redirect(String.Format("~/Error/{0}/?message={1}", action, exception.Message));
                Response.Redirect(String.Format("~/Error/{0}", action));
            }

        }
    }
}
