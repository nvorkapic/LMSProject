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
        private ApplicationDbContext dbUser = new ApplicationDbContext();
        private LMSContext db = new LMSContext();

        public TeacherStudentRepository()
        {
            var roleStore = new RoleStore<IdentityRole>(new ApplicationDbContext());
            roleManager = new ApplicationRoleManager(roleStore);

            myUserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        }

        //public IEnumerable<IdentityRole> GetAllRoles()
        //{
        //    IEnumerable<IdentityRole> results;
        //    results = roleManager.Roles.ToList();
        //    return results;
        //}

        //public IEnumerable<IdentityUserRole> GetUsersByRole(string role)
        //{
        //    var users = roleManager.Roles.Where(p => p.Name == role).FirstOrDefault();
        //    return users.Users;
        //}

        //public bool IsUserInRole(string userId, string role)
        //{
        //    if (myUserManager.GetRoles(userId).Contains(role))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}


        public string GetRoleByUserName(string UserName)
        {
            return myUserManager.GetRoles(this.GetUserIdByName(UserName)).FirstOrDefault().ToString(); 
        }

        public string GetRoleByUserId(string UserId)
        {
            return myUserManager.GetRoles(UserId).FirstOrDefault().ToString();
        }


        public string GetRoleIdByUserId(string UserId)
        {
            var roleObject = roleManager.FindByName(myUserManager.GetRoles(UserId).FirstOrDefault().ToString());
            return roleObject.Id;
        }

        public string GetRoleIdByRoleName(string RoleName)
        {
            var roleObject = roleManager.FindByName(RoleName);
            return roleObject.Id;
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

        public string GetUsernameById(string Id)
        {
            var results = myUserManager.Users.Where(p => p.Id == Id);
            return results.FirstOrDefault().UserName;
        }

        public string GetUserIdByName(string name) {
            string results;
            try
            {
                results = myUserManager.Users.Where(p => p.UserName == name).FirstOrDefault().Id;
            }
            catch(Exception e)
            {
                results = e.Message;
            }
            return results;
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return myUserManager.Users.ToList();
        }

        public bool CheckifUserExistByName(string userName)
        {
            List<ApplicationUser> results = myUserManager.Users.Where(p => p.UserName == userName).ToList();

            return (results.Count != 0);
        }


        public string getRoleName(string RoleId)
        {
            var roleObject = roleManager.FindById(RoleId);
            return roleObject.Name;
        }

        public List<userViewModel> getUserViewModel() { 
        
            List<userViewModel> userInfo = (from mUM in myUserManager.Users
                                            select (new userViewModel { 
                                                UserId = mUM.Id , 
                                                UserName = mUM.UserName, 
                                                RoleId = mUM.Roles.FirstOrDefault().RoleId
                                            })).ToList();

            
            foreach (var item in userInfo) {
                
                item.RoleName = this.getRoleName(item.RoleId);

                int maxClasses = 5;
                int maxClassesCount = 0;

                var usersSchoolClasses = from UsrSC in db.users
                                         where UsrSC.UserId == item.UserId
                                         select UsrSC.schoolClasses.name;


                foreach (var SCitem in usersSchoolClasses) {

                    if (String.IsNullOrEmpty(item.schoolClasses)){
                        item.schoolClasses += SCitem;
                    }
                    else
                    {
                        item.schoolClasses += "," + SCitem;
                    }

                    maxClassesCount++;
                    if (maxClassesCount > maxClasses) {
                        item.schoolClasses += " ...";
                        break;
                    }
                }

            }

            return userInfo;
        }

        public userViewModel getUserDetailViewModel(string p_UserId)
        {

            userViewModel userInfo = (from mUM in myUserManager.Users
                                            where mUM.Id == p_UserId
                                            select (new userViewModel
                                            {
                                                UserId = mUM.Id,
                                                UserName = mUM.UserName,
                                                RoleId = mUM.Roles.FirstOrDefault().RoleId
                                            })).FirstOrDefault();


            userInfo.RoleName = this.getRoleName(userInfo.RoleId);

            var usersSchoolClasses = from UsrSC in db.users
                                     where UsrSC.UserId == userInfo.UserId
                                     select UsrSC.schoolClasses.name;


            foreach (var SCitem in usersSchoolClasses)
            {
                if (String.IsNullOrEmpty(userInfo.schoolClasses))
                {
                    userInfo.schoolClasses += SCitem;
                }
                else
                {
                    userInfo.schoolClasses += "," + SCitem;
                }

            }

            return userInfo;
        }

        public List<int> getCurrentUserSchoolClasses(string p_userName)
        {
            string currentUserid = this.GetUserIdByName(p_userName);

            if (string.IsNullOrEmpty(currentUserid))
            {
                return null;
            }

            return (from cUSC in db.users
                          where cUSC.UserId == currentUserid
                          select cUSC.schoolClassID).ToList();

        }

        public List<int> getCurrentUserSchoolClassesByID(string currentUserid)
        {
            if (string.IsNullOrEmpty(currentUserid))
            {
                return null;
            }

            return (from cUSC in db.users
                    where cUSC.UserId == currentUserid
                    select cUSC.schoolClassID).ToList();

        }

        public void DeleteUser(string userId)
        {
            ApplicationUser _user = myUserManager.FindById(userId);
            myUserManager.Delete(_user);
        }

    }
}