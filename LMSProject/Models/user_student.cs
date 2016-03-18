using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class user_student
	{
		[Key]
		public string userId { get; set; }
		
		[ForeignKey("schoolClassID")]
		public virtual schoolClass schoolClasses { get; set; }
		public int schoolClassID { get; set; }
	}
}