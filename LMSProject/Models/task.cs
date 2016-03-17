using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class task
	{
		[Key]
		public int taskID { get; set; }

		[Required]
		public string name { get; set; }

		[ForeignKey("userID"), Required]
		public virtual user_teacher user_teachers { get; set; }
		public int userID { get; set; }

		[ForeignKey("schoolClassID"), Required]
		public virtual schoolClass schoolClasses { get; set; }
		public int schoolClassID { get; set; }
	}
}