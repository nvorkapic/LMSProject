using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LMSProject.Models
{
	public class user
	{

		[Key]
		[Column(Order = 0)]
		public string UserId { get; set; }

		[Key]
		[Column(Order = 1)]
        [DisplayName("Schoolclass")]
		public int schoolClassID { get; set; }

		[ForeignKey("schoolClassID")]
		public virtual schoolClass schoolClasses { get; set; }

        [DisplayName("Role")]
		public string RoleId { get; set; }

        //[NotMapped]
        //public virtual ApplicationUser User { get; set; }
        //[NotMapped]
        //public virtual IdentityRole Role { get; set; }

        // to create new users
        [NotMapped]
        [DisplayName("Email")]
        public string UserName { get; set; }
        [NotMapped]
        [DisplayName("Password")]
        public string UserPassword { get; set; }
	}

    public class userViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string schoolClasses { get; set; }

    }


}