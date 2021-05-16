using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mdrrp_prototype.Models;
using mdrrp_prototype.ViewModels;

namespace mdrrp_prototype.Controllers.Administrator
{
    [Authorize(Roles = "Administrator,Project Manager")]
    public class SubComponentController : Controller
    {

        private readonly ApplicationDbContext _context; 

        public SubComponentController()
        {
            this._context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            this._context.Dispose();
        }

        // GET: SubComponent
        public ActionResult Index(int id)
        {
            //check if the component id sent is available from the database

            var ProjeccomponentDb = this._context.ProjectComponents.SingleOrDefault(pc => pc.Id == id);

            //if the project baring the id wasn't found then throw a page not found exception
            if (ProjeccomponentDb == null)
            {
                return HttpNotFound();
            }

            var subcomponents = this._context.SubComponents.Where(sc => sc.ProjectComponentId == id && sc.DeletedFlag == false).ToList();

            //get the project Id that has the corresponding subcomponent

            ViewBag.ProjectId = ProjeccomponentDb.ProjectId;
            return View(subcomponents);
           
        }

        public ActionResult Create(int id)
        {
            var ProjeccomponentDb = this._context.ProjectComponents.SingleOrDefault(pc => pc.Id == id);

            //if the project baring the id wasn't found then throw a page not found exception
            if (ProjeccomponentDb == null)
            {
                return HttpNotFound();
            }


            return View();
        }

        public ActionResult Save(SubComponent subComponent)
        {
            //validate model before anything

            if(ModelState.IsValid)
            {
                //before adding the subcomponent search for if there is a component that is about to be linked to the subcomponent that is to be added
                var ProjeccomponentDb = this._context.ProjectComponents.SingleOrDefault(pc => pc.Id == subComponent.ProjectComponentId);


                //if the project baring the id wasn't found then throw a page not found exception
                if (ProjeccomponentDb == null)
                {
                    return HttpNotFound();
                }



                //check if adding the new subcomponent wont overun the budget

                bool isWithinBudgeet = this.IsWithinBudget(ProjeccomponentDb, subComponent);

                //return an error if the budget of the new sub component is too large
                if (!isWithinBudgeet)
                {
                    ModelState.AddModelError("Budget", "The budget specified for the subcomponent overruns the component budget");

                    return View("Create", subComponent);
                }
                //check if this a new request to create a new subcomponent or update an existing one

                if (subComponent.Id == 0)
                {

                    //check if there is a subcomponent with the same name as the new name given

                    if (this._context.SubComponents.Any(sbc => sbc.Name == subComponent.Name))
                    {
                        //A sub component with the same name already exists in the system

                        ModelState.AddModelError("Name", "A subcomponent with the name you typed already exist in the system");

                        return View("Create", subComponent);
                    }

                    //add a new subcomponent

                    this._context.SubComponents.Add(subComponent);

                    //save to the database

                    this._context.SaveChanges();

                    TempData["SuccessMessage"] = string.Format("Added new subcomponent to {0}", ProjeccomponentDb.Name);
                }
                else
                {
                    var subcomponentDb = this._context.SubComponents.SingleOrDefault(sc => sc.Id == subComponent.Id);

                    //check if the subcomponent exist

                    if (subcomponentDb == null)
                    {
                        //if subcomponent having the Id doesnt exist then return page not found

                        return HttpNotFound();
                    }

                    subcomponentDb.Name = subComponent.Name;
                    subcomponentDb.Budget = subComponent.Budget;
                    subcomponentDb.Description = subComponent.Description;

                    //save the updated details to the database

                    this._context.SaveChanges();

                    TempData["SuccessMessage"] = "Updated Subcomponent details successfully";


                }

                return RedirectToAction("Index", new { id = subComponent.ProjectComponentId });
            }else
            {
                return View("Create", subComponent);
            }
           



        }

        public ActionResult Delete(int id)
        {
            //find the subcommponent with the Id sent

            var subComponentDb = this._context.SubComponents.SingleOrDefault(sb => sb.Id == id);

            if (subComponentDb == null)
            {
                //if subcomponent wasnt found in the database then return page not found.

                return HttpNotFound();
            }

            //update the subcomponentDb status to deleted

            subComponentDb.DeletedFlag = true;

            this._context.SaveChanges();

            TempData["SuccessMessage"] = "Subcomponent removed";

            return RedirectToAction("Index", new { id = subComponentDb.ProjectComponentId });
        }

