using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using Shop.Models;
namespace Shop.Controllers
{
    public class ProductCategoryController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: ProductCategory
        public ActionResult Index()
        {
            var productCategories = db.ProductCategories.Include(p => p.ProductCategory1);
            return View(productCategories.ToList());
        }
        [RequsetLogin(2)]
        // GET: ProductCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        [RequsetLogin(2)]
        // GET: ProductCategory/Create
        public ActionResult Create()
        {
            ViewBag.parentCategory = new SelectList(db.ProductCategories, "categoryID", "categoryName");
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "categoryID,categoryName,parentCategory")] ProductCategory productCategory, List<string> name, List<byte> attributeValueTypeID, List<int> unitID, List<bool> searchable , List<int?> ProductAttributeTemplateGroupID)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.ProductCategories.Add(productCategory);
        //        db.SaveChanges();

        //        if ((name != null) && (name.Any()))
        //        {
        //            int i = 0;
        //            foreach (var item in name)
        //            {
        //                ProductAttributeTemplate productAttributeTemplate = new ProductAttributeTemplate
        //                {
        //                    name = item,
        //                    attributeValueTypeID = attributeValueTypeID[i],
        //                    unitID = unitID[i],
        //                    ProductAttributeTemplateGroupID = ProductAttributeTemplateGroupID[i],
        //                    searchable = searchable[i],
        //                    categoryID = productCategory.categoryID
        //                };
        //                i++;
        //                db.ProductAttributeTemplates.Add(productAttributeTemplate);
        //            }
        //            db.SaveChanges();
        //        }
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.parentCategory = new SelectList(db.ProductCategories, "categoryID", "categoryName", productCategory.parentCategory);
        //    return View(productCategory);
        //}



        [RequsetLogin(2)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public int ccc([Bind(Include = "categoryID,categoryName,parentCategory,logo")] ProductCategory productCategory, string googletitle, string googledescription, HttpPostedFileBase file)
        {
            var hhh = Request.Headers;
            if (ModelState.IsValid && productCategory.categoryName != null)
            {



                Seo seo = new Seo();
                seo.categoryID = productCategory.categoryID;
                seo.title = googletitle;
                seo.description = googledescription;
                db.Seos.Add(seo);



                db.ProductCategories.Add(productCategory);
                db.SaveChanges();



                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/Category/Menu/" + productCategory.categoryID + ".jpg"));
                }
                return productCategory.categoryID;
            }

            return 0;
        }
        [RequsetLogin(2)]
        [HttpPost]
        public ActionResult bbb(string productAttribiuteTemplateGroupName, int productCategoryID)
        {

            ProductAttributeTemplateGroup productAttributeTemplateGroup = new ProductAttributeTemplateGroup { name = productAttribiuteTemplateGroupName, categoryID = productCategoryID };

            db.ProductAttributeTemplateGroups.Add(productAttributeTemplateGroup);
            db.SaveChanges();

            return PartialView("~/Views/ProductCategory/_CreateElement.cshtml", productAttributeTemplateGroup);

        }
        [RequsetLogin(2)]
        [HttpPost]
        public ActionResult aaa(List<ProductAttributeTemplate> productAttributeTemplates)
        {
            db.ProductAttributeTemplates.AddRange(productAttributeTemplates);
            db.SaveChanges();

            return Json(Url.Action("Index", "ProductCategory"));
        }






        [RequsetLogin(2)]
        // GET: ProductCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }

            ProductAttributeTemplate productAttributeTemplate = db.ProductAttributeTemplates.FirstOrDefault(pat => pat.categoryID == id);
            ViewBag.parentCategory = new SelectList(db.ProductCategories, "categoryID", "categoryName", productCategory.parentCategory);


            ViewBag.attribteValueTypes = db.AttribteValueTypes.ToList();
            ViewBag.units = db.Units.ToList();
            ViewBag.productAttributeTemplateGroupID = db.ProductAttributeTemplateGroups.Where(p => p.categoryID == id).ToList();
            ViewBag.productAttributeTemplates = db.ProductAttributeTemplates.Where(pta => pta.categoryID == id).ToList();


            return View(productCategory);
        }

        [RequsetLogin(2)]
        [HttpPost]
        public ActionResult Edit(ProductCategory productCategory, List<ProductAttributeTemplateGroup> productAttribiuteTemplateGroups, List<ProductAttributeTemplate> productAttributeTemplates, string googletitle, string googledescription, string logo)
        {



            db.Seos.RemoveRange(db.Seos.Where(s => s.categoryID == productCategory.categoryID));

            productCategory.Seos.Add(new Seo()
            {
                categoryID = productCategory.categoryID,
                title = googletitle,
                description = googledescription
            });


            productCategory.logo = logo;


            db.Entry(productCategory).State = EntityState.Modified;
            db.SaveChanges();


            if (productAttribiuteTemplateGroups != null)
            {
                foreach (var item in productAttribiuteTemplateGroups)
                {

                    if (item.ProductAttributeTemplateGroupID == 0)
                    {
                        item.categoryID = productCategory.categoryID;
                        db.ProductAttributeTemplateGroups.Add(item);
                        db.SaveChanges();

                    }
                    else
                    {
                        item.categoryID = productCategory.categoryID;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }


                }
            }

            if (productAttributeTemplates != null)
            {

                foreach (var item in productAttributeTemplates)
                {
                    if (item.productAtrributeTemplateID == 0)
                    {
                        item.categoryID = productCategory.categoryID;

                        //item.ProductAttributeTemplateGroupID

                        db.ProductAttributeTemplates.Add(item);
                        db.SaveChanges();

                    }
                    else
                    {
                        item.categoryID = productCategory.categoryID;
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                    }

                }

            }

            return Json(Url.Action("Index", "ProductCategory"));

        }

        [RequsetLogin(2)]
        [HttpPost]
        public ActionResult deleteProductAttributeTemplates(int ProductAttributeTemplateID)
        {

            var ProductAttributeTemplate = db.ProductAttributeTemplates.Find(ProductAttributeTemplateID);

            db.ProductAttributeTemplates.Remove(ProductAttributeTemplate);
            db.SaveChanges();

            return Json(new { success = true });
        }
        [RequsetLogin(2)]
        [HttpPost]
        public ActionResult deleteProductAttributeTemplatesGroups(int ProductAttributeTemplateGroupID)
        {


            var ProductAttributeTemplate = db.ProductAttributeTemplates.Where(p => p.ProductAttributeTemplateGroupID == ProductAttributeTemplateGroupID);
            db.ProductAttributeTemplates.RemoveRange(ProductAttributeTemplate);
            db.SaveChanges();



            var ProductAttributeTemplateGroup = db.ProductAttributeTemplateGroups.Find(ProductAttributeTemplateGroupID);

            db.ProductAttributeTemplateGroups.Remove(ProductAttributeTemplateGroup);
            db.SaveChanges();

            return Json(new { success = true });
        }




























        //[RequsetLogin(2)]
        //// POST: ProductCategory/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "categoryID,categoryName,parentCategory")] ProductCategory productCategory, List<long> productAtrributeTemplateID,  List<string> name, List<byte> attributeValueTypeID, List<int> unitID, List<bool> searchable, List<int?> ProductAttributeTemplateGroupID)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(productCategory).State = EntityState.Modified;
        //        db.SaveChanges();



        //        var pta = db.ProductAttributeTemplates.Where(p => productAtrributeTemplateID.Contains(p.productAtrributeTemplateID));

        //        int i = 0;
        //        foreach (var item in pta)
        //        {
        //            item.name = name[i];
        //            if (attributeValueTypeID[i] > 0)
        //            {
        //                item.attributeValueTypeID = attributeValueTypeID[i];
        //            }
        //            else
        //            {
        //                item.attributeValueTypeID = null;
        //            }


        //            if (unitID[i] > 0)
        //            {
        //                item.unitID = unitID[i];
        //            }
        //            else
        //            {
        //                item.unitID = null;
        //            }

        //            if (ProductAttributeTemplateGroupID[i] > 0)
        //            {
        //                item.ProductAttributeTemplateGroupID = ProductAttributeTemplateGroupID[i];
        //            }
        //            else
        //            {
        //                item.ProductAttributeTemplateGroupID = null;
        //            }

        //            item.searchable = searchable[i];

        //            i++;

        //            db.Entry(item).State = EntityState.Modified;
        //        }



        //        db.SaveChanges();




        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.parentCategory = new SelectList(db.ProductCategories, "categoryID", "categoryName", productCategory.parentCategory);
        //    return View(productCategory);
        //}

        [RequsetLogin(2)]
        // GET: ProductCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategories.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }
        [RequsetLogin(2)]
        // POST: ProductCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategory productCategory = db.ProductCategories.Find(id);
            db.ProductCategories.Remove(productCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [RequsetLogin(2)]
        public ActionResult CreateAttribueFormElement()
        {
            //ProductAttributeTemplate productAttributeTemplate = new ProductAttributeTemplate() { searchable =false };
            //ViewBag.unitID = new SelectList(db.Units, "unitID", "unitName");
            //ViewBag.attributeValueTypeID = new SelectList(db.AttribteValueTypes, "attributeValueTypeID", "valueType");
            //ViewBag.ProductAttributeTemplateGroupID =  new SelectList(db.ProductAttributeTemplateGroups, "productAttributeTemplateGroupID", "name");
            return PartialView("~/Views/ProductCategory/_CreateElement.cshtml");

        }
        [RequsetLogin(2)]
        public ActionResult CreateAttribueTemplateFormElement()
        {
            ProductAttributeTemplate productAttributeTemplate = new ProductAttributeTemplate() { searchable = false };
            ViewBag.unitID = new SelectList(db.Units, "unitID", "unitName");
            ViewBag.attributeValueTypeID = new SelectList(db.AttribteValueTypes, "attributeValueTypeID", "valueType");
            ViewBag.ProductAttributeTemplateGroupID = new SelectList(db.ProductAttributeTemplateGroups, "productAttributeTemplateGroupID", "name");
            return PartialView("~/Views/ProductCategory/_CreateElementAttribute.cshtml", productAttributeTemplate);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPatch]
        public ActionResult List(int? c, int? b, int? page, string test)
        {
            if (c.HasValue)
            {
                var category = db.ProductCategories.Find(c);
                ViewBag.category = category;
                return View(category.Products.OrderByDescending(w => w.productID).ToPagedList(page ?? 1, 24));
            }
            if (b.HasValue)
            {
                var brand = db.Brands.Find(b);
                ViewBag.brand = brand;
                return View(brand.Products.OrderByDescending(w => w.productID).ToPagedList(page ?? 1, 24));
            }
            return View(db.Products.OrderByDescending(w => w.productID).ToPagedList(page ?? 1, 24));

        }


        public ActionResult List(int? c, int? b, int? page)
        {
            ViewBag.categoryID = c;
            ViewBag.brandID = b;

            //List<Product> OfferProducts = new List<Product>();
            //var offers = db.Offers.Where(o => (o.offPercent != null || o.price != null)).OrderByDescending(o => o.offPercent).ToList();
            //offers = offers.Distinct().ToList();

            //foreach (var item in offers)
            //{
            //    OfferProducts.Add(item.Product);
            //}
            //ViewBag.productHaveOffer = OfferProducts;
            //ViewBag.productsWithHighSales = db.sp_product_orderProduct_join().Take(20).ToList();
            //if (brandID.HasValue)
            //{
            //    List<Product> products = new List<Product>();
            //    products = db.Products.Where(p => p.brandID == brandID).ToList();

            //    var product = db.Products.Where(p => p.brandID == brandID).First();
            //    ViewBag.brandCategory = db.ProductCategories.Where(pc => pc.categoryID == product.categoryID).ToList();

            //    return View(products);
            //}
            //else if(categoryID.HasValue)
            //{
            //    List<Product> products = new List<Product>();
            //    products = db.Products.Where(p => p.categoryID == categoryID).ToList();


            //    ViewBag.brandCategory = db.ProductCategories.Where(pc => pc.categoryID == categoryID).ToList();

            //    return View(products);
            //}

            //if(!c.HasValue)
            //{
            //    ViewBag.brands = db.Brands.ToList();
            //    ViewBag.products = db.Products.Take(30).ToList().ToPagedList(page??1,24);
            //    return View(db.ProductCategories.ToList());

            //}

            if (c.HasValue)
            {
                var products = db.Products.Where(p => p.categoryID == c || p.ProductCategory.parentCategory == c).ToList();
                ViewBag.products = products.ToPagedList(page ?? 1, 24);

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

                var brands = products.Select(p => p.brandID).Distinct();
                ViewBag.brands = db.Brands.Where(br => brands.Contains(br.brandID)).ToList();

                if (products.Any())
                {
                    var maxPrice = products.Max(max => max.price);
                    ViewBag.maxPrice = maxPrice;
                    var minPrice = products.Min(min => min.price);
                    ViewBag.minPrice = minPrice;
                }
                else
                {
                    ViewBag.maxPrice = 1000000000;
                    ViewBag.minPrice = 0;
                }

                var filterItems = db.ProductAttributeTemplates.Where(pta => pta.categoryID == c && pta.searchable == true).ToList();

                List<filter> filters = new List<filter>();

                foreach (var item in filterItems)
                {
                    //var subFilterItems = db.ProductAttributes.Where(pa => pa.productAtrributeTemplateID == item.productAtrributeTemplateID).ToList();

                    List<string> subFilterItems = db.ProductAttributes.Where(pa => pa.productAtrributeTemplateID == item.productAtrributeTemplateID).Select(p => p.value).Distinct().ToList();
                    //subFilterItems = subFilterItems.Select(p => new ProductAttribute { value=p.value }).ToList();
                    //subFilterItems = subFilterItems.Distinct().ToList();

                    List<ProductAttribute> productAttributes = new List<ProductAttribute>();

                    foreach (var item3 in subFilterItems)
                    {
                        ProductAttribute productAttribute = new ProductAttribute
                        {
                            value = item3
                        };
                        productAttributes.Add(productAttribute);
                    }

                    filter filter = new filter()
                    {
                        nameID = item.productAtrributeTemplateID,
                        name = item.name,
                        productAttribute = productAttributes
                    };
                    filters.Add(filter);
                }
                ViewBag.filterItems = filters;


                return View(db.ProductCategories.Where(pc => pc.parentCategory == c).ToList());

            }
            else if (b.HasValue)
            {
                var products = db.Products.Where(p => p.brandID == b).ToList();
                ViewBag.products = products.ToPagedList(page ?? 1, 24);


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


                ViewBag.brands = null;

                if (products.Any())
                {
                    var maxPrice = products.Max(max => max.price);
                    ViewBag.maxPrice = maxPrice;
                    var minPrice = products.Min(min => min.price);
                    ViewBag.minPrice = minPrice;
                }
                else
                {
                    ViewBag.maxPrice = 1000000000;
                    ViewBag.minPrice = 0;
                }


                var cid = products.Select(p => p.categoryID).ToList();


                var filterItems = db.ProductAttributeTemplates.Where(pta => cid.Contains(pta.categoryID) && pta.searchable == true).ToList();

                List<filter> filters = new List<filter>();

                foreach (var item in filterItems)
                {
                    //var subFilterItems = db.ProductAttributes.Where(pa => pa.productAtrributeTemplateID == item.productAtrributeTemplateID).ToList();
                    //subFilterItems = subFilterItems.Select(p => new ProductAttribute {productAtrributeTemplateID =  p.productAtrributeTemplateID , ProductAttributeTemplate =  p.ProductAttributeTemplate , value = p.value }).ToList();
                    //subFilterItems = subFilterItems.Distinct().ToList();

                    List<string> subFilterItems = db.ProductAttributes.Where(pa => pa.productAtrributeTemplateID == item.productAtrributeTemplateID).Select(p => p.value).Distinct().ToList();

                    List<ProductAttribute> productAttributes = new List<ProductAttribute>();

                    foreach (var item3 in subFilterItems)
                    {
                        ProductAttribute productAttribute = new ProductAttribute
                        {
                            value = item3
                        };
                        productAttributes.Add(productAttribute);
                    }


                    filter filter = new filter()
                    {
                        nameID = item.productAtrributeTemplateID,
                        name = item.name,
                        productAttribute = productAttributes
                    };
                    filters.Add(filter);
                }
                ViewBag.filterItems = filters;

                return View(db.ProductCategories.Where(pc => cid.Contains(pc.parentCategory)).ToList());
            }


            return View();


        }
        public ActionResult Category()
        {
            return View(db.ProductCategories.ToList());
        }


        [HttpPost]
        public ActionResult filterByDynamicProperty(List<int> productID, List<int?> productColorID, string filtersJson, int? maxPrice, int? minPrice, int? categoryID, List<int> brandID, bool justAvailable)
        {
            List<Product> products = new List<Product>();

            //var a = JsonConvert.DeserializeObject<string>(filtersJson);
            List<filter> filters = new List<filter>();

            if (filtersJson != null && filtersJson.Length > 2)
            {
                filters = JsonConvert.DeserializeObject<List<filter>>(filtersJson);
            }


            if (filters.Count > 0)
            {
                foreach (var item in filters)
                {
                    var productAttribute = item.productAttribute;
                    List<string> values = new List<string>();
                    foreach (var item2 in productAttribute)
                    {
                        values.Add(item2.value);
                    }
                    //List<string> values =  (List<string>)item.productAttribute.Select(p=>p.value);
                    var productAttributeID = db.ProductAttributes.Where(p => p.productAtrributeTemplateID == item.nameID && values.Contains(p.value));

                    List<ProductAttribute> pa = new List<ProductAttribute>();

                    foreach (var item3 in values)
                    {
                        pa.AddRange(db.ProductAttributes.Where(p => p.value == item3 && p.productAtrributeTemplateID == item.nameID).ToList());
                    }
                    pa = pa.Distinct().ToList();
                    var valueID = pa.Select(p => p.productAttributeID).ToList();
                    foreach (var item2 in productID)
                    {
                        var product = db.Products.Find(item2);
                        List<long> productAttributes = product.ProductAttributes.Select(p => p.productAttributeID).ToList();
                        valueID.Sort();
                        productAttributes.Sort();
                        bool flag = false;
                        foreach (var item3 in valueID)
                        {
                            if (productAttributes.Contains(item3))
                            {
                                flag = true;
                            }
                        }
                        if (flag)
                        {
                            products.Add(product);
                        }
                    }
                }
            }
            else
            {
                products = db.Products.Where(p => productID.Contains(p.productID)).ToList();
            }

            if (productColorID != null && productColorID.Count > 0)
            {
                List<Product> productFilterColor = new List<Product>();
                foreach (var item in products)
                {
                    var ProductColorAssinments = item.ProductColorAssinments.ToList();

                    if (ProductColorAssinments != null && ProductColorAssinments.Count > 0)
                    {
                        var pcID = ProductColorAssinments.Select(p => p.productColorID).ToList();
                        var productColor = db.ProductColors.Where(p => pcID.Contains(p.productColorID));
                        var colorID = productColor.Select(c => c.productColorID).ToList();
                        if (colorID != null && productColorID != null)
                        {
                            foreach (var item2 in productColorID)
                            {
                                if ((colorID.Contains((int)item2)))
                                {
                                    productFilterColor.Add(item);
                                }
                            }
                        }
                    }
                }
                products = productFilterColor;
            }


            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.price <= maxPrice).ToList();
            }
            if (minPrice.HasValue)
            {
                products = products.Where(p => p.price >= minPrice).ToList();
            }
            if (categoryID.HasValue)
            {
                products = products.Where(p => p.categoryID == categoryID).ToList();
            }
            if (brandID != null && brandID.Count > 0)
            {
                products = products.Where(p => brandID.Contains((int)p.brandID)).ToList();
            }
            if (justAvailable)
            {
                products = products.Where(p => p.existingCount > 0).ToList();
            }

            products = products.Distinct().ToList();

            var sortedProductID = products.Select(p => p.productID).ToList();


            if (products.Any())
            {
                ViewBag.sortedProductID = sortedProductID;
                return PartialView("~/Views/Partials/_ProductPaginationList.cshtml", products.ToList().ToPagedList(1, 20));
            }
            else
            {
                return Json(new { success = false, data = "موردی یافت نشد" });
            }

        }

    }
}
