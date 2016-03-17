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
        IEnumerable<string> GetRolesOfUser(string userId);
        void AddRoleToUser(string userId, string role);
        void AddRole(string role);
        void AddUser(IdentityUser user);
        void AddUser(string userName, string email, string password);
        IdentityUser GetUserById(string Id);
    }
}