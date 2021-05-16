using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using mdrrp_prototype.CustomAuthorize;
using mdrrp_prototype.Models;
using Microsoft.AspNet.Identity;


namespace mdrrp_prototype.Controllers.Administrator
{
    [Authorize(Roles = "Administrator,Project Manager")]
    public class ProjectController : Controller
    {
        //define a database context object 

        private ApplicationDbContext _context { get; set; }

        //define a constructor that initiates the database context to be used by methods on the project controller
        public ProjectController()
        {
            this._context = new ApplicationDbContext();
        }
        // GET: Project
        public static System.Globalization.CultureInfo CurrentCulture { get; set; }
        public ActionResult Index()
        {
           

            IEnumerable<Project> projects = null;
            //get the user type of the person logged in

            if (User.IsInRole("Administrator"))
            {
                 projects = this._context.Projects.Include(p => p.ProjectComponents).Where(p => p.DeletedFlag == false).ToList();
            } else if(User.IsInRole("Project Manager"))
            {
                var userId = User.Identity.GetUserId();

                var user = this._context.Users.Include(u1 => u1.Projects.Select(p => p.ProjectComponents)).First(u => u.Id == userId);

                //get only the projects that are assigned to the manager

                projects = user.Projects.ToList();

            }
            //get all projects from the database related to a particular user

            

            return View(projects);
        }

        public ActionResult Create()
        {
            //This method returns a view tha allows the administrator to create a new project
            return View();
        }

        [HttpPost]
        public ActionResult Save(Project project)
        {
            //This method creates a new project in the database or updates the details of an existing one

          
            if (ModelState.IsValid)
            {
                //check if it is a new project or not


                //check if  the project start date is less than the close date

                DateTime approvalDate = project.ApprovalDate;

                DateTime closeDate = project.ClosingDate;

                TimeSpan result = closeDate - approvalDate;

                if (result.Days <= 0)
                {
                    ModelState.AddModelError("ClosingDate", "The project closing date can not be less than or equal to approval date");

                    return View("Create", project);
                }


                if (project.Id == 0)
                {
                    //check if the supplied name is already available in the database

                    if (this._context.Projects.Any(p => p.Name == project.Name))
                    {
                        ModelState.AddModelError("Name", "Project Name entered already exists");

                        return View("Create", project);
                    }

                    //create a new project


                    this._context.Projects.Add(project);

                    //save changes to the database

                    this._context.SaveChanges();

                    TempData["SuccessMessage"] = $"Project {project.Name} has been created";

                    return RedirectToAction("Index");
                }
                else
                {
                    //get the project having the ID submitted from the database

                    var projectDb = this._context.Projects.SingleOrDefault(p => p.Id == project.Id);

                    //find the projecct wasn't found in the database then the request must be bogus so return page not found
                    if (projectDb == null)
                    {
                        return HttpNotFound();
                    }
                    
                    //update the detail of the project

                    projectDb.ApprovalDate = project.ApprovalDate;
                    projectDb.BankIFCCollaboration = project.BankIFCCollaboration;
                    projectDb.Budget = project.Budget;
                    projectDb.ClosingDate = project.ClosingDate;
                    projectDb.IsRegionallyTagged = project.IsRegionallyTagged;
                    projectDb.Name = project.Name;
                    projectDb.Code = project.Code;
                    projectDb.Objective = project.Objective;


                    //save the changes to the database

                    this._context.SaveChanges();

                    //set a success message to be displayed back to the user

                    TempData["SuccessMessage"] = $"Project {project.Name} has been updated";

                    return RedirectToAction("Index");
                }
            }

            return View("Create", project);

        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            //find the project that is required to be edited

            var projectDb = this._context.Projects.SingleOrDefault(p => p.Id == id);

            //check if the project exists in the database

            if (projectDb == null)
            {
                //The request is probably bogus so throw a page not found response

                return HttpNotFound();
            }

            return View(projectDb);
        }

