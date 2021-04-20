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
    public class BrandController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        // GET: Brand
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }
        [RequsetLogin(2)]
        // GET: Brand/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }
        [RequsetLogin(2)]
        // GET: Brand/Create
        public ActionResult Create()
        {
            return View();
        }
        [RequsetLogin(2)]
        // POST: Brand/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "brandID,brandName,enBrandName,logo")] Brand brand,HttpPostedFileBase file , string googletitle, string googledescription)
        {
            if (ModelState.IsValid)
            {
                brand.Seos.Add(new Seo()
                {
                    brandID = brand.brandID,
                    title = googletitle,
                    description = googledescription
                });




                db.Brands.Add(brand);
                db.SaveChanges();
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/Brand/" + brand.brandID + ".png"));
                }
                return RedirectToAction("Index");
            }

            return View(brand);
        }
        [RequsetLogin(2)]
        // GET: Brand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }
        [RequsetLogin(2)]
        // POST: Brand/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "brandID,brandName,enBrandName,logo")] Brand brand,HttpPostedFileBase file , string googletitle, string googledescription)
        {
            if (ModelState.IsValid)
            {


                db.Seos.RemoveRange(db.Seos.Where(s => s.brandID == brand.brandID));

                Seo seo = new Seo();
                seo.brandID = brand.brandID;
                seo.title = googletitle;
                seo.description = googledescription;
                db.Seos.Add(seo);




                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/Brand/" + brand.brandID + ".png"));
                }
                return RedirectToAction("Index");
            }
            return View(brand);
        }
        [RequsetLogin(2)]
        // GET: Brand/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }
        [RequsetLogin(2)]
        // POST: Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
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

        public ActionResult BrandInfo(int id)
        {
            ViewBag.productCategory = db.Products.Where(z=>z.brandID==id ).Select(c=>c.ProductCategory).Distinct().ToList();
            return View(db.Brands.Find(id));
        }
    }
}
