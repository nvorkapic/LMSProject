using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSProject.Models;
using LMSProject.DataAccess;
using System.Net;
using System.Data.Entity;

namespace LMSProject.Controllers
{
    public class CalendarViewModel
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string Label { get; set; }
        public int DayOfTheWeek { get; set; }
        public double DurationInMinutes { get; set; }
        public string TimeDisplayStart { get; set;  }
        public string TimeDisplayEnd { get; set;  }
        public string Room { get; set; }
        public string Task { get; set; }
    };

    public class CalendarController : Controller
    {
        private LMSContext db = new LMSContext();
        [Authorize]
        public JsonResult getSchedule(int id)
        {
            List<CalendarViewModel> viewModels = new List<CalendarViewModel>();
            
            var details = from d in db.scheduleDetails
                          where d.scheduleID == id
                          select d;
            foreach(var d in details)
            {
                viewModels.Add(new CalendarViewModel
                {
                    DayOfTheWeek = (int)d.startTime.DayOfWeek,
                    Hours = d.startTime.Hour,
                    Minutes = d.startTime.Minute,
                    Label = d.name,
                    DurationInMinutes = (d.endTime - d.startTime).TotalMinutes,
                    TimeDisplayStart = d.startTime.ToShortTimeString(),
                    TimeDisplayEnd = d.endTime.ToShortTimeString(),
                    Room = d.room,
					Task = d.tasks.name ?? ""
                });
            }
                return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getScheduleForStudent(string userId)
        {
            List<CalendarViewModel> viewModels = new List<CalendarViewModel>();

            var details = from u in db.users
                            from s in db.schedules
                            from d in db.scheduleDetails
                            where u.UserId == userId &&
                            s.schoolClassID == u.schoolClassID && 
                            d.scheduleID == s.scheduleID
                            select d;
            foreach (var d in details)
            {
                viewModels.Add(new CalendarViewModel
                {
                    DayOfTheWeek = (int)d.startTime.DayOfWeek,
                    Hours = d.startTime.Hour,
                    Minutes = d.startTime.Minute,
                    Label = d.name,
                    DurationInMinutes = (d.endTime - d.startTime).TotalMinutes,
                    TimeDisplayStart = d.startTime.ToShortTimeString(),
                    TimeDisplayEnd = d.endTime.ToShortTimeString(),
                    Room = d.room,
					Task = d.tasks.name ?? ""
                });
            }
            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            
            return View(db.schedules.ToList());
        }
        // GET: CalendarTest
        public ActionResult ViewSchedule(int id)
        {
            return View(id);
        }

        [HttpGet]
        public ActionResult ViewStudentSchedule(string id)
        {
            return View((object) id);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult CreateScheduleDetail(scheduleDetail detail)
        {
            ViewData["taskID"] = new List<SelectListItem> {
                new SelectListItem { Text = "9A", Value = "1"},
                new SelectListItem { Text = "9B", Value = "1"}
            };
            ViewData["scheduleID"] = new List<SelectListItem> {
                new SelectListItem { Text = "9A", Value = "5"},
                new SelectListItem { Text = "9B", Value = "5"}
            };

            if (ModelState.IsValid)
            {
                if (IsOverlappingWithAnother(detail))
                {
                    return RedirectToAction("ErrorOverlapping");
                }
                db.scheduleDetails.Add(detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(detail);
        }

        // GET: schedules/Create
        public ActionResult Create()
        {
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name");
            return View();
        }

        // POST: schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "scheduleID,schoolClassID")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.schedules.Add(schedule);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", schedule.schoolClassID);
            return View(schedule);
        }

        [HttpGet]
        public ActionResult CreateScheduleDetail(int id)
        {
            scheduleDetail detail = new scheduleDetail() { startTime = DateTime.Today.AddHours(9), name = "Test", endTime = DateTime.Today.AddHours(12) };
            ViewData["taskID"] = new SelectList(db.tasks.Where(p => p.schoolClassID == id).ToList(), "taskID", "name");
            ViewData["scheduleID"] = new SelectList(db.schedules, "scheduleID", "schoolClasses.name");

            return View(detail);
        }

        // GET: Calendar/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scheduleDetail scheduleDetail = db.scheduleDetails.Find(id);
            if (scheduleDetail == null)
            {
                return HttpNotFound();
            }
            return View(scheduleDetail);
        }

        // POST: Calendar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            scheduleDetail scheduleDetail = db.scheduleDetails.Find(id);
            db.scheduleDetails.Remove(scheduleDetail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Calendar/DeleteSchedule/5
        public ActionResult DeleteSchedule(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            schedule schedule = db.schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            return View(schedule);
        }

        // POST: Calendar/DeleteSchedule/5
        [HttpPost, ActionName("DeleteSchedule")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteScheduleConfirmed(int id)
        {
            schedule schedule = db.schedules.Find(id);
            db.schedules.Remove(schedule);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: scheduleDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            scheduleDetail scheduleDetail = db.scheduleDetails.Find(id);
            if (scheduleDetail == null)
            {
                return HttpNotFound();
            }
            //ViewData["scheduleDetailID"] = new SelectList(db.scheduleDetails.Where(p => p.scheduleDetailID == id).ToList(), "scheduleDetailID", "schoolClasses.name", scheduleDetail.scheduleDetailID);
            ViewData["scheduleID"] = new SelectList(db.schedules, "scheduleID", "schoolClasses.name");
            ViewData["taskID"] = new SelectList(db.tasks.Where(p => p.schoolClassID == scheduleDetail.schedules.schoolClassID).ToList(), "taskID", "name");
            ViewData["schoolClassID"] = new SelectList(db.schoolClasses, "schoolClassID", "name");
            return View(scheduleDetail);
        }

        private bool IsOverlappingWithAnother(scheduleDetail a)
        {
            var details = from d in db.scheduleDetails
                          where d.scheduleID == a.scheduleID
                          select d;
            foreach(scheduleDetail b in details.ToList())
            {
                if (b.endTime > a.startTime && b.startTime < a.endTime)
                {
                    return true;
                }
            }
            return false;
        }
        // POST: scheduleDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "scheduleID,starTime,endTime,name,room")] scheduleDetail scheduleDetail)
        {
            if (ModelState.IsValid)
            {
                if(IsOverlappingWithAnother(scheduleDetail))
                {
                    return Content("ErrorOverlapping");
                }
                db.Entry(scheduleDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["scheduleDetailID"] = new SelectList(db.scheduleDetails.Where(p => p.scheduleDetailID == scheduleDetail.scheduleID).ToList(), "scheduleDetailID", "schoolClasses.name", scheduleDetail.scheduleDetailID);
            return View(scheduleDetail);
        }

        [HttpGet]
        public ActionResult ManageScheduleDetails(int id)
        {
            return View(db.scheduleDetails.Where(x => x.scheduleID == id).ToList());
        }

        public ActionResult ErrorOverlapping()
        {
            return View();
        }
    }
}