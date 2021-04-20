using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Shop.Models;
using pep;
using PagedList;
using System.Net.NetworkInformation;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Text;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private Rizkaran_SiteEntities db = new Rizkaran_SiteEntities();
        [RequsetLogin(2)]
        // GET: User
        public ActionResult Index(string search, int? page, string sort)
        {
            IEnumerable<User> model = db.Users.Include(u => u.UserGroup);
            if (search != null)
            {
                model = db.Users.Where(s => s.name.Contains(search)).ToList();
            }
            switch (sort)
            {
                case "username":
                    model = model.OrderBy(s => s.username);
                    break;
                case "username_desc":
                    model = model.OrderByDescending(s => s.username);
                    break;
                case "password":
                    model = model.OrderBy(s => s.password);
                    break;
                case "password_desc":
                    model = model.OrderByDescending(s => s.password);
                    break;
                case "name":
                    model = model.OrderBy(s => s.name);
                    break;
                case "name_desc":
                    model = model.OrderByDescending(s => s.name);
                    break;
                case "mobile":
                    model = model.OrderBy(s => s.mobile);
                    break;
                case "mobile_desc":
                    model = model.OrderByDescending(s => s.mobile);
                    break;
                case "email":
                    model = model.OrderBy(s => s.email);
                    break;
                case "email_desc":
                    model = model.OrderByDescending(s => s.email);
                    break;
                case "sex":
                    model = model.OrderBy(s => s.sex);
                    break;
                case "sex_desc":
                    model = model.OrderByDescending(s => s.sex);
                    break;
                case "groupName":
                    model = model.OrderBy(s => s.UserGroup.groupName);
                    break;
                case "groupName_desc":
                    model = model.OrderByDescending(s => s.UserGroup.groupName);
                    break;
            }
            ViewBag.SortType = sort;
            return View(model.ToList().ToPagedList(page ?? 1, 10));
        }
        [RequsetLogin(2)]
        // GET: User/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [RequsetLogin(2)]
        // GET: User/Create
        public ActionResult Create()
        {
            ViewBag.usergroupID = new SelectList(db.UserGroups, "usergroupID", "groupName");
            return View();
        }
        [RequsetLogin(2)]
        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "userID,username,password,name,mobile,usergroupID,email,sex")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.usergroupID = new SelectList(db.UserGroups, "usergroupID", "groupName", user.usergroupID);
            return View(user);
        }
        [RequsetLogin(2)]
        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.usergroupID = new SelectList(db.UserGroups, "usergroupID", "groupName", user.usergroupID);
            return View(user);
        }
        [RequsetLogin(2)]
        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "userID,username,password,name,mobile,usergroupID,email,sex")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.usergroupID = new SelectList(db.UserGroups, "usergroupID", "groupName", user.usergroupID);
            return View(user);
        }
        [RequsetLogin(2)]
        // GET: User/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [RequsetLogin(2)]
        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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



        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(User user)
        {
            if (user != null)
            {
                if (user.password != user.confirmPassword)
                {
                    ViewBag.registerMessage = "تکرار رمز عبور اشتباه است";
                    return View();
                }

                if (!IsValidPhone(user.mobile))
                {
                    ViewBag.registerMessage = "شماره تلفن اشتباه است";
                    return View();
                }



                var oldUser = db.Users.FirstOrDefault(u => u.username == user.username || u.email == user.email);

                if (oldUser == null)
                {
                    //user.birthday = pep.pep.toMiladiDate(Convert.ToString(user.birthday));
                    db.Users.Add(user);
                    db.SaveChanges();

                    return RedirectToAction("Login", "User", new { message = "ثبت نام شما با موفقیت انجام شد" });
                }

                ViewBag.registerMessage = "این نام کاربری و یا ایمیل قبلا ثبت شده است";

                return View();


            }

            return RedirectToAction("index", "home");
        }



        public bool IsValidPhone(string Phone)
        {
            try
            {
                if (string.IsNullOrEmpty(Phone))
                    return false;
                var r = new Regex(@"^(\+98?)?{?(0?9[0-9]{9,9}}?)$");
                return r.IsMatch(Phone);

            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult Login(string message)
        {
            ViewBag.message = message;
            return View();
        }
        [HttpPost]
        public ActionResult Login(User user)
        {
            if (user != null)
            {
                var logged = db.Users.FirstOrDefault(u => u.username == user.username && u.password == user.password && u.usergroupID != 2);
                if (logged != null)
                {
                    Session["UU!#user"] = logged;

                    return RedirectToAction("index", "home");

                }
            }
            ViewBag.loginMessage = "نام کاربری و یا رمز عبور اشتباه است";
            return View();
        }

        public ActionResult Logout()
        {
            Session["UU!#user"] = null;
            return RedirectToAction("index", "home");
        }




        public ActionResult LoginAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginAdmin(User user)
        {
            if (user != null)
            {
                var logged = db.Users.FirstOrDefault(u => u.username == user.username && u.password == user.password);
                if (logged != null)
                {
                    // is admin
                    if (logged.usergroupID == 2)
                    {

                        Session["UU!#user"] = logged;
                        HttpCookie myCookie = new HttpCookie("c_uuu_rizkar");
                        var s = Convert.ToBase64String(MachineKey.Protect(Encoding.UTF8.GetBytes(logged.userID.ToString())));
                        //  Encoding.UTF8.GetString(MachineKey.Unprotect(Convert.FromBase64String("your cookie value")))
                        myCookie.Values.Add("id", s);
                        myCookie.Expires = DateTime.Now.AddDays(7);
                        Response.Cookies.Add(myCookie);
                        if (logged.usergroupID == 2)
                        {
                            return RedirectToAction("index", "product");
                        }
                    }
                }
            }
            ViewBag.loginMessage = "نام کاربری و یا رمز عبور اشتباه است";
            return View();
        }









        public ActionResult sendResetPasswordCode()
        {
            return View();
        }



        [HttpPost]
        public ActionResult sendResetPasswordCode(string Email)
        {
            User user = db.Users.FirstOrDefault(u => u.email == Email);
            if (user != null)
            {
                user.resetPasswordCode = RandomString(20);
                user.resetPasswordCodeDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));

                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();

                if (sendResetPasswordEmail(user.email, user.resetPasswordCode))
                {
                    return Json(new { success = true, data = "لینک تغییر رمز عبور با موفقیت ارسال شد" });
                }
                return Json(new { success = false, data = "خطایی رخ داده است" });
            }
            return Json(new { success = false, data = "ایمیل نادرست است" });
        }



        // .../user/resetPassword/gfsdyhfgjksdfgjksdf

        public ActionResult resetPassword(string resetPasswordCode)
        {
            User user = db.Users.FirstOrDefault(u => u.resetPasswordCode == resetPasswordCode);

            if (user != null)
            {
                DateTime codeDate = (DateTime)user.resetPasswordCodeDate;
                DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                TimeSpan duration = now - codeDate;
                double diffirent = duration.TotalMinutes;

                if (diffirent < 180)
                {
                    ViewBag.resetPasswordCode = resetPasswordCode;
                    return View();

                }
                else
                {
                    ViewBag.message = "اعتبار این لینک تمام شده است";
                    return View();

                }
            }
            else
            {
                ViewBag.message = "خطایی رخ داده است";
                return View();
            }
        }






        [HttpPost]
        public ActionResult resetPassword(string newPassword, string passwordConfirm, string resetPasswordCode)
        {
            ViewBag.resetPasswordCode = resetPasswordCode;
            if (newPassword != passwordConfirm)
            {
                return Json(new { success = false, data = "رمز عبور با تکرارش برابر نیست" });
            }


            User user = db.Users.FirstOrDefault(u => u.resetPasswordCode == resetPasswordCode);

            if (user != null)
            {
                DateTime codeDate = (DateTime)user.resetPasswordCodeDate;
                DateTime now = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                TimeSpan duration = now - codeDate;
                double diffirent = duration.TotalMinutes;

                if (diffirent < 180)
                {
                    user.password = newPassword;
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();

                    return Json(new { success = true, data = "رمز عبور شما با موفقیت تغییر یافت" });

                }
                else
                {
                    return Json(new { success = false, data = "اعتبار این لینک تمام شده است" });
                }
            }
            else
            {
                return Json(new { success = false, data = "خطایی رخ داده است" });
            }



        }



        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public bool sendResetPasswordEmail(string email, string resetPasswordCode)
        {
            int port = 25;
            string fromEmail = "";
            string fromEmailPassword = "";
            string smtp = "smtp.gmail.com";


            string url = System.Web.HttpContext.Current.Request.Url + "/user/resetPassword/" + resetPasswordCode;

            Ping png = new Ping();
            PingReply pi = png.Send("Google.com");
            if (pi.Status == IPStatus.Success)
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail, "فروشگاه");
                mail.To.Add(new MailAddress(email));
                mail.Body = url;
                mail.IsBodyHtml = false;
                mail.Subject = "تغییر رمز عبور";
                SmtpClient client = new SmtpClient(smtp, port);
                client.Credentials = new NetworkCredential(fromEmail, fromEmailPassword);
                client.EnableSsl = true;
                try
                {
                    client.Send(mail);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }



    }
}
