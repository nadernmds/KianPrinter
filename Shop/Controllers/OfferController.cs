using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using PagedList;

namespace Shop.Controllers
{
    public class OfferController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: Offer
        public ActionResult Index(string search,int ? page, string sort)
        {
            IEnumerable<Offer> model = db.Offers.Include(o => o.Product);
            if (search != null)
            {
                model = db.Offers.Where(s => s.Product.name.Contains(search)).ToList();
            }
            switch (sort)
            {
                case "startDate":
                    model = model.OrderBy(s => s.startDate);
                    break;
                case "startDate_desc":
                    model = model.OrderByDescending(s => s.startDate);
                    break;
                case "endDate":
                    model = model.OrderBy(s => s.endDate);
                    break;
                case "endDate_desc":
                    model = model.OrderByDescending(s => s.endDate);
                    break;
                case "price":
                    model = model.OrderBy(s => s.price);
                    break;
                case "price_desc":
                    model = model.OrderByDescending(s => s.price);
                    break;
                case "offPercent":
                    model = model.OrderBy(s => s.offPercent);
                    break;
                case "offPercent_desc":
                    model = model.OrderByDescending(s => s.offPercent);
                    break;
                case "name":
                    model = model.OrderBy(s => s.Product.name);
                    break;
                case "name_desc":
                    model = model.OrderByDescending(s => s.Product.name);
                    break;
            }
            ViewBag.SortType = sort;
            return View(model.ToList().ToPagedList(page ?? 1,10));
        }
        [RequsetLogin(2)]
        // GET: Offer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }
        [RequsetLogin(2)]
        // GET: Offer/Create
        public ActionResult Create()
        {
            ViewBag.productID = new SelectList(db.Products, "productID", "name");
            return View();
        }
        [RequsetLogin(2)]
        // POST: Offer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "offerID,startDate,endDate,price,offPercent,productID")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                if (offer.price == null)
                    offer.price = 0;
                if (offer.offPercent == null)
                    offer.offPercent = 0;

                db.Offers.Add(offer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productID = new SelectList(db.Products, "productID", "name", offer.productID);
            return View(offer);
        }
        [RequsetLogin(2)]
        // GET: Offer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "name", offer.productID);
            return View(offer);
        }
        [RequsetLogin(2)]
        // POST: Offer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "offerID,startDate,endDate,price,offPercent,productID")] Offer offer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(offer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "name", offer.productID);
            return View(offer);
        }
        [RequsetLogin(2)]
        // GET: Offer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Offer offer = db.Offers.Find(id);
            if (offer == null)
            {
                return HttpNotFound();
            }
            return View(offer);
        }
        [RequsetLogin(2)]
        // POST: Offer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Offer offer = db.Offers.Find(id);
            db.Offers.Remove(offer);
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
