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
    [RequsetLogin]
    public class GuaranteeStateController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        // GET: GuaranteeState
        public ActionResult Index()
        {
            return View(db.GuaranteeStates.ToList());
        }

        // GET: GuaranteeState/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeState guaranteeState = db.GuaranteeStates.Find(id);
            if (guaranteeState == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeState);
        }

        // GET: GuaranteeState/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuaranteeState/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "guaranteeStateID,stateName")] GuaranteeState guaranteeState)
        {
            if (ModelState.IsValid)
            {
                db.GuaranteeStates.Add(guaranteeState);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guaranteeState);
        }

        // GET: GuaranteeState/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeState guaranteeState = db.GuaranteeStates.Find(id);
            if (guaranteeState == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeState);
        }

        // POST: GuaranteeState/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "guaranteeStateID,stateName")] GuaranteeState guaranteeState)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guaranteeState).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guaranteeState);
        }

        // GET: GuaranteeState/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeState guaranteeState = db.GuaranteeStates.Find(id);
            if (guaranteeState == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeState);
        }

        // POST: GuaranteeState/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuaranteeState guaranteeState = db.GuaranteeStates.Find(id);
            db.GuaranteeStates.Remove(guaranteeState);
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
