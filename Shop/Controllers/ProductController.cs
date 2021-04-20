using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using PagedList;
using Shop.Models;

namespace Shop.Controllers
{
    public class ProductController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: Product
        public ActionResult Index(string search, int? page, string sort)
        {
            IEnumerable<Product> model = db.Products.Include(p => p.Brand).Include(p => p.ProductCategory).Include(p => p.ProductState);
            if (search != null)
            {
                model = db.Products.Where(s => s.name.Contains(search)).ToList();
            }
            switch (sort)
            {
                case "name":
                    model = model.OrderBy(s => s.name);
                    break;
                case "name_desc":
                    model = model.OrderByDescending(s => s.name);
                    break;
                case "price":
                    model = model.OrderBy(s => s.price);
                    break;
                case "price_desc":
                    model = model.OrderByDescending(s => s.price);
                    break;
                case "shortDescription":
                    model = model.OrderBy(s => s.shortDescription);
                    break;
                case "shortDescription_desc":
                    model = model.OrderByDescending(s => s.shortDescription);
                    break;
                case "existingCount":
                    model = model.OrderBy(s => s.existingCount);
                    break;
                case "existingCount_desc":
                    model = model.OrderByDescending(s => s.existingCount);
                    break;
                case "brandName":
                    model = model.OrderBy(s => s.Brand?.brandName);
                    break;
                case "brandName_desc":
                    model = model.OrderByDescending(s => s.Brand?.brandName);
                    break;
                case "categoryName":
                    model = model.OrderBy(s => s.ProductCategory?.categoryName);
                    break;
                case "categoryName_desc":
                    model = model.OrderByDescending(s => s.ProductCategory?.categoryName);
                    break;
                case "state":
                    model = model.OrderBy(s => s.ProductState?.state);
                    break;
                case "state_desc":
                    model = model.OrderByDescending(s => s.ProductState?.state);
                    break;
            }
            ViewBag.SortType = sort;
            return View(model.ToList().ToPagedList(page ?? 1, 10));
        }
        [RequsetLogin(2)]
        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [RequsetLogin(2)]
        // GET: Product/Create
        public ActionResult Create()
        {
            ViewBag.brandID = new SelectList(db.Brands, "brandID", "brandName");
            ViewBag.categoryID = new SelectList(db.ProductCategories, "categoryID", "categoryName");
            ViewBag.productStateID = new SelectList(db.ProductStates, "productStateID", "state");


            ViewBag.productColors = db.ProductColors.ToList();


            return View();
        }
        [RequsetLogin(2)]
        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "productID,name,price,shortDesciption,description,categoryID,brandID,productStateID,existingCount,vitrin")] Product product, HttpPostedFileBase file, HttpPostedFileBase[] Images, long[] productAtrributeTemplateID, string[] value, string[] keywords, List<bool> exist, List<int?> colorID, string googletitle, string googledescription)
        {


            colorID = colorID.Where(c => c.HasValue).ToList();


            if (ModelState.IsValid)
            {



                product.Seos.Add(new Seo()
                {
                    productID = product.productID,
                    title = googletitle,
                    description = googledescription
                });


                if (product.vitrin == null)
                {
                    product.vitrin = false;
                }
                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        product.ProductAttributes.Add(
                           new ProductAttribute
                           {
                               value = value[i],
                               productAtrributeTemplateID = productAtrributeTemplateID[i],

                           });
                    }
                }


                db.Products.Add(product);
                db.SaveChanges();

                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/Product/icon/" + product.productID + ".jpg"));
                }

                if (Images != null)
                {
                    foreach (var item in Images)
                    {

                        var productimage = new ProductImage()
                        {
                            productID = product.productID

                        };
                        db.ProductImages.Add(productimage);
                        db.SaveChanges();
                        if (item != null)
                        {
                            item.SaveAs(Server.MapPath("~/Images/Product/images/" + productimage.productImageID + ".jpg"));
                        }
                    }
                }


                if (keywords != null)
                {
                    foreach (var item in keywords)
                    {
                        ProductKeyword productKeyword = new ProductKeyword
                        {
                            text = item,
                            productID = product.productID,
                        };
                        db.ProductKeywords.Add(productKeyword);

                    }

                    db.SaveChanges();
                }



                if (colorID != null)
                {
                    foreach (var item in colorID)
                    {
                        ProductColorAssinment productColorAssinment = new ProductColorAssinment
                        {

                            productID = product.productID,
                            productColorID = item
                        };
                        db.ProductColorAssinments.Add(productColorAssinment);
                    }

                    db.SaveChanges();
                }




                //if (color != null)
                //{
                //    int i = 0;
                //    foreach (var item in color)
                //    {
                //        ProductColor productColor = new ProductColor
                //        {
                //            color = color[i],
                //            colorCode = colorCode[i],
                //            exist = exist[i],
                //            productID = product.productID
                //        };
                //        db.ProductColors.Add(productColor);
                //        i++;
                //    }

                //    db.SaveChanges();
                //}

                return RedirectToAction("Index");
            }

            ViewBag.brandID = new SelectList(db.Brands, "brandID", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.ProductCategories, "categoryID", "categoryName", product.categoryID);
            ViewBag.productStateID = new SelectList(db.ProductStates, "productStateID", "state", product.productStateID);

            ViewBag.productKeywords = db.ProductKeywords.Where(b => b.productID == product.productID).ToList();
            return View(product);
        }
        [RequsetLogin(2)]
        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.brandID = new SelectList(db.Brands, "brandID", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.ProductCategories, "categoryID", "categoryName", product.categoryID);
            ViewBag.productStateID = new SelectList(db.ProductStates, "productStateID", "state", product.productStateID);

            ViewBag.productKeywords = db.ProductKeywords.Where(b => b.productID == product.productID).ToList();


            ViewBag.productImages = db.ProductImages.Where(p => p.productID == id).ToList();

            //var ProductColorAssinments = 
            //if(ProductColorAssinments != null)
            //{
            //    var productColorID = ProductColorAssinments.Select(p => p.productColorID).ToList();
            //    ViewBag.productColors = db.ProductColors.Where(p => productColorID.Contains(p.productColorID)).ToList();
            //}
            //else
            //{
            ViewBag.productColors = db.ProductColors.Distinct().ToList();
            //}



            var ProductAttribute = db.ProductAttributes.Where(pta => pta.productID == product.productID).ToList();
            var patID = ProductAttribute.Select(pa => pa.productAtrributeTemplateID).Distinct().ToList();
            var ProductAttributeTemplates = db.ProductAttributeTemplates
                .Where(pt => patID
                .Contains(pt.productAtrributeTemplateID))
                .ToList();
            ViewBag.ProductAttributeTemplates = ProductAttributeTemplates;


            return View(product);
        }
        [RequsetLogin(2)]
        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "productID,name,price,shortDescription,description,categoryID,brandID,productStateID,existingCount,vitrin")] Product product, HttpPostedFileBase file, HttpPostedFileBase[] Images, long[] productAtrributeTemplateID, string[] value, string[] keywords, List<bool> exist, List<int?> colorID, string googletitle, string googledescription)
        {
            colorID = colorID.Where(c => c.HasValue).ToList();

            if (ModelState.IsValid)
            {

                db.Seos.RemoveRange(db.Seos.Where(s => s.productID == product.productID));


                Seo seo = new Seo();
                seo.productID = product.productID;
                seo.title = googletitle;
                seo.description = googledescription;
                db.Seos.Add(seo);




                if (product.vitrin == null)
                {
                    product.vitrin = false;
                }

                var ProductAttributes = db.ProductAttributes.Where(pa => pa.productID == product.productID).ToList();
                db.ProductAttributes.RemoveRange(ProductAttributes);
                db.SaveChanges();


                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();


                if (value != null)
                {
                    for (int i = 0; i < value.Length; i++)
                    {

                        ProductAttribute productAttribut = new ProductAttribute
                        {
                            value = value[i],
                            productAtrributeTemplateID = productAtrributeTemplateID[i],
                            productID = product.productID
                        };

                        db.ProductAttributes.Add(productAttribut);
                    }
                }
                db.SaveChanges();


                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/Product/" + product.productID + ".jpg"));
                }

                if (Images != null)
                {


                    foreach (var item in Images)
                    {

                        var productimage = new ProductImage()
                        {
                            productID = product.productID

                        };
                        db.ProductImages.Add(productimage);
                        db.SaveChanges();
                        if (item != null)
                        {
                            item.SaveAs(Server.MapPath("~/Images/Product/images/" + productimage.productImageID + ".jpg"));
                        }
                    }
                }

                var pk = db.ProductKeywords.Where(c => c.productID == product.productID).ToList();

                if (pk != null)
                {
                    foreach (var item in pk)
                    {
                        db.ProductKeywords.Remove(item);
                    }
                    db.SaveChanges();
                }


                if (keywords != null)
                {
                    foreach (var item in keywords)
                    {
                        ProductKeyword productKeyword = new ProductKeyword
                        {
                            text = item,
                            productID = product.productID,
                        };
                        db.ProductKeywords.Add(productKeyword);

                    }
                    db.SaveChanges();
                }




                var pca = db.ProductColorAssinments.Where(c => c.productID == product.productID).ToList();

                if (pca != null)
                {
                    foreach (var item in pca)
                    {
                        db.ProductColorAssinments.Remove(item);
                    }
                    db.SaveChanges();
                }

                if (colorID != null)
                {
                    foreach (var item in colorID)
                    {
                        ProductColorAssinment productColorAssinment = new ProductColorAssinment
                        {
                            productID = product.productID,
                            productColorID = item
                        };
                        db.ProductColorAssinments.Add(productColorAssinment);
                    }

                    db.SaveChanges();
                }



                return RedirectToAction("Index");
            }
            ViewBag.brandID = new SelectList(db.Brands, "brandID", "brandName", product.brandID);
            ViewBag.categoryID = new SelectList(db.ProductCategories, "categoryID", "categoryName", product.categoryID);
            ViewBag.productStateID = new SelectList(db.ProductStates, "productStateID", "state", product.productStateID);

            ViewBag.productKeywords = db.ProductKeywords.Where(b => b.productID == product.productID).ToList();



            var ProductColorAssinments = db.ProductColorAssinments.Where(b => b.productID == product.productID).ToList();
            if (ProductColorAssinments != null)
            {
                var productColorID = ProductColorAssinments.Select(p => p.productColorID).ToList();
                ViewBag.ProductColors = db.ProductColors.Where(p => productColorID.Contains(p.productColorID)).ToList();
            }
            else
            {
                ViewBag.ProductColors = null;
            }


            //var productColorID = db.ProductColorAssinments.Where(b => b.productID == product.productID).Select(p => p.productColorID).ToList();
            //ViewBag.ProductColors = db.ProductColors.Where(p => productColorID.Contains(p.productColorID)).ToList();

            return View(product);
        }
        [RequsetLogin(2)]
        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        [RequsetLogin(2)]
        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [RequsetLogin(2)]
        [HttpPost]
        public ActionResult deleteProductImage(int id)
        {
            var productImage = db.ProductImages.Find(id);
            db.ProductImages.Remove(productImage);
            db.SaveChanges();

            return Json(new { success = true });
        }


        public ActionResult Vitrin(string search, int? page)
        {
            IEnumerable<Product> model = db.Products.Include(p => p.Brand).Include(p => p.ProductCategory).Include(p => p.ProductState);
            if (search != null)
            {
                model = db.Products.Where(s => s.name.Contains(search)).ToList();
            }
            return View(model.ToList().ToPagedList(page ?? 1, 9));
        }

        [HttpPost]
        public ActionResult Vitrin(int id)
        {
            var vitrins = db.Products.Where(v => v.vitrin == true).ToList();
            var product = db.Products.Find(id);




            if (product.vitrin == true && product.vitrin != null)
            {
                product.vitrin = false;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, state = "removed" });
            }
            else if (vitrins.Count > 20)
            {
                return Json(new { success = false, data = "ظرفیت ویترین کامل است" });
            }
            else
            {
                product.vitrin = true;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { success = true, state = "added" });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult Show(int id, string message)
        {

            //////////////////////////////////////////////////////////////////////////////
            List<Product> products = new List<Product>();
            User user = (User)Session["UU!#user"];
            HttpCookie cookie = Request.Cookies["useTtrackingProductCategory"];
            if (user != null)
            {
                var offerForCustomer = db.UserTrackings.Where(ut => ut.userID == user.userID).ToList();
                offerForCustomer = offerForCustomer.Distinct().ToList();

                offerForCustomer = offerForCustomer.Take(3).ToList();

                var catID = offerForCustomer.Select(p => p.Product.ProductCategory.categoryID).ToList();

                foreach (var item in catID)
                {
                    products.AddRange(db.Products.Where(p => catID.Contains((int)p.categoryID)).ToList());
                }

            }
            if (cookie != null)
            {
                List<userTrackingCategrory> userTrackingCategrory = new List<userTrackingCategrory>();
                if (cookie.HasKeys)
                {
                    foreach (string value in cookie.Values)
                    {
                        userTrackingCategrory.Add(JsonConvert.DeserializeObject<userTrackingCategrory>(value));
                    }
                }


                userTrackingCategrory = userTrackingCategrory.Distinct().ToList();
                userTrackingCategrory = userTrackingCategrory.Take(3).ToList();


                List<Product> lowerPriceproducts = new List<Product>();
                List<Product> higherPriceproducts = new List<Product>();


                foreach (var item in userTrackingCategrory)
                {
                    var product = db.Products.Where(p => p.categoryID == item.categoryID);
                    lowerPriceproducts.AddRange(product.Where(p => p.price < item.productPrice).ToList());
                    higherPriceproducts.AddRange(product.Where(p => p.price > item.productPrice).ToList());
                }



                lowerPriceproducts = lowerPriceproducts.OrderByDescending(p => p.price).ToList();
                higherPriceproducts = higherPriceproducts.OrderBy(p => p.price).ToList();

                if (lowerPriceproducts.Count > 0)
                {
                    products.AddRange(lowerPriceproducts);

                }
                if (higherPriceproducts.Count > 0)
                {
                    products.AddRange(higherPriceproducts);
                }
                if (!(products.Count > 0))
                {
                    foreach (var item in userTrackingCategrory)
                    {
                        products.AddRange(db.Products.Where(p => p.categoryID == item.categoryID).ToList());

                    }
                }

            }




            if (!(products.Count > 0))
            {

                var highSalesProducts = db.Products.Where(c => c.OrderProducts.Sum(w => w.count) > 0).OrderBy(w => w.OrderProducts.Sum(e => e.count)).Take(10).ToList();
                highSalesProducts = highSalesProducts.Random(3).ToList();

                List<Product> product = new List<Product>();

                foreach (var item in highSalesProducts)
                {
                    product.AddRange(db.Products.Where(p => p.categoryID == item.categoryID).Take(15).ToList());
                }


                products = product;

            }

            products = products.Distinct().Take(20).ToList();



            ViewBag.offerForCustomer = products;

            ///////////////////////////////////////////////////////////////////////////////////////////////

            ViewBag.message = message;
            List<Product> OfferProducts = new List<Product>();
            var offers = db.Offers.Where(o => (o.offPercent != null || o.price != null)).OrderByDescending(o => o.offPercent).ToList();
            offers = offers.Distinct().ToList();
            ViewBag.brands = db.Brands.ToList();
            foreach (var item in offers)
            {
                OfferProducts.Add(item.Product);
            }

            ViewBag.productHaveOffer = OfferProducts.Distinct().ToList(); ;
            ViewBag.productsWithHighSales = db.Products.Where(c => c.OrderProducts.Sum(w => w.count) > 0).OrderBy(w => w.OrderProducts.Sum(e => e.count)).Take(20).ToList();
            var producted = db.Products.Find(id);



            try
            {
                var relatedProducts = db.Products.Where(p => p.categoryID == producted.categoryID).ToList();
                ViewBag.relatedProducts = relatedProducts.Distinct().ToList();
            }
            catch
            {

                ViewBag.relatedProducts = null;
            }


            /////////////////////////////////////////////////////////////////////////////



            var visitCookie = Request.Cookies["visited"];

            if (visitCookie != null)
            {
                List<int> productID = new List<int>();
                if (visitCookie.HasKeys)
                {
                    foreach (string value in visitCookie.Values)
                    {
                        productID.Add(Convert.ToInt32(value));
                    }
                }
                visitCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(visitCookie);
                //////////////////////////////////////////////////////////////////
                HttpCookie updateVisitCookie = new HttpCookie("visited");
                DateTime now = DateTime.Now;
                updateVisitCookie.Expires = now.AddHours(1);

                bool visited = false;
                foreach (var item in productID)
                {
                    if (item == id)
                    {
                        visited = true;
                    }
                    updateVisitCookie.Values.Add(Convert.ToString(item), Convert.ToString(item));
                }


                if (!visited)
                {
                    updateVisitCookie.Values.Add(Convert.ToString(id), Convert.ToString(id));


                    //////////////////visited product counter
                    var v = db.Products.Find(id);
                    v.visit = v.visit + 1;
                    db.Entry(v).State = EntityState.Modified;
                    db.SaveChanges();
                    ////////////////////////////////////////////
                }

                Response.Cookies.Add(updateVisitCookie);



            }
            else
            {
                HttpCookie makeVisitCookie = new HttpCookie("visited");

                DateTime now = DateTime.Now;
                makeVisitCookie.Expires = now.AddHours(1);


                makeVisitCookie.Values.Add(Convert.ToString(id), Convert.ToString(id));
                Response.Cookies.Add(makeVisitCookie);



                //////////////////visited product counter
                var v = db.Products.Find(id);
                v.visit = v.visit + 1;
                db.Entry(v).State = EntityState.Modified;
                db.SaveChanges();
                ////////////////////////////////////////////


            }


            /////////////////////////////////////////////////////////////////////////////


            ViewBag.productInfoes = db.ProductAttributes.Where(pa => pa.productID == id).ToList();

            ViewBag.AttributeGroup = db.ProductAttributeTemplateGroups.Where(p => p.ProductCategory.categoryID == producted.categoryID).ToList();

            var ProductColorAssinments = db.ProductColorAssinments.Where(b => b.productID == producted.productID).ToList();
            if (ProductColorAssinments != null)
            {
                var productColorID = ProductColorAssinments.Select(p => p.productColorID).ToList();
                ViewBag.productColors = db.ProductColors.Where(p => productColorID.Contains(p.productColorID)).ToList();
            }
            else
            {
                ViewBag.productColors = null;
            }

            //var productColorID = db.ProductColorAssinments.Where(b => b.productID == product.productID).Select(p => p.productColorID).ToList();
            //ViewBag.productColors = db.ProductColors.Where(p => productColorID.Contains(p.productColorID)).ToList();



            // ViewBag.productColors = db.ProductColors.Where(pc => pc.productID == id && pc.exist == true).ToList();


            var comments = db.Comments.Where(p => p.productID == id && p.approveUserID != null).ToList();
            ViewBag.comments = comments;
            ViewBag.commentRate = comments.Average(c => c.rate).ToString();


            ViewBag.questionAnswers = db.Questions.Where(q => q.productID == id && q.approveUserID != null).ToList(); ;

            return View(producted);
        }

        [HttpPost]
        public ActionResult updateCart(List<int> quantity, List<int> productID, List<string> color, List<string> colorCode)
        {
            var positiveValue = quantity.Where(q => q < 0).ToList();
            if (positiveValue.Any())
            {
                return RedirectToAction("showCart", "product");
            }


            DateTime now = DateTime.Now;

            User user = (User)Session["UU!#user"];
            if (user != null)
            {
                var userCart = db.Carts.Where(c => c.userID == user.userID);
                foreach (var item in userCart)
                {
                    db.Carts.Remove(item);
                }

                db.SaveChanges();





                int i = 0;
                foreach (var item in productID)
                {
                    now.AddDays(1);
                    Cart cart = new Cart { userID = user.userID, productID = item, count = quantity[i], expire = now, ProductColor = db.ProductColors.Where(a => a.color == color[i]).FirstOrDefault() };
                    i++;
                    db.Carts.Add(cart);

                }
                db.SaveChanges();

            }


            HttpCookie cookie = new HttpCookie("uu!@#cart");

            cookie.Expires = now.AddDays(1);

            int j = 0;
            foreach (var item in productID)
            {
                //var defaultColor = db.ProductColors.FirstOrDefault(pc => pc.productID == item);
                ProductColor defaultColor = null;
                var ProductColorAssinments = db.ProductColorAssinments.FirstOrDefault(p => p.productID == item);
                if (ProductColorAssinments != null)
                {
                    defaultColor = ProductColorAssinments.ProductColor;
                }

                //var colordb = db.ProductColors.FirstOrDefault(a => a.color.Equals(color[j]));

                cartProduct cartProduct = new cartProduct { productID = item, count = quantity[j], color = color[j], colorCode = colorCode[j] };
                j++;


                string json = JsonConvert.SerializeObject(cartProduct);


                cookie.Values.Add(Convert.ToString(json), Convert.ToString(item));

            }


            Response.Cookies.Add(cookie);



            return RedirectToAction("showCart", "product");

        }


























        [HttpPost]
        public ActionResult addToCart(int productID, int? colorID)
        {

            ViewBag.add = true;

            User user = (User)Session["UU!#user"];
            if (user != null)
            {
                if (colorID > 0)
                {
                    DateTime now = DateTime.Now;
                    now.AddDays(1);
                    Cart cart = new Cart { userID = user.userID, productID = productID, count = 1, expire = now, productColorID = colorID };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }
                else
                {
                    DateTime now = DateTime.Now;
                    now.AddDays(1);
                    Cart cart = new Cart { userID = user.userID, productID = productID, count = 1, expire = now };
                    db.Carts.Add(cart);
                    db.SaveChanges();
                }
            }



            var cookie = Request.Cookies["uu!@#cart"];

            if (cookie != null)
            {

                List<cartProduct> product = new List<cartProduct>();


                if (cookie.HasKeys)
                {
                    foreach (string value in cookie.Values.AllKeys)
                    {

                        string json = value;
                        cartProduct cartProduct = JsonConvert.DeserializeObject<cartProduct>(json);


                        product.Add(cartProduct);


                    }

                }


                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
                //////////////////////////////////////////////////////////////////

                HttpCookie compareCookie = new HttpCookie("uu!@#cart");
                DateTime now = DateTime.Now;
                compareCookie.Expires = now.AddDays(1);




                foreach (var item in product)
                {
                    if (colorID > 0)
                    {
                        var color = db.ProductColors.Find(colorID);

                        cartProduct cartProduct = new cartProduct { productID = item.productID, count = 1, color = color.color, colorCode = color.colorCode };
                        string json = JsonConvert.SerializeObject(cartProduct);


                        compareCookie.Values.Add(Convert.ToString(json), Convert.ToString(item));
                    }
                    else
                    {
                        //var defaultColors = db.ProductColors.FirstOrDefault(pc => pc.productID == item.productID);


                        ProductColor defaultColors = null;
                        var ProductColorAssinments = db.ProductColorAssinments.FirstOrDefault(p => p.productID == item.productID);
                        if (ProductColorAssinments != null)
                        {
                            defaultColors = ProductColorAssinments.ProductColor;
                        }

                        //var defaultColors = db.ProductColorAssinments.FirstOrDefault(p => p.productID == item.productID).ProductColor;


                        cartProduct cartProduct = new cartProduct { productID = item.productID, count = 1, color = (defaultColors != null) ? defaultColors.color : null, colorCode = (defaultColors != null) ? defaultColors.colorCode : null };
                        string json = JsonConvert.SerializeObject(cartProduct);


                        compareCookie.Values.Add(Convert.ToString(json), Convert.ToString(item));
                    }


                }

                cartProduct cartProduct2 = new cartProduct { };

                if (colorID > 0)
                {
                    var color = db.ProductColors.Find(colorID);

                    cartProduct2 = new cartProduct { productID = productID, count = 1, color = color.color, colorCode = color.colorCode };

                }
                else
                {
                    //var defaultColor = db.ProductColorAssinments.FirstOrDefault(p => p.productID == productID).ProductColor;


                    ProductColor defaultColor = null;
                    var ProductColorAssinments = db.ProductColorAssinments.FirstOrDefault(p => p.productID == productID);
                    if (ProductColorAssinments != null)
                    {
                        defaultColor = ProductColorAssinments.ProductColor;
                    }


                    //var defaultColor = db.ProductColors.FirstOrDefault(pc => pc.productID == productID);

                    cartProduct2 = new cartProduct { productID = productID, count = 1, color = (defaultColor != null) ? defaultColor.color : null, colorCode = (defaultColor != null) ? defaultColor.colorCode : null };

                   

                }

                string json2 = JsonConvert.SerializeObject(cartProduct2);

                compareCookie.Values.Add(Convert.ToString(json2), Convert.ToString(productID));
                Response.Cookies.Add(compareCookie);




                var pro = db.Products.Find(productID);
                var cart = new Cart { Product = pro, productID = pro.productID, count = 1 };
                if (colorID > 0)
                {
                    cart.ProductColor = db.ProductColors.Find(colorID);
                    // return Json(new { success = true, name = pro.name, colorcode = col.colorCode, pID = pro.productID });
                }

                ///    return Json(new { success = true, name = pro.name, color = "", });

                return PartialView("_cartPopupItem", cart);


            }
            else
            {
                HttpCookie cartCookie = new HttpCookie("uu!@#cart");

                DateTime now = DateTime.Now;
                cartCookie.Expires = now.AddDays(1);




                cartProduct cartProduct = new cartProduct { };

                if (colorID > 0)
                {
                    var color = db.ProductColors.Find(colorID);

                    cartProduct = new cartProduct { productID = productID, count = 1, color = color.color, colorCode = color.colorCode };

                }
                else
                {
                    //var defaultColor = db.ProductColorAssinments.FirstOrDefault(p => p.productID == productID).ProductColor;

                    ProductColor defaultColor = null;
                    var ProductColorAssinments = db.ProductColorAssinments.FirstOrDefault(p => p.productID == productID);
                    if (ProductColorAssinments != null)
                    {
                        defaultColor = ProductColorAssinments.ProductColor;
                    }


                    //var defaultColor = db.ProductColors.FirstOrDefault(pc => pc.productID == productID);

                    cartProduct = new cartProduct { productID = productID, count = 1, color = (defaultColor != null) ? defaultColor.color : null, colorCode = (defaultColor != null) ? defaultColor.colorCode : null };

                }




                string json = JsonConvert.SerializeObject(cartProduct);


                cartCookie.Values.Add(Convert.ToString(json), Convert.ToString(productID));
                Response.Cookies.Add(cartCookie);




                var pro = db.Products.Find(productID);
                var cart = new Cart { Product = pro, productID = pro.productID ,count = 1 };
               if (colorID > 0)
               {
                 cart.ProductColor =    db.ProductColors.Find(colorID);
                  // return Json(new { success = true, name = pro.name, colorcode = col.colorCode, pID = pro.productID });
               }

                ///    return Json(new { success = true, name = pro.name, color = "", });

                return PartialView("_cartPopupItem", cart);


            }


        }

        [HttpPost]
        public ActionResult removeOfCart(int productID)
        {

            decimal removedProductPrice = 0;
            decimal removedProductOffer = 0;
            decimal removedProductCount = 0;


            List<cartProduct> product = new List<cartProduct>();


            var getcartCookie = Request.Cookies["uu!@#cart"];


            if (getcartCookie != null)
            {
                if (getcartCookie.HasKeys)
                {
                    foreach (string value in getcartCookie.Values.AllKeys)
                    {
                        string json = value;
                        cartProduct cartProduct = JsonConvert.DeserializeObject<cartProduct>(json);

                        if (cartProduct.productID > 0 && cartProduct.productID != productID)
                        {
                            product.Add(cartProduct);
                        }
                        if (cartProduct.productID == productID)
                        {
                            var removedProduct = db.Products.Find(productID);



                            removedProductCount = cartProduct.count;



                            DateTime nows = Convert.ToDateTime(DateTime.Now.ToString());
                            var offers = removedProduct.Offers.Where(o => o.startDate < nows && o.endDate > nows).ToList();
                            decimal offPrice = 0;
                            decimal offPercent = 0;
                            foreach (var item2 in offers)
                            {
                                offPrice += (int)item2.price;
                                offPercent += (int)item2.offPercent;
                            }
                            decimal productPrice = removedProduct.price;
                            decimal finalPrice = productPrice;
                            if (offPercent > 0)
                            {
                                finalPrice = finalPrice - (productPrice * (offPercent / 100));
                            }
                            if (offPrice > 0)
                            {
                                finalPrice = finalPrice - offPrice;
                            }

                            removedProductPrice = finalPrice;
                            removedProductOffer = productPrice - finalPrice;


                        }

                    }

                }
            }
            getcartCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(getcartCookie);


            HttpCookie cartCookie = new HttpCookie("uu!@#cart");
            DateTime now = DateTime.Now;
            cartCookie.Expires = now.AddDays(1);
            foreach (var item in product)
            {
                //var defaultColor = db.ProductColorAssinments.FirstOrDefault(p => p.productID == item.productID).ProductColor;

                ProductColor defaultColor = null;
                var ProductColorAssinments = db.ProductColorAssinments.FirstOrDefault(p => p.productID == item.productID);
                if (ProductColorAssinments != null)
                {
                    defaultColor = ProductColorAssinments.ProductColor;
                }


                //var defaultColor = db.ProductColors.FirstOrDefault(pc => pc.productID == item.productID);

                cartProduct cartProduct = new cartProduct { productID = item.productID, count = 1, color = (defaultColor != null) ? defaultColor.color : null, colorCode = (defaultColor != null) ? defaultColor.colorCode : null };
                string json = JsonConvert.SerializeObject(cartProduct);


                cartCookie.Values.Add(Convert.ToString(json), Convert.ToString(item));
            }
            //cartCookie.Values.Add(Convert.ToString(productID), Convert.ToString(productID));


            if (product.Count > 0)
            {
                Response.Cookies.Add(cartCookie);
            }


            return Json(new { success = true, data = productID, counter = cartCookie.Values.Count, removeProductPrice = removedProductPrice, removeProductOffer = removedProductOffer, removedProductCount = removedProductCount });

        }



        public ActionResult showCart()
        {

            List<Product> products = new List<Product>();
            List<cartProduct> cartProducts = new List<cartProduct>();



            var cartCookie = Request.Cookies["uu!@#cart"];
            if (cartCookie != null)
            {
                if (cartCookie.HasKeys)
                {

                    foreach (string value in cartCookie.Values.AllKeys)
                    {
                        string json = value;
                        cartProduct cartProduct = JsonConvert.DeserializeObject<cartProduct>(json);

                        cartProducts.Add(cartProduct);

                        var p = db.Products.Find(cartProduct.productID);
                        products.Add(p);
                    }

                }

            }
            //products =  products.GroupBy(o => o.productID)
            //.Select(p => new Product{ price = p.Sum(i=>i.price), existingCount = p.Sum(i => i.existingCount) }).ToList();
            ViewBag.cartProduct = cartProducts;

            return View(products);

        }



        public ActionResult specialOffers(int? page)
        {

            ViewBag.productsWithHighSales = db.Products.Where(c => c.OrderProducts.Sum(w => w.count) > 0).OrderBy(w => w.OrderProducts.Sum(e => e.count)).Take(20).ToList();




            List<Product> products = new List<Product>();
            var offPercent = db.Offers.OrderByDescending(o => o.offPercent).ToList();
            offPercent = offPercent.Distinct().ToList();

            foreach (var item in offPercent)
            {
                products.Add(item.Product);
            }

            var price = db.Offers.OrderByDescending(o => o.price).ToList();
            price = price.Distinct().ToList();
            foreach (var item in price)
            {
                products.Add(item.Product);
            }

            var count = db.Products.Where(c => c.OrderProducts.Sum(w => w.count) > 0).OrderBy(w => w.OrderProducts.Sum(e => e.count));
            products.AddRange(count);


            products = products.Distinct().ToList();
            List<int?> cid = products.Select(p => p.categoryID).ToList();


            ViewBag.category = db.ProductCategories.Where(pc => cid.Contains(pc.categoryID)).ToList();


            return View(products.ToList().ToPagedList(page ?? 1, 20));

        }





        [HttpPost]
        public ActionResult userTracking(int productID, int categoryID)
        {
            User user = (User)Session["UU!#user"];

            if (user != null)
            {
                UserTracking usertracking = new UserTracking
                {
                    userID = user.userID,
                    productID = productID
                };

                db.UserTrackings.Add(usertracking);
                db.SaveChanges();

            }

            var product = db.Products.Find(productID);
            userTrackingCategrory userTrackingCategrory = new userTrackingCategrory
            {
                categoryID = categoryID,
                productPrice = product.price
            };


            HttpCookie cookie = Request.Cookies["useTtrackingProductCategory"];

            if (cookie != null)
            {

                List<string> usertracking = new List<string>();


                if (cookie.HasKeys)
                {
                    foreach (string value in cookie.Values.AllKeys)
                    {
                        usertracking.Add(Convert.ToString(value));

                    }

                }


                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
                //////////////////////////////////////////////////////////////////

                HttpCookie compareCookie = new HttpCookie("useTtrackingProductCategory");
                DateTime now = DateTime.Now;
                compareCookie.Expires = now.AddMonths(6);


                foreach (var item in usertracking)
                {
                    compareCookie.Values.Add(Convert.ToString(item), Convert.ToString(item));

                }

                compareCookie.Values.Add(new JavaScriptSerializer().Serialize(userTrackingCategrory), Convert.ToString(productID));
                Response.Cookies.Add(compareCookie);

                return Json(new { success = true, data = "" });


            }
            else
            {



                HttpCookie cartCookie = new HttpCookie("useTtrackingProductCategory");
                DateTime now = DateTime.Now;
                cartCookie.Expires = now.AddMonths(6);

                cartCookie.Values.Add(new JavaScriptSerializer().Serialize(userTrackingCategrory), Convert.ToString(productID));
                Response.Cookies.Add(cartCookie);

                return Json(new { success = true, data = "" });

            }
        }





        [HttpPost]
        public ActionResult brandsProducts(int brandID)
        {

            var productsOfBrands = db.Products.Where(p => p.brandID == brandID).ToList();
            return View(productsOfBrands);

        }



        [HttpPost]
        public ActionResult sortProductsCategory(string sortType, int categoryID, List<int> productID, bool justAvailable)
        {
            if (categoryID > 0)
            {
                var products = sortProducts(sortType, categoryID, justAvailable);
                return PartialView("~/Views/Partials/_ProductPaginationList.cshtml", products.ToList().ToPagedList(1, 30));

            }
            else
            {

                var products = sortProducts(sortType, db.Products.Where(p => productID.Contains(p.productID)).ToList(), justAvailable);

                return PartialView("~/Views/Partials/_ProductPaginationList.cshtml", products.ToList().ToPagedList(1, 30));
            }
        }






        private List<Product> sortProducts(string sortType, int categoryID, bool justAvailable)
        {
            List<Product> products = new List<Product>();
            products = db.Products.Where(p => p.categoryID == categoryID).ToList();
            switch (sortType)
            {
                case "price":
                    products = products.Where(a => a.existingCount > 0).OrderBy(p => p.price).ToList();
                    break;
                case "pricedesc":
                    products = products.Where(a => a.existingCount > 0).OrderByDescending(p => p.price).ToList();
                    break;
                case "date":
                    products = products.OrderByDescending(p => p.productID).ToList();
                    break;
                case "offer":
                    List<productPrice> productPrices2 = new List<productPrice>();
                    foreach (var item in products)
                    {
                        DateTime now = Convert.ToDateTime(DateTime.Now.ToString());
                        var offers = item.Offers.Where(o => o.startDate < now && o.endDate > now).ToList();
                        decimal offPrice = 0;
                        decimal offPercent = 0;
                        foreach (var item2 in offers)
                        {
                            offPrice += (int)item2.price;
                            offPercent += (int)item2.offPercent;
                        }
                        decimal productPrice = item.price;
                        decimal finalPrice = productPrice;
                        if (offPercent > 0)
                        {
                            finalPrice = finalPrice - (productPrice * (offPercent / 100));
                        }
                        if (offPrice > 0)
                        {
                            finalPrice = finalPrice - offPrice;
                        }
                        productPrice p = new productPrice()
                        {
                            Product = item,
                            price = productPrice - finalPrice
                        };
                        productPrices2.Add(p);
                    }
                    productPrices2 = productPrices2.OrderByDescending(p => p.price).ToList();
                    products = productPrices2.Select(p => p.Product).ToList();
                    //products = db.Products.Where(p => (p.categoryID == categoryID)).OrderByDescending(p => p.price).ToList();
                    break;
                case "sale":
                    products = products.OrderByDescending(w => w.OrderProducts.Sum(e => e.count)).ToList();
                    break;
                case "visit":
                    products = products.OrderByDescending(p => p.visit).ToList();
                    break;
                case "rate":
                    products = products.OrderByDescending(p => p.Comments.Average(c => c.rate)).ToList();
                    break;
                default:
                    products = products.OrderByDescending(p => p.productID).ToList();
                    break;
            }

            if (justAvailable)
            {
                products = products.Where(p => p.existingCount > 0).ToList();
            }


            return products;
        }

        private List<Product> sortProducts(string sortType, List<Product> products, bool justAvailable)
        {
            switch (sortType)
            {
                case "price":
                    products = products.Where(a => a.existingCount > 0).OrderBy(p => p.price).ToList();
                    break;
                case "pricedesc":
                    products = products.Where(a => a.existingCount > 0).OrderByDescending(p => p.price).ToList();
                    break;

                case "date":
                    products = products.OrderByDescending(p => p.productID).ToList();
                    break;
                case "offer":
                    List<productPrice> productPrices2 = new List<productPrice>();
                    foreach (var item in products)
                    {
                        DateTime now = Convert.ToDateTime(DateTime.Now.ToString());
                        var offers = item.Offers.Where(o => o.startDate < now && o.endDate > now).ToList();
                        decimal offPrice = 0;
                        decimal offPercent = 0;
                        foreach (var item2 in offers)
                        {
                            offPrice += (int)item2.price;
                            offPercent += (int)item2.offPercent;
                        }
                        decimal productPrice = item.price;
                        decimal finalPrice = productPrice;
                        if (offPercent > 0)
                        {
                            finalPrice = finalPrice - (productPrice * (offPercent / 100));
                        }
                        if (offPrice > 0)
                        {
                            finalPrice = finalPrice - offPrice;
                        }
                        productPrice p = new productPrice()
                        {
                            Product = item,
                            price = productPrice - finalPrice
                        };
                        productPrices2.Add(p);
                    }
                    productPrices2 = productPrices2.OrderByDescending(p => p.price).ToList();
                    products = productPrices2.Select(p => p.Product).ToList();
                    break;
                case "sale":
                    products = products.OrderByDescending(w => w.OrderProducts.Sum(e => e.count)).ToList();
                    break;
                case "visit":
                    products = products.OrderByDescending(p => p.visit).ToList();
                    break;
                case "rate":
                    var pid = products.Select(p => p.productID);
                    products = db.Products.OrderByDescending(p => p.Comments.Average(c => c.rate)).ToList();
                    products = products.Where(p => pid.Contains(p.productID)).ToList();
                    break;
                default:
                    products = products.OrderByDescending(p => p.productID).ToList();
                    break;
            }

            if (justAvailable)
            {
                products = products.Where(p => p.existingCount > 0).ToList();
            }

            return products.Distinct().ToList();
        }

        public PartialViewResult getAttributes(int id)
        {
            var attrs = db.ProductCategories.Find(id).ProductAttributeTemplates;
            return PartialView("~/Views/Partials/_ProductAttributesForm.cshtml", attrs);
        }



        public ActionResult CreateColorFormElement()
        {
            ProductColor productColor = new ProductColor() { exist = false, colorCode = "#ff0000" };
            return PartialView("~/Views/product/_CreateColorFormElement.cshtml", productColor);

        }



        public class productPrice
        {
            public Product Product { get; set; }
            public decimal price { get; set; }
        }

    }
}
