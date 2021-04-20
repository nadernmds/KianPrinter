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

namespace Shop.Controllers
{
    public class FavoriteController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        // GET: Favorite
        public ActionResult Index(string search,int?page, string sort)
        {
            IEnumerable<Favorite>model= db.Favorites.Include(f => f.Product).Include(f => f.User);
            if (search != null)
            {
                model = db.Favorites.Where(s => s.Product.name.Contains(search)).ToList();
            }
            switch (sort)
            {
                case "name":
                    model = model.OrderBy(s => s.Product.name);
                    break;
                case "name_desc":
                    model = model.OrderByDescending(s => s.Product.name);
                    break;
                case "username":
                    model = model.OrderBy(s => s.User.username);
                    break;
                case "username_desc":
                    model = model.OrderByDescending(s => s.User.username);
                    break;
            }
                    ViewBag.SortType = sort;
            return View(model.ToList().ToPagedList(page?? 1,10));
        }
        [RequsetLogin(2)]
        // GET: Favorite/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorite favorite = db.Favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }
        [RequsetLogin(2)]
        // GET: Favorite/Create
        public ActionResult Create()
        {
            ViewBag.productID = new SelectList(db.Products, "productID", "name");
            ViewBag.userID = new SelectList(db.Users, "userID", "username");
            return View();
        }
        [RequsetLogin(2)]
        // POST: Favorite/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "favoriteID,productID,userID")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                db.Favorites.Add(favorite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.productID = new SelectList(db.Products, "productID", "name", favorite.productID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", favorite.userID);
            return View(favorite);
        }
        [RequsetLogin(2)]
        // GET: Favorite/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorite favorite = db.Favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "name", favorite.productID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", favorite.userID);
            return View(favorite);
        }
        [RequsetLogin(2)]
        // POST: Favorite/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "favoriteID,productID,userID")] Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                db.Entry(favorite).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "name", favorite.productID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", favorite.userID);
            return View(favorite);
        }
        [RequsetLogin(2)]
        // GET: Favorite/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorite favorite = db.Favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }
        [RequsetLogin(2)]
        // POST: Favorite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Favorite favorite = db.Favorites.Find(id);
            db.Favorites.Remove(favorite);
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








        public ActionResult FavoriteList()
        {
            User user = (User)Session["UU!#user"];


            if (user == null)
            { 
                return View();
            }

            return View(db.Favorites.Where(f=>f.userID == user.userID));
        }


        [HttpPost]
        public ActionResult Favorite(int productID)
        {
            User user = (User)Session["UU!#user"];


            if(user == null)
            {
                return Json(new { success = false, data = "برای افزودن به علاقمندی باید وارد حساب کاربری شوید" });

            }

            var favor = db.Favorites.Where(f=>(f.productID == productID && f.userID == user.userID)).ToList();

            if(favor.Count > 0)
            {
                db.Favorites.RemoveRange(favor);
                db.SaveChanges();

                return Json(new {message="از لیست علاقه مندی حذف شد", success = true, data = 0 });
            }

            Favorite favorite = new Favorite()
            {
                productID = productID,
                userID = user.userID
            };

            db.Favorites.Add(favorite);
            db.SaveChanges();


            var product = db.Products.Find(productID);


            return Json(new { message = "به لیست علاقه مندی اضافه شد", success = true, data = 1 });


        }


        [HttpPost]
        public ActionResult removeOfFavorite(int productID)
        {
            User user = (User)Session["UU!#user"];
            
            var favorite = db.Favorites.FirstOrDefault(f => (f.productID == productID && f.userID == user.userID));

            if (favorite != null)
            {
                db.Favorites.Remove(favorite);
                db.SaveChanges();

               
            }

            return Json(new { success = true, data = productID });


        }
    }
}
