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
    public class folderTypesController : Controller
    {
        private LMSContext db = new LMSContext();

        // GET: folderTypes
        public ActionResult Index()
        {
            return View(db.folderTypes.ToList());
        }

        // GET: folderTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            folderType folderType = db.folderTypes.Find(id);
            if (folderType == null)
            {
                return HttpNotFound();
            }
            return View(folderType);
        }

        // GET: folderTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: folderTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "folderTypeID,name")] folderType folderType)
        {
            if (ModelState.IsValid)
            {
                db.folderTypes.Add(folderType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(folderType);
        }

        // GET: folderTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            folderType folderType = db.folderTypes.Find(id);
            if (folderType == null)
            {
                return HttpNotFound();
            }
            return View(folderType);
        }

        // POST: folderTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "folderTypeID,name")] folderType folderType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(folderType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(folderType);
        }

        // GET: folderTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            folderType folderType = db.folderTypes.Find(id);
            if (folderType == null)
            {
                return HttpNotFound();
            }
            return View(folderType);
        }

        // POST: folderTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            folderType folderType = db.folderTypes.Find(id);
            db.folderTypes.Remove(folderType);
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
