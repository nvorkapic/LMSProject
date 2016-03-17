﻿using System;
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
		public int user_teacherID { get; set; }

		[Required]
		public string userId { get; set; }
		//public int schoolClassID { get; set; }

		//[Column(Order = 1)]
		[ForeignKey("schoolClassID"), Required]
		public virtual schoolClass schoolClasses { get; set; }
		public int schoolClassID { get; set; }
		
	}
}