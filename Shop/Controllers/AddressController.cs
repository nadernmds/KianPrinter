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
    public class AddressController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        
        [RequsetLogin(2)]
        // GET: Address
        public ActionResult Index()
        {
            var addresses = db.Addresses.Include(a => a.City).Include(a => a.User);
            return View(addresses.ToList());
        }

        [RequsetLogin(2)]
        // GET: Address/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }
        [RequsetLogin(2)]
        // GET: Address/Create
        public ActionResult Create()
        {
            ViewBag.cityID = new SelectList(db.Cities, "cityID", "cityName");
            ViewBag.userID = new SelectList(db.Users, "userID", "username");
            return View();
        }
        [RequsetLogin(2)]
        // POST: Address/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "addressID,addressDetail,postalCode,userID,cityID,phone,mobile,location")] Address address)
        {
            if (ModelState.IsValid)
            {
                address.active = true;
                db.Addresses.Add(address);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cityID = new SelectList(db.Cities, "cityID", "cityName", address.cityID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", address.userID);
            return View(address);
        }
        [RequsetLogin(2)]
        // GET: Address/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            ViewBag.cityID = new SelectList(db.Cities, "cityID", "cityName", address.cityID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", address.userID);
            return View(address);
        }
        [RequsetLogin(2)]
        // POST: Address/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "addressID,addressDetail,postalCode,userID,cityID,phone,mobile,location")] Address address)
        {
            if (ModelState.IsValid)
            {
                db.Entry(address).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cityID = new SelectList(db.Cities, "cityID", "cityName", address.cityID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", address.userID);
            return View(address);
        }
        [RequsetLogin(2)]
        // GET: Address/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return HttpNotFound();
            }
            return View(address);
        }
        [RequsetLogin(2)]
        // POST: Address/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Address address = db.Addresses.Find(id);
            db.Addresses.Remove(address);
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
