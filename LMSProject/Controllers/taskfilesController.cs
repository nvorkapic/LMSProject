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
    public class taskfilesController : Controller
    {
        private LMSContext db = new LMSContext();

        // GET: taskfiles
        public ActionResult Index()
        {
            var files = db.files.Include(f => f.folders).Include(f => f.tasks);
            return View(files.ToList());
        }

        // GET: taskfiles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            file file = db.files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // GET: taskfiles/Create
        public ActionResult Create()
        {
            ViewBag.folderID = new SelectList(db.folders, "folderID", "name");
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "name");
            return View();
        }

        // POST: taskfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "fileID,folderID,name,path,taskID,attachment")] file file)
        {
			if (file.attachment != null && file.attachment.ContentLength > 0)
			{
				try
				{
					var fileName = Path.GetFileName(file.attachment.FileName);
					var myFolderPath = db.folders.Where(x => x.folderID == file.folderID).First().path;				   
					var path = Path.Combine(Server.MapPath(myFolderPath), fileName);
					file.attachment.SaveAs(path);
					file.path = myFolderPath;
					db.files.Add(file);
					db.SaveChanges();
					return RedirectToAction("Index");
				}
				catch (Exception err) { 
					return Content("File save error !!!!! " + err.Message);
				}
			}

            ViewBag.folderID = new SelectList(db.folders, "folderID", "name", file.folderID);
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "name", file.taskID);
            return View(file);
        }


        // GET: taskfiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            file file = db.files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            ViewBag.folderID = new SelectList(db.folders, "folderID", "name", file.folderID);
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "name", file.taskID);
            return View(file);
        }

        // POST: taskfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "fileID,folderID,name,path,taskID")] file file)
        {
            if (ModelState.IsValid)
            {
                db.Entry(file).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.folderID = new SelectList(db.folders, "folderID", "name", file.folderID);
            ViewBag.taskID = new SelectList(db.tasks, "taskID", "name", file.taskID);
            return View(file);
        }

        // GET: taskfiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            file file = db.files.Find(id);
            if (file == null)
            {
                return HttpNotFound();
            }
            return View(file);
        }

        // POST: taskfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            file file = db.files.Find(id);
            db.files.Remove(file);
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
