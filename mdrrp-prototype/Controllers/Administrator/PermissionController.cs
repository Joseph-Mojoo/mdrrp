using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mdrrp_prototype.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace mdrrp_prototype.Controllers.Administrator
{
    [Authorize(Roles = "Administrator")]
    public class PermissionController : Controller
    {
        private ApplicationDbContext _context; 

        public PermissionController()
        {
            this._context = new ApplicationDbContext();
        }
        // GET: Permission
        [HttpGet]
        public ActionResult Index()
        {

            //get all permisions from the system

            var userPermissions = this._context.Permissions.Where(p => p.DeletedFlag == false).ToList();


            return View(userPermissions);
        }
        [HttpGet]
        public ActionResult Create(string s)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Save(Permission permission)
        {
            if (ModelState.IsValid)
            {


                //check if it is a request to add a new permission or update an existing one


                if (permission.Id == 0)
                {
                    //check if the permision is not present in the database

                    if (this._context.Permissions.Any(p => p.Description == permission.Description.ToLower()))
                    {
                        ModelState.AddModelError("", "A permission with the description given is already present in the system");

                        return View("Create", permission);
                    }
                    //add a new permission

                    var newpermission = new Permission()
                    {
                        Id = permission.Id,
                        Description = permission.Description.ToLower()

                    };

                    this._context.Permissions.Add(newpermission);
                    this._context.SaveChanges();

                    TempData["SuccessMessage"] = "Permission created successfully";

                    return RedirectToAction("Index");
                }

                //find a permission with the Id given

                var permissionDb = this._context.Permissions.SingleOrDefault(p => p.Id == permission.Id);

                if (permissionDb == null)
                {
                    //Throw page not found error if permission wasnt found to update
                    return HttpNotFound();
                }

                permissionDb.Description = permission.Description.ToLower();

                this._context.SaveChanges();

                TempData["SuccessMessage"] = "Permission updated successfully";

                return RedirectToAction("Index");

            }

            return View("Create", permission);



        }

        public ActionResult Edit(int id)
        {
            //find the the permission 

            var permission = this._context.Permissions.SingleOrDefault(p => p.Id == id);

            if (permission == null)
            {
                //return page not found

                return HttpNotFound();
            }

            return View(permission);
        }

        [HttpGet]
        public ActionResult Assign()
        {

            var rolesdb = this._context.Roles.ToList();

            List<SelectListItem> rolesList = new List<SelectListItem>();

            //loop through the roles from the database to insert into the selectionListItem list

            foreach (var role in rolesdb)
            {
                rolesList.Add(new SelectListItem()
                {
                    Value = role.Id,
                    Text = role.Name
                });
            }

            ViewBag.RoleId = rolesList;


            //get all permissions from the database

            var permissionsDb = this._context.Permissions.Where(p => p.DeletedFlag == false).ToList();

            var permissionsList = new MultiSelectList(permissionsDb, "Id", "Description");

            ViewBag.Permissions = permissionsList;




            return View();
        }

        [HttpPost]
        public ActionResult Assign(RolePermission rolePermission)
        {

            if (ModelState.IsValid)
            {

                var role = this._context.Roles.FirstOrDefault(r => r.Id == rolePermission.RoleId);


                //loop through permissions 

                foreach (var singlepermission in rolePermission.Permissions)
                {
                    var singlePermission = int.Parse(singlepermission);

                    var permission = this._context.Permissions.SingleOrDefault(p => p.Id == singlePermission);

                    //this._context.Permissions.Include("Roles").FirstOrDefault(p => p.Id == singlePermission).Roles
                    //    .Add(newRole);

                    this._context.Roles.Include("Permissions").SingleOrDefault(r => r.Id == role.Id).Permissions.Add(permission);

                }

                try
                {
                    this._context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    //foreach (var eve in e.EntityValidationErrors)
                    //{
                    //    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                    //        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    //    foreach (var ve in eve.ValidationErrors)
                    //    {
                    //        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                    //            ve.PropertyName, ve.ErrorMessage);
                    //    }
                    //}

                    TempData["SuccessMessage"] = "Failed to associated role with permission";

                    return RedirectToAction("Index", "Role");
                    //throw;
                }



                return RedirectToAction("ViewPermissions", new {id = role.Id});

            }

            return View("Assign", rolePermission);


        }

        public ActionResult Delete(int id)
        {
            //find the permission to be deleted

            var permission = this._context.Permissions.SingleOrDefault(p => p.Id == id);

            if (permission == null)
            {
                return HttpNotFound();
            }

            permission.DeletedFlag = true;

            this._context.SaveChanges();

            TempData["SuccessMessage"] = "Permission removed from the system";

            return RedirectToAction("Index");


        }

        public ActionResult ViewPermissions(string id)
        {
            //get the role with the Id given

            var roles = _context.Roles.Include(r2 => r2.Permissions).SingleOrDefault(r3 => r3.Id == id);


            //return Content(roles.Permissions.First().Description);
            if (roles == null)
            {
                return HttpNotFound();

            }

            return View(roles);


        }

        public ActionResult DeleteAssignment(int id)
        {
            //get the permission 
            var permission = this._context.Permissions.Include(p => p.ApplicationRoles).SingleOrDefault(p => p.Id == id);

            if (permission == null)
            {
                return HttpNotFound();
            }

            var applicationRole = permission.ApplicationRoles.First();
            var permission1 = permission.ApplicationRoles.Remove(applicationRole);


            this._context.SaveChanges();

            TempData["SuccessMessage"] = "Permission removed from the user";

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}