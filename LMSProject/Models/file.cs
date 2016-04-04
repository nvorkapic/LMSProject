using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class file
	{
		[Key]
		public int fileID { get; set; }

		[ForeignKey("folderID")]
		public virtual folder folders {get; set;}
		public int folderID { get; set; }

		[Required]
		public string name {get;set;}

		[Required]
		public string path {get;set;}

		[ForeignKey("taskID")]
		public virtual task tasks { get; set; }
		public int? taskID { get; set; }

		[Required]
		public string userID { get; set; }

		[NotMapped]
		public HttpPostedFileBase attachment { get; set; }
	}
}