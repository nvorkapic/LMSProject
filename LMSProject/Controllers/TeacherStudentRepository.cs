//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Mvc;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.AspNet.Identity;
//using Microsoft.Owin.Security;
//using Microsoft.AspNet.Identity.EntityFramework;
//using LMSProject.Models;

using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using LMSProject.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using LMSProject.DataAccess;

namespace LMSProject.Controllers
{
    public class TeacherStudentRepository : Controller, IUserRepository
    {

        private ApplicationRoleManager roleManager;
        private UserManager<ApplicationUser> myUserManager;

        public TeacherStudentRepository()
        {
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            roleManager = new ApplicationRoleManager(roleStore);

            myUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
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
            if (myUserManager.GetRoles(userId).Contains(role))
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
            var results = myUserManager.GetRoles(userId);
            return results;
        }

        public void AddUserToRole(string userId, string roleId)
        {
            IdentityRole _role = roleManager.FindById(roleId);
            //IdentityUser _user = myUserManager.FindById(userId);
            //IdentityUserRole _userRole = new IdentityUserRole()
            //{
            //    RoleId = _role.Id,
            //    UserId = _user.Id
            //};

            myUserManager.AddToRole(userId, _role.Name);
        }

        public void AddRole(string role)
        {
            IdentityRole identity = new IdentityRole(role);
            roleManager.Create(identity);
        }

        public void AddUser(ApplicationUser user)
        {
            myUserManager.Create(user);
        }

        public void AddUser(string userName, string email, string password)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = userName,
                Email = email
            };
            IdentityResult UserResult;

            UserResult = myUserManager.Create(user, password);
           
        }

        public IdentityUser GetUserById(string Id)
        {
            var results = myUserManager.Users.Where(p => p.Id == Id);
            return results.FirstOrDefault();
        }

        public string GetUserIdByName(string name) {

            var results = myUserManager.Users.Where(p => p.UserName == name).FirstOrDefault().Id;
            return results;
        }

    }
}