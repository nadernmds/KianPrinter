//using Stimulsoft.Report;
//using Stimulsoft.Report.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using Shop.Models;
//using System.Drawing;

//namespace Shop.Controllers
//{
//    [RequsetLoginStore()]
//    public class ReportController : Controller
//    {
//        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

//        public ActionResult getReports(string report, int id)
//        {
//            TempData["reportID"] = id;
//            return RedirectToAction("print", routeValues: new { report = report, reportID = id });
//        }

//        [ActionName("getReports2")]
//        public ActionResult getReports(string report, DateTime date, int store, bool color)
//        {
//            TempData["date"] = date;
//            TempData["store"] = store;
//            TempData["color"] = color;
//            return RedirectToAction("print", routeValues: new { report = report, reportID = 0 });
//        }

//        public List<customProduct> getProducts(DateTime date , int store , bool color)
//        {
//            date = pep.pep.toMiladiDate(Convert.ToString(date));
//            List<customProduct> customProducts = new List<customProduct>();
//            if (color)
//            {
//                customProducts = db.InvoiceDetails
//                    .Where(c => c.Invoice.date == date)
//                    .GroupBy(c => new { c.Product, c.ProductColor })
//                    .Select(c => new customProduct { productID = c.Key.Product.productID,  productName = c.Key.Product.name, colorName = c.Key.ProductColor.color, count = c.Sum(w => (w.increaseID.HasValue && !w.decreaseID.HasValue ? w.count : -1 * w.count)) })
//                    .ToList();
//            }
//            else
//            {
//                customProducts = db.InvoiceDetails
//                   .Where(c => c.Invoice.date == date)
//                   .GroupBy(c => new { c.Product, c.productID })
//                   .Select(c => new customProduct { productID=c.Key.productID ,  productName = c.Key.Product.name, count = c.Sum(w => (w.increaseID.HasValue && !w.decreaseID.HasValue ? w.count : -1 * w.count)) })
//                   .ToList();
//            }

//            return customProducts;
//        }

//        public ActionResult cartexReport()
//        {

//            DateTime date = (DateTime)TempData["date"];
//            int store = (int)TempData["store"];
//            bool color = (bool)TempData["color"];

//            List<customProduct> f = getProducts(date , store , color);

//            var mainReport = new StiReport();
//            mainReport.Load(Server.MapPath("~/reports/Cartex.mrt"));
//            mainReport.Compile();

//            List<cartex> cartices = new List<cartex>();
//            foreach (var item in f)
//            {
//                cartex cartex = new cartex();
//                cartex.productCode = productCoding(Convert.ToString(item.productID), 0);
//                cartex.productName = item.productName;
//                if (color)
//                {
//                    cartex.color = item.colorName;
//                }
//                cartex.count = (int)item.count;
//                cartices.Add(cartex);
//            }
//            mainReport.RegBusinessObject("cartex", "cartex", cartices);
//            mainReport["date"] = pep.pep.ToPersianDateShortString(date);
//            //mainReport["reportType"] = "کاردکس";
//            mainReport["shopName"] = " فروشگاه مارکت شاپ";
//            mainReport["storeName"] = db.Stores.Find(store).name;
//            // Image image = File. "~/Images/Banner/17.jpg";
//            mainReport["image"] = Image.FromFile(Server.MapPath("~/img/logo.png"));
//            mainReport.Render();

//            return StiMvcViewer.GetReportSnapshotResult(mainReport);
//        }

        
//        public ActionResult transferReport()
//        {
//            int invoiceID = (int)TempData["reportID"];

//            var f = transfer_havale_resid(invoiceID);

//            var mainReport = new StiReport();
//            mainReport.Load(Server.MapPath("~/reports/Transfer.mrt"));
//            mainReport.Compile();

