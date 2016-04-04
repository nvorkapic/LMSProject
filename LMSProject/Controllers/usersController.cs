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
	[Authorize(Roles="Teacher")]
    public class usersController : Controller
    {
        private LMSContext db = new LMSContext();
        private ApplicationDbContext dbUser = new ApplicationDbContext();

        private TeacherStudentRepository myUserRepo = new TeacherStudentRepository();

        // GET: users
        public ActionResult Index()
        {
            //ViewBag.InternalUsers = (List<ApplicationUser>)(dbUser.Users.ToList());
            //var users = db.users.Include(u => u.schoolClasses);

            //var Results = from g in DB.Galleries
            //  join m in DB.Media on g.GalleryID equals m.GalleryID
            //  where g.GalleryID == GalleryID
            //  orderby m.MediaDate descending, m.MediaID descending
            //  select new { g.GalleryTitle, Media = m };

            //dbUser.Roles.
            //var userInfo = from dbu in dbUser.Users
            //               join dbr in dbUser.

            //List<userViewModel> userInfo = new List<userViewModel>();

            //return View(userInfo.ToList());
            return View(myUserRepo.getUserViewModel());
        }

        // GET: users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.UserSchoolClasses = (from usr in db.users
                                        where usr.UserId == id
                                        select usr).ToList();
            //if (user == null)
            //{
            //    return HttpNotFound();
            //}userViewModel


            //Data to partial view to add new schoolclass
            List<int> userCurrentSchollclasses = myUserRepo.getCurrentUserSchoolClassesByID(id);

            IEnumerable<SelectListItem> mySchoolClassSelectList = from mySC in db.schoolClasses
                                                                  where !userCurrentSchollclasses.Contains(mySC.schoolClassID)
                                                                  select new SelectListItem { Value = mySC.schoolClassID.ToString(), Text = mySC.name };

            ViewBag.SchoolClassSelectList = mySchoolClassSelectList;
            ViewBag.HiddenUserId = id;
            //ViewBag.HiddenRoleId = myUserRepo.get
            //END Data to partial view to add new schoolclass



            return View(myUserRepo.getUserDetailViewModel(id));
        }

        // GET: users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add()
        {
            return Content("Test Add function call");
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
                    myUserRepo.AddUser(user.UserName, user.UserName, user.UserPassword);
                    user.UserId = myUserRepo.GetUserIdByName(user.UserName);
                    myUserRepo.AddUserToRole(user.UserId, user.RoleId);
                }
                catch (Exception err)
                {
                    return Content("Mayor Error in create user:" + err.Message);
                }

                //Auto added by VS
                db.users.Add(user);
                db.SaveChanges();


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


        // GET: users/Create Roles Backend
        public ActionResult CreateRoles()
        {
            return View();
        }

        // POST: users/Create Roles Backend
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRolesPost()
        {
            if (dbUser.Roles.Count() <= 0) {

                myUserRepo.AddRole("Student");
                myUserRepo.AddRole("Teacher");
                return Content("Added Roles for Teacher and Student");
            }
            else
            {
                return Content("Roles do already exist in the database !!!!");
            }
            
        }


        // GET: users/Create
        public ActionResult AddUserSchoolClass()
        {
            //CODE NOT RUN BCAS it is filled as partial view in the Detial Action (actually a bug in MVC), this is only used if this form called seperate.
            IEnumerable<SelectListItem> mySchoolClassSelectList = from mySC in db.schoolClasses
                                                                  select new SelectListItem { Value = mySC.schoolClassID.ToString(), Text = mySC.name };

            ViewBag.SchoolClassSelectList = mySchoolClassSelectList;

            return View();
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
