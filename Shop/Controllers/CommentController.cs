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
    public class CommentController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        // GET: Comment/
        public ActionResult Index(string search,int?page, string sort)
        {
            IEnumerable<Comment>model = db.Comments.Include(c => c.Product).Include(c => c.User);
            if (search != null)
            {
                model = db.Comments.Where(s => s.Product.name.Contains(search)).ToList();
            }
            switch (sort)
            {
                case "commentText":
                    model = model.OrderBy(s => s.commentText);
                    break;
                case "commentText_desc":
                    model = model.OrderByDescending(s => s.commentText);
                    break;
                case "positive":
                    model = model.OrderBy(s => s.positive);
                    break;
                case "positive_desc":
                    model = model.OrderByDescending(s => s.positive);
                    break;
                case "negative":
                    model = model.OrderBy(s => s.negative);
                    break;
                case "negative_desc":
                    model = model.OrderByDescending(s => s.negative);
                    break;
                case "rate":
                    model = model.OrderBy(s => s.rate);
                    break;
                case "rate_desc":
                    model = model.OrderByDescending(s => s.rate);
                    break;
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
            return View(model.ToList().ToPagedList(page ?? 1,10));
        }
        [RequsetLogin(2)]
        // GET: Comment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        [RequsetLogin(2)]
        // GET: Comment/Create
        public ActionResult Create()
        {
            ViewBag.productID = new SelectList(db.Products, "productID", "name");
            ViewBag.userID = new SelectList(db.Users, "userID", "username");
            return View();
        }

        // POST: Comment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "commentID,commentText,positive,negative,productID,userID,rate")] Comment comment , int productID)
        {
            User user = (User)Session["UU!#user"];

            if (user != null)
            {
                //if (ModelState.IsValid)
                //{
                comment.userID = user.userID;
                db.Comments.Add(comment);
                db.SaveChanges();

               
                //var s = new ViewDataDictionary<int>();
                //s.Add("Message", "نظر با موفقیت ثبت شد");
                return RedirectToAction("show", "product",new { message = "نظر با موفقیت ثبت شد", id=productID });

                //}

                //ViewBag.productID = new SelectList(db.Products, "productID", "name", comment.productID);
                //ViewBag.userID = new SelectList(db.Users, "userID", "username", comment.userID);
                //return View(comment);
            }
            else
            {
                //ViewBag.commentSaveMessage = "برای ثبت نظر لطقا وارد حساب کاربری شوید";
                return RedirectToAction("show", "product", new { message = "برای ثبت نظر لطقا وارد حساب کاربری شوید", id = productID });

            }

        }
        [RequsetLogin(2)]
        // GET: Comment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "name", comment.productID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", comment.userID);
            return View(comment);
        }
        [RequsetLogin(2)]
        // POST: Comment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "commentID,commentText,positive,negative,productID,userID,rate")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "name", comment.productID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", comment.userID);
            return View(comment);
        }
        [RequsetLogin(2)]
        // GET: Comment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Comment comment = db.Comments.Find(id);
            if (comment == null)
            {
                return HttpNotFound();
            }
            return View(comment);
        }
        [RequsetLogin(2)]
        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
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

        [RequsetLogin(2)]
        [HttpPost]
        public ActionResult approveComment(int commentID , bool approve)
        {
            User user = new User();
            if (Session["UU!#user"] != null)
            {
                user = (User)Session["UU!#user"];
            }


            if (approve && user != null)
            {
                Comment comment = db.Comments.Find(commentID);
                if(comment != null)
                {
                    comment.approveUserID = user.userID;
                    db.Entry(comment).State = EntityState.Modified;
                    db.SaveChanges();


                    return Json(new { success = true ,state = true });


                }
                return Json(new { success = false , state = false });
            }
            else if(!approve)
            {
                Comment comment = db.Comments.Find(commentID);
                db.Comments.Remove(comment);
                db.SaveChanges();

                return Json(new { success = true , state = false });

            }

            return Json(new { success = false , state = false });
        }
    }
}
