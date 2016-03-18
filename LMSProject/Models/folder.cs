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

		[ForeignKey("folderTypeID")]
		public virtual folderType folderTypes { get; set; }
		public int folderTypeID { get; set; }

		[ForeignKey("schoolClassID")]
		public virtual schoolClass schoolClasses { get; set; }
		public int schoolClassID { get; set; }

		[Required]
		public string name { get; set; }

		public string path { get; set; }
	}
}