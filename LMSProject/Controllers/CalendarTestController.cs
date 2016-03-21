using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LMSProject.Models;

namespace LMSProject.Controllers
{
    // This is Emelies temporary controller for calendars or something.

    public class CalendarTestController : Controller
    {

        // GET: CalendarTest
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            schedule s = new schedule();
            Random ran = new Random();
            List<scheduleDetail> d = new List<scheduleDetail>();
            for (int i = 0; i < 10; i++)
            {
                scheduleDetail detail = new scheduleDetail() { startTime = DateTime.Now, scheduleDetailID = i, schedules = s, scheduleID = 0, name = "Test" };
                detail.startTime = detail.startTime.AddMinutes(ran.Next(480));
                detail.startTime = detail.startTime.AddDays(ran.Next(7));
                d.Add(detail);
            }
            //detail.startTime.AddHours(8.0f);
            //schoolClass myClass = new schoolClass();
            //s.schoolClasses = myClass;
            //detail.schedules = s;
            //List<scheduleDetail> sch = new List<scheduleDetail>();
            //sch.Add( = detail);
            //detail.startTime = detail.startTime.AddHours(3);
            //sch.Add(detail);
            //detail.startTime = detail.startTime.AddHours(1);
            //sch.Add(detail);
            //detail.startTime = detail.startTime.AddHours(2);
            //sch.Add(detail);

            return View(d);
        }
    }
}