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
    public class GuaranteeServiceCompanyController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        // GET: GuaranteeServiceCompany
        public ActionResult Index()
        {
            return View(db.GuaranteeServiceCompanies.ToList());
        }

        // GET: GuaranteeServiceCompany/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeServiceCompany guaranteeServiceCompany = db.GuaranteeServiceCompanies.Find(id);
            if (guaranteeServiceCompany == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeServiceCompany);
        }

        // GET: GuaranteeServiceCompany/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GuaranteeServiceCompany/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "guaranteeServiceCompanyID,companyName")] GuaranteeServiceCompany guaranteeServiceCompany)
        {
            if (ModelState.IsValid)
            {
                db.GuaranteeServiceCompanies.Add(guaranteeServiceCompany);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(guaranteeServiceCompany);
        }

        // GET: GuaranteeServiceCompany/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeServiceCompany guaranteeServiceCompany = db.GuaranteeServiceCompanies.Find(id);
            if (guaranteeServiceCompany == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeServiceCompany);
        }

        // POST: GuaranteeServiceCompany/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "guaranteeServiceCompanyID,companyName")] GuaranteeServiceCompany guaranteeServiceCompany)
        {
            if (ModelState.IsValid)
            {
                db.Entry(guaranteeServiceCompany).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(guaranteeServiceCompany);
        }

        // GET: GuaranteeServiceCompany/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeServiceCompany guaranteeServiceCompany = db.GuaranteeServiceCompanies.Find(id);
            if (guaranteeServiceCompany == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeServiceCompany);
        }

        // POST: GuaranteeServiceCompany/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuaranteeServiceCompany guaranteeServiceCompany = db.GuaranteeServiceCompanies.Find(id);
            db.GuaranteeServiceCompanies.Remove(guaranteeServiceCompany);
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
