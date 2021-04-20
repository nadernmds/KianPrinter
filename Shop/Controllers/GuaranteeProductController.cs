using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using pep;
namespace Shop.Controllers
{
    [RequsetLogin]
    public class GuaranteeProductController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        // GET: GuaranteeProduct
        public ActionResult Index(string search)
        {
            IEnumerable<GuaranteeProduct> model = db.GuaranteeProducts.Include(g => g.GuaranteeCustomer).Include(g => g.GuaranteeServiceCompany).Include(g => g.GuaranteeState).Include(g => g.User);
            if (search != null)
            {
                model = db.GuaranteeProducts.Where(s => s.trackingCode.ToString() == (search) || s.GuaranteeCustomer.nationalCode.Contains(search) || s.GuaranteeCustomer.mobile.Contains(search));
            }
            return View(model.OrderBy(c => c.guaranteeStateID).ThenByDescending(c => c.guaranteeProductID));


        }
        public ActionResult StateHistory(int id)
        {
            return View(db.GuaranteeProducts.Find(id));
        }
        public ActionResult Track()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Tracked(int trackingCode, string mobile)
        {
            var model = db.GuaranteeProducts.Where(c => c.trackingCode == trackingCode && c.GuaranteeCustomer.mobile == mobile).FirstOrDefault();
            return PartialView("_TrackingResult", model);
        }
        public ActionResult PrintDeliveryToCustomer(int id)
        {
            return PartialView("_PrintDeliveryToCustomer", db.GuaranteeProducts.Find(id));
        }
        public ActionResult PrintDeliveryToCompany(int id)
        {

            return PartialView("_PrintDeliveryToCompany", db.GuaranteeProducts.Find(id));
        }
        [HttpPost]
        public ActionResult UpdateState(int stateID, int productID)
        {
            try
            {
                var product = db.GuaranteeProducts.Find(productID);
                product.guaranteeStateID = stateID;
                if (stateID == 2)
                {
                    product.doneDate = null;
                    product.customerDeliveryDate = null;
                }
                if (stateID == 4)
                {
                    product.doneDate = DateTime.Now;
                }
                if (stateID == 5)
                {
                    product.customerDeliveryDate = DateTime.Now;
                }
                var user = Session["UU!#user"] as User;
                db.GuaranteeProductStates.Add(new GuaranteeProductState { guaranteeStateID = stateID, guaranteeProductID = productID, date = DateTime.Now, userID = user.userID });
                db.SaveChanges();
                return Json(new { res = "1", doneDate = product.doneDate.HasValue ? product.doneDate.Value.toPersianDateString() : "", customerDeliveryDate = product.customerDeliveryDate.HasValue ? product.customerDeliveryDate.Value.toPersianDateString() : "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                return Json(new { res = "0" }, JsonRequestBehavior.AllowGet);
            }

        }
        // GET: GuaranteeProduct/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeProduct guaranteeProduct = db.GuaranteeProducts.Find(id);
            if (guaranteeProduct == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeProduct);
        }

        // GET: GuaranteeProduct/Create
        public ActionResult Create(string r)
        {
            ViewBag.guaranteeCustomerID = new SelectList(db.GuaranteeCustomers.OrderByDescending(c => c.guaranteeCustomerID), "guaranteeCustomerID", "name");
            ViewBag.guaranteeServiceCompanyID = new SelectList(db.GuaranteeServiceCompanies, "guaranteeServiceCompanyID", "companyName");
            ViewBag.guaranteeStateID = new SelectList(db.GuaranteeStates, "guaranteeStateID", "stateName");
            ViewBag.userID = new SelectList(db.Users, "userID", "username");
            ViewBag.extraItems = db.GuaranteeExtraItems;

            ViewBag.r = r;
            return View();
        }

        // POST: GuaranteeProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "guaranteeProductID,productName,model,serialNumber,guaranteeStateID,guaranteeServiceCompanyID,guaranteeCustomerID,userID,description,result,customerOpinion,repairManOpinion,physicalProblems,amvalNumber,include")] GuaranteeProduct guaranteeProduct, string buyTime, List<int> extraItem)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(buyTime.Trim()))
                {
                    guaranteeProduct.buyTime = buyTime.toMiladiDate();

                }
                var user = Session["UU!#user"] as User;
                guaranteeProduct.userID = user.userID;
                guaranteeProduct.trackingCode = new Random().Next(111111, 999999);
                guaranteeProduct.deliveryDate = DateTime.Now;

                if (extraItem != null)
                {
                    foreach (var item in extraItem)
                    {
                        guaranteeProduct.GuaranteeProductExtraItems.Add(new GuaranteeProductExtraItem
                        {
                            guaranteeExtraItemID = item
                        });
                    }
                }


                db.GuaranteeProducts.Add(guaranteeProduct);
                db.SaveChanges();
                if (ViewBag.r != null)
                {
                    return RedirectToAction(ViewBag.r);

                }
                else
                {
                    return RedirectToAction("Index");

                }
            }

            ViewBag.guaranteeCustomerID = new SelectList(db.GuaranteeCustomers.OrderByDescending(c => c.guaranteeCustomerID), "guaranteeCustomerID", "name", guaranteeProduct.guaranteeCustomerID);
            ViewBag.guaranteeServiceCompanyID = new SelectList(db.GuaranteeServiceCompanies, "guaranteeServiceCompanyID", "companyName", guaranteeProduct.guaranteeServiceCompanyID);
            ViewBag.guaranteeStateID = new SelectList(db.GuaranteeStates, "guaranteeStateID", "stateName", guaranteeProduct.guaranteeStateID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", guaranteeProduct.userID);
            ViewBag.extraItems = db.GuaranteeExtraItems;

            return View(guaranteeProduct);
        }

        // GET: GuaranteeProduct/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeProduct guaranteeProduct = db.GuaranteeProducts.Find(id);
            if (guaranteeProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.guaranteeCustomerID = new SelectList(db.GuaranteeCustomers.OrderByDescending(c => c.guaranteeCustomerID), "guaranteeCustomerID", "name", guaranteeProduct.guaranteeCustomerID);
            ViewBag.guaranteeServiceCompanyID = new SelectList(db.GuaranteeServiceCompanies, "guaranteeServiceCompanyID", "companyName", guaranteeProduct.guaranteeServiceCompanyID);
            ViewBag.guaranteeStateID = new SelectList(db.GuaranteeStates, "guaranteeStateID", "stateName", guaranteeProduct.guaranteeStateID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", guaranteeProduct.userID);
            ViewBag.extraItems = db.GuaranteeExtraItems;

            return View(guaranteeProduct);
        }

        // POST: GuaranteeProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "guaranteeProductID,trackingCode,productName,model,serialNumber,guaranteeStateID,buyTime,deliveryDate,doneDate,customerDeliveryDate,guaranteeServiceCompanyID,guaranteeCustomerID,userID,description,result,confirmDate,customerOpinion,repairManOpinion,physicalProblems,amvalNumber,include")] GuaranteeProduct guaranteeProduct, List<int> extraItem)
        {
            if (ModelState.IsValid)
            {
                var user = Session["UU!#user"] as User;
                guaranteeProduct.userID = user.userID;
                db.Entry(guaranteeProduct).State = EntityState.Modified;

                db.SaveChanges();
                if (extraItem != null)
                {
                    db.GuaranteeProductExtraItems.RemoveRange(db.GuaranteeProductExtraItems.Where(c => c.guaranteeProductID == guaranteeProduct.guaranteeProductID));
                    db.SaveChanges();
                    foreach (var item in extraItem)
                    {
                        guaranteeProduct.GuaranteeProductExtraItems.Add(new GuaranteeProductExtraItem
                        {
                            guaranteeExtraItemID = item
                        });
                    }
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            ViewBag.guaranteeCustomerID = new SelectList(db.GuaranteeCustomers.OrderByDescending(c => c.guaranteeCustomerID), "guaranteeCustomerID", "name", guaranteeProduct.guaranteeCustomerID);
            ViewBag.guaranteeServiceCompanyID = new SelectList(db.GuaranteeServiceCompanies, "guaranteeServiceCompanyID", "companyName", guaranteeProduct.guaranteeServiceCompanyID);
            ViewBag.guaranteeStateID = new SelectList(db.GuaranteeStates, "guaranteeStateID", "stateName", guaranteeProduct.guaranteeStateID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", guaranteeProduct.userID);
            ViewBag.extraItems = db.GuaranteeExtraItems;

            return View(guaranteeProduct);
        }

        // GET: GuaranteeProduct/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GuaranteeProduct guaranteeProduct = db.GuaranteeProducts.Find(id);
            if (guaranteeProduct == null)
            {
                return HttpNotFound();
            }
            return View(guaranteeProduct);
        }

        // POST: GuaranteeProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GuaranteeProduct guaranteeProduct = db.GuaranteeProducts.Find(id);
            db.GuaranteeProducts.Remove(guaranteeProduct);
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
