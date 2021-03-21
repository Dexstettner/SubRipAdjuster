using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SubRipAdjuster
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SubRipHistory",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "SubRipController", action = "SubRipHistory", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "SubRipController", action = "SubRipForm", id = UrlParameter.Optional }
            );
        }
    }
}
