using Newtonsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Shop.Models;
using Newtonsoft.Json;
using PagedList;

namespace Shop.Controllers
{
    public class HomeController : Controller
    {
        Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        public ActionResult Index()
        {
            List<Banner> banners = db.Banners.OrderBy(b => b.bannerID).ToList();
            ViewBag.sidebanner = (List<Banner>)banners.Take(2).ToList();
            banners.RemoveRange(0, 2);
            ViewBag.banner = (List<Banner>)banners.Take(3).ToList();
            banners.RemoveRange(0, 3);
            ViewBag.largeBanner = (List<Banner>)banners.Take(1).ToList();
            var shop = db.Contents.Select(c => new { c.title, c.adress, c.phones, c.email }).First();
            //shopInfo shopInfo = new shopInfo { title = shop.title, address = shop.adress, phones = shop.phones, email = shop.email };
            //ViewBag.shopInfo = shopInfo;

            ViewBag.shopCarousel = db.ShopCarousels.Where(sc => sc.show == true).ToList();


            ViewBag.socials = db.Socials.ToList();


            ViewBag.brands = db.Brands.Random(12).ToList();



            //ViewBag.productsWithHighSales = db.Products.OrderBy(c =>c.OrderProducts.Sum(w => w.count)).Take(20).ToList();
            ViewBag.productsWithHighSales = db.Products.Where(c => c.OrderProducts.Sum(w => w.count) > 0).OrderBy(w => w.OrderProducts.Sum(e => e.count)).Take(20).ToList();


            ViewBag.vitrinProducts = db.Products.Where(c => c.vitrin == true).ToList();



            var offers = db.Offers.Where(o => (o.offPercent.HasValue || o.price.HasValue) && o.Product.existingCount > 0 && o.startDate < DateTime.Now && o.endDate > DateTime.Now).OrderByDescending(o => o.offPercent).Take(8);

            if (offers.Count() > 0)
            {
                ViewBag.productHaveOffer = offers;
            }

            var newProducts = db.Products.OrderByDescending(p => p.productID).Take(20).ToList();
            ViewBag.newProducts = newProducts;

            List<int?> cid = newProducts.Select(p => p.categoryID).Distinct().ToList();
            var newProductCategories = db.ProductCategories.Where(c => cid.Contains(c.categoryID)).Distinct().ToList();
            ViewBag.newProductCategories = newProductCategories;




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


            return View(db.ProductCategories.ToList());

        }

        public ActionResult About()
        {
            return View(db.Contents.FirstOrDefault());
        }
        public ActionResult Privacy()
        {
            return View(db.Contents.FirstOrDefault());
        }

        public ActionResult Contact()
        {
            return View(db.Contents.FirstOrDefault());
        }
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Faqs()
        {

            return View(db.Faqs.ToList());
        }











        public ActionResult CategoryProducts(int categoryID)
        {

            var products = db.Products.Where(c => c.OrderProducts.Sum(w => w.count) > 0).OrderBy(w => w.OrderProducts.Sum(e => e.count)).ToList();
            ViewBag.productsCategoryWithHighSales = products.Where(p => p.categoryID == categoryID).ToList();


            var offer = db.Offers.Where(o => (o.Product.categoryID == categoryID && (o.price != null || o.offPercent != null))).ToList();



            List<Product> specialProducts = new List<Product>();

            foreach (var item in offer)
            {
                specialProducts.Add(db.Products.Find(item.productID));
            }
            specialProducts = specialProducts.Distinct().ToList();
            ViewBag.specialProducts = specialProducts;




            return PartialView("", null);
        }



