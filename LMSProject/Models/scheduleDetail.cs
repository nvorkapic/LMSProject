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

		[ForeignKey("schedule")]
		public virtual schedule scheduleID { get; set; }

		public DateTime startTime { get; set; }

		public DateTime endTime { get; set; }

		public string name { get; set; }

		public string room { get; set; }

		[ForeignKey("task")]
		public virtual task taskID { get; set; }
	}
}