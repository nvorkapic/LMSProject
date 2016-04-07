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
    public class tasksController : Controller
    {
        private LMSContext db = new LMSContext();

        // GET: tasks
        public ActionResult Index()
        {
            var tasks = db.tasks.Include(t => t.folders).Include(t => t.schoolClasses);
            Session["nav"] = "backend";
            return View(tasks.ToList());
        }

        public ActionResult _List()
        {
            var tasks = db.tasks.Include(t => t.folders).Include(t => t.schoolClasses);
            return PartialView(tasks.ToList());
        }
        // GET: tasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // GET: tasks/Create
        public ActionResult Create()
        {
            ViewBag.folderID = new SelectList(db.folders, "folderID", "name");
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name");
            return View();
        }

        // POST: tasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "taskID,name,schoolClassID,folderID")] task task)
        {
            if (ModelState.IsValid)
            {
                db.tasks.Add(task);
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
            }

            ViewBag.folderID = new SelectList(db.folders, "folderID", "name", task.folderID);
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", task.schoolClassID);
            return View(task);
        }

        // GET: tasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            ViewBag.folderID = new SelectList(db.folders, "folderID", "name", task.folderID);
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", task.schoolClassID);
            return View(task);
        }

        // POST: tasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "taskID,name,schoolClassID,folderID")] task task)
        {
            if (ModelState.IsValid)
            {
                db.Entry(task).State = EntityState.Modified;
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
            ViewBag.folderID = new SelectList(db.folders, "folderID", "name", task.folderID);
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", task.schoolClassID);
            return View(task);
        }

        // GET: tasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            task task = db.tasks.Find(id);
            if (task == null)
            {
                return HttpNotFound();
            }
            return View(task);
        }

        // POST: tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            task task = db.tasks.Find(id);
            db.tasks.Remove(task);
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
