using mdrrp_prototype.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace mdrrp_prototype.Controllers.Administrator
{
    [Authorize(Roles = "Administrator")]
    public class RoleController : Controller
    {
        private ApplicationDbContext _context { get; set; }

        public RoleController()
        {
            this._context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            this._context.Dispose();
        }
        // GET: Role
        public ActionResult Index()
        {


            var roles = this._context.Roles.ToList();

            ViewBag.Roles = roles;

            return View();
        }
        public ViewResult Create()
        {
            return View();
        }

        public async Task<ActionResult> CreateAsync(Role role)
        {

            if(ModelState.IsValid)
            {
                var roleStore = new RoleStore<ApplicationRole>(new ApplicationDbContext());
                var roleManager = new RoleManager<ApplicationRole>(roleStore);

                var result = await roleManager.CreateAsync(new ApplicationRole(role.Name));

                return RedirectToAction("Index");

            }

            return View("Create", role);
            

            
        }
        [HttpGet]
        public async Task<ActionResult> EditUser(string Id)
        {
            var roleStore = new RoleStore<ApplicationRole>(new ApplicationDbContext());
            var roleManager = new RoleManager<ApplicationRole>(roleStore);
            var role = await roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                return HttpNotFound();
            }
            var model = new Role
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);
        }


    }
}