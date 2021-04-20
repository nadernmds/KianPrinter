using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System
{
    public class RequsetLoginAttribute : AuthorizeAttribute
    {
        private int[] UserGroupID;
        public RequsetLoginAttribute(params int[] UserGroupID)
        {

            this.UserGroupID = UserGroupID;

        }
        public RequsetLoginAttribute()
        {

        }
        protected override HttpValidationStatus OnCacheAuthorization(HttpContextBase httpContext)
        {
            return base.OnCacheAuthorization(httpContext);
        }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {

            User user = null;
            if (httpContext.Session["UU!#user"] != null)
            {
                user = (User)httpContext.Session["UU!#user"];

            }
            else
            {
                if (httpContext.Request.Cookies["c_uuu_rizkar"] != null)
                {
                    var u = httpContext.Request.Cookies["c_uuu_rizkar"];
                    var id = Convert.ToInt32(Text.Encoding.UTF8.GetString(Web.Security.MachineKey.Unprotect(Convert.FromBase64String(u["id"]))));

                    if (httpContext.Session["UU!#user"] == null)
                    {
                        var db = new Rizkaran_SiteEntities();
                        user = db.Users.Find(id);
                        httpContext.Session["UU!#user"] = user;
                    }
                }
                else
                {
                    return false;
                }
            }
            if (UserGroupID != null)
            {
                return httpContext.Session["UU!#user"] != null && UserGroupID.Contains(user.usergroupID.Value);
            }
            else
            {
                return httpContext.Session["UU!#user"] != null;
            }


            //return (httpContext.Session["UU!#user"] != null);
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/user/login");
        }
    }
}