        private bool IsWithinBudget(ProjectComponent projectComponent, SubComponent subComponent)
        {
            //get the component budget

            var componentBudget = projectComponent.Budget;

            //get the total amount of money in each component

            decimal totalsubcomponentBudget = 0;

            //loop through all the subcomponents adding each components budget 

            foreach (var subcomponent in projectComponent.SubComponents)
            {
                totalsubcomponentBudget += subcomponent.Budget;
            }


            var newProposedTotal = totalsubcomponentBudget + subComponent.Budget;

            if (newProposedTotal > componentBudget)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public ActionResult Details(int id)
        {
            //find the subcomponent and eager load multilevel using string path

            var subComponent = this._context.SubComponents.Include("IntermediateIndicators.Results").Include("IntermediateIndicators.ProjectIndicator")
                .SingleOrDefault(sb1 => sb1.Id == id);

            if (subComponent == null)
            {
                return HttpNotFound();
            }

            return View(subComponent);
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            //find the subcomponent to be edited

            var subcomponent = this._context.SubComponents.SingleOrDefault(subc => subc.Id == id);

            if (subcomponent == null)
            {
                return HttpNotFound();
            }

            return View(subcomponent);
        }

        [HttpGet]
        public ActionResult ViewAssignedSubcomponents(string id)
        {

            //get a list projects assigned to a particular user

            var user = this._context.Users.Include(u1 => u1.SubComponents).SingleOrDefault(u2 => u2.Id == id);

            if (user == null)
            {
                //user not found

                return HttpNotFound();
            }


            ViewBag.User = user;

            return View(user);
        }

        [HttpGet]
        public ActionResult AssignSubcomponent(string id)
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
        [ValidateAntiForgeryToken]
        public ActionResult AssignSubcomponent(SubcomponentClerkViewModel subcomponentClerkViewModel)
        {
            //validate the data before saving

            if (ModelState.IsValid)
            {
                var subcomponent =
                    this._context.SubComponents.Include(user => user.ApplicationUsers).SingleOrDefault(
                        sb => sb.Id == subcomponentClerkViewModel.SubcomponentId);
                if (subcomponent == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    //get the user

                    var user = this._context.Users.SingleOrDefault(u => u.Id == subcomponentClerkViewModel.UserId);

                    //assign the user to the subcomponent
                    subcomponent.ApplicationUsers.Add(user);
                    this._context.SaveChanges();


                    TempData["SuccessMessage"] = "Data clerk assigned to subcomponent successfully";

                    
                }
            }
            else
            {
                //return to the previous page displaying validation errors
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

               


                return View("AssignSubcomponent", subcomponentClerkViewModel);
            }

            return RedirectToAction("ViewAssignedSubcomponents", new {id = subcomponentClerkViewModel.UserId});
        }

        public ActionResult RemoveAssignment(string userId, int subcomponentId)
        {
            //get the user

            var user = this._context.Users.SingleOrDefault(u => u.Id == userId);
            var subcomponent = this._context.SubComponents.Include(sbc => sbc.ApplicationUsers).FirstOrDefault(s => s.Id == subcomponentId);

            //remove the pair of a user to a particular subcomponent
            subcomponent.ApplicationUsers.Remove(user);

            //save the changes in the database
            this._context.SaveChanges();

            TempData["SuccessMessage"] = $"Successfully unassigned {user.LastName} to subcomponent";


            return RedirectToAction("UserRoles", "Manage");
        }

        [HttpGet]
        public ActionResult GetComponents(string id)
        {
            //find the project select

            int projectId = int.Parse(id);

            var project = this._context.Projects.Where(p1 => p1.DeletedFlag ==false).Include(p2 => p2.ProjectComponents).Single(p3 => p3.Id == projectId);

            var components = project.ProjectComponents.Where(pc => pc.DeletedFlag == false).ToList();

            ViewBag.ComponentOptions = new SelectList(components, "Id", "Name");

            return PartialView();

        }

        [HttpGet]
        public ActionResult GetSubComponents(int id)
        {
            //find the project select

            var component = this._context.ProjectComponents.Where(pc => pc.DeletedFlag == false).Include(s1 => s1.SubComponents).Single(s2 => s2.Id == id);

            var subcomponents = component.SubComponents.Where(sb =>sb.DeletedFlag == false).ToList();

            ViewBag.SubComponentOptions = new SelectList(subcomponents, "Id", "Name");

            return PartialView();

        }
    }
}