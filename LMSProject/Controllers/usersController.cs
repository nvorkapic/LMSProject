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
            Session["nav"] = "backend";
            return View(myUserRepo.getUserViewModel());
        }

        public ActionResult _List()
        {
            return PartialView(myUserRepo.getUserViewModel());
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


            //Data to partial view to add new schoolclass
            List<int> userCurrentSchollclasses = myUserRepo.getCurrentUserSchoolClassesByID(id);

            IEnumerable<SelectListItem> mySchoolClassSelectList = from mySC in db.schoolClasses
                                                                  where !userCurrentSchollclasses.Contains(mySC.schoolClassID)
                                                                  select new SelectListItem { Value = mySC.schoolClassID.ToString(), Text = mySC.name };

            ViewBag.SchoolClassSelectList = mySchoolClassSelectList;
            ViewBag.HiddenUserId = id;
            ViewBag.HiddenRoleId = myUserRepo.GetRoleIdByUserId(id);
            //END Data to partial view to add new schoolclass

            return View(myUserRepo.getUserDetailViewModel(id));
        }

        // Add schoolclass to user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSchoolClassToUser([Bind(Include = "schoolClassID")] user user, string HiddenUserId, string HiddenRoleId)
        {

            user.UserId = HiddenUserId;

            user.RoleId = HiddenRoleId;

            //Auto added by VS
            db.users.Add(user);
            db.SaveChanges();

            if (Session["nav"] != null)
            {
                if ((string)Session["nav"] == "frontend")
                {
                    return RedirectToAction("Index", "Teacher");
                }
                else
                {
                    return RedirectToAction("Details", new { id = user.UserId });
                }
            }
            else
            {
                return RedirectToAction("Details", new { id = user.UserId });
            }

        }


        // Delete schoolclass to user
        public ActionResult DeleteSchoolClassFromUser(string userId, int SchooClassId)
        {

            user myUserEnt = (from dbu in db.users
                              where dbu.UserId == userId && dbu.schoolClassID == SchooClassId
                              select dbu).FirstOrDefault();
                
            //Auto added by VS
            db.users.Remove(myUserEnt);
            db.SaveChanges();

            if (Session["nav"] != null)
            {
                if ((string)Session["nav"] == "frontend")
                {
                    return RedirectToAction("Index", "Teacher");
                }
                else
                {
                    return RedirectToAction("Details", new { id = userId });
                }
            }
            else
            {
                return RedirectToAction("Details", new { id = userId });
            }

            
        }

        // GET: users/Create
        public ActionResult Create()
        {
            ViewBag.RoleId = new SelectList(dbUser.Roles, "Id", "Name");
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name");
            return View();
        }

        // POST: users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,schoolClassID,RoleId,UserName,UserPassword")] user user)
        {

            if (ModelState.IsValid)
            {
                if ((String.IsNullOrEmpty(user.UserName))||(String.IsNullOrEmpty(user.UserPassword))){
                    return Content("Username & password is requeried !!!" + @"<p><a href=""#"" onclick=""history.back();"">Back</a></p>");
                }

                if (myUserRepo.CheckifUserExistByName(user.UserName))
                {
                    return Content("Username allready exist !!!" + @"<p><a href=""#"" onclick=""history.back();"">Back</a></p>");
                }

                try
                {
                    myUserRepo.AddUser(user.UserName, user.UserName, user.UserPassword);
                    user.UserId = myUserRepo.GetUserIdByName(user.UserName);
                    myUserRepo.AddUserToRole(user.UserId, user.RoleId);
                }
                catch (Exception err)
                {
                    return Content("Mayor Error in create user:" + err.Message + @"<p><a href=""#"" onclick=""history.back();"">Back</a></p>");
                }

                db.users.Add(user);
                db.SaveChanges();

                if (Session["nav"] != null)
                {
                    if ((string)Session["nav"] == "frontend")
                    {
                        return RedirectToAction("Index", "Teacher");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    return RedirectToAction("Index");
                }
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

        // GET: Delete User
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (myUserRepo.GetUsernameById(id) == "admin@root.app") // block delete of admin user
            {
                return Content("Can not remove root admin user !!!!" + @"<p><a href=""#"" onclick=""history.back();"">Back</a></p>");
            }

            List<user> MyUserConns = (from dbu in db.users
                                     where dbu.UserId == id
                                     select dbu).ToList();

            if (MyUserConns.Count > 0)
            {
                foreach (user myUserItem in MyUserConns)
                {
                    db.users.Remove(myUserItem);
                }

                db.SaveChanges();
            }


            myUserRepo.DeleteUser(id);

            if (Session["nav"] != null) {
                if ((string)Session["nav"] == "frontend")
                {
                    return RedirectToAction("Index","Teacher");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
 


        }


        // GET: users/Create Roles Backend
		[AllowAnonymous]
        public ActionResult CreateRoles()
        {
            return View();
        }

        // POST: users/Create Roles Backend
        [HttpPost]
        [ValidateAntiForgeryToken]
		[AllowAnonymous]
        public ActionResult CreateRolesPost()
        {
            if (dbUser.Roles.Count() <= 0) {

                myUserRepo.AddRole("Student");
                myUserRepo.AddRole("Teacher");

				myUserRepo.AddUser("admin@root.app", "admin@root.app", "manager");
				string myUserId = myUserRepo.GetUserIdByName("admin@root.app");
                myUserRepo.AddUserToRole(myUserId, myUserRepo.GetRoleIdByRoleName("Teacher"));

                //Add default and only folderTypes
                folderType myfolderTypePrivate = new folderType { folderTypeID = 1, name = "Private"};
                folderType myfolderTypePublic = new folderType { folderTypeID = 2, name = "Public" };
                db.folderTypes.Add(myfolderTypePrivate);
                db.folderTypes.Add(myfolderTypePublic);
                db.SaveChanges();

                return Content("Added Roles for Teacher and Student and root user" + @"<p><a href=""#"" onclick=""history.back();"">Back</a></p>");

            }
            else
            {
                return Content("Roles and data do already exist in the database !!!!" + @"<p><a href=""#"" onclick=""history.back();"">Back</a></p>");
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
