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
	[Authorize]
    public class UserViewModelFullViewsController : Controller
    {
        private LMSContext db = new LMSContext();
		private ApplicationDbContext dbUser = new ApplicationDbContext();
		private TeacherStudentRepository myUserRepo = new TeacherStudentRepository();

        // GET: UserViewModelFullViews
        public ActionResult Index()
        {

			ViewBag.RoleName = myUserRepo.GetRoleByUserName(User.Identity.Name);
			ViewBag.UserName = User.Identity.Name;
			ViewBag.UserId = myUserRepo.GetUserIdByName(User.Identity.Name);

			

			List<int> userCurrentSchollclasses = myUserRepo.getCurrentUserSchoolClasses(User.Identity.Name);

			string userIDbyName = myUserRepo.GetUserIdByName(User.Identity.Name);
			IEnumerable<SelectListItem> myfileSelectList = from fil in db.files
														   where fil.userID == userIDbyName
														   select new SelectListItem { Value = fil.fileID.ToString(), Text = fil.fileID + " in " + fil.path + " for " + fil.tasks.name};
			//IEnumerable<SelectListItem> myfileSelectList = from fil in db.files
			//											   where fil.userID == myUserRepo.GetUserIdByName(User.Identity.Name)
			//											   select new SelectListItem { Value = fil.fileID.ToString(), Text = fil.fileID + " in " + fil.path + " for " + fil.tasks.name };							   
			ViewBag.fileList = myfileSelectList;
			
			
			IEnumerable<SelectListItem> mySchoolClassSelectList = from mySC in db.schoolClasses
																  where userCurrentSchollclasses.Contains(mySC.schoolClassID)
																  select new SelectListItem { Value = mySC.schoolClassID.ToString(), Text = mySC.name };
			ViewBag.schoolClassList = mySchoolClassSelectList;

			IEnumerable<SelectListItem> myFolderSelectableList = from myfolder in db.folders
																 where userCurrentSchollclasses.Contains(myfolder.schoolClassID)
																 select new SelectListItem { Value = myfolder.folderID.ToString(), Text = myfolder.name };
			ViewBag.folderList = myFolderSelectableList;

			
			IEnumerable<SelectListItem> mytaskSelectableList = from mytask in db.tasks
															   where userCurrentSchollclasses.Contains(mytask.schoolClassID)
															   select new SelectListItem { Value = mytask.taskID.ToString(), Text = mytask.name + " for " + mytask.schoolClasses.name };
			ViewBag.TaskList = mytaskSelectableList;

			return View();
        }

		//// GET: UserViewModelFullViews/Details/5
		//public ActionResult Details(string id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	UserViewModelFullView userViewModelFullView = db.UserViewModelFullViews.Find(id);
		//	if (userViewModelFullView == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(userViewModelFullView);
		//}

		//// GET: UserViewModelFullViews/Create
		//public ActionResult Create()
		//{
		//	return View();
		//}

		//// POST: UserViewModelFullViews/Create
		//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Create([Bind(Include = "UserId,UserName,scheduleID")] UserViewModelFullView userViewModelFullView)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		db.UserViewModelFullViews.Add(userViewModelFullView);
		//		db.SaveChanges();
		//		return RedirectToAction("Index");
		//	}

		//	return View(userViewModelFullView);
		//}

		//// GET: UserViewModelFullViews/Edit/5
		//public ActionResult Edit(string id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	UserViewModelFullView userViewModelFullView = db.UserViewModelFullViews.Find(id);
		//	if (userViewModelFullView == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(userViewModelFullView);
		//}

		//// POST: UserViewModelFullViews/Edit/5
		//// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		//// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public ActionResult Edit([Bind(Include = "UserId,UserName,scheduleID")] UserViewModelFullView userViewModelFullView)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		db.Entry(userViewModelFullView).State = EntityState.Modified;
		//		db.SaveChanges();
		//		return RedirectToAction("Index");
		//	}
		//	return View(userViewModelFullView);
		//}

		//// GET: UserViewModelFullViews/Delete/5
		//public ActionResult Delete(string id)
		//{
		//	if (id == null)
		//	{
		//		return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
		//	}
		//	UserViewModelFullView userViewModelFullView = db.UserViewModelFullViews.Find(id);
		//	if (userViewModelFullView == null)
		//	{
		//		return HttpNotFound();
		//	}
		//	return View(userViewModelFullView);
		//}

		//// POST: UserViewModelFullViews/Delete/5
		//[HttpPost, ActionName("Delete")]
		//[ValidateAntiForgeryToken]
		//public ActionResult DeleteConfirmed(string id)
		//{
		//	UserViewModelFullView userViewModelFullView = db.UserViewModelFullViews.Find(id);
		//	db.UserViewModelFullViews.Remove(userViewModelFullView);
		//	db.SaveChanges();
		//	return RedirectToAction("Index");
		//}

		//protected override void Dispose(bool disposing)
		//{
		//	if (disposing)
		//	{
		//		db.Dispose();
		//	}
		//	base.Dispose(disposing);
		//}
    }
}
