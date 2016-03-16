﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class schedule
	{
		[Key]
		public int scheduleID { get; set; }

		[ForeignKey("schoolClass")]
		public virtual schoolClass schoolClassID { get; set; }
	}
}