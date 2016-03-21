using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSProject.Models;

namespace LMSProject.Controllers
{
    // This is Emelies temporary controller for calendars or something.

    public class CalendarViewModel
    {
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public string Label { get; set; }
        public int DayOfTheWeek { get; set; }
    };

    public class CalendarTestController : Controller
    {
        List<CalendarViewModel> d;
        public CalendarTestController()
        {
            schedule s = new schedule();
            Random ran = new Random();
            d = new List<CalendarViewModel>();
            for (int i = 0; i < 10; i++)
            {
                scheduleDetail detail = new scheduleDetail() { startTime = DateTime.Now, scheduleDetailID = i, schedules = s, scheduleID = 0, name = "Test", endTime = DateTime.Now };
                detail.startTime = detail.startTime.AddMinutes(ran.Next(480));
                detail.startTime = detail.startTime.AddDays(ran.Next(7));
                detail.endTime = detail.startTime.AddMinutes(30 + ran.Next(100));
                //detail.endTime = detail.startTime.AddHours(1);
                CalendarViewModel cvm = new CalendarViewModel() { Hours = detail.startTime.Hour, Label = detail.name, Minutes = (int) detail.startTime.Minute, DayOfTheWeek = (int) detail.startTime.DayOfWeek};

                d.Add(cvm);
            }
        }
        [Authorize]
        public JsonResult getSchedule()
        {
            return Json(d, JsonRequestBehavior.AllowGet);
        }
        // GET: CalendarTest
        //[Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            return View(d);
        }

        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public ActionResult CreateScheduleDetail(scheduleDetail model)
        {
            return RedirectToAction("Index");
        }


    }
}