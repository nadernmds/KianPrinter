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
    public class AttribteValueTypeController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        // GET: AttribteValueType
        public ActionResult Index()
        {
            return View(db.AttribteValueTypes.ToList());
        }
        [RequsetLogin(2)]
        // GET: AttribteValueType/Details/5
        public ActionResult Details(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttribteValueType attribteValueType = db.AttribteValueTypes.Find(id);
            if (attribteValueType == null)
            {
                return HttpNotFound();
            }
            return View(attribteValueType);
        }
        [RequsetLogin(2)]
        // GET: AttribteValueType/Create
        public ActionResult Create()
        {
            return View();
        }
        [RequsetLogin(2)]
        // POST: AttribteValueType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "attributeValueTypeID,valueType")] AttribteValueType attribteValueType)
        {
            if (ModelState.IsValid)
            {
                db.AttribteValueTypes.Add(attribteValueType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(attribteValueType);
        }
        [RequsetLogin(2)]
        // GET: AttribteValueType/Edit/5
        public ActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttribteValueType attribteValueType = db.AttribteValueTypes.Find(id);
            if (attribteValueType == null)
            {
                return HttpNotFound();
            }
            return View(attribteValueType);
        }
        [RequsetLogin(2)]
        // POST: AttribteValueType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "attributeValueTypeID,valueType")] AttribteValueType attribteValueType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(attribteValueType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attribteValueType);
        }
        [RequsetLogin(2)]
        // GET: AttribteValueType/Delete/5
        public ActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AttribteValueType attribteValueType = db.AttribteValueTypes.Find(id);
            if (attribteValueType == null)
            {
                return HttpNotFound();
            }
            return View(attribteValueType);
        }
        [RequsetLogin(2)]
        // POST: AttribteValueType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(byte id)
        {
            AttribteValueType attribteValueType = db.AttribteValueTypes.Find(id);
            db.AttribteValueTypes.Remove(attribteValueType);
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
