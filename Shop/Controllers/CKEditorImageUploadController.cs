using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
namespace Shop.Controllers
{
    public class CKEditorImageUploadController : Controller
    {
        Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        // GET: ImageUpload
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Upload(HttpPostedFileBase upload)
        {
            var c = new CKEditorImageUpload();
            db.CKEditorImageUploads.Add(c);
            db.SaveChanges();
            if (upload != null)
            {
                upload.SaveAs(Server.MapPath("~/images/CKeditor/" + c.ckEditorImageUploadID + ".jpg"));

            }
            var obj = new { uploaded = 1, fileName = c.ckEditorImageUploadID + ".jpg", url = "/images/CKeditor/" + c.ckEditorImageUploadID + ".jpg" };
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

    }
}