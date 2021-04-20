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
    public class ProductStateController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: ProductState
        public ActionResult Index()
        {
            return View(db.ProductStates.ToList());
        }
        [RequsetLogin(2)]
        // GET: ProductState/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductState productState = db.ProductStates.Find(id);
            if (productState == null)
            {
                return HttpNotFound();
            }
            return View(productState);
        }
        [RequsetLogin(2)]
        // GET: ProductState/Create
        public ActionResult Create()
        {
            return View();
        }
        [RequsetLogin(2)]
        // POST: ProductState/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productStateID,state")] ProductState productState)
        {
            if (ModelState.IsValid)
            {
                db.ProductStates.Add(productState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productState);
        }
        [RequsetLogin(2)]
        // GET: ProductState/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductState productState = db.ProductStates.Find(id);
            if (productState == null)
            {
                return HttpNotFound();
            }
            return View(productState);
        }
        [RequsetLogin(2)]
        // POST: ProductState/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productStateID,state")] ProductState productState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productState);
        }
        [RequsetLogin(2)]
        // GET: ProductState/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductState productState = db.ProductStates.Find(id);
            if (productState == null)
            {
                return HttpNotFound();
            }
            return View(productState);
        }
        [RequsetLogin(2)]
        // POST: ProductState/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductState productState = db.ProductStates.Find(id);
            db.ProductStates.Remove(productState);
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
