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
        public ActionResult Create([Bind(Include = "folderID,folderTypeID,schoolClassID,name,path")] folder folder)
        {
            if (ModelState.IsValid)
            {
                db.folders.Add(folder);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.folderTypeID = new SelectList(db.folderTypes, "folderTypeID", "name", folder.folderTypeID);
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", folder.schoolClassID);
            return View(folder);
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
            db.folders.Remove(folder);
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
