using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mdrrp_prototype.Models;

namespace mdrrp_prototype.Controllers.Administrator
{
    [Authorize(Roles = "Administrator,Project Manager")]
    public class IntemediateIndicatorController : Controller
    {
        private  ApplicationDbContext _context;

        public IntemediateIndicatorController()
        {
            this._context = new ApplicationDbContext();
        }
        

        public ActionResult Create(int id)
        {
            //get a list of project Indicators 

            var projectIndicators = this._context.ProjectIndicators.Where(p => p.DeletedFlag == false).ToList();

           List<SelectListItem> list = new List<SelectListItem>();

            //add project indicators to the list

            

            foreach (var indicator in projectIndicators)
            {
                list.Add( new SelectListItem()
                {
                    Value = indicator.Id.ToString(),
                    Text = indicator.Name
                });
            }

            ViewBag.ProjectIndicatorId = list;


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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(IntermediateIndicator intermediateIndicator)
        {
            //do validation
            if (ModelState.IsValid)
            {
                //check if the is an indicator with the name provided in the form in database

                if (this._context.IntermediateIndicators.Any(i => i.Name == intermediateIndicator.Name))
                {
                    //Its a duplicate so flag an error

                    ModelState.AddModelError("Name","An Indicator with the name you provided already exist in the system");

                    //get a list of project Indicators 

                    var projectIndicators = this._context.ProjectIndicators.ToList();

                    List<SelectListItem> list = new List<SelectListItem>();

                    //add project indicators to the list



                    foreach (var indicator in projectIndicators)
                    {
                        list.Add(new SelectListItem()
                        {
                            Value = indicator.Id.ToString(),
                            Text = indicator.Name
                        });
                    }

                    ViewBag.ProjectIndicatorId = list;

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


                    return View("Create", intermediateIndicator);
                }

                //validate the base target

                //get the target of the object indicator

                var projectIndicator = this._context.ProjectIndicators.SingleOrDefault(pi => pi.Id == intermediateIndicator.ProjectIndicatorId);


                //check if the intermediate target doesnt supersede the project indicator target

                var projectIndicatorTarget = projectIndicator.Target;

                var projectIntermediates = projectIndicator.IntermediateIndicators;


               

                bool isWithinTarget = this.IsWithinTarget(projectIndicator, intermediateIndicator);

               
                if (!isWithinTarget)
                {
                    ModelState.AddModelError("Target", "The Target entered superseded the project indicator target");

                    //get a list of project Indicators 

                    var projectIndicators = this._context.ProjectIndicators.ToList();

                    List<SelectListItem> list = new List<SelectListItem>();

                    //add project indicators to the list



                    foreach (var indicator in projectIndicators)
                    {
                        list.Add(new SelectListItem()
                        {
                            Value = indicator.Id.ToString(),
                            Text = indicator.Name
                        });
                    }

                    ViewBag.ProjectIndicatorId = list;

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
                    return View("Create", intermediateIndicator);
                }

                //check if this is a new request or an update

                if (intermediateIndicator.Id == 0)
                {
                    this._context.IntermediateIndicators.Add(intermediateIndicator);

                    this._context.SaveChanges();

                    TempData["SuccessMessage"] = "Added a new intermediate indicator";

                    return RedirectToAction("Details", "SubComponent", new {id = intermediateIndicator.SubComponentId});
                }

                //update the existing intermediate indicator

                //get the intermediate indicator from the database

                var intermediateIndicatorDb =
                    this._context.IntermediateIndicators.SingleOrDefault(i => i.Id == intermediateIndicator.Id);

                if (intermediateIndicatorDb != null)
                {
                    intermediateIndicatorDb.Name = intermediateIndicator.Name;
                    intermediateIndicatorDb.ProjectIndicatorId = intermediateIndicator.ProjectIndicatorId;
                    intermediateIndicatorDb.Target = intermediateIndicator.Target;
                    intermediateIndicatorDb.BaseLine = intermediateIndicator.BaseLine;
                    intermediateIndicatorDb.DataSourceMethodology = intermediateIndicator.DataSourceMethodology;
                    intermediateIndicatorDb.DataCollector = intermediateIndicator.DataCollector;
                    intermediateIndicatorDb.Frequency = intermediateIndicator.Frequency;
                    intermediateIndicatorDb.Unit = intermediateIndicator.Unit;
                }
                else
                {
                    return HttpNotFound();
                }

                this._context.SaveChanges();

                TempData["SuccessMessage"] = string.Format("Update details of InterMediate indicator successfully");

               


            }

            return RedirectToAction("Details", "SubComponent", new {id = intermediateIndicator.SubComponentId});
        }

        private bool IsWithinTarget(ProjectIndicator projectIndicator, IntermediateIndicator intermediateIndicator)
        {
            var projectIndicatorTarget = projectIndicator.Target;

            var projectIntermediates = projectIndicator.IntermediateIndicators;

            decimal totalItermediateTargets = 0;

            //loop through intermediate

            foreach (var intermediate in projectIntermediates)
            {
                totalItermediateTargets += intermediate.Target;
            }

            totalItermediateTargets += intermediateIndicator.Target;

            if (totalItermediateTargets > projectIndicatorTarget)
            {
                return false;
            }

            return true;



        }

        public ActionResult Delete(int id)
        {
            //find the intermediate indicator

            var interMediateIndicator = this._context.IntermediateIndicators.SingleOrDefault(i => i.Id == id);

            if (interMediateIndicator == null)
            {
                return HttpNotFound();
            }

            interMediateIndicator.DeletedFlag = true;

            this._context.SaveChanges();

            TempData["SuccessMessage"] = "Indicator removed from the system";

            return Redirect(Request.UrlReferrer.ToString());
        }

       




    }
}