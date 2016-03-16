using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class folder
	{
		[Key]
		public int folderID { get; set; }

		public int folderTypeID { get; set; }

		[ForeignKey("schoolClass")]
		public virtual schoolClass schoolClassID { get;set;}

		public string name { get; set; }

		public string path { get; set; }
	}
}