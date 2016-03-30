using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

		public virtual ICollection<user> Users {get;set;}
		
		public schoolClass()
		{
			Users = new List<user>();
		}
	}
}