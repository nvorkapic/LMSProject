using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class user_teacher
	{
		[Key]
		[Column(Order = 0)]
		public int userID { get; set; }

		[Key]
		[Column(Order = 1)]
		[ForeignKey("schoolClassID")]
		public virtual schoolClass schoolClasses { get; set; }
		public int schoolClassID { get; set; }

		
		
		//public int schoolClassID { get; set; }
		
	}
}