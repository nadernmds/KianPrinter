using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;

namespace Shop.Controllers
{
    public class BannerController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        // GET: Banner
        public ActionResult Index()
        {
            return View(db.Banners.ToList());
        }
        [RequsetLogin(2)]
        // GET: Banner/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }
        [RequsetLogin(2)]
        // GET: Banner/Create
        public ActionResult Create()
        {
            return View();
        }
        [RequsetLogin(2)]
        // POST: Banner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "bannerID,url,show")] Banner banner,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                db.Banners.Add(banner);
                db.SaveChanges();

                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/Banner/" + banner.bannerID + ".jpg"));
                }
                return RedirectToAction("Index");
            }

            return View(banner);
        }
        [RequsetLogin(2)]
        // GET: Banner/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }
        [RequsetLogin(2)]
        // POST: Banner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "bannerID,url,show")] Banner banner,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (banner.show == null)
                {
                    banner.show = false;
                }
                db.Entry(banner).State = EntityState.Modified;
                db.SaveChanges();
                if(file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/Banner/" + banner.bannerID + ".jpg"));
                }
                return RedirectToAction("Index");
            }
            return View(banner);
        }
        [RequsetLogin(2)]
        // GET: Banner/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }
        [RequsetLogin(2)]
        // POST: Banner/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
