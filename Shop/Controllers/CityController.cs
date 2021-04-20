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
    public class CityController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: City
        public ActionResult Index(string search, int? page)
        {
            IEnumerable<City>model = db.Cities.Include(c => c.State);
            if (search != null)
            {
                model = db.Cities.Where(s =>s.cityName.Contains(search)).ToList();
            }
            return View(model.ToList().ToPagedList(page ?? 1,10));
        }
        [RequsetLogin(2)]
        // GET: City/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }
        [HttpPost]
        public ActionResult loadCities(int? id)
        {
            var p = db.Cities.Where(c => c.stateID == id.Value).Select(c => new { c.cityID, c.cityName }).ToList();
            return Json(p, JsonRequestBehavior.AllowGet);
        }

        [RequsetLogin(2)]
        // GET: City/Create
        public ActionResult Create()
        {
            ViewBag.stateID = new SelectList(db.States, "stateID", "stateName");
            return View();
        }
        [RequsetLogin(2)]
        // POST: City/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cityID,cityName,stateID")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Cities.Add(city);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.stateID = new SelectList(db.States, "stateID", "stateName", city.stateID);
            return View(city);
        }
        [RequsetLogin(2)]
        // GET: City/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            ViewBag.stateID = new SelectList(db.States, "stateID", "stateName", city.stateID);
            return View(city);
        }
        [RequsetLogin(2)]
        // POST: City/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cityID,cityName,stateID")] City city)
        {
            if (ModelState.IsValid)
            {
                db.Entry(city).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.stateID = new SelectList(db.States, "stateID", "stateName", city.stateID);
            return View(city);
        }
        [RequsetLogin(2)]
        // GET: City/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            City city = db.Cities.Find(id);
            if (city == null)
            {
                return HttpNotFound();
            }
            return View(city);
        }
        [RequsetLogin(2)]
        // POST: City/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            City city = db.Cities.Find(id);
            db.Cities.Remove(city);
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
