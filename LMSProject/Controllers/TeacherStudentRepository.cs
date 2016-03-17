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
    public class TeacherStudentRepository : Controller, IUserRepository
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

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            IEnumerable<IdentityRole> results;
            results = roleManager.Roles.ToList();
            return results;
        }

        public IEnumerable<IdentityUserRole> GetUsersByRole(string role)
        {
            var users = roleManager.Roles.Where(p => p.Name == role).FirstOrDefault();
            return users.Users;
        }

        public bool IsUserInRole(string userId, string role)
        {
            if (userManager.GetRoles(userId).Contains(role))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public IEnumerable<string> GetRolesOfUser(string userId)
        {
            var results = userManager.GetRoles(userId);
            return results;
        }

        public void AddRoleToUser(string userId, string role)
        {
            IdentityRole _role = roleManager.FindByName(role);
            IdentityUser _user = userManager.FindById(userId);

            IdentityUserRole _userRole = new IdentityUserRole()
            {
                RoleId = _role.Id,
                UserId = _user.Id
            };

            _role.Users.Add(_userRole);
            //var results = from u in userManager.Users
            //              where u.Id == userId
            //              select u.Roles.Add(userRole.);
            //throw new NotImplementedException();
        }
    }
}