using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mdrrp_prototype.Models;

namespace mdrrp_prototype.Controllers.Administrator
{
    [Authorize(Roles = "Administrator,Project Manager")]
    public class ComponentController : Controller
    { //define a database context object 

        private ApplicationDbContext _context;

        //define a constructor that initiates the database context to be used by methods on the project controller
        public ComponentController()
        {
            this._context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            this._context.Dispose();
        }

        // GET: Component
        public ActionResult Index(int id)
        {
            //get all components that belong to the project selected

            var project = this._context.Projects.Where(c => c.DeletedFlag == false).SingleOrDefault(p => p.Id == id);

            if (project == null)
            {
                return HttpNotFound();
            }

            var probjectComponents = this._context.ProjectComponents.Where(pc => pc.DeletedFlag == false && pc.ProjectId == id).ToList();

            ViewBag.ProjectName = project.Name;
            return View(probjectComponents);
        }
        [HttpGet]
        public ActionResult Create(int id)
        {
            // This method displays a form to create a new component on a project

            //fetch the fetch first the project from the database to make sure it does exist

            if (this._context.Projects.Any(p => p.Id == id))
            {
                return View();
            }
            else
            {
                //request might be bogus so return page not found

                return HttpNotFound();
            }


        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(ProjectComponent projectComponent)
        {
            //find the project id first before creating a component 

            if (this._context.Projects.Any(p => p.Id == projectComponent.ProjectId))
            {
                //check if the data sent is valid

                var project = this._context.Projects.SingleOrDefault(p => p.Id == projectComponent.ProjectId);

                //check if adding the component will overun budget

                bool isWithninBudget = this.isWithinBudget(project, projectComponent);

                //if the component exceeds the budget then throw an error

                if (isWithninBudget == false)
                {
                    ModelState.AddModelError("Budget", "Adding the new component with the budget specified will overun the funds for the project");

                    return View("Create", projectComponent);
                }

               
                if (ModelState.IsValid)
                {
                    //check if the projectId has a value greater than zero or not

                    if (projectComponent.Id == 0)
                    {
                        this._context.ProjectComponents.Add(projectComponent);
                        this._context.SaveChanges();

                        TempData["SuccessMessage"] = "New component created successfully";

                    }
                    else
                    {
                        //update the existing project with the details sent

                        //find the component having the Id sent and then update the details

                        var projectComponentDb =
                            this._context.ProjectComponents.SingleOrDefault(pc => pc.Id == projectComponent.Id);

                        if (projectComponentDb == null)
                        {
                            //its probably a bogus request so return page not found

                            return HttpNotFound();
                        }

                        //update details

                        projectComponentDb.Name = projectComponent.Name;
                        projectComponentDb.Budget = projectComponent.Budget;
                        projectComponentDb.Description = projectComponent.Description;
                        

                        //save the changes to the database


                        this._context.SaveChanges();

                        TempData["SuccessMessage"] = "New component updated successfully";


                    }



                    return RedirectToAction("Index", new {id = projectComponent.ProjectId });

                }
                else
                {
                    return View("Create", projectComponent);
                }


            }
            else
            {
                return HttpNotFound();
            }
        }

        public ActionResult Delete(int id)
        {
            //find the component to delete

            var component = this._context.ProjectComponents.SingleOrDefault(pc => pc.Id == id);

            if (component == null)
            {
                //return a page not found 

                return HttpNotFound();
            }

            //update the deleted flag to true

           

            component.DeletedFlag = true;

            //save the changes to the database

          
               

                try
                {
                    // Your code...
                    // Could also be before try if you know the exception occurs in SaveChanges

                    this._context.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }


            //set a success message to be displayed to the administrator

            TempData["SuccessMessage"] = "Component removed from the system";

            return RedirectToAction("Index", new{id = component.ProjectId});
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //find the component to delete

            var component = this._context.ProjectComponents.SingleOrDefault(pc => pc.Id == id);

            if (component == null)
            {
                //return a page not found 

                return HttpNotFound();
            }

            return View(component);
        }

        private bool isWithinBudget(Project project, ProjectComponent projectComponent)
        {
            //get the project budget

            var projectBudget = project.Budget;

            //get the total amount of money in each component

            decimal totalComponentBudget = 0;

            //loop through all the components adding each components budget 

            foreach (var component in project.ProjectComponents)
            {
                totalComponentBudget += component.Budget;
            }


            var newProposedTotal = totalComponentBudget + projectComponent.Budget;

            if (newProposedTotal > projectBudget)
            {
                return false;
            }
            else
            {
                return true;
            }


        }
    }
}