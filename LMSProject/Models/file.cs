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

		[ForeignKey("folder")]
		public virtual folder folderID {get; set;}

		public string name {get;set;}

		public string path {get;set;}

		[ForeignKey("task")]
		public virtual task taskID {get; set;}
	}
}