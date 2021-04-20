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
 
    public class ProductColorController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: ProductColor
        public ActionResult Index()
        {
            return View(db.ProductColors.ToList());
        }
        [RequsetLogin(2)]
        // GET: ProductColor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = db.ProductColors.Find(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }
        [RequsetLogin(2)]
        // GET: ProductColor/Create
        public ActionResult Create()
        {
            return View();
        }
        [RequsetLogin(2)]
        // POST: ProductColor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "productColorID,color")] ProductColor productColor , string colorCode)
        {
            if (ModelState.IsValid)
            {
                productColor.exist = true;
                productColor.colorCode = colorCode;
                db.ProductColors.Add(productColor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productColor);
        }
        [RequsetLogin(2)]
        // GET: ProductColor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = db.ProductColors.Find(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }
        [RequsetLogin(2)]
        // POST: ProductColor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "productColorID,color")] ProductColor productColor , string colorCode)
        {
            if (ModelState.IsValid)
            {
                productColor.colorCode = colorCode;

                db.Entry(productColor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productColor);
        }
        [RequsetLogin(2)]
        // GET: ProductColor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductColor productColor = db.ProductColors.Find(id);
            if (productColor == null)
            {
                return HttpNotFound();
            }
            return View(productColor);
        }
        [RequsetLogin(2)]
        // POST: ProductColor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductColor productColor = db.ProductColors.Find(id);
            db.ProductColors.Remove(productColor);
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
