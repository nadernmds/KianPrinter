using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Shop.Models;
namespace Shop.Controllers
{
    [RequsetLogin]
    public class PanelController : Controller
    {
        // GET: Panel
        Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        public ActionResult Index()
        {

            /////////////////////////////////////////////////////////////////////////////////////////////////////
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
            products = products.Distinct().Take(10).ToList();
            ViewBag.offerForCustomer = products;
            //////////////////////////////////////////////////////////////////////////////////////////////////////////

            List<Orde> orders = db.Ordes.Where(o => o.userID == user.userID).ToList();
            List<int?> productsID = new List<int?>();
            if (orders != null)
            {
                ViewBag.last5orders = orders.Take(5).ToList();
                foreach (var item in orders)
                {
                    var productID = db.OrderProducts.Where(o => o.orderID == item.orderID).Select(p => p.productID);
                    productsID.AddRange(productID);
                }
            }
            if (productsID.Count > 0)
            {
                productsID = productsID.Distinct().ToList();
                List<Product> last5products = db.Products.Where(p => productsID.Contains(p.productID)).Take(5).ToList();
                ViewBag.last5products = last5products;
            }



            return View(user);
        }
        public ActionResult profile()
        {
            User user = (User)Session["UU!#user"];
            ViewBag.user = user;
            return View();
        }
        [HttpPost]
        public ActionResult updateProfile(User user)
        {
            var userinfo = db.Users.Find(user.userID);

            userinfo.name = user.name;
            userinfo.mobile = user.mobile;
            userinfo.email = user.email;


            db.Entry(userinfo).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Panel");
        }

        public ActionResult addresses()
        {
            User user = (User)Session["UU!#user"];
            ViewBag.user = user;
            var addresses = db.Addresses.Where(a => a.userID == user.userID).OrderByDescending(a => a.addressID).ToList();
            return View(addresses);
        }

        public ActionResult AddAddresses()
        {
            User user = (User)Session["UU!#user"];
            ViewBag.user = user;
            ViewBag.cities = db.Cities.ToList();
            ViewBag.states = db.States.ToList();
            return View();
        }
        [HttpPost]
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
        [HttpPost]
        public ActionResult AddAddresses(Address address)
        {
            address.active = true;
            db.Addresses.Add(address);
            db.SaveChanges();
            return RedirectToAction("addresses");

        }
        [HttpPost]
        public ActionResult deleteAddresses(int? id)
        {
            if (!id.HasValue)
            {
                return Content("0");

            }
            Address address = db.Addresses.Find(id);
            if (address == null)
            {
                return Content("0");
            }
            db.Addresses.Remove(address);
            db.SaveChanges();
            return Content("1");
        }

        public ActionResult Orders()
        {
            User user = (User)Session["UU!#user"];
            ViewBag.user = user;
            return View(db.Ordes.Where(o => o.userID == user.userID).ToList());
        }
        public ActionResult comments()
        {
            User user = (User)Session["UU!#user"];
            ViewBag.user = user;
            var productsID = db.Comments.Where(c => c.userID == user.userID).Select(c => c.productID);
            List<Product> productsHaveComment = db.Products.Where(p => productsID.Contains(p.productID)).Distinct().ToList();

            return View(productsHaveComment.ToList());
        }
        public ActionResult questinsAnswers()
        {
            User user = (User)Session["UU!#user"];
            ViewBag.user = user;


            //var questions = db.Questions.Where(c => c.userID == user.userID);
            //var productsID = questions.Select(c => c.productID);
            //List<Product> productsHaveQuestion = db.Products.Where(p => productsID.Contains(p.productID)).Distinct().ToList();
            var model = db.Questions.Where(c => c.userID == user.userID || c.Answers.Any(w => w.userID == user.userID)).Select(c => c.Product).Distinct();
            return View(model);
        }
        public ActionResult changePassword()
        {
            User user = (User)Session["UU!#user"];
            ViewBag.user = user;
            return View();
        }
        [HttpPost]
        public ActionResult changePassword(string oldPassword, string newPassword, string confirmNewPassword)
        {
            User loggedIn = null;
            loggedIn = Session["UU!#user"] as Shop.Models.User;
            if (loggedIn != null)
            {
                var user = db.Users.Find(loggedIn.userID);
                if (user.password == oldPassword)
                {
                    if (newPassword == confirmNewPassword)
                    {
                        user.password = newPassword;
                        db.SaveChanges();
                        return RedirectToAction("index");
                    }
                    else
                    {
                        ViewBag.message = "کلمه های عبور وارد شده تطابق ندارند لطفا دوباره امتحان کنید";
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "کلمه عبور قبلی نادرست است";
                    return View();

                }
            }
            else
            {
                return RedirectToAction("login");
            }
            //User userInfo = db.Users.Find(u.userID);
            //userInfo.password = user.password;
            //userInfo.confirmPassword = user.confirmPassword;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}