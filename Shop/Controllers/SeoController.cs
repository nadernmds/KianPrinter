using MvcSitemapBuilder;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shop.Controllers
{
    public class SeoController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        // GET: Seo
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult sitemap()
        {
            SitemapBuilder builder = new SitemapBuilder();

            builder.AppendUrl(Url.Action("Index", "Home", null, this.Request.Url.Scheme), ChangefreqEnum.daily);
            

            
            builder.AppendUrl(Url.Action("register", "user", null, this.Request.Url.Scheme), ChangefreqEnum.monthly);
            builder.AppendUrl(Url.Action("login", "user", null, this.Request.Url.Scheme), ChangefreqEnum.monthly);
            builder.AppendUrl(Url.Action("contact", "home", null, this.Request.Url.Scheme), ChangefreqEnum.monthly);
            builder.AppendUrl(Url.Action("specialOffers", "product", null, this.Request.Url.Scheme), ChangefreqEnum.monthly);
            builder.AppendUrl(Url.Action("about", "Home", null, this.Request.Url.Scheme), ChangefreqEnum.monthly);
            builder.AppendUrl(Url.Action("privacy", "home", null, this.Request.Url.Scheme), ChangefreqEnum.daily);
            builder.AppendUrl(Url.Action("faqs", "home", null, this.Request.Url.Scheme), ChangefreqEnum.daily);

            var pID = db.Products.Select(c => c.productID);
            foreach (var item in pID)
            {
                builder.AppendUrl(Url.Action("show", "product", new { id = item }, this.Request.Url.Scheme), ChangefreqEnum.daily);
            }
            var brands = db.Brands.Select(c => c.brandID);
            foreach (var item in brands)
            {
                builder.AppendUrl(Url.Action("list", "productcategory", new { b = item }, this.Request.Url.Scheme), ChangefreqEnum.daily);
            }
            var categories = db.ProductCategories.Select(c => c.categoryID);
            foreach (var item in categories)
            {
                builder.AppendUrl(Url.Action("list", "productcategory", new { c = item }, this.Request.Url.Scheme), ChangefreqEnum.monthly);
            }

            
            return new XmlViewResult(builder.XmlDocument);
        }
        public ActionResult robots()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

            stringBuilder.AppendLine("user-agent: *");
            stringBuilder.AppendLine("disallow: ");
            //stringBuilder.AppendLine("allow: /error/foo");
            stringBuilder.Append("sitemap: https://www.kianprinter.com/sitemap.xml");
            return this.Content(stringBuilder.ToString(), "text/plain", System.Text.Encoding.UTF8);
        }
        public ActionResult googlebeb064e65fcc93a1()
        {
            return View();
        }
    }
}