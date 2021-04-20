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
    public class GuaranteeExtraItemController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        // GET: GuaranteeExtraItem
        public ActionResult Index()
        {
            return View(db.GuaranteeExtraItems.ToList());
        }

        // GET: GuaranteeExtraItem/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeExtraItem guaranteeExtraItem = db.GuaranteeExtraItems.Find(id);
            if (guaranteeExtraItem == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeExtraItem);
        }

        // GET: GuaranteeExtraItem/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuaranteeExtraItem/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "guaranteeExtraItemID,itemName")] GuaranteeExtraItem guaranteeExtraItem)
        {
            if (ModelState.IsValid)
            {
                db.GuaranteeExtraItems.Add(guaranteeExtraItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guaranteeExtraItem);
        }

        // GET: GuaranteeExtraItem/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeExtraItem guaranteeExtraItem = db.GuaranteeExtraItems.Find(id);
            if (guaranteeExtraItem == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeExtraItem);
        }

        // POST: GuaranteeExtraItem/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "guaranteeExtraItemID,itemName")] GuaranteeExtraItem guaranteeExtraItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guaranteeExtraItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guaranteeExtraItem);
        }

        // GET: GuaranteeExtraItem/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeExtraItem guaranteeExtraItem = db.GuaranteeExtraItems.Find(id);
            if (guaranteeExtraItem == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeExtraItem);
        }

        // POST: GuaranteeExtraItem/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuaranteeExtraItem guaranteeExtraItem = db.GuaranteeExtraItems.Find(id);
            db.GuaranteeExtraItems.Remove(guaranteeExtraItem);
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
