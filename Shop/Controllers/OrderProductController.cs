using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using PagedList;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace Shop.Controllers
{
    public class OrderProductController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: OrderProduct
        public ActionResult Index(string search, int? page, string sort)
        {
            IEnumerable<OrderProduct> model = db.OrderProducts.Include(o => o.Orde).Include(o => o.Product);
            if (search != null)
            {
                model = db.OrderProducts.Where(s => s.Product.name.Contains(search));
            }
            switch (sort)
            {
                case "count":
                    model = model.OrderBy(s => s.count);
                    break;
                case "count_desc":
                    model = model.OrderByDescending(s => s.count);
                    break;
                case "username":
                    model = model.OrderBy(s => s.Orde.User.username);
                    break;
                case "username_desc":
                    model = model.OrderByDescending(s => s.Orde.User.username);
                    break;
                case "name":
                    model = model.OrderBy(s => s.Product.name);
                    break;
                case "name_desc":
                    model = model.OrderByDescending(s => s.Product.name);
                    break;
            }
            ViewBag.SortType = sort;
            return View(model.ToList().ToPagedList(page ?? 1, 10));
        }
        [RequsetLogin(2)]
        // GET: OrderProduct/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }
        [RequsetLogin(2)]
        // GET: OrderProduct/Create
        public ActionResult Create()
        {
            ViewBag.orderID = new SelectList(db.Ordes, "orderID", "orderID");
            ViewBag.productID = new SelectList(db.Products, "productID", "name");
            return View();
        }
        [RequsetLogin(2)]
        // POST: OrderProduct/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "orderProductID,count,orderID,productID")] OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                db.OrderProducts.Add(orderProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.orderID = new SelectList(db.Ordes, "orderID", "orderID", orderProduct.orderID);
            ViewBag.productID = new SelectList(db.Products, "productID", "name", orderProduct.productID);
            return View(orderProduct);
        }
        [RequsetLogin(2)]
        // GET: OrderProduct/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            ViewBag.orderID = new SelectList(db.Ordes, "orderID", "orderID", orderProduct.orderID);
            ViewBag.productID = new SelectList(db.Products, "productID", "name", orderProduct.productID);
            return View(orderProduct);
        }
        [RequsetLogin(2)]
        // POST: OrderProduct/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "orderProductID,count,orderID,productID")] OrderProduct orderProduct)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orderProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.orderID = new SelectList(db.Ordes, "orderID", "orderID", orderProduct.orderID);
            ViewBag.productID = new SelectList(db.Products, "productID", "name", orderProduct.productID);
            return View(orderProduct);
        }
        [RequsetLogin(2)]
        // GET: OrderProduct/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            if (orderProduct == null)
            {
                return HttpNotFound();
            }
            return View(orderProduct);
        }
        [RequsetLogin(2)]
        // POST: OrderProduct/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OrderProduct orderProduct = db.OrderProducts.Find(id);
            db.OrderProducts.Remove(orderProduct);
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






















        public ActionResult orderInfoes()
        {

            User user = (User)Session["UU!#user"];


            if (user != null)
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


                
                if (cartProducts.Count < 1)
                {
                    ViewBag.emptyMessage = "سبد خرید خالی است";
                    return RedirectToAction("showCart", "product");
                }

                foreach (var item in cartProducts)
                {
                    var p = db.Products.Find(item.productID);

                    if (!(item.count <= p.existingCount))
                    {
                        ViewBag.errorProductCount = "تعداد انتخابی از " + p.name + "بیشتر از موجودی است";
                        return RedirectToAction("showCart", "product");
                    }
                }





                ViewBag.Payment = db.PaymentTypes.ToList();

                ViewBag.cartProduct = cartProducts;
                ViewBag.products = products;




                ViewBag.paymentType = db.PaymentTypes.Where(p => p.active == true).ToList();

                ViewBag.states = db.States.ToList();
                //ViewBag.cities = db.Cities.ToList();

                var oldAddress = db.Addresses.Where(o => o.userID == user.userID).ToList();

                if (oldAddress != null && oldAddress.Count > 0)
                {
                    return View(oldAddress);
                }




                return View();
            }
            else
            {
                return RedirectToAction("login", "user");
            }

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
        public ActionResult getAdresses(int? addressID)
        {
            var adressDetail = db.Addresses.Find(addressID);


            return Json(new { success = true, address = adressDetail.addressDetail, state = adressDetail.City.stateID, city = adressDetail.cityID, postalCode = adressDetail.postalCode, mobile = adressDetail.mobile, phone = adressDetail.phone });
        }

        public ActionResult FindCity(int cityID)
        {
            var cities = db.Cities.Where(c => c.cityID == cityID).Select(x => new { x.cityName, x.cityID }).ToList();


            var city = cities.FirstOrDefault();


            return Json(new { success = true, c = city });
        }











        [HttpPost]
        public ActionResult saveOrderInfoes(Address address, byte paymentTypeID , int? activeAddressID)
        {
            User user = (User)Session["UU!#user"];
            user = db.Users.Find(user.userID);

            DateTime today = DateTime.Now;


            if(! (activeAddressID != null && activeAddressID != 0 ) )
            {
                address.userID = user.userID;
                address.active = true;
                db.Addresses.Add(address);
                db.SaveChanges();
            }

            Session["userActiveAddress"] = address.addressID;


            List<Cart> carts = new List<Cart>();
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

                        var productColor = db.ProductColors.FirstOrDefault(p => p.color == cartProduct.color);
                        if (productColor != null)
                        {
                            Cart cart = new Cart
                            {
                                productID = cartProduct.productID,
                                userID = user.userID,
                                count = cartProduct.count,
                                expire = today.AddDays(2),
                                productColorID = productColor.productColorID
                            };
                            carts.Add(cart);
                        }
                        else
                        {
                            Cart cart = new Cart
                            {
                                productID = cartProduct.productID,
                                userID = user.userID,
                                count = cartProduct.count,
                                expire = today.AddDays(2)
                            };
                            carts.Add(cart);
                        }

                    }
                }
            }

            var oldCart = db.Carts.Where(c => c.userID == user.userID).ToList();
            db.Carts.RemoveRange(oldCart);
            db.SaveChanges();

            db.Carts.AddRange(carts);
            db.SaveChanges();

            var newCarts = db.Carts.Where(c => c.userID == user.userID).ToList();


            //dargah
            if (paymentTypeID == 1)
            {
                int price = 0;
                foreach (var p in newCarts)
                {
                    var product = db.Products.Find(p.productID);

                    DateTime now = Convert.ToDateTime(DateTime.Now.ToString());
                    var offers = product.Offers.Where(o => o.startDate < now && o.endDate > now).ToList();
                    decimal offPrice = 0;
                    decimal offPercent = 0;
                    foreach (var item in offers)
                    {
                        offPrice += (int)item.price;
                        offPercent += (int)item.offPercent;
                    }
                    decimal productPrice = product.price;
                    decimal finalPrice = productPrice;
                    if (offPercent > 0)
                    {
                        finalPrice = finalPrice - (productPrice * (offPercent / 100));
                    }
                    if (offPrice > 0)
                    {
                        finalPrice = finalPrice - offPrice;
                    }

                    int totalThisProduct = Convert.ToInt32(finalPrice * p.count);
                    price += totalThisProduct;
                }
                return Payment(price, user.email, user.mobile, "فروشگاه", "http://localhost:50099/orderproduct/showPaymentInfo");
            }
            else
            {
                saveOrder(user, address, paymentTypeID,"",DateTime.Now, newCarts);
                return RedirectToAction("", "", new { });
            }

        }











        public zarinpal.PaymentGatewayImplementationService zp = new zarinpal.PaymentGatewayImplementationService();


        public ActionResult Payment(int Price, string email, string mobile, string Title, string CallbackUrl)
        {
            ServicePointManager.Expect100Continue = false;

            string Authority;

            //  Response.Redirect(CallbackUrl);
            int Status = zp.PaymentRequest("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", Price, Title, email, mobile, CallbackUrl, out Authority);

            if (Status == 100)
            {
                if (Request.IsLocal)
                    return Redirect("https://sandbox.zarinpal.com/pg/StartPay/" + Authority);
                else
                    return Redirect("https://zarinpal.com/pg/StartPay/" + Authority);
            }
            else
            {
                return Content("error: " + Status);
            }
        }




        [HttpGet]
        public ActionResult showPaymentInfo(string Authority, string Status)
        {
            User user = (User)Session["UU!#user"];
            user = db.Users.Find(user.userID);
            ViewBag.user = user;
            if (Status != "" && Status != null && Authority != "" && Authority != null)
            {
                if (Status.ToString().Equals("OK"))
                {

                    int price = 0;


                    
                    var newCarts = db.Carts.Where(c => c.userID == user.userID).ToList();
                    foreach (var p in newCarts)
                    {
                        var product = db.Products.Find(p.productID);

                        DateTime now = Convert.ToDateTime(DateTime.Now.ToString());
                        var offers = product.Offers.Where(o => o.startDate < now && o.endDate > now).ToList();
                        decimal offPrice = 0;
                        decimal offPercent = 0;
                        foreach (var item in offers)
                        {
                            offPrice += (int)item.price;
                            offPercent += (int)item.offPercent;
                        }
                        decimal productPrice = product.price;
                        decimal finalPrice = productPrice;
                        if (offPercent > 0)
                        {
                            finalPrice = finalPrice - (productPrice * (offPercent / 100));
                        }
                        if (offPrice > 0)
                        {
                            finalPrice = finalPrice - offPrice;
                        }
                        int totalThisProduct = Convert.ToInt32(finalPrice * p.count);
                        price += totalThisProduct;

                    }




                    int Amount = price;
                    long RefID;
                    ServicePointManager.Expect100Continue = false;
                    int status = zp.PaymentVerification("xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx", Authority.ToString(), Amount, out RefID);
                    if (status == 100 || status == 101 || status == -11)
                    {
                        // یعنی دانلود مستقیم بعد از خرید
                        if (status == 100)
                        {

                            int userActiveAddress = Convert.ToInt32(Session["userActiveAddress"]);
                            var address = db.Addresses.Find(userActiveAddress);
                            saveOrder(user, address, 1,Convert.ToString(RefID), DateTime.Now, newCarts);
                            
                            
                            ViewBag.status = true;
                            ViewBag.TransCode = RefID;
                            return View();
                        }// end if 100
                    }
                    else if (status == -33) // -33 : رقم تراکنش با مبلغ پرداخت شده برابر نیست
                    {
                        ViewBag.status = false;
                        ViewBag.message = "این تراکنش منقضی شده است.";
                        return View();
                    }
                    else
                    {
                        ViewBag.status = false;
                        ViewBag.message = "Error!! Status: " + status;
                        return View();
                    }
                }
                else
                {
                    ViewBag.status = false;
                    ViewBag.message = "Error! Authority: " + Request.QueryString["Authority"].ToString() + " Status: " + Request.QueryString["Status"].ToString();
                    return View();
                }
            }
            else
            {
                ViewBag.status = false;
                ViewBag.message = "هیچ اطلاعاتی از درگاه دریافت نشد!!";
                return View();
            }
            ViewBag.status = false;
            ViewBag.message = "هیچ اطلاعاتی از درگاه دریافت نشد!!";
            return View();
        }









        
        private void saveOrder(User user, Address address, byte paymentTypeID, string paymentCode, DateTime paymentDate, List<Cart> carts)
        {

            //dargah
            if (paymentTypeID == 1)
            {

                Orde order = new Orde
                {
                    orderOff = 0,
                    userID = user.userID,
                    addressID = address.addressID,
                    orderStateID = 4,
                    date = DateTime.Now,
                    paymentTypeID = paymentTypeID,
                    description = "",
                    shippingCost = 0,


                    paymentCode = paymentCode,
                    paymentDate = paymentDate
                };
                db.Ordes.Add(order);
                db.SaveChanges();


                List<OrderProduct> orderProducts = new List<OrderProduct>();
                foreach (var item in carts)
                {
                    if (item.productColorID != null)
                    {
                        var color = db.ProductColors.FirstOrDefault(c => c.productColorID == item.productColorID);
                        OrderProduct orderProduct = new OrderProduct
                        {
                            count = item.count,
                            productID = item.productID,
                            productColorID = color.productColorID,
                            orderID = order.orderID
                        };
                        orderProducts.Add(orderProduct);
                    }
                    else
                    {
                        OrderProduct orderProduct = new OrderProduct
                        {
                            count = item.count,
                            productID = item.productID,
                            orderID = order.orderID
                        };
                        orderProducts.Add(orderProduct);
                    }
                }

                db.OrderProducts.AddRange(orderProducts);
                db.SaveChanges();



                foreach (var item in orderProducts)
                {
                    var product = db.Products.Find(item.productID);
                    product.existingCount = product.existingCount - item.count;

                    if (product.existingCount < 1)
                    {
                        product.productStateID = 2;
                    }
                    db.Entry(product).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
            else
            {
                Orde order = new Orde
                {
                    orderOff = 0,
                    userID = user.userID,
                    addressID = address.addressID,
                    orderStateID = 4,
                    date = DateTime.Now,
                    paymentTypeID = paymentTypeID,
                    description = "",
                    shippingCost = 0
                };
                db.Ordes.Add(order);
                db.SaveChanges();


                List<OrderProduct> orderProducts = new List<OrderProduct>();
                foreach (var item in carts)
                {
                    if (item.productColorID != null)
                    {
                        var color = db.ProductColors.FirstOrDefault(c => c.productColorID == item.productColorID);
                        OrderProduct orderProduct = new OrderProduct
                        {
                            count = item.count,
                            productID = item.productID,
                            productColorID = color.productColorID,
                            orderID = order.orderID
                        };
                        orderProducts.Add(orderProduct);
                    }
                    else
                    {
                        OrderProduct orderProduct = new OrderProduct
                        {
                            count = item.count,
                            productID = item.productID,
                            orderID = order.orderID
                        };
                        orderProducts.Add(orderProduct);
                    }
                }

                db.OrderProducts.AddRange(orderProducts);
                db.SaveChanges();



                foreach (var item in orderProducts)
                {
                    var product = db.Products.Find(item.productID);
                    product.existingCount = product.existingCount - item.count;

                    if (product.existingCount < 1)
                    {
                        product.productStateID = 2;
                    }
                    db.Entry(product).State = EntityState.Modified;
                }

                db.SaveChanges();
                
            }
        }





    }
}
