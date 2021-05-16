using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mdrrp_prototype.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Serialization;

namespace mdrrp_prototype.CustomAuthorize
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class PermissionAuthorize : AuthorizeAttribute
    {
        private string Permission;
        private ApplicationDbContext _context;

        public PermissionAuthorize(string permission)
        {
            this.Permission = permission;
            this._context = new ApplicationDbContext();
        }

       

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            bool isAuthorized = base.AuthorizeCore(httpContext);

            if (!isAuthorized)
            {
                return false;
            }

            //get the current logged in user

            var user_id = HttpContext.Current.User.Identity.GetUserId();

            var user = this._context.Users.Include(u1 => u1.Roles).First(u => u.Id == user_id);

            if (user == null)
            {
                return false;
            }

            //get roles of the user

            var userRole = user.Roles.First();

            var applicationRole = this._context.Roles.Include(role => role.Permissions)
                .SingleOrDefault(r => r.Id == userRole.RoleId);

            var permissions = applicationRole.Permissions.ToList();


            if (permissions.Any(p => p.Description == this.Permission))
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