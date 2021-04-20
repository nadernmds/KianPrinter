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
    public class OrderStateController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: OrderState
        public ActionResult Index()
        {
            return View(db.OrderStates.ToList());
        }
        [RequsetLogin(2)]
        // GET: OrderState/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderState orderState = db.OrderStates.Find(id);
            if (orderState == null)
            {
                return HttpNotFound();
            }
            return View(orderState);
        }
        [RequsetLogin(2)]
        // GET: OrderState/Create
        public ActionResult Create()
        {
            return View();
        }
        [RequsetLogin(2)]
        // POST: OrderState/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "orderStateID,state")] OrderState orderState)
        {
            if (ModelState.IsValid)
            {
                db.OrderStates.Add(orderState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(orderState);
        }
        [RequsetLogin(2)]
        // GET: OrderState/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderState orderState = db.OrderStates.Find(id);
            if (orderState == null)
            {
                return HttpNotFound();
            }
            return View(orderState);
        }
        [RequsetLogin(2)]
        // POST: OrderState/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "orderStateID,state")] OrderState orderState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(orderState);
        }
        [RequsetLogin(2)]
        // GET: OrderState/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderState orderState = db.OrderStates.Find(id);
            if (orderState == null)
            {
                return HttpNotFound();
            }
            return View(orderState);
        }
        [RequsetLogin(2)]
        // POST: OrderState/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderState orderState = db.OrderStates.Find(id);
            db.OrderStates.Remove(orderState);
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