//            IEnumerable<customeInvoiceDeatils> invoicedetails = f.invoiceDetails.Select(c => new customeInvoiceDeatils { productCode = Convert.ToString(c.productID), productName = c.Product.name, count = c.count, fromStore = c.Store.name, toStore = c.Store1.name, productColorID = c.productColorID });
//            List<customeInvoiceDeatils> updateInvoicedetails = new List<customeInvoiceDeatils>();
//            foreach (var item in invoicedetails)
//            {
//                if (item.productColorID != null)
//                {
//                    item.color = db.ProductColors.Find(item.productColorID).color;
//                    item.productCode = Convert.ToString(productCoding(item.productCode, (int)item.productColorID));
//                }
//                else
//                {
//                    item.productCode = Convert.ToString(productCoding(item.productCode, 0));
//                }
//                updateInvoicedetails.Add(item);
//            }
//            mainReport.RegBusinessObject("transferReport", "transferReport", updateInvoicedetails);
//            mainReport["description"] = f.invoice.description;
//            mainReport["date"] = pep.pep.toPersianDateString((DateTime)f.invoice.date);
//            //mainReport["image"] = ;
//            //var fromstore = db.Stores.First(s => s.storeID == f.invoiceDetails.First().decreaseID);
//            //var tostore = db.Stores.First(s => s.storeID == f.invoiceDetails.First().decreaseID);
//            //mainReport["fromStoreName"] = fromstore.name;
//            //mainReport["toStoreName"] = tostore.name;
//            mainReport["reportType"] = "فاکتور انتقال";
//            mainReport["shopName"] = " فروشگاه مارکت شاپ";
//           // Image image = File. "~/Images/Banner/17.jpg";
//            mainReport["image"] = Image.FromFile(Server.MapPath("~/img/logo.png"));
//            mainReport.Render();

//            return StiMvcViewer.GetReportSnapshotResult(mainReport);
//        }

//        public ActionResult residReport()
//        {
//            int invoiceID = (int)TempData["reportID"];

//            var f = transfer_havale_resid(invoiceID);

//            var mainReport = new StiReport();
//            mainReport.Load(Server.MapPath("~/reports/resid.mrt"));
//            mainReport.Compile();

//            IEnumerable<customeInvoiceDeatils> invoicedetails = f.invoiceDetails.Select(c => new customeInvoiceDeatils {toStore = c.Store.name ,  productCode = Convert.ToString(c.productID), productName = c.Product.name, count = c.count , storeID = c.increaseID , productColorID = c.productColorID});
//            List<customeInvoiceDeatils> updateInvoicedetails = new List<customeInvoiceDeatils>();
//            foreach (var item in invoicedetails)
//            {

//                if (item.productColorID != null)
//                {
//                    item.color = db.ProductColors.Find(item.productColorID).color;
//                    item.productCode = Convert.ToString(productCoding(item.productCode, (int)item.productColorID));
//                }
//                else
//                {
//                    item.productCode = Convert.ToString(productCoding(item.productCode, 0));
//                }
//                updateInvoicedetails.Add(item);
//            }
//            mainReport.RegBusinessObject("resid", "resid", updateInvoicedetails);
//            mainReport["description"] = f.invoice.description;
//            mainReport["date"] = pep.pep.toPersianDateString((DateTime)f.invoice.date);
//            //mainReport["image"] = ;
//            //var storeID = updateInvoicedetails.First().storeID;
//            //var store = db.Stores.First(s => s.storeID == storeID );
//            //mainReport["storeName"] = store.name;
//            mainReport["reportType"] = "فاکتور رسید";
//            mainReport["shopName"] = " فروشگاه مارکت شاپ";
//            mainReport["image"] = Image.FromFile(Server.MapPath("~/img/logo.png"));
//            mainReport.Render();

//            return StiMvcViewer.GetReportSnapshotResult(mainReport);
//        }

//        public ActionResult havaleReport()
//        {
//            int invoiceID = (int)TempData["reportID"];

//            var f = transfer_havale_resid(invoiceID);

//            var mainReport = new StiReport();
//            mainReport.Load(Server.MapPath("~/reports/havale.mrt"));
//            mainReport.Compile();

//            IEnumerable<customeInvoiceDeatils> invoicedetails = f.invoiceDetails.Select(c => new customeInvoiceDeatils {fromStore = Convert.ToString(c.Store1.name) ,   productCode = Convert.ToString(c.productID), productName = c.Product.name, count = c.count, storeID = c.increaseID , productColorID = c.productColorID });
//            List<customeInvoiceDeatils> updateInvoicedetails = new List<customeInvoiceDeatils>();
//            foreach (var item in invoicedetails)
//            {
//                if (item.productColorID != null)
//                {
//                    item.color = db.ProductColors.Find(item.productColorID).color;
//                    item.productCode = Convert.ToString(productCoding(item.productCode, (int)item.productColorID));
//                }
//                else
//                {
//                    item.productCode = Convert.ToString(productCoding(item.productCode, 0));
//                }
//                updateInvoicedetails.Add(item);
//            }
//            mainReport.RegBusinessObject("havale", "havale", updateInvoicedetails);
//            mainReport["description"] = f.invoice.description;
//            mainReport["date"] = pep.pep.toPersianDateString((DateTime)f.invoice.date);
//            //mainReport["image"] = ;
//            //var storeID = updateInvoicedetails.First().storeID;
//            //var store = db.Stores.First(s => s.storeID == storeID);
//            //mainReport["storeName"] = store.name;
//            mainReport["reportType"] = "فاکتور حواله";
//            mainReport["shopName"] = " فروشگاه مارکت شاپ";
//            mainReport["image"] = Image.FromFile(Server.MapPath("~/img/logo.png"));
//            mainReport.Render();

