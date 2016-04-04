using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class UserViewModelFullView
	{
		public string UserId { get; set; }
		public string UserName { get; set; }

		public List<file> fileList { get; set; }
		public List<schoolClass> schoolClassList { get; set; }
		public List<folder> folderList { get; set; }
		public List<task> TaskList { get; set; }

		public int scheduleID { get; set; }
	}
}