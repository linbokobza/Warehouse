using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WareHouse22
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Users", action = "Registration", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "Home",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Users", action = "Home", id = UrlParameter.Optional }
           );

            routes.MapRoute(
              name: "UserProfile",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Users", action = "UserProfile", id = UrlParameter.Optional }
        );
        }
    }
}
