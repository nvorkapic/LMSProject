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
using System.IO;

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

			IEnumerable<file> myfileSelectList = from fil in db.files
												 where fil.userID == userIDbyName && fil.folders.folderTypeID == 1
												select fil;

			//IEnumerable<SelectListItem> myfileSelectList = from fil in db.files
			//											   where fil.userID == userIDbyName
			//											   select new SelectListItem { Value = fil.fileID.ToString(), Text = fil.fileID + " in " + fil.path + " for " + fil.tasks.name };

			//(IEnumerable<file>)ViewBag.fileList   new SelectListItem { Value = fil.fileID.ToString(), Text = fil.fileID + " in " + fil.path + " for " + fil.tasks.name }
			   
			ViewBag.fileList = myfileSelectList;

			IEnumerable<file> myfileSharedList = from fil in db.files
												 where fil.userID == userIDbyName && fil.folders.folderTypeID == 2
												 select fil;
			ViewBag.fileShList = myfileSharedList;

			IEnumerable<file> mySharedList = from fil in db.files
												 where fil.userID != userIDbyName && fil.folders.folderTypeID == 2
												 select fil;
			ViewBag.SharedList = mySharedList;
			
			//Does not work, find solution
			IEnumerable<user> userList= from usi in db.users
								select usi;
			ViewBag.UserList = userList;

			IEnumerable<SelectListItem> mySchoolClassSelectList = from mySC in db.schoolClasses
																  where userCurrentSchollclasses.Contains(mySC.schoolClassID)
																  select new SelectListItem { Value = mySC.schoolClassID.ToString(), Text = mySC.name };
			ViewBag.schoolClassList = mySchoolClassSelectList;

			IEnumerable<folder> myFolderSelectableList = from myfolder in db.folders
														 where userCurrentSchollclasses.Contains(myfolder.schoolClassID)
														 select myfolder;
			ViewBag.folderList = myFolderSelectableList;

			
			IEnumerable<SelectListItem> mytaskSelectableList = from mytask in db.tasks
															   where userCurrentSchollclasses.Contains(mytask.schoolClassID)
															   select new SelectListItem { Value = mytask.taskID.ToString(), Text = mytask.name + " for " + mytask.schoolClasses.name };
			ViewBag.TaskList = mytaskSelectableList;

			return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult UploadShared([Bind(Include = "fileID,folderID,name,path,taskID,attachment")] file file)
		{
			if (file.attachment != null && file.attachment.ContentLength > 0)
			{
				try
				{
					var fileName = Path.GetFileName(file.attachment.FileName);
					var myFolderPath = db.folders.Where(x => x.folderID == file.folderID).First().path;

					var path = Path.Combine(Server.MapPath(myFolderPath), fileName);

					file.attachment.SaveAs(path);
					file.path = myFolderPath + file.attachment.FileName;
					file.userID = myUserRepo.GetUserIdByName(User.Identity.Name);
					db.files.Add(file);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				catch (Exception err)
				{
					return Content("File save error !!!!! " + err.Message);
				}
			}

			ViewBag.folderID = new SelectList(db.folders, "folderID", "name", file.folderID);
			ViewBag.taskID = new SelectList(db.tasks, "taskID", "name", file.taskID);
			return View(file);
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
