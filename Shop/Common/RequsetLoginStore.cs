using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System
{
    public class RequsetLoginStoreAttribute : AuthorizeAttribute
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        public RequsetLoginStoreAttribute()
        {

        }
        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return base.OnCacheAuthorization(httpContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var user = (User)httpContext.Session["UU!#user"] ?? new User();
            List<int> accessStoreID = db.AccessStores.Select(a => (int)a.userID).ToList();
            return httpContext.Session["UU!#user"] != null && accessStoreID.Contains(user.userID);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/user/LoginAdmin");
        }
    }
}