        [HttpGet]
        [PermissionAuthorize("can delete project")]
        public ActionResult Delete(int id)
        {
            //find the project that is required to be edited

            var projectDb = this._context.Projects.SingleOrDefault(p => p.Id == id);

            //check if the project exists in the database

            if (projectDb == null)
            {
                //The request is probably bogus so throw a page not found response

                return HttpNotFound();
            }

            //mark the project as deleted by changing the deleted flag from false to true

            projectDb.DeletedFlag = true;

            //save changed to the database

            this._context.SaveChanges();

            TempData["SuccessMessage"] = $"Project {projectDb.Name} has been removed successfully";

            return RedirectToAction("Index");

        }


        public ActionResult Details(int id)
        {
            //get the project having the id sent

            var project = this._context.Projects.Include(p => p.ProjectIndicators).SingleOrDefault(p => p.Id == id);

            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        public ActionResult ViewAssignedProjects(string id)
        {
            //get a list projects assigned to a particular user

            var user = this._context.Users.Include(u1 => u1.Projects).SingleOrDefault(u2 => u2.Id == id);

            if (user == null)
            {
                //user not found

                return HttpNotFound();
            }
            

            ViewBag.User = user;
          

            return View();
        }

        [HttpGet]
        public ActionResult AssignManager(string id)
        {
            //get the list of available projects

            var projects = this._context.Projects.Where(p => p.DeletedFlag == false).ToList();


            //create a list of project select items to be displayed on a drop down

            ICollection<SelectListItem> projectSelectListItems = new List<SelectListItem>();

            //loop through the list of projects to add them to the selection list items

            foreach (var project in projects)
            {
                projectSelectListItems.Add(new SelectListItem()
                {
                    Value = project.Id.ToString(),
                    Text = project.Code

                });
            }

            ViewBag.ProjectId = projectSelectListItems;
            return View();
        }

        [HttpPost]
        public ActionResult AssignManager(ProjectToManagerAssignmentModel projectToManagerAssignment)
        {
            //check for validation

           

            if (ModelState.IsValid)
            {

               
                var project = this._context.Projects.Include(p1 => p1.User).SingleOrDefault(p => p.Id == projectToManagerAssignment.ProjectId);

                if (project == null)
                {
                    //drop the request

                    return HttpNotFound();
                }

                //get the user

                var user = this._context.Users.FirstOrDefault(u =>
                    u.Id == projectToManagerAssignment.UserId.ToString());


                //assign the project to the user

                project.User.Add(user);

                TempData["SuccessMessage"] = "Project Assigned to the Manager successfully";
                this._context.SaveChanges();

                return RedirectToAction("ViewAssignedProjects", new {id = user.Id});
            }
            //get the list of available projects

            var projects = this._context.Projects.Where(p => p.DeletedFlag == false).ToList();


            //create a list of project select items to be displayed on a drop down

            ICollection<SelectListItem> projectSelectListItems = new List<SelectListItem>();

            //loop through the list of projects to add them to the selection list items

            foreach (var project in projects)
            {
                projectSelectListItems.Add(new SelectListItem()
                {
                    Value = project.Id.ToString(),
                    Text = project.Code

                });
            }

            ViewBag.ProjectId = projectSelectListItems;

            return View("AssignManager", projectToManagerAssignment);

        }

        public ActionResult RemoveAssignment(int id)
        {
            //get the project submitted 

            var project = this._context.Projects.Include(p1 => p1.User).SingleOrDefault(p => p.Id == id);

            if (project == null)
            {
                //drop the request

                return HttpNotFound();
            }

            //get user tha is mapped to that project

            var user = project.User.First();

            //remove the project that is mapped to a user

            project.User.Remove(user);


            this._context.SaveChanges();

            TempData["SuccessMessage"] = "Project Manager unassigned to a project successfully";

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}