//            return StiMvcViewer.GetReportSnapshotResult(mainReport);
//        }

//        public ActionResult factorReport(int orderID)
//        {
//            var f = factors(orderID);

//            var mainReport = new StiReport();
//            mainReport.Load(Server.MapPath("~/reports/factor.mrt"));
//            mainReport.Compile();

//            mainReport.RegBusinessObject("factorProducts", "factorProducts", f.factorProducts);
//            mainReport["shopName"] = f.shopName;
//            mainReport["orderID"] = f.orderID;
//            mainReport["date"] = pep.pep.toPersianDateString(Convert.ToDateTime(f.date));
//            mainReport["customerName"] = f.customerName;
//            mainReport["address"] = f.address;
//            mainReport["postalCode"] = f.postalCode;
//            mainReport["description"] = f.description;
//            mainReport["shippingCost"] = f.shippingCost;
//            mainReport["reportType"] = "فاکتور";
//            mainReport["image"] = Image.FromFile(Server.MapPath("~/img/logo.png"));
//            mainReport.Render();

//            return StiMvcViewer.GetReportSnapshotResult(mainReport);
//        }

//        //public virtual ActionResult ViewerEvent()
//        //{
//        //    var id = ViewBag.RID;
//        //    return null;// StiMvcViewer.ViewerEventResult(HttpContext);
//        //}

//        public virtual ActionResult Print(string report, int reportID)
//        {
//            ViewBag.ReportID = reportID;
//            ViewBag.Report = report;
//            return View();
//        }

//        //public virtual ActionResult PrintReport()
//        //{
//        //    return StiMvcViewer.PrintReportResult(HttpContext);
//        //}

//        //public virtual FileResult ExportReport()
//        //{
//        //    return StiMvcViewer.ExportReportResult(HttpContext);
//        //}

//        //public virtual ActionResult Interaction()
//        //{
//        //    return StiMvcViewer.InteractionResult(HttpContext);
//        //}

//        private string productCoding(string productID, int colorID)
//        {

//            string code = "";

//            var product = db.Products.Find(Convert.ToInt32(productID));
//            var categoryID = product.categoryID;

//            switch (categoryID.ToString().Length)
//            {
//                case 1:
//                    code += "00" + categoryID;
//                    break;
//                case 2:
//                    code += "0" + categoryID;
//                    break;
//                default:
//                    code += categoryID;
//                    break;
//            }
//            switch (productID.ToString().Length)
//            {
//                case 1:
//                    code += "000" + productID;
//                    break;
//                case 2:
//                    code += "00" + productID;
//                    break;
//                case 3:
//                    code += "0" + productID;
//                    break;
//                default:
//                    code += productID;
//                    break;
//            }
//            if (colorID != 0)
//            {
//                switch (colorID.ToString().Length)
//                {
//                    case 1:
//                        code += "00" + colorID;
//                        break;
//                    case 2:
//                        code += "0" + colorID;
//                        break;
//                    default:
//                        code += colorID;
//                        break;
//                }
//            }
//            return code;
//        }



//        private string orderCoding(int orderID, int userID, int cityID)
//        {
//            string code = "";
//            switch (orderID.ToString().Length)
//            {
//                case 1:
//                    code += "000" + orderID;
//                    break;
//                case 2:
//                    code += "00" + orderID;
//                    break;
//                case 3:
//                    code += "0" + orderID;
//                    break;
//                default:
//                    code += orderID;
//                    break;
//            }
//            switch (userID.ToString().Length)
//            {
//                case 1:
//                    code += "000" + userID;
//                    break;
//                case 2:
//                    code += "00" + userID;
//                    break;
//                case 3:
//                    code += "0" + userID;
//                    break;
//                default:
//                    code += userID;
//                    break;
//            }
//            switch (cityID.ToString().Length)
//            {
//                case 1:
//                    code += "000" + cityID;
//                    break;
//                case 2:
//                    code += "00" + cityID;
//                    break;
//                case 3:
//                    code += "0" + cityID;
//                    break;
//                default:
//                    code += cityID;
//                    break;
//            }
//            return code;
//        }

//        public transfer transfer_havale_resid(int invoiceID)
//        {
//            transfer t = new transfer();
//            t.invoice = db.Invoices.Find(invoiceID);
//            var invoiceDetails = db.InvoiceDetails.Where(i => i.invoiceID == invoiceID).ToList();
//            t.invoiceDetails = invoiceDetails;
//            return t;
//        }


//        public factor factors(int orderID)
//        {
//            User user = (User)Session["UU!#user"];
//            string shopName = db.Contents.First().title;
//            var dateAndTime = DateTime.Now;
//            var date = dateAndTime.Date;
//            user = db.Users.FirstOrDefault(u => u.userID == user.userID);
//            string customerName = user.name;
//            var order = db.Ordes.Find(orderID);
//            var address = order.Address;
//            var fullAddress = address.City.State.stateName + "\t" + address.City.cityName + "\t" + address.addressDetail;
//            string postalCode = address.postalCode;
//            int shippingCost = (int)order.shippingCost;
//            string orderDescription = order.description;
//            //var OrderProducts = order.OrderProducts.Select(p => p.productID).ToList();
//            //var products = db.Products.Where(p => productsID.Contains(p.productID)).ToList();
//            decimal allProductsPrice = 0;
//            List<factorProducts> factorProducts = new List<factorProducts>();
//            DateTime now = Convert.ToDateTime(DateTime.Now.ToString());
//            int row = 0;
//            foreach (var item2 in order.OrderProducts)
//            {
//                var product = item2.Product;
//                row++;
//                var offers = product.Offers.Where(o => o.startDate < now && o.endDate > now).ToList();
//                decimal offPrice = 0;
//                decimal offPercent = 0;
//                foreach (var item in offers)
//                {
//                    offPrice += (int)item.price;
//                    offPercent += (int)item.offPercent;
//                }
//                decimal productPrice = product.price;
//                decimal finalPrice = productPrice;
//                if (offPercent > 0)
//                {
//                    finalPrice = finalPrice - (productPrice * (offPercent / 100));
//                }
//                if (offPrice > 0)
//                {
//                    finalPrice = finalPrice - offPrice;
//                }
//                var offer = productPrice - finalPrice;
//                var counter = (int)item2.count;
//                factorProducts factorProduct = new factorProducts
//                {
//                    row = row,
//                    productName = product.name,
//                    count = (int)item2.count,
//                    color = item2.ProductColor.color,
//                    pricePerOne = productPrice,
//                    offerPerOne = offer,
//                    finalOffer = offer * counter,
//                    finalPrice = (productPrice * counter) - (offer * counter)
//                };
//                factorProducts.Add(factorProduct);
//                allProductsPrice += factorProduct.finalPrice;
//            }
//            factor factor = new factor
//            {
//                shopName = shopName,
//                orderID = orderID,
//                date = Convert.ToString(date),
//                customerName = user.name,
//                address = fullAddress,
//                postalCode = postalCode,
//                factorProducts = factorProducts,
//                description = orderDescription,
//                shippingCost = shippingCost
//            };
//            return factor;
//        }
//    }
//    public class factor
//    {
//        public string shopName { get; set; }
//        public int orderID { get; set; }
//        public string date { get; set; }
//        public string customerName { get; set; }
//        public string address { get; set; }
//        public string postalCode { get; set; }
//        public List<factorProducts> factorProducts { get; set; }
//        public string description { get; set; }
//        public int shippingCost { get; set; }
//    }
//    public class factorProducts
//    {
//        public int row { get; set; }
//        public string productName { get; set; }
//        public int count { get; set; }
//        public string color { get; set; }
//        public decimal pricePerOne { get; set; }
//        public decimal offerPerOne { get; set; }
//        public decimal finalPrice { get; set; }
//        public decimal finalOffer { get; set; }
//    }
//    public class transfer
//    {
//        public List<InvoiceDetail> invoiceDetails { get; set; }
//        public Invoice invoice { get; set; }
//    }
//    public class customeInvoiceDeatils
//    {
//        public string productCode { get; set; }
//        public string productName { get; set; }
//        public int? count { get; set; }
//        public string fromStore { get; set; }
//        public string toStore { get; set; }
//        public int? productColorID { get; set; }
//        public Image image { get; set; }
//        public int? storeID { get; set; }
//        public string color { get; set; }
//    }
//    public class cartex
//    {
//        public string productCode { get; set; }
//        public string productName { get; set; }
//        public string description { get; set; }
//        public int count { get; set; }
//        public string color { get; set; }
//    }

//}