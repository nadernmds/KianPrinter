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
    public class GuaranteeCustomerController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        // GET: GuaranteeCustomer
        public ActionResult Index()
        {
            return View(db.GuaranteeCustomers.ToList());
        }

        // GET: GuaranteeCustomer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeCustomer guaranteeCustomer = db.GuaranteeCustomers.Find(id);
            if (guaranteeCustomer == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeCustomer);
        }

        // GET: GuaranteeCustomer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuaranteeCustomer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "guaranteeCustomerID,name,phone,companyName,mobile,nationalCode")] GuaranteeCustomer guaranteeCustomer)
        {
            if (ModelState.IsValid)
            {
                db.GuaranteeCustomers.Add(guaranteeCustomer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guaranteeCustomer);
        }

        // GET: GuaranteeCustomer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeCustomer guaranteeCustomer = db.GuaranteeCustomers.Find(id);
            if (guaranteeCustomer == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeCustomer);
        }

        // POST: GuaranteeCustomer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "guaranteeCustomerID,name,phone,companyName,mobile,nationalCode")] GuaranteeCustomer guaranteeCustomer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guaranteeCustomer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guaranteeCustomer);
        }

        // GET: GuaranteeCustomer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeCustomer guaranteeCustomer = db.GuaranteeCustomers.Find(id);
            if (guaranteeCustomer == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeCustomer);
        }

        // POST: GuaranteeCustomer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuaranteeCustomer guaranteeCustomer = db.GuaranteeCustomers.Find(id);
            db.GuaranteeCustomers.Remove(guaranteeCustomer);
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
