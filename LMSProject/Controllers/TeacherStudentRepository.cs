using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LMSProject.Controllers
{
    public class TeacherStudentRepository : Controller
    {
        private ApplicationRoleManager roleManager;
        private ApplicationUserManager userManager;
        private ApplicationSignInManager signinManager;

        public TeacherStudentRepository()
        {
            roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            userManager = HttpContext.GetOwinContext().Get<ApplicationUserManager>();
            signinManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
        }

        public IEnumerable<IdentityRole> GetRoles()
        {
            return roleManager.Roles.ToList();
        }

        public ICollection<IdentityUserRole> GetUsersByRole(string role)
        {
            var users = from u in roleManager.Roles
                        where u.Name == role
                        select u.Users;
            //var users = roleManager.Roles.Select((p) => (p.Name == role));
            return users.FirstOrDefault();
        }

        public bool IsUserInRole(string id, string role)
        {
            if (userManager.GetRoles(id).Contains(role))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}