        public ActionResult Search(string search, int? page)
        {
            ViewBag.searchValue = search;
            //ViewBag.category = db.ProductCategories.ToList();



            ViewBag.productsWithHighSales = db.Products.Where(c => c.OrderProducts.Sum(w => w.count) > 0).OrderBy(w => w.OrderProducts.Sum(e => e.count)).Take(20).ToList();

            List<Product> OfferProducts = new List<Product>();
            var offers = db.Offers.Where(o => (o.offPercent != null || o.price != null)).OrderByDescending(o => o.offPercent).ToList();
            offers = offers.Distinct().ToList();
            foreach (var item in offers)
            {
                OfferProducts.Add(item.Product);
            }
            ViewBag.productHaveOffer = OfferProducts.Distinct().ToList();

            ///////////////////////////////////////////////////////////////////////// brand 
            List<Product> products = new List<Product>();
            var brands = db.Brands.Where(b => b.brandName.Contains(search) || b.enBrandName.Contains(search)).ToList();
            foreach (var item in brands)
            {
                var product = db.Products.Where(p => p.brandID == item.brandID).ToList();
                products.AddRange(product);
            }
            if (products.Any())
            {
                ViewBag.minPrice = products.Min(p => p.price).ToString();
                ViewBag.maxPrice = products.Max(p => p.price).ToString();

                List<int?> cid = products.Select(p => p.categoryID).ToList();
                ViewBag.category = db.ProductCategories.Where(pc => cid.Contains(pc.categoryID)).ToList();

                List<int?> bid = products.Select(p => p.brandID).ToList();
                ViewBag.brands = db.Brands.Where(pc => bid.Contains(pc.brandID)).ToList();



                List<ProductColor> productColors = new List<ProductColor>();
                foreach (var item in products)
                {
                    var ProductColorAssinments = item.ProductColorAssinments.ToList();
                    if (ProductColorAssinments != null)
                    {
                        var pcID = ProductColorAssinments.Select(p => p.productColorID).ToList();
                        var productColor = db.ProductColors.Where(p => pcID.Contains(p.productColorID));
                        productColors.AddRange(productColor);
                    }

                }
                productColors = productColors.Distinct().ToList();
                ViewBag.colors = productColors;



                return View(products.ToList().ToPagedList(page ?? 1, 20));
            }
            //////////////////////////////////////////////////////////////////// category
            var categories = db.ProductCategories.Where(pc => pc.categoryName.Contains(search)).ToList();
            foreach (var item in categories)
            {
                var product = db.Products.Where(p => p.categoryID == item.categoryID).ToList();
                products.AddRange(product);
            }
            if (products.Any())
            {
                ViewBag.minPrice = products.Min(p => p.price).ToString();
                ViewBag.maxPrice = products.Max(p => p.price).ToString();

                List<int?> cid = products.Select(p => p.categoryID).ToList();
                ViewBag.category = db.ProductCategories.Where(pc => cid.Contains(pc.categoryID)).ToList();

                List<int?> bid = products.Select(p => p.brandID).ToList();
                ViewBag.brands = db.Brands.Where(pc => bid.Contains(pc.brandID)).ToList();

                List<ProductColor> productColors = new List<ProductColor>();
                foreach (var item in products)
                {
                    var ProductColorAssinments = item.ProductColorAssinments.ToList();
                    if (ProductColorAssinments != null)
                    {
                        var pcID = ProductColorAssinments.Select(p => p.productColorID).ToList();
                        var productColor = db.ProductColors.Where(p => pcID.Contains(p.productColorID));

                        productColors.AddRange(productColor);
                    }

                }
                productColors = productColors.Distinct().ToList();
                ViewBag.colors = productColors;


                return View(products.ToList().ToPagedList(page ?? 1, 20));
            }
            //////////////////////////////////////////////////////////////////  prodct

            products = db.Products.Where(p => p.name.Contains(search)).ToList();

            if (products.Any())
            {
                ViewBag.minPrice = products.Min(p => p.price).ToString();
                ViewBag.maxPrice = products.Max(p => p.price).ToString();

                List<int?> cid = products.Select(p => p.categoryID).ToList();
                ViewBag.category = db.ProductCategories.Where(pc => cid.Contains(pc.categoryID)).ToList();

                List<int?> bid = products.Select(p => p.brandID).ToList();
                ViewBag.brands = db.Brands.Where(pc => bid.Contains(pc.brandID)).ToList();

                List<ProductColor> productColors = new List<ProductColor>();
                foreach (var item in products)
                {
                    var ProductColorAssinments = item.ProductColorAssinments.ToList();

                    if (ProductColorAssinments != null)
                    {
                        var pcID = ProductColorAssinments.Select(p => p.productColorID).ToList();
                        var productColor = db.ProductColors.Where(p => pcID.Contains(p.productColorID));

                        productColors.AddRange(productColor);
                    }
                }
                productColors = productColors.Distinct().ToList();
                ViewBag.colors = productColors;


                return View(products.ToList().ToPagedList(page ?? 1, 20));
            }
            //////////////////////////////////////////////////////////////////  keyword

            var keywords = db.ProductKeywords.Where(pk => pk.text.Contains(search)).ToList();
            foreach (var item in keywords)
            {

                var product = db.Products.Where(p => p.productID == item.productID).ToList();
                products.AddRange(product);
            }
            if (products.Any())
            {
                ViewBag.minPrice = products.Min(p => p.price).ToString();
                ViewBag.maxPrice = products.Max(p => p.price).ToString();

                List<int?> cid = products.Select(p => p.categoryID).ToList();
                ViewBag.category = db.ProductCategories.Where(pc => cid.Contains(pc.categoryID)).ToList();

                List<int?> bid = products.Select(p => p.brandID).ToList();
                ViewBag.brands = db.Brands.Where(pc => bid.Contains(pc.brandID)).ToList();

                List<ProductColor> productColors = new List<ProductColor>();
                foreach (var item in products)
                {
                    var ProductColorAssinments = item.ProductColorAssinments.ToList();

                    if (ProductColorAssinments != null)
                    {

                        var pcID = ProductColorAssinments.Select(p => p.productColorID).ToList();
                        var productColor = db.ProductColors.Where(p => pcID.Contains(p.productColorID));


                        productColors.AddRange(productColor);
                    }

                }
                productColors = productColors.Distinct().ToList();
                ViewBag.colors = productColors;


                return View(products.ToList().ToPagedList(page ?? 1, 20));
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            //List<int?> cid = products.Select(p => p.categoryID).ToList();
            //ViewBag.category = db.ProductCategories.Where(pc => cid.Contains(pc.categoryID)).ToList();


            ViewBag.category = null;

            ViewBag.brands = null;

            ViewBag.minPrice = 0;
            ViewBag.maxPrice = 1000000000;

            ViewBag.colors = null;

            return View();

        }

        [HttpPost]
        public ActionResult searchAutoComplete(string searchValue)
        {
            if (searchValue.Length < 2)
            {
                return Json(new { success = false, data = "length is less than two character" });
            }

            search search = new search();

            var searchCategory = db.ProductCategories.Where(pc => pc.categoryName.Contains(searchValue) && pc.parentCategory == null).Take(2);
            List<details> sc = searchCategory.Select(a => new details { name = a.categoryName, itemID = a.categoryID }).ToList();

            //var searchSubCategory = db.ProductCategories.Where(pc => pc.categoryName.Contains(searchValue) && pc.parentCategory != null).Take(2);
            //List<string> ssc = searchSubCategory.Select(a => a.categoryName).ToList();

            //var searchKeywords = db.ProductKeywords.Where(pk => pk.text.Contains(searchValue)).Take(3).ToList();
            //List<string> sk = searchKeywords.Select(a => a.text).ToList();

            var searchBrandFA = db.Brands.Where(sb => sb.brandName.Contains(searchValue)).Take(1).ToList();
            List<details> sbfa = searchBrandFA.Select(a => new details { name = a.brandName, itemID = a.brandID }).ToList();

            //var searchBrandEN = db.Brands.Where(sb => sb.enBrandName.Contains(searchValue)).Take(1).ToList();
            //List<string> sben = searchBrandEN.Select(a => a.enBrandName).ToList();

            var searchProduct = db.Products.Where(pk => pk.name.Contains(searchValue)).Take(3).ToList();
            List<details> sp = searchProduct.Select(a => new details { name = a.name, itemID = a.productID }).ToList();




            search.category = sc;
            search.brand = sbfa;
            search.product = sp;




            string json = JsonConvert.SerializeObject(search);

            //string Category = JsonConvert.SerializeObject( searchCategory.Select(c => c.categoryName).ToList());
            //string SubCategory = JsonConvert.SerializeObject( searchSubCategory.Select(c => c.categoryName).ToList());
            //string Keywords = JsonConvert.SerializeObject(searchKeywords.Select(c => c.text).ToList());



            return Json(new { success = true, values = json });

        }




        public ActionResult Compare()
        {

            List<int?> productid = new List<int?>();


            HttpCookie getcartCookie = Request.Cookies["uu!@#Compare"];



            if (getcartCookie != null)
            {
                if (getcartCookie.HasKeys)
                {
                    foreach (string value in getcartCookie.Values.AllKeys)
                    {
                        int pid = Convert.ToInt32(value);

                        if (pid > 0)
                        {
                            productid.Add(pid);
                        }

                    }

                }
            }


            var products = db.Products.Where(p => productid.Contains(p.productID)).ToList();
            if (productid != null && productid.Any())
            {
                int? productCategory = products.FirstOrDefault().categoryID;
                ViewBag.AttributeGroup = db.ProductAttributeTemplateGroups.Where(p => p.ProductCategory.categoryID == productCategory).ToList();

                //var ProductAttributeTemplates = db.ProductAttributes.Where(p => productid.Contains(p.productID)).Select(p=>p.ProductAttributeTemplate).ToList();
                //var ProductAttributeTemplates = db.ProductAttributeTemplates.AsNoTracking().Where(pat => pat.categoryID == productCategory && ProductAttribute.Contains(pat.)).ToList();
                //ViewBag.template = ProductAttributeTemplates;
            }







            //if (getcartCookie != null)
            //{
            //    var value = getcartCookie.Values;
            //    var cookieValues = value.AllKeys;
            //    List<ProductAttribute> attributes = new List<ProductAttribute>();
            //    foreach (var item in cookieValues)
            //    {
            //      var item2 = Convert.ToInt32(item);
            //        var categoryid = db.Products.Find(item2);
            //        if (categoryid!=null)
            //        {

            //        var template = db.ProductAttributeTemplates.Where(z=>z.categoryID==categoryid.categoryID).ToList();

            //        }
            //    }
            //}
            //var ProductAttributes = db.ProductAttributes.Where(pa => productid.Contains(pa.productID)).ToList();
            //foreach (var item in ProductAttributes)
            //{
            //    productAttributeTemplates.AddRange(db.ProductAttributeTemplates.Where(pat => pat.productAtrributeTemplateID == item.productAtrributeTemplateID).ToList());
            //}


            //ViewBag.ProductAttributesTemplate = productAttributeTemplates.Distinct().ToList();






            return View(products);
        }

        [HttpPost]
        public ActionResult addToCompare(int productID)
        {
            HttpCookie cookie = Request.Cookies["uu!@#Compare"];

            if (cookie != null)
            {
                if (cookie.Values.Count < 4)
                {
                    List<int> productid = new List<int>();
                    if (cookie.HasKeys)
                    {
                        foreach (string value in cookie.Values.AllKeys)
                        {
                            int pid = Convert.ToInt32(value);
                            if (pid > 0)
                            {
                                productid.Add(pid);
                            }

                        }

                        var newProduct = db.Products.Find(productID);
                        int? newCategoryID = newProduct.categoryID;

                        var oldProduct = db.Products.Find(productid.FirstOrDefault());
                        int? oldCategoryID = oldProduct.categoryID;

                        if (newCategoryID == oldCategoryID)
                        {
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(cookie);


                            ///////////////////////////////////////////////////////////////////////////

                            HttpCookie compareCookie = new HttpCookie("uu!@#Compare");
                            DateTime now = DateTime.Now;
                            compareCookie.Expires = now.AddHours(6);



                            foreach (var item in productid)
                            {
                                compareCookie.Values.Add(Convert.ToString(item), Convert.ToString(item));

                            }

                            compareCookie.Values.Add(Convert.ToString(productID), Convert.ToString(productID));
                            Response.Cookies.Add(compareCookie);


                            return Json(new { success = true, data = "با موفقیت به مقایسه افزوده شد" });
                        }
                        else
                        {
                            cookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(cookie);


                            ///////////////////////////////////////////////////////////////////////////

                            HttpCookie compareCookie = new HttpCookie("uu!@#Compare");
                            DateTime now = DateTime.Now;
                            compareCookie.Expires = now.AddHours(6);


                            compareCookie.Values.Add(Convert.ToString(productID), Convert.ToString(productID));
                            Response.Cookies.Add(compareCookie);


                            return Json(new { success = true, data = "با موفقیت به مقایسه افزوده شد" });

                        }

                    }
                    else
                    {
                        return Json(new { success = false, data = "" });

                    }
                }
                else
                {
                    return Json(new { success = false, data = "بیشتر از 4 مقایسه امکان پذیر نیست" });
                }
            }
            else
            {
                HttpCookie cartCookie = new HttpCookie("uu!@#Compare");
                DateTime now = DateTime.Now;
                cartCookie.Expires = now.AddHours(6);

                cartCookie.Values.Add(Convert.ToString(productID), Convert.ToString(productID));
                Response.Cookies.Add(cartCookie);

                return Json(new { success = true, data = "با موفقیت به مقایسه افزوده شد" });

            }


        }



        [HttpPost]
        public ActionResult removeOfCompare(int productID)
        {

            var cookie = Request.Cookies["uu!@#Compare"];

            List<int> productid = new List<int>();
            if (cookie.HasKeys)
            {
                foreach (string value in cookie.Values.AllKeys)
                {
                    int pid = Convert.ToInt32(value);

                    if (pid > 0 && pid != productID)
                    {
                        productid.Add(pid);
                    }

                }

            }


            cookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(cookie);
            //////////////////////////////////////////////////////////////////

            HttpCookie compareCookie = new HttpCookie("uu!@#Compare");
            DateTime now = DateTime.Now;
            compareCookie.Expires = now.AddHours(6);


            foreach (var item in productid)
            {
                compareCookie.Values.Add(Convert.ToString(item), Convert.ToString(item));

            }


            if (productid.Count > 0)
            {
                Response.Cookies.Add(compareCookie);
            }
            //compareCookie.Values.Add(Convert.ToString(productID), Convert.ToString(productID));



            return Json(new { success = true, data = productID, count = cookie.Values.Count });

        }


    }
}