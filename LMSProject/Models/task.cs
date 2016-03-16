﻿using System;
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

		public string name { get; set; }

		[ForeignKey("user_teacherID")]
		public virtual user_teacher user_teachers { get; set; }
		public int user_teacherID { get; set; }
	}
}