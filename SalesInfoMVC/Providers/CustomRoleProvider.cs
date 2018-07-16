using System;
using System.Linq;
using System.Web.Security;
using System.Web.Providers.Entities;
using System.Data.Entity;
using SalesInfoMVC.Models;

namespace SalesInfoMVC.Providers
{
    public class CustomRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            using (UsersContext db = new UsersContext())
            {
                // Получаем пользователя
#pragma warning disable GCop132 // Since the type is inferred, use 'var' instead
                UserProfile userProfile = db.UserProfiles.Include(u => u.Role).FirstOrDefault(u => u.UserName == username);
#pragma warning restore GCop132 // Since the type is inferred, use 'var' instead
#pragma warning disable GCop132 // Since the type is inferred, use 'var' instead
                string[] roles = new string[] { };
#pragma warning restore GCop132 // Since the type is inferred, use 'var' instead
                if (userProfile != null && userProfile.Role != null)
                {
                    // получаем роль
                    roles = new string[] { userProfile.Role.Name };
                }
                return roles;
            }
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            using (UsersContext db = new UsersContext())
            {
                // Получаем пользователя
#pragma warning disable GCop132 // Since the type is inferred, use 'var' instead
                UserProfile userProfile = db.UserProfiles.Include(u => u.Role).FirstOrDefault(u => u.UserName == username);
#pragma warning restore GCop132 // Since the type is inferred, use 'var' instead

                if (userProfile != null && userProfile.Role != null && userProfile.Role.Name == roleName)
                    return true;
                else
                    return false;
            }
        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            throw new NotImplementedException();
        }
    }
}