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
    public class ManageScheduleDetailsViewModel
    {
        public IEnumerable<scheduleDetail> details;
        public int scheduleID;
    }

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

        [Authorize(Roles = "Student, Teacher")]
        public JsonResult getSchedule(int id)
        {
            List<CalendarViewModel> viewModels = new List<CalendarViewModel>();
            
            var details = from d in db.scheduleDetails
                          where d.scheduleID == id
                          select d;
            foreach(var d in details)
            {
                string task_string = "";
                if(d.tasks != null)
                {
                    task_string = d.tasks.name;
                }
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
                    Task = task_string
                });
            }
                return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Student, Teacher")]
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
                string task_string = "";
                if (d.tasks != null)
                {
                    task_string = d.tasks.name;
                }
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
                    Task = task_string
                });
            }
            return Json(viewModels, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            Session["nav"] = "backend";
            return View(db.schedules.ToList());
        }

        public ActionResult _List()
        {
            return PartialView(db.schedules.ToList());
        }

        // GET: CalendarTest
        public ActionResult ViewSchedule(int id)
        {
            return View(id);
        }

        [Authorize(Roles = "Student, Teacher")]
        [HttpGet]
        public ActionResult ViewStudentSchedule(string id)
        {
            return PartialView((object) id);
        }

        [HttpGet]
        public ActionResult ViewFullStudentSchedule(string id)
        {
            return View("ViewStudentSchedule",(object)id);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult CreateScheduleDetail(scheduleDetail detail)
        {
            int detailID = detail.scheduleDetailID;
            //ViewData["taskID"] = new List<SelectListItem> {
            //    new SelectListItem { Text = "9A", Value = "1"},
            //    new SelectListItem { Text = "9B", Value = "1"}
            //};
            //ViewData["scheduleID"] = new List<SelectListItem> {
            //    new SelectListItem { Text = "9A", Value = "5"},
            //    new SelectListItem { Text = "9B", Value = "5"}
            //};

            if (ModelState.IsValid)
            {
                //if (IsOverlappingWithAnother(detail,detailID))
                //{
                //    return Content("ErrorOverlapping");
                //}
                db.scheduleDetails.Add(detail);
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
            return View(detail);
        }

        // GET: schedules/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateSchedule()
        {
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name");
            return View();
        }

        // POST: schedules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateSchedule([Bind(Include = "scheduleID,schoolClassID")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.schedules.Add(schedule);
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

            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", schedule.schoolClassID);
            return View(schedule);
        }

        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult CreateScheduleDetail(int id)
        {
            scheduleDetail detail = new scheduleDetail() { startTime = DateTime.Today.AddHours(9), name = "", endTime = DateTime.Today.AddHours(12), scheduleID = id};
            ViewData["taskID"] = new SelectList(db.tasks.Where(p => p.schoolClassID == id).ToList(), "taskID", "name");
            ViewData["scheduleID"] = new SelectList(db.schedules, "scheduleID", "schoolClasses.name", detail.scheduleID);

            return View(detail);
        }

        // GET: Calendar/Delete/5
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteScheduleDetail(int? id)
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
        [HttpPost, ActionName("DeleteScheduleDetail")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteScheduleDetailConfirmed(int id)
        {
            scheduleDetail scheduleDetail = db.scheduleDetails.Find(id);
            db.scheduleDetails.Remove(scheduleDetail);
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

        // GET: Calendar/DeleteSchedule/5
        [Authorize(Roles = "Teacher")]
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
        [Authorize(Roles = "Teacher")]
        public ActionResult DeleteScheduleConfirmed(int id)
        {
            schedule schedule = db.schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }
            List<scheduleDetail> details = db.scheduleDetails.Where(p => p.scheduleID == id).ToList();
            foreach( var d in details )
            {
                db.scheduleDetails.Remove(d);
            }
            db.SaveChanges();

            db.schedules.Remove(schedule);

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
        private bool IsOverlappingWithAnother(scheduleDetail a, int detailID)
        {
            var details = from d in db.scheduleDetails
                          where d.scheduleID == detailID
                          select d;
            foreach (scheduleDetail b in details.ToList())
            {
                if ((a.startTime < b.endTime && b.startTime < a.endTime)
                    && a.scheduleDetailID != b.scheduleDetailID)
                {
                    return true;
                }
            }
            return false;
        }

        // GET: scheduleDetails/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult EditScheduleDetail(int? id)
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
            ViewData["scheduleDetailID"] = new SelectList(db.scheduleDetails.Where(p => p.scheduleDetailID == id).ToList(), "scheduleDetailID", "schoolClasses.name", scheduleDetail.scheduleDetailID);
            ViewData["scheduleID"] = new SelectList(db.schedules, "scheduleID", "schoolClasses.name", scheduleDetail.scheduleID);
            ViewData["taskID"] = new SelectList(db.tasks.Where(p => p.schoolClassID == scheduleDetail.schedules.schoolClassID).ToList(), "taskID", "name");
            ViewData["schoolClassID"] = new SelectList(db.schoolClasses, "schoolClassID", "name");

            return View(scheduleDetail);
        }

        // POST: scheduleDetails/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //public ActionResult EditScheduleDetail([Bind(Include = "scheduleID,starTime,endTime,name,room,taskID")] scheduleDetail scheduleDetail)

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult EditScheduleDetail(scheduleDetail scheduleDetail)
        {
            //int scheduleDetailID = scheduleDetail.scheduleDetailID;
            //if (IsOverlappingWithAnother(scheduleDetail, scheduleDetailID))
            //{
            //    return Content("Error: That scheduleDetail overlaps with another in the schedule.");
            //}

            if (ModelState.IsValid)
            {
                db.Entry(scheduleDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewData["scheduleDetailID"] = new SelectList(db.scheduleDetails.Where(p => p.scheduleDetailID == scheduleDetail.scheduleID).ToList(), "scheduleDetailID", "schoolClasses.name", scheduleDetail.scheduleDetailID);
            return View(scheduleDetail);
        }

        // GET: schedules/EditSchedule/5
        [Authorize(Roles = "Teacher")]
        public ActionResult EditSchedule(int? id)
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
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", schedule.schoolClassID);
            return View(schedule);
        }

        // POST: schedules/EditSchedule/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher")]
        public ActionResult EditSchedule([Bind(Include = "scheduleID,schoolClassID")] schedule schedule)
        {
            if (ModelState.IsValid)
            {
                db.Entry(schedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.schoolClassID = new SelectList(db.schoolClasses, "schoolClassID", "name", schedule.schoolClassID);
            return View(schedule);
        }


        [HttpGet]
        [Authorize(Roles = "Teacher")]
        public ActionResult ManageScheduleDetails(int id)
        {
            ManageScheduleDetailsViewModel viewModel = new ManageScheduleDetailsViewModel
            {
                details = db.scheduleDetails.Where(x => x.scheduleID == id).ToList(),
                scheduleID = id
            };
            return View(viewModel);
        }
    }
}