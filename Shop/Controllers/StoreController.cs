using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;

namespace Shop.Controllers
{

    public class StoreController : Controller
    {
        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    base.OnActionExecuted(filterContext);
        //}

        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        public ActionResult getCities(int? stateID)
        {
            string cityID = "";
            string cityName = "";
            if (stateID != null)
            {

                var cities = db.Cities.Where(c => c.stateID == stateID).ToList();
                foreach (var item in cities)
                {
                    cityName += item.cityName + ",";
                    cityID += item.cityID + ",";
                }
                cityName = cityName.Substring(0, cityName.Length - 1);
                cityID = cityID.Substring(0, cityID.Length - 1);
            }
            else
            {
                cityName = "";
                cityID = "";
            }
            return Json(new { success = true, cityName = cityName, cityID = cityID });
        }


        [RequsetLogin(2)]
        // GET: Store
        public ActionResult Index()
        {
            var stores = db.Stores.Include(s => s.City).Include(s => s.User);
            return View(stores.ToList());
        }

        [RequsetLogin(2)]
        // GET: Store/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }
        [RequsetLogin(2)]
        // GET: Store/Create
        public ActionResult Create()
        {
            ViewBag.states = db.States.ToList();
            ViewBag.cityID = new SelectList(db.Cities, "cityID", "cityName");
            ViewBag.userID = new SelectList(db.Users, "userID", "username");
            ViewBag.users = db.Users.Where(z => z.usergroupID == 2).ToList();
            return View();
        }
        [RequsetLogin(2)]
        // POST: Store/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "storeID,name,phone,mobile,address,active,cityID,userID,accessStoreID")] Store store, int[] accessStoreID)
        {
            if (ModelState.IsValid)
            {
                if (accessStoreID != null)
                {
                    foreach (var item in accessStoreID)
                    {
                        store.AccessStores.Add(new AccessStore() { storeID = store.storeID, userID = item });
                    }
                }
                db.Stores.Add(store);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cityID = new SelectList(db.Cities, "cityID", "cityName", store.cityID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", store.name);
            return View(store);
        }
        [RequsetLogin(2)]
        // GET: Store/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            ViewBag.states = db.States.ToList();
            ViewBag.cityID = new SelectList(db.Cities, "cityID", "cityName", store.cityID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", store.userID);
            return View(store);
        }
        [RequsetLogin(2)]
        // POST: Store/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "storeID,name,phone,mobile,address,active,cityID,userID,accessStoreID")] Store store, int[] accessStoreID)
        {
            if (ModelState.IsValid)
            {
                if (accessStoreID!=null)
                {
                    foreach (var item in accessStoreID)
                    {
                        var removableAccess = db.AccessStores.Where(z=>z.storeID==store.storeID).ToList();
                        db.AccessStores.RemoveRange(removableAccess);
                        db.SaveChanges();
                        store.AccessStores.Add(new AccessStore() { storeID = store.storeID, userID = item });
                    }
                }
                db.Entry(store).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cityID = new SelectList(db.Cities, "cityID", "cityName", store.cityID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", store.userID);
            return View(store);
        }
        [RequsetLogin(2)]
        // GET: Store/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }
        [RequsetLogin(2)]
        // POST: Store/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Store store = db.Stores.Find(id);
            db.Stores.Remove(store);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [RequsetLogin(2)]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }





        [RequsetLoginStore]
        public ActionResult Cartex()
        {
            User user = (User)Session["UU!#user"];
            ViewBag.store =db.AccessStores.Where(a => a.userID == user.userID).Select(s => s.Store).ToList();
            PersianCalendar pc = new PersianCalendar();
            DateTime dt = DateTime.Now;
            string PersianDate = string.Format("{0}/{1}/{2}", pc.GetYear(dt), pc.GetMonth(dt), pc.GetDayOfMonth(dt));
            ViewBag.persianDate = PersianDate;
            return View();

        }
        [RequsetLoginStore]
        [HttpPost]
        public ActionResult getProducts(DateTime date, int store, bool color)
        {
            date = pep.pep.toMiladiDate(Convert.ToString(date));

            List<customProduct> customProducts = new List<customProduct>();
            if (color)
            {
                customProducts = db.InvoiceDetails
                    .Where(c => c.Invoice.date == date)
                    .GroupBy(c => new { c.Product, c.ProductColor })
                    .Select(c => new customProduct { productID = c.Key.Product.productID, productName = c.Key.Product.name, colorName = c.Key.ProductColor.color, count = c.Sum(w => (w.increaseID.HasValue && !w.decreaseID.HasValue ? w.count : -1 * w.count)) })
                    .ToList();
            }
            else
            {
                customProducts = db.InvoiceDetails
                   .Where(c => c.Invoice.date == date)
                   .GroupBy(c => new { c.Product, c.productID })
                   .Select(c => new customProduct { productID = c.Key.Product.productID, productName = c.Key.Product.name, count = c.Sum(w => (w.increaseID.HasValue && !w.decreaseID.HasValue ? w.count : -1 * w.count)) })
                   .ToList();
            }
            ViewBag.colorfiltered = color;
            return PartialView("~/Views/Partials/_ProductTableList.cshtml", customProducts);
            // return PartialView("~/Views/Partials/_ProductTableList.cshtml", products);
        }
        [RequsetLoginStore]
        public ActionResult cartexCards()
        {
            User user = (User)Session["UU!#user"];
            List<Store> accessStores = db.AccessStores.Where(a => a.userID == user.userID).Select(s => s.Store).ToList();
            ViewBag.Stores = accessStores;
            ViewBag.Category = db.ProductCategories.ToList();
            return View();
        }
        [RequsetLoginStore]
        [HttpPost]
        public ActionResult getProductList(int categoryID)
        {
            var products = db.Products.Where(z => z.categoryID == categoryID).ToList();
            return PartialView("~/Views/Partials/_ProductList.cshtml", products);
        }
        [RequsetLoginStore]
        [HttpPost]
        public ActionResult cartexCards(DateTime? startDate, DateTime? endDate, int? storeID, List<int> pID)
        {

            if (startDate == null)
            {
                var invoice = db.Invoices.OrderBy(a => a.date).FirstOrDefault();
                startDate = ((DateTime)invoice.date).AddDays(-2);
            }
            if (endDate == null)
            {
                endDate = DateTime.Now;
            }


            List<Chart> charts = new List<Chart>();
            foreach (var item2 in pID)
            {
                Chart chart = new Chart();
                int productID = item2;
                chart.productID = productID;
                List<string> type = new List<string>();
                List<int> counts = new List<int>();


                int discordDays = (int)((DateTime)endDate - (DateTime)startDate).TotalDays;

                List<customProduct> customProduct = new List<customProduct>();
                if (discordDays > 0 && discordDays <= 31)
                {
                    DateTime sd = (DateTime)startDate;
                    DateTime ed = (DateTime)endDate;

                    customProduct = db.InvoiceDetails
                        .Where(i => (i.Invoice.date < ed && i.Invoice.date > sd && i.productID == productID))
                        .GroupBy(c => new { c.Product, c.Invoice.date, c.Invoice })
                        .Select(c => new customProduct { invoice = c.Key.Invoice, productName = c.Key.Product.name, count = c.Sum(w => (w.increaseID.HasValue && !w.decreaseID.HasValue ? w.count : -1 * w.count)) })
                        .ToList();

                    for (int i = 0; i < discordDays; i++)
                    {

                        sd = sd.AddDays(1);
                        type.Add(pep.pep.ToPersianDateShortString(sd).Replace("/", "/"));
                        int sumcount = 0;
                        sumcount = (int)customProduct
                             .Where(c => c.invoice.date == sd)
                             .Sum(c => c.count);
                        counts.Add(sumcount);
                    }

                }
                else if (discordDays > 31 && discordDays <= 93)
                {
                    DateTime sd = (DateTime)startDate;
                    DateTime ed = ((DateTime)startDate).AddDays(7);
                    for (int j = 0; j < discordDays; j += 7)
                    {
                        customProduct.AddRange(db.InvoiceDetails
                            .Where(i => (i.Invoice.date < ed && i.Invoice.date > sd && i.productID == productID))
                            .GroupBy(c => new { c.Product, c.Invoice.date })
                            .Select(c => new customProduct { productName = c.Key.Product.name, count = c.Sum(w => (w.increaseID.HasValue && !w.decreaseID.HasValue ? w.count : -1 * w.count)) })
                            .ToList()
                        );
                        if (customProduct != null && customProduct.Count > 0)
                        {
                            type.Add(pep.pep.ToPersianDateShortString(sd).Replace("/", "/"));
                            int sumcount = 0;
                            sumcount = (int)customProduct
                                 .Sum(c => c.count);
                            counts.Add(sumcount);
                        }
                        else
                        {
                            type.Add(pep.pep.ToPersianDateShortString(sd).Replace("/", "/"));
                            int sumcount = 0;
                            counts.Add(sumcount);
                        }
                        sd = sd.AddDays(7);
                        ed = ed.AddDays(7);
                    }
                }
                else if (discordDays > 93)
                {
                    DateTime sd = (DateTime)startDate;
                    DateTime ed = ((DateTime)startDate).AddDays(31);
                    for (int j = 0; j < discordDays; j += 31)
                    {
                        customProduct.AddRange(db.InvoiceDetails
                            .Where(i => (i.Invoice.date < ed && i.Invoice.date > sd && i.productID == productID))
                            .GroupBy(c => new { c.Product, c.Invoice.date })
                            .Select(c => new customProduct { productName = c.Key.Product.name, count = c.Sum(w => (w.increaseID.HasValue && !w.decreaseID.HasValue ? w.count : -1 * w.count)) })
                            .ToList()
                        );
                        if (customProduct != null && customProduct.Count > 0)
                        {
                            type.Add(pep.pep.ToPersianDateShortString(sd).Replace("/", "/"));
                            int sumcount = 0;
                            sumcount = (int)customProduct
                                 .Sum(c => c.count);
                            counts.Add(sumcount);
                        }
                        else
                        {
                            type.Add(pep.pep.ToPersianDateShortString(sd).Replace("/", "/"));
                            int sumcount = 0;
                            counts.Add(sumcount);
                        }
                        sd = sd.AddDays(31);
                        ed = ed.AddDays(31);
                    }
                }


                chart.xrows = type;
                chart.yrows = counts;

                charts.Add(chart);

            }

            return PartialView("~/Views/Partials/_Chart.cshtml", charts);

        }
    }
}