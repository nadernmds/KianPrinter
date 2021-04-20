using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;

namespace Shop.Controllers
{
    public class InvoiceController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLoginStore]
        public ActionResult TransferList()
        {
            return View(db.Invoices.Where(z=>z.invoiceTypeID==3).ToList());
        }
        [RequsetLoginStore]

        public ActionResult ReceiptList()
        {
            return View(db.Invoices.Where(z => z.invoiceTypeID == 2).ToList());
        }
        [RequsetLoginStore]

        public ActionResult RemittanceList()
        {
            return View(db.Invoices.Where(z => z.invoiceTypeID == 1).ToList());
        }


        [RequsetLoginStore]
        // GET: Invoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }


        //public ActionResult Create()
        //{
        //    ViewBag.invoiceTypeID = new SelectList(db.InvoiceTypes, "invoiceTypeID", "name");
        //    ViewBag.userID = new SelectList(db.Users, "userID", "username"); viewbag
        //    return View();
        //}


        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "invoiceID,description,date,invoiceTypeID,userID")] Invoice invoice)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Invoices.Add(invoice);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.invoiceTypeID = new SelectList(db.InvoiceTypes, "invoiceTypeID", "name", invoice.invoiceTypeID);
        //    ViewBag.userID = new SelectList(db.Users, "userID", "username", invoice.userID);
        //    return View(invoice);
        //}
        [RequsetLoginStore]
        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.invoiceTypeID = new SelectList(db.InvoiceTypes, "invoiceTypeID", "name", invoice.invoiceTypeID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", invoice.userID);
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequsetLoginStore]
        public ActionResult Edit([Bind(Include = "invoiceID,description,date,invoiceTypeID,userID")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                if (invoice.invoiceTypeID == 1)
                {
                    return RedirectToAction("RemittanceList");

                }
                else if (invoice.invoiceTypeID == 2)
                {
                    return RedirectToAction("receiptList");

                }
                else if (invoice.invoiceTypeID == 3)
                {
                    return RedirectToAction("transferList");

                }
            }
            ViewBag.invoiceTypeID = new SelectList(db.InvoiceTypes, "invoiceTypeID", "name", invoice.invoiceTypeID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", invoice.userID);
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        [RequsetLoginStore]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }
        [RequsetLoginStore]
        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            if (invoice.invoiceTypeID == 1)
            {
                return RedirectToAction("RemittanceList");

            }
            else if (invoice.invoiceTypeID == 2)
            {
                return RedirectToAction("receiptList");

            }
            else if (invoice.invoiceTypeID == 3)
            {
                return RedirectToAction("transferList");

            }
            return RedirectToAction("transferList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }











        [RequsetLoginStore]
        public ActionResult Receipt(int? id)
        {
            if (id!=null)
            {
                Invoice invoice = db.Invoices.Find(id);
                return View(invoice);
            }
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = DateTime.Now;
            string PersianDate = string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
            ViewBag.persianDate = PersianDate;
            return View();
        }
        [RequsetLoginStore]
        [HttpPost]
        public ActionResult updateReciept(int invoiceID, string description, DateTime date, List<int> increaseID, List<int> productID, List<int> colorID, List<int> count)
        {

            User user = (User)Session["UU!#user"];


            if (user != null)
            {
                Invoice invoice = db.Invoices.Find(invoiceID);
                invoice.description = description;
                invoice.date = date;
                db.Invoices.Add(invoice);
                db.Entry(invoice).State = EntityState.Modified;
                db.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);
                db.SaveChanges();
                if (increaseID != null)
                {
                    List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                    int i = 0;

                    foreach (var item in increaseID)
                    {
                        InvoiceDetail invoiceDetail = new InvoiceDetail();
                        invoiceDetail.invoiceID = invoice.invoiceID;
                        invoiceDetail.increaseID = increaseID[i];
                        invoiceDetail.productID = productID[i];
                        if (colorID[i] == 0)
                        {
                            invoiceDetail.productColorID = null;
                        }
                        else
                        {
                            invoiceDetail.productColorID = colorID[i];
                        }
                        invoiceDetail.count = count[i];
                        i++;
                        invoiceDetails.Add(invoiceDetail);

                    }
                    db.InvoiceDetails.AddRange(invoiceDetails);
                    db.SaveChanges();
                }

            }

            return Json(Url.Action("ReceiptList", "invoice"));
        }

        [HttpPost]
        [RequsetLoginStore]
        public ActionResult SaveReceipt(string description, DateTime transferdatetime, List<int> increaseID, List<int> productID, List<int> colorID, List<int> count)
        {

            User user = (User)Session["UU!#user"];


            if (user != null)
            {

                Invoice invoice = new Invoice();

                invoice.description = description;
                invoice.date = transferdatetime;
                invoice.invoiceTypeID = 2;
                invoice.userID = user.userID;

                db.Invoices.Add(invoice);
                db.SaveChanges();



                if (increaseID != null)
                {
                    List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                    int i = 0;

                    foreach (var item in increaseID)
                    {
                        InvoiceDetail invoiceDetail = new InvoiceDetail();
                        invoiceDetail.invoiceID = invoice.invoiceID;
                        invoiceDetail.increaseID = increaseID[i];
                        invoiceDetail.productID = productID[i];
                        if (colorID[i] == 0)
                        {
                            invoiceDetail.productColorID = null;
                        }
                        else
                        {
                            invoiceDetail.productColorID = colorID[i];
                        }
                        invoiceDetail.count = count[i];
                        i++;
                        invoiceDetails.Add(invoiceDetail);

                    }
                    db.InvoiceDetails.AddRange(invoiceDetails);
                    db.SaveChanges();
                }

            }

            return Json(Url.Action("ReceiptList", "invoice"));

        }
        [RequsetLoginStore]
        [HttpPost]
        public ActionResult addInvoiceDetailsReciept()
        {
            return PartialView("~/Views/Partials/_CreateElementReceipt.cshtml", new InvoiceDetail());
        }
        [RequsetLoginStore]
        [HttpPost]
        public ActionResult getStoresReciept(int storeid)
        {
            string productID = "0,";
            string productName = " ,";
            if (storeid > 0)
            {
                
               var  products = db.Products.Where(p => p.existingCount > 0).ToList();


                foreach (var item in products)
                {
                    productName += item.name + ",";
                    productID += item.productID + ",";
                }
                if (products.Count > 0)
                {
                    productName = productName.Substring(0, productName.Length - 1);
                    productID = productID.Substring(0, productID.Length - 1);
                }

            }
            else
            {
                productName = "";
                productID = "";
            }


            ////////////////////////////////////////////////////////////////////////////////


            return Json(new { success = true, productName = productName, productID = productID });

        }

        [RequsetLoginStore]
        public ActionResult Remittance(int? id)
        {





            //TODO 
            //int userID = 0; //seesion
            //List<int?> accessStoresID = db.AccessStores.Where(a => a.userID == userID).Select(s=>s.storeID).ToList();
            //if(accessStoresID.Count > 0)
            //{

            //    // mojaz
            //    db.InvoiceDetails.Where(q => accessStoresID.Contains(q.Store.storeID));



            //}
            //else
            //{
            //    //return error
            //}











            if (id!=null)
            {
                    Invoice invoice = db.Invoices.Find(id);
                    return View(invoice);
            }
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = DateTime.Now;
            string PersianDate = string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
            ViewBag.persianDate = PersianDate;
            return View();
        }

        [RequsetLoginStore]
        [HttpPost]
        public ActionResult SaveRemittance(string description, DateTime date, List<int> decreaseID, List<int> productID, List<int> colorID, List<int> count)
        {

            User user = (User)Session["UU!#user"];


            if (user != null)
            {

                Invoice invoice = new Invoice();

                invoice.description = description;
                invoice.date = date;
                invoice.invoiceTypeID = 1;
                invoice.userID = user.userID;

                db.Invoices.Add(invoice);
                db.SaveChanges();



                if (decreaseID != null)
                {
                    List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                    int i = 0;
                    InvoiceDetail invoiceDetail = new InvoiceDetail();

                    foreach (var item in decreaseID)
                    {
                        invoiceDetail.invoiceID = invoice.invoiceID;

                        invoiceDetail.decreaseID = decreaseID[i];
                        invoiceDetail.productID = productID[i];
                        if (colorID[i] == 0)
                        {
                            invoiceDetail.productColorID = null;
                        }
                        else
                        {
                            invoiceDetail.productColorID = colorID[i];
                        }
                        invoiceDetail.count = count[i];
                        i++;
                        invoiceDetails.Add(invoiceDetail);

                    }
                    db.InvoiceDetails.AddRange(invoiceDetails);
                    db.SaveChanges();
                }

            }

            return Json(Url.Action("RemittanceList", "invoice"));
        }


        [RequsetLoginStore]
        [HttpPost]
        public ActionResult updateRemittance(int invoiceID,string description, DateTime date, List<int> decreaseID, List<int> productID, List<int> colorID, List<int> count)
        {

            User user = (User)Session["UU!#user"];


            if (user != null)
            {

                Invoice invoice = db.Invoices.Find(invoiceID);
                invoice.description = description;
                invoice.date = date;
                db.Invoices.Add(invoice);
                db.Entry(invoice).State = EntityState.Modified;
                db.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);
                db.SaveChanges();



                if (decreaseID != null)
                {
                    List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                    int i = 0;

                    foreach (var item in decreaseID)
                    {
                        InvoiceDetail invoiceDetail = new InvoiceDetail();
                        invoiceDetail.invoiceID = invoice.invoiceID;
                        invoiceDetail.decreaseID = decreaseID[i];
                        invoiceDetail.productID = productID[i];
                        if (colorID[i] == 0)
                        {
                            invoiceDetail.productColorID = null;
                        }
                        else
                        {
                            invoiceDetail.productColorID = colorID[i];
                        }
                        invoiceDetail.count = count[i];
                        i++;
                        invoiceDetails.Add(invoiceDetail);

                    }
                    db.InvoiceDetails.AddRange(invoiceDetails);
                    db.SaveChanges();
                }

            }

            return Json(Url.Action("RemittanceList", "invoice"));
        }
        [RequsetLoginStore]
        [HttpPost]
        public ActionResult addInvoiceDetailsRemittance()
        {
            return PartialView("~/Views/Partials/_CreateElementRemittance.cshtml", new InvoiceDetail());
        }


        [RequsetLoginStore]
        [HttpPost]
        public ActionResult getStoresRemittance(int storeid)
        {
            string productID = "0,";
            string productName = " ,";
            if (storeid > 0)
            {
                var invoicedetails = db.InvoiceDetails.Where(i => i.increaseID == storeid).ToList();
                var products = invoicedetails.Select(p => p.Product).Distinct().ToList();
                products = products.Where(p => p.existingCount > 0).ToList();


                foreach (var item in products)
                {
                    productName += item.name + ",";
                    productID += item.productID + ",";
                }
                if (products.Count > 0)
                {
                    productName = productName.Substring(0, productName.Length - 1);
                    productID = productID.Substring(0, productID.Length - 1);
                }

            }
            else
            {
                productName = "";
                productID = "";
            }


            ////////////////////////////////////////////////////////////////////////////////


            return Json(new { success = true, productName = productName, productID = productID });

        }


        [RequsetLoginStore]
        public ActionResult transfer(int? id)
        {
            //ViewBag.InvoiceType = new SelectList(db.InvoiceTypes, "InvoiceTypeID", "name");
            if (id!=null)
            {
                var invoice = db.Invoices.Find(id);
                return View(invoice);
            }
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = DateTime.Now;
            string PersianDate = string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
            ViewBag.persianDate = PersianDate;

            return View();
        }


        [RequsetLoginStore]
        [HttpPost]
        public ActionResult addInvoiceDetails(int count)
        {
            InvoiceDetail invoiceDetail = new InvoiceDetail { };

            ViewBag.count = count;
            return PartialView("~/Views/invoice/_CreateElement.cshtml", invoiceDetail);

        }



        [RequsetLoginStore]
        [HttpPost]
        public ActionResult getStores(int storeid)
        {
            string storeID = "";
            string storeName = "";
            if (storeid > 0)
            {
                var store = db.Stores.Where(s => s.storeID != storeid).Select(x => new { x.storeID, x.name }).ToList();

                foreach (var item in store)
                {
                    storeName += item.name + ",";
                    storeID += item.storeID + ",";
                }
                if (store.Count > 0)
                {
                    storeName = storeName.Substring(0, storeName.Length - 1);
                    storeID = storeID.Substring(0, storeID.Length - 1);
                }

            }
            else
            {
                storeName = "";
                storeID = "";
            }


            ///////////////////////////////////////////////////////////////////////////////////

            string productID = "0,";
            string productName = " ,";
            if (storeid > 0)
            {
                var invoicedetails = db.InvoiceDetails.Where(i => i.increaseID == storeid).ToList();
                var products = invoicedetails.Select(p => p.Product).Distinct().ToList();
                products = products.Where(p => p.existingCount > 0).ToList();


                foreach (var item in products)
                {
                    productName += item.name + ",";
                    productID += item.productID + ",";
                }
                if (products.Count > 0)
                {
                    productName = productName.Substring(0, productName.Length - 1);
                    productID = productID.Substring(0, productID.Length - 1);
                }

            }
            else
            {
                productName = "";
                productID = "";
            }


            ////////////////////////////////////////////////////////////////////////////////


            return Json(new { success = true, storeName = storeName, storeID = storeID, productName = productName, productID = productID });

        }

        [RequsetLoginStore]
        [HttpPost]
        public ActionResult getcolors(int productID)
        {
            string productexistingcount = "";
            string colorID = "";
            string colorName = "";
            if (productID > 0)
            {
                productexistingcount = Convert.ToString(db.Products.Find(productID).existingCount);
                var colorasinments = db.ProductColorAssinments.Where(p => p.productID == productID).ToList();
                foreach (var item in colorasinments)
                {
                    colorName += item.ProductColor.color + ",";
                    colorID += item.ProductColor.productColorID + ",";
                }


                if (colorasinments.Count > 0)
                {
                    colorName = colorName.Substring(0, colorName.Length - 1);
                    colorID = colorID.Substring(0, colorID.Length - 1);
                }
                else
                {
                    colorName = "";
                    colorName = "";
                }

            }
            else
            {
                colorName = "";
                colorName = "";
            }


            ////////////////////////////////////////////////////////////////////////////////

            return Json(new { success = true, colorName = colorName, colorID = colorID, productexistingcount = productexistingcount });

        }









        [RequsetLoginStore]
        [HttpPost]
        public ActionResult getcounts(int productColorID, int productID)
        {
            string productexistingcount = "";
            if (productColorID > 0)
            {

                var colorasinments = db.ProductColorAssinments.FirstOrDefault(p => p.productID == productID && p.productColorID == productColorID);
                if (colorasinments != null)
                {
                    productexistingcount = Convert.ToString(colorasinments.count);
                }


            }
            else
            {
                productexistingcount = "0";
            }


            ////////////////////////////////////////////////////////////////////////////////

            return Json(new { success = true, productexistingcount = productexistingcount });

        }


        [RequsetLoginStore]
        [HttpPost]
        public ActionResult updateTransfer(int invoiceID, string description, DateTime date, List<int> increaseID, List<int> decreaseID, List<int> productID, List<int> colorID, List<int> count)
        {
            User user = (User)Session["UU!#user"];
            if (user != null)
            {
                Invoice invoice = db.Invoices.Find(invoiceID);
                invoice.description = description;
                invoice.date = date;
                invoice.userID = user.userID;
                db.Entry(invoice).State = EntityState.Modified;
                db.InvoiceDetails.RemoveRange(invoice.InvoiceDetails);
                db.SaveChanges();
                if (increaseID != null)
                {
                    List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                    int i = 0;
                    
                    foreach (var item in increaseID)
                    {
                        InvoiceDetail invoiceDetail = new InvoiceDetail();

                        invoiceDetail.invoiceID = invoice.invoiceID;

                        invoiceDetail.increaseID = increaseID[i];
                        invoiceDetail.decreaseID = decreaseID[i];
                        invoiceDetail.productID = productID[i];
                        if (colorID[i] == 0)
                        {
                            invoiceDetail.productColorID = null;
                        }
                        else
                        {
                            invoiceDetail.productColorID = colorID[i];
                        }
                        invoiceDetail.count = count[i];
                        i++;
                        invoiceDetails.Add(invoiceDetail);

                    }
                    db.InvoiceDetails.AddRange(invoiceDetails);
                    db.SaveChanges();
                }
            }
            return Json(Url.Action("transferList", "invoice"));
        }

        [RequsetLoginStore]
        [HttpPost]
        public ActionResult saveTransfer(string description, DateTime date, List<int> increaseID, List<int> decreaseID, List<int> productID, List<int> colorID, List<int> count)
        {
            User user = (User)Session["UU!#user"];
            if (user != null)
            {
                Invoice invoice = new Invoice();
                invoice.description = description;
                invoice.date = date;
                invoice.invoiceTypeID = 3;
                invoice.userID = user.userID;
                db.Invoices.Add(invoice);
                db.SaveChanges();
                if (increaseID != null)
                {
                    List<InvoiceDetail> invoiceDetails = new List<InvoiceDetail>();
                    int i = 0;
                    foreach (var item in increaseID)
                    {
                        InvoiceDetail invoiceDetail = new InvoiceDetail();
                        invoiceDetail.invoiceID = invoice.invoiceID;

                        invoiceDetail.increaseID = increaseID[i];
                        invoiceDetail.decreaseID = decreaseID[i];
                        invoiceDetail.productID = productID[i];
                        if (colorID[i] == 0)
                        {
                            invoiceDetail.productColorID = null;
                        }
                        else
                        {
                            invoiceDetail.productColorID = colorID[i];
                        }
                        invoiceDetail.count = count[i];
                        i++;
                        invoiceDetails.Add(invoiceDetail);

                    }
                    db.InvoiceDetails.AddRange(invoiceDetails);
                    db.SaveChanges();
                }
            }
            return Json(Url.Action("transferList", "invoice"));
        }

    }
}
