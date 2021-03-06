﻿using System;
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
	[Authorize(Roles = "Teacher")]
    public class schoolClassesController : Controller
    {
        private LMSContext db = new LMSContext();
        private IUserRepository userRepository = new TeacherStudentRepository();
        // GET: schoolClasses
        public ActionResult Index()
        {
            return View(db.schoolClasses.ToList());
        }

        public ActionResult _List()
        {
            //string TeacherUserId = userRepository.GetUserIdByName(User.Identity.Name);

            //var classes = from c in db.users
            //              where c.UserId == TeacherUserId
            //              select c.schoolClasses;

            return PartialView(db.schoolClasses.ToList());
        }
        // GET: schoolClasses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schoolClass schoolClass = db.schoolClasses.Find(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
            return View(schoolClass);
        }

        // GET: schoolClasses/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: schoolClasses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "schoolClassID,name")] schoolClass schoolClass)
        {
            if (ModelState.IsValid)
            {
                db.schoolClasses.Add(schoolClass);
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

            return View(schoolClass);
        }

        // GET: schoolClasses/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schoolClass schoolClass = db.schoolClasses.Find(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
            return View(schoolClass);
        }

        // POST: schoolClasses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "schoolClassID,name")] schoolClass schoolClass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schoolClass).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(schoolClass);
        }

        // GET: schoolClasses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schoolClass schoolClass = db.schoolClasses.Find(id);
            if (schoolClass == null)
            {
                return HttpNotFound();
            }
            return View(schoolClass);
        }

        // POST: schoolClasses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            schoolClass schoolClass = db.schoolClasses.Find(id);
            db.schoolClasses.Remove(schoolClass);
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
