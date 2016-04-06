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
	[Authorize(Roles="Teacher")]
    public class foldersController : Controller
    {
        private LMSContext db = new LMSContext();

        // GET: folders
        public ActionResult Index()
        {
            var folders = db.folders.Include(f => f.folderTypes).Include(f => f.schoolClasses);
            return View(folders.ToList());
        }


        // GET: folders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            folder folder = db.folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // GET: folders/Create
        public ActionResult Create()
        {
            ViewBag.folderTypeID = new SelectList(db.folderTypes, "folderTypeID", "name");
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name");
            return View();
        }

        // POST: folders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "folderTypeID,schoolClassID, name")] folder folder)
        {

			//if (db.folders.Where(x => x.schoolClassID == folder.schoolClassID).Count() != 0) {
			//	return RedirectToAction("Index");
			//}

			int startPos = 1;
			int stopPos = 2;
	
			if (db.folders.Where(x => x.schoolClassID == folder.schoolClassID && x.folderTypeID == 1).Count() != 0)
			{
				startPos = 2;
			}

			if (db.folders.Where(x => x.schoolClassID == folder.schoolClassID && x.folderTypeID == 2).Count() != 0)
			{
				stopPos = 1;
			}

			for (int i = startPos; i <= stopPos; i++) //i loop through predefined and fixed FolderTypeID's
			{	
                var foldername = db.folderTypes.Where(x => x.folderTypeID == i).First().name;
                var schoolclassname = db.schoolClasses.Where(x => x.schoolClassID == folder.schoolClassID).First().name;

				var dirpath = Server.MapPath("~/Holders/" + foldername + "/" + schoolclassname + "/" + foldername + schoolclassname + "/");

				folder.path = "~/Holders/" + foldername + "/" + schoolclassname + "/" + foldername + schoolclassname + "/";
				folder.name = schoolclassname + foldername;
                folder.folderTypeID = i;

                Directory.CreateDirectory(dirpath);
      
                db.folders.Add(folder);
				db.SaveChanges();
			}

			
            
            return RedirectToAction("Index");
            
            //ViewBag.folderTypeID = new SelectList(db.folderTypes, "folderTypeID", "name", folder.folderTypeID);
            //ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", folder.schoolClassID);
            //return View(folder);
		
        }

        // GET: folders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            folder folder = db.folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            ViewBag.folderTypeID = new SelectList(db.folderTypes, "folderTypeID", "name", folder.folderTypeID);
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", folder.schoolClassID);
            return View(folder);
        }

        // POST: folders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "folderID,folderTypeID,schoolClassID,name,path")] folder folder)
        {
            if (ModelState.IsValid)
            {
                db.Entry(folder).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.folderTypeID = new SelectList(db.folderTypes, "folderTypeID", "name", folder.folderTypeID);
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", folder.schoolClassID);
            return View(folder);
        }

        // GET: folders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            folder folder = db.folders.Find(id);
            if (folder == null)
            {
                return HttpNotFound();
            }
            return View(folder);
        }

        // POST: folders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

			folder folder = db.folders.Find(id);
			try
			{
				Directory.Delete(Server.MapPath(folder.path));
				db.folders.Remove(folder);
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			catch (Exception err)
			{

				TempData["Success"] = true;
				ViewBag.ErrorMessage = err.Message;
				return RedirectToAction("Delete");
			}

		
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
