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
    public class QuestionController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: Question
        public ActionResult Index(string search,int?page, string sort)
        {
            IEnumerable<Question>model = db.Questions.Include(q => q.Product).Include(q => q.User);
            if(search != null)
            {
                model = db.Questions.Where(s=>s.Product.name.Contains(search)).ToList();
            }
            switch (sort)
            {
                case "questionText":
                    model = model.OrderBy(s => s.questionText);
                    break;
                case "questionText_desc":
                    model = model.OrderByDescending(s => s.questionText);
                    break;
                case "questionDate":
                    model = model.OrderBy(s => s.questionDate);
                    break;
                case "questionDate_desc":
                    model = model.OrderByDescending(s => s.questionDate);
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
        // GET: Question/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }
        [RequsetLogin(2)]
        // GET: Question/Create
        public ActionResult Create()
        {
            ViewBag.productID = new SelectList(db.Products, "productID", "name");
            ViewBag.userID = new SelectList(db.Users, "userID", "username");
            return View();
        }

        // POST: Question/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "questionID,questionText,questionDate,productID,userID")] Question question, int productID)
        {

            User user = (User)Session["UU!#user"];

            if (user != null)
            {

                //if (ModelState.IsValid)
                //{
                question.questionDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                question.userID = user.userID;
                db.Questions.Add(question);
                db.SaveChanges();


                return RedirectToAction("show", "product", new { message = "نظر با موفقیت ثبت شد", id = productID });

                //return RedirectToAction("Index");
                //}

                //ViewBag.productID = new SelectList(db.Products, "productID", "name", question.productID);
                //ViewBag.userID = new SelectList(db.Users, "userID", "username", question.userID);
                //return View(question);

            }
            else
            {
                return RedirectToAction("show", "product", new { message = "برای ثبت نظر لطقا وارد حساب کاربری شوید", id = productID });

            }

        }
        [RequsetLogin(2)]
        // GET: Question/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "name", question.productID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", question.userID);
            return View(question);
        }
        [RequsetLogin(2)]
        // POST: Question/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "questionID,questionText,questionDate,productID,userID")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.productID = new SelectList(db.Products, "productID", "name", question.productID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", question.userID);
            return View(question);
        }
        [RequsetLogin(2)]
        // GET: Question/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }
        [RequsetLogin(2)]
        // POST: Question/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
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
        public ActionResult approveQuestion(int questionID, bool approve)
        {
            User user = new User();
            if (Session["UU!#user"] != null)
            {
                user = (User)Session["UU!#user"];
            }


            if (approve && user != null)
            {
                Question question = db.Questions.Find(questionID);
                if (question != null)
                {
                    question.approveUserID = user.userID;
                    db.Entry(question).State = EntityState.Modified;
                    db.SaveChanges();


                    return Json(new { success = true ,state=true});


                }
                return Json(new { success = false , state = false });
            }
            else if (!approve)
            {
                Question question = db.Questions.Find(questionID);
                db.Questions.Remove(question);
                db.SaveChanges();

                return Json(new { success = true,state=false });

            }

            return Json(new { success = false, state = false });
        }


    }
}
