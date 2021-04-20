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
    public class SocialController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        // GET: Social
        public ActionResult Index()
        {
            var socials = db.Socials.Include(s => s.Content).Include(s => s.Employee);
            return View(socials.ToList());
        }
        [RequsetLogin(2)]
        // GET: Social/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Social social = db.Socials.Find(id);
            if (social == null)
            {
                return HttpNotFound();
            }
            return View(social);
        }
        [RequsetLogin(2)]
        // GET: Social/Create
        public ActionResult Create()
        {
            ViewBag.contentID = new SelectList(db.Contents, "contentID", "employeText");
            ViewBag.employeID = new SelectList(db.Employees, "employeID", "employeName");
            return View();
        }
        [RequsetLogin(2)]
        // POST: Social/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "socialID,type,socialIcon,url,contentID,employeID")] Social social)
        {
            if (ModelState.IsValid)
            {
                db.Socials.Add(social);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.contentID = new SelectList(db.Contents, "contentID", "employeText", social.contentID);
            ViewBag.employeID = new SelectList(db.Employees, "employeID", "employeName", social.employeID);
            return View(social);
        }
        [RequsetLogin(2)]
        // GET: Social/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Social social = db.Socials.Find(id);
            if (social == null)
            {
                return HttpNotFound();
            }
            ViewBag.contentID = new SelectList(db.Contents, "contentID", "employeText", social.contentID);
            ViewBag.employeID = new SelectList(db.Employees, "employeID", "employeName", social.employeID);
            return View(social);
        }
        [RequsetLogin(2)]
        // POST: Social/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "socialID,type,socialIcon,url,contentID,employeID")] Social social)
        {
            if (ModelState.IsValid)
            {
                db.Entry(social).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.contentID = new SelectList(db.Contents, "contentID", "employeText", social.contentID);
            ViewBag.employeID = new SelectList(db.Employees, "employeID", "employeName", social.employeID);
            return View(social);
        }
        [RequsetLogin(2)]
        // GET: Social/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Social social = db.Socials.Find(id);
            if (social == null)
            {
                return HttpNotFound();
            }
            return View(social);
        }
        [RequsetLogin(2)]
        // POST: Social/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Social social = db.Socials.Find(id);
            db.Socials.Remove(social);
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
