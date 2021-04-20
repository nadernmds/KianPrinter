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
    public class AnswerController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();

        [RequsetLogin(2)]
        // GET: Answer
        public ActionResult Index(string search,int?page,string sort)
        {
            IEnumerable<Answer>model = db.Answers.Include(a => a.Question).Include(a => a.User);
            if (search != null)
            {
                model = db.Answers.Where(s => s.Question.Product.name.Contains(search)).ToList();
            }
            switch (sort)
            {
                case "answerText":
                    model = model.OrderBy(s => s.answerText);
                    break;
                case "answerText_desc":
                    model = model.OrderByDescending(s => s.answerText);
                    break;
                case "answerDate":
                    model = model.OrderBy(s => s.answerDate);
                    break;
                case "answerDate_desc":
                    model = model.OrderByDescending(s => s.answerDate);
                    break;
                case "questionText":
                    model = model.OrderBy(s => s.Question.questionText);
                    break;
                case "questionText_desc":
                    model = model.OrderByDescending(s => s.Question.questionText);
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
        // GET: Answer/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }
        [RequsetLogin(2)]
        // GET: Answer/Create
        public ActionResult Create()
        {
            ViewBag.questionID = new SelectList(db.Questions, "questionID", "questionText");
            ViewBag.userID = new SelectList(db.Users, "userID", "username");
            return View();
        }

        // POST: Answer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "answerID,answerText,answerDate,questionID,userID")] Answer answer, int? questionID , int? productID)
        {
            User user = (User)Session["UU!#user"];

            if (user != null)
            {

                answer.questionID = questionID;

                answer.answerDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                answer.userID = user.userID;
                db.Answers.Add(answer);
                db.SaveChanges();

                if (productID == null)
                {
                    return RedirectToAction("index", "answer");

                }
                else
                {
                    return RedirectToAction("show", "product", new { message = "نظر با موفقیت ثبت شد", id = productID });

                }
                //if (ModelState.IsValid)
                //{


                //return RedirectToAction("show", "product", new { message = "نظر با موفقیت ثبت شد", id = productID });
                //}

                //ViewBag.questionID = new SelectList(db.Questions, "questionID", "questionText", answer.questionID);
                //ViewBag.userID = new SelectList(db.Users, "userID", "username", answer.userID);
                //return View(answer);
            }
            else
            {
                return RedirectToAction("show", "product", new { message = "برای ثبت نظر لطقا وارد حساب کاربری شوید", id = productID });
            }



        }
        [RequsetLogin(2)]
        // GET: Answer/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            ViewBag.questionID = new SelectList(db.Questions, "questionID", "questionText", answer.questionID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", answer.userID);
            return View(answer);
        }
        [RequsetLogin(2)]
        // POST: Answer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "answerID,answerText,answerDate,questionID,userID")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(answer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.questionID = new SelectList(db.Questions, "questionID", "questionText", answer.questionID);
            ViewBag.userID = new SelectList(db.Users, "userID", "username", answer.userID);
            return View(answer);
        }
        [RequsetLogin(2)]
        // GET: Answer/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Answer answer = db.Answers.Find(id);
            if (answer == null)
            {
                return HttpNotFound();
            }
            return View(answer);
        }
        [RequsetLogin(2)]
        // POST: Answer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Answer answer = db.Answers.Find(id);
            db.Answers.Remove(answer);
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
        public ActionResult approveAnswer(int answerID, bool approve)
        {
            User user = new User();
            if (Session["UU!#user"] != null)
            {
                user = (User)Session["UU!#user"];
            }


            if (approve && user != null)
            {
                Answer answer = db.Answers.Find(answerID);
                if (answer != null)
                {
                    answer.approveUserID = user.userID;
                    db.Entry(answer).State = EntityState.Modified;
                    db.SaveChanges();


                    return Json(new { success = true , state = true});


                }
                return Json(new { success = false, state = false });
            }
            else if (!approve)
            {
                Answer answer = db.Answers.Find(answerID);
                db.Answers.Remove(answer);
                db.SaveChanges();

                return Json(new { success = true, state = false });

            }

            return Json(new { success = false, state = false });
        }






    }
}
