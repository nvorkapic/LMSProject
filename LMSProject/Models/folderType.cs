using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class folderType
	{
		[Key]
		public int folderTypeID { get; set; }

		public string name { get; set; }
	}
}