using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class schoolClass
	{
		[Key]
		public int schoolClassID { get; set; }

		[Required]
		public string name { get; set; }
	}
}