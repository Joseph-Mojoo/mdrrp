using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mdrrp_prototype.Models;

namespace mdrrp_prototype.Controllers.Administrator
{
    [Authorize(Roles = "Administrator,Project Manager,Project Manager")]
    public class ProjectIndicatorController : Controller
    {
        //define a database context object 

        private ApplicationDbContext _context { get; set; }

        //define a constructor that initiates the database context to be used by methods on the project controller
        public ProjectIndicatorController()
        {
            this._context = new ApplicationDbContext();
        }

      
        [HttpGet]
        public ActionResult Create(int id)
        {
            //return a form that allows the administrator to add indicators

            //create a list of unit types

            List<SelectListItem> data = new List<SelectListItem>();

            List<string> units = new List<string>()
            {
                "Number","Percentage", "Metric ton","Hectare(Ha)","Yes/No"
            };

            foreach (var unit in units)
            {
                data.Add(new SelectListItem()
                {
                    Value = unit,
                    Text = unit,
                });
            }

            ViewBag.Unit = data;


            return View();

        }

        public ActionResult Save(ProjectIndicator projectIndicator)
        {
            if (ModelState.IsValid)
            {
                //check if the indicator is already available

                if (this._context.ProjectIndicators.Any(i => i.Name == projectIndicator.Name ))
                {
                    ModelState.AddModelError("Name", "The indicator by this name is already present in the system");

                    //return View("Create", projectIndicator);
                }

              

                //check if the request to save is for a new entry or edit

                if (projectIndicator.Id == 0)
                {
                    //add a new indicator

                    this._context.ProjectIndicators.Add(projectIndicator);
                    this._context.SaveChanges();

                    TempData["SuccessMessage"] = string.Format("New indicator {0} created successfully", projectIndicator.Name);

                    //return RedirectToAction("Index");
                }

                //get the indicator having the id sent

                var indicatorDb = this._context.ProjectIndicators.Single(i => i.Id == projectIndicator.Id);

                if (indicatorDb == null)
                {
                    return HttpNotFound();
                }

                indicatorDb.Name = projectIndicator.Name;
                indicatorDb.BaseLine = projectIndicator.BaseLine;
                indicatorDb.Target = projectIndicator.Target;
                indicatorDb.Unit = projectIndicator.Unit;


                //save the changes

                this._context.SaveChanges();

                TempData["SuccessMessage"] = string.Format("Indicator {0} has been updated", projectIndicator.Name);

                return RedirectToAction("Details","Project", new {id = projectIndicator.ProjectId});


            }

            //create a list of unit types

            List<SelectListItem> data = new List<SelectListItem>();

            List<string> units = new List<string>()
            {
                "Number","Percentage", "Metric ton","Hectare(Ha)","Yes/No"
            };

            foreach (var unit in units)
            {
                data.Add(new SelectListItem()
                {
                    Value = unit,
                    Text = unit,
                });
            }

            ViewBag.Unit = data;
            return View("Create", projectIndicator);


        }

        public ActionResult Delete(int id)
        {
            //find the project Indicator with the id sent

            var projectIndicator = this._context.ProjectIndicators.SingleOrDefault(pi => pi.Id == id);

            if (projectIndicator == null)
            {
                return HttpNotFound();
            }


            projectIndicator.DeletedFlag = true;
            this._context.SaveChanges();

            TempData["SUccessMessage"] = string.Format("Project Indicator {0} deleted", projectIndicator.Name);

            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}