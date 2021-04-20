using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapMvcAttributeRoutes();

            routes.MapRoute("googlebeb064e65fcc93a1.html",
                "googlebeb064e65fcc93a1.html",
            new { controller = "seo", action = "googlebeb064e65fcc93a1" });


            routes.MapRoute("robots.txt",
                "robots.txt",
            new { controller = "seo", action = "robots" });



            routes.MapRoute("sitemap.xml",
            "sitemap.xml",
            new { controller = "seo", action = "sitemap" });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
