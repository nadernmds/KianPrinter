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
using Stimulsoft.Report.Dictionary;
using pep;
using Stimulsoft.Report.Mvc;

namespace Shop.Controllers
{
    public class OrdeController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: Orde
        public ActionResult Index(string search, int? page, string sort)
        {
            IEnumerable<Orde> model = db.Ordes.Include(o => o.Address).Include(o => o.OrderState).Include(o => o.User);
            if (search != null)
            {
                model = db.Ordes.Where(s => s.User.name.Contains(search)).ToList();
            }

            try
            {
                switch (sort)
                {
                    case "orderOff":
                        model = model.OrderBy(s => s.orderOff);
                        break;
                    case "orderOff_desc":
                        model = model.OrderByDescending(s => s.orderOff);
                        break;
                    //case "addressDetail":
                    //model = model.OrderBy(s => s.Address.addressDetail);
                    //break;
                    //case "addressDetail_desc":
                    //model = model.OrderByDescending(s => s.Address.addressDetail);
                    //break;
                    //case "state":
                    //    model = model.OrderBy(s => s.OrderState.state);
                    //    break;
                    //case "state_desc":
                    //    model = model.OrderByDescending(s => s.OrderState.state);
                    //    break;
                    case "username":
                        model = model.OrderBy(s => s.User.username);
                        break;
                    case "username_desc":
                        model = model.OrderByDescending(s => s.User.username);
                        break;
                    case "date":
                        model = model.OrderBy(s => s.date);
                        break;
                    case "date_desc":
                        model = model.OrderByDescending(s => s.date);
                        break;
                }
            }
            catch (Exception e)
            {

            }




            ViewBag.SortType = sort;
            return View(model.ToList().ToPagedList(page ?? 1, 10));
        }
        [RequsetLogin(2)]
        // GET: Orde/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orde orde = db.Ordes.Find(id);
            if (orde == null)
            {
                return HttpNotFound();
            }
            return View(orde);
        }
        [RequsetLogin(2)]
        // GET: Orde/Create
        public ActionResult Create()
        {
            ViewBag.addressID = new SelectList(db.Addresses, "addressID", "addressDetail");
            ViewBag.orderStateID = new SelectList(db.OrderStates, "orderStateID", "state");
            ViewBag.userID = new SelectList(db.Users, "userID", "username");
            return View();
        }
        [RequsetLogin(2)]
        // POST: Orde/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "orderID,orderOff,date,userID,addressID,orderStateID")] Orde orde)
        {
            if (ModelState.IsValid)
            {
                db.Ordes.Add(orde);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.addressID = new SelectList(db.Addresses, "addressID", "addressDetail", orde.addressID);
            ViewBag.orderStateID = new SelectList(db.OrderStates, "orderStateID", "state", orde.orderStateID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", orde.userID);
            return View(orde);
        }
        [RequsetLogin(2)]
        // GET: Orde/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orde orde = db.Ordes.Find(id);
            if (orde == null)
            {
                return HttpNotFound();
            }
            ViewBag.addressID = new SelectList(db.Addresses, "addressID", "addressDetail", orde.addressID);
            ViewBag.orderStateID = new SelectList(db.OrderStates, "orderStateID", "state", orde.orderStateID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", orde.userID);
            return View(orde);
        }
        [RequsetLogin(2)]
        // POST: Orde/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "orderID,orderOff,date,userID,addressID,orderStateID")] Orde orde)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orde).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.addressID = new SelectList(db.Addresses, "addressID", "addressDetail", orde.addressID);
            ViewBag.orderStateID = new SelectList(db.OrderStates, "orderStateID", "state", orde.orderStateID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", orde.userID);
            return View(orde);
        }
        [RequsetLogin(2)]
        // GET: Orde/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orde orde = db.Ordes.Find(id);
            if (orde == null)
            {
                return HttpNotFound();
            }
            return View(orde);
        }
        [RequsetLogin(2)]
        // POST: Orde/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orde orde = db.Ordes.Find(id);
            db.Ordes.Remove(orde);
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
