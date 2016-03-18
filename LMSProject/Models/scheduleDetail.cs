using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class scheduleDetail
	{
		[Key]
		public int scheduleDetailID { get; set; }

		[ForeignKey("scheduleID")]
		public virtual schedule schedules { get; set; }
		public int scheduleID { get; set; }

		[Required]
		public DateTime startTime { get; set; }

		[Required]
		public DateTime endTime { get; set; }

		[Required]
		public string name { get; set; }

		public string room { get; set; }

		[ForeignKey("taskID")]
		public virtual task tasks { get; set; }
		public int? taskID { get; set; }
	}
}