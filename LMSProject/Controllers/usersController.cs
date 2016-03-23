using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LMSProject.DataAccess;
using LMSProject.Models;

namespace LMSProject.Controllers
{
    public class usersController : Controller
    {
        private LMSContext db = new LMSContext();
        private ApplicationDbContext dbUser = new ApplicationDbContext();
        //private TeacherStudentRepository myUserRepo = new TeacherStudentRepository();

        // GET: users
        public ActionResult Index()
        {
            ViewBag.InternalUsers = (List<ApplicationUser>)(dbUser.Users.ToList());
            var users = db.users.Include(u => u.schoolClasses);
            return View(users.ToList());
        }

        // GET: users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(dbUser.Roles, "Id", "Name");
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name");
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,schoolClassID,RoleId,UserName,UserPassword")] user user)
        {
            if (ModelState.IsValid)
            {
                //TeacherStudentRepository myUserRepo = new TeacherStudentRepository();
                try
                {
                    //HttpContext.User.Identity.
                    //manually call the reopistory to save user in user tables
                    TeacherStudentRepository myUserRepo = new TeacherStudentRepository();

                    myUserRepo.AddUser(user.UserName, user.UserName, user.UserPassword);
                    user.UserId = myUserRepo.GetUserIdByName(user.UserName);
                    myUserRepo.AddUserToRole(user.UserId, user.RoleId);

                    //Auto added by VS
                    db.users.Add(user);
                    db.SaveChanges();
                }
                catch (Exception err)
                {
                    return Content("Mayor Error in create user:" + err.Message);
                }

                return RedirectToAction("Index");
            }

            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", user.schoolClassID);
            return View(user);
        }

        // GET: users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", user.schoolClassID);
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,schoolClassID,RoleId")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", user.schoolClassID);
            return View(user);
        }

        // GET: users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
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
