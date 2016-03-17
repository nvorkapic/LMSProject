using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class folderType
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int folderTypeID { get; set; }

		[Required]
		public string name { get; set; }
	}
}