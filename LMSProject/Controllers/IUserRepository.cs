using LMSProject.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LMSProject.Controllers
{
    public interface IUserRepository
    {
        IEnumerable<IdentityRole> GetAllRoles();
        IEnumerable<IdentityUserRole> GetUsersByRole(string role);
        bool IsUserInRole(string userId, string role);
        string GetRoleByUserName(string UserName);
        void AddUserToRole(string userId, string roleId);
        void AddRole(string role);
        void AddUser(ApplicationUser user);
        void AddUser(string userName, string email, string password);
        IdentityUser GetUserById(string Id);
        string GetUserIdByName(string name);
        string getRoleName(string RoleId);
        List<userViewModel> getUserViewModel();
        userViewModel getUserDetailViewModel(string p_UserId);
        List<int> getCurrentUserSchoolClasses(string p_userName);
        List<int> getCurrentUserSchoolClassesByID(string currentUserid);
    }
}