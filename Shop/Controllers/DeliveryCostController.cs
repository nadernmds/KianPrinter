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
    public class DeliveryCostController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        // GET: DeliveryCost
        public ActionResult Index()
        {
            var deliveryCosts = db.DeliveryCosts.Include(d => d.City).Include(d => d.City1);
            return View(deliveryCosts.ToList());
        }

        // GET: DeliveryCost/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCost deliveryCost = db.DeliveryCosts.Find(id);
            if (deliveryCost == null)
            {
                return HttpNotFound();
            }
            return View(deliveryCost);
        }
        [HttpPost]
        public ActionResult SaveCost(DeliveryCost deliveryCost)

        {
            var exists = db.DeliveryCosts.Where(c => c.fromCityID == deliveryCost.fromCityID && c.toCityID == deliveryCost.toCityID).FirstOrDefault();
            if (exists != null)
            {
                exists.fromCityID = deliveryCost.fromCityID;
                exists.toCityID = deliveryCost.toCityID;
                exists.cost = deliveryCost.cost;
                db.SaveChanges();
                return Content("OK");
            }
            else
            {
                db.DeliveryCosts.Add(deliveryCost);
                db.SaveChanges();
                return Content("OK");
            }


        }
        // GET: DeliveryCost/Create
        public ActionResult Create()
        {
            ViewBag.fromCityID = new SelectList(db.Cities, "cityID", "cityName");
            ViewBag.toCityID = new SelectList(db.Cities, "cityID", "cityName");
            return View();
        }

        // POST: DeliveryCost/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "deliveryCostID,fromCityID,toCityID,cost")] DeliveryCost deliveryCost)
        {
            if (ModelState.IsValid)
            {
                db.DeliveryCosts.Add(deliveryCost);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.fromCityID = new SelectList(db.Cities, "cityID", "cityName", deliveryCost.fromCityID);
            ViewBag.toCityID = new SelectList(db.Cities, "cityID", "cityName", deliveryCost.toCityID);
            return View(deliveryCost);
        }

        // GET: DeliveryCost/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCost deliveryCost = db.DeliveryCosts.Find(id);
            if (deliveryCost == null)
            {
                return HttpNotFound();
            }
            ViewBag.fromCityID = new SelectList(db.Cities, "cityID", "cityName", deliveryCost.fromCityID);
            ViewBag.toCityID = new SelectList(db.Cities, "cityID", "cityName", deliveryCost.toCityID);
            return View(deliveryCost);
        }

        // POST: DeliveryCost/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "deliveryCostID,fromCityID,toCityID,cost")] DeliveryCost deliveryCost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(deliveryCost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.fromCityID = new SelectList(db.Cities, "cityID", "cityName", deliveryCost.fromCityID);
            ViewBag.toCityID = new SelectList(db.Cities, "cityID", "cityName", deliveryCost.toCityID);
            return View(deliveryCost);
        }

        // GET: DeliveryCost/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeliveryCost deliveryCost = db.DeliveryCosts.Find(id);
            if (deliveryCost == null)
            {
                return HttpNotFound();
            }
            return View(deliveryCost);
        }

        // POST: DeliveryCost/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeliveryCost deliveryCost = db.DeliveryCosts.Find(id);
            db.DeliveryCosts.Remove(deliveryCost);
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
