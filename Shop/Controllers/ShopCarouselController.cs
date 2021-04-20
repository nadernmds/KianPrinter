using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;

namespace Shop.Controllers
{
    public class ShopCarouselController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        // GET: ShopCarousels
   
        public ActionResult Index()
        {
            return View(db.ShopCarousels.ToList());
        }
        [RequsetLogin(2)]
        // GET: ShopCarousels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopCarousel shopCarousel = db.ShopCarousels.Find(id);
            if (shopCarousel == null)
            {
                return HttpNotFound();
            }
            return View(shopCarousel);
        }

        // GET: ShopCarousels/Create
        public ActionResult Create()
        {
            return View();
        }
        [RequsetLogin(2)]
        // POST: ShopCarousels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "shopCarouselsID,show")] ShopCarousel shopCarousel,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if(shopCarousel.show == null)
                {
                    shopCarousel.show = false;
                }
                db.ShopCarousels.Add(shopCarousel);
                db.SaveChanges();
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/ShopCarousel/" + shopCarousel.shopCarouselID + ".jpg"));
                }
                return RedirectToAction("Index");
            }

            return View(shopCarousel);
        }
        [RequsetLogin(2)]
        // GET: ShopCarousels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopCarousel shopCarousel = db.ShopCarousels.Find(id);
            if (shopCarousel == null)
            {
                return HttpNotFound();
            }
            return View(shopCarousel);
        }
        [RequsetLogin(2)]
        // POST: ShopCarousels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "shopCarouselID,show")] ShopCarousel shopCarousel,HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (shopCarousel.show == null)
                {
                    shopCarousel.show = false;
                }

                db.Entry(shopCarousel).State = EntityState.Modified;
                db.SaveChanges();
                if (file != null)
                {
                    file.SaveAs(Server.MapPath("~/Images/ShopCarousel/" + shopCarousel.shopCarouselID + ".jpg"));
                }
                return RedirectToAction("Index");
            }
            return View(shopCarousel);
        }
        [RequsetLogin(2)]
        // GET: ShopCarousels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShopCarousel shopCarousel = db.ShopCarousels.Find(id);
            if (shopCarousel == null)
            {
                return HttpNotFound();
            }
            return View(shopCarousel);
        }
        [RequsetLogin(2)]
        // POST: ShopCarousels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShopCarousel shopCarousel = db.ShopCarousels.Find(id);
            db.ShopCarousels.Remove(shopCarousel);
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
