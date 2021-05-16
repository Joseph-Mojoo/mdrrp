using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mdrrp_prototype.Models;
using Microsoft.AspNet.Identity;


namespace mdrrp_prototype.Controllers.DataEntry
{
    public class ResultController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultController()
        {
            this._context =  new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
           this._context.Dispose();
        }

        // GET: Result
        public ActionResult Index()
        {
            //get the instance of the user currently logged in

            var user_id = User.Identity.GetUserId();


            var user = this._context.Users.Include(u1 => u1.SubComponents).SingleOrDefault(u => u.Id == user_id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var subComponents = user.SubComponents.ToList();

            List<int?> subcomponentIds = new List<int?>();

            subcomponentIds.AddRange(subComponents.Select(s => (int?)s.Id));


            var intermediateIndicators = this._context.IntermediateIndicators.Include(i => i.SubComponent).Where(i1 => subcomponentIds.Contains(i1.SubComponentId)).ToList();

            List<int> intermidiateIndicatorIds = new List<int>();

            intermidiateIndicatorIds.AddRange(intermediateIndicators.Select(i => i.Id));





            //get the all the results in the system

            var results = this._context.Results.Include(re => re.IntermediateIndicator.SubComponent.ProjectComponent).Where(r => r.DeletedFlag == false && intermidiateIndicatorIds.Contains(r.IntermediateIndicatorId)).ToList();






            return View(results);
        }
        [HttpGet]
        public ActionResult Create()
        {
            var user_id = User.Identity.GetUserId();


            var user = this._context.Users.Include(u1 => u1.SubComponents).SingleOrDefault(u => u.Id == user_id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var subComponents = user.SubComponents.ToList();

            ViewBag.SubcomponentId = new SelectList(subComponents, "Id","Name");


            var result = new Result();

            result.Date = DateTime.Today;
            result.Value = 0;



            //var intermediateIndicators = this._context.IntermediateIndicators.Include(i => i.SubComponent).Where(i1 => subcomponentIds.Contains(i1.SubComponentId)).ToList(); //create a SelectListItem of Intermediate indicators

            //List<SelectListItem> indicators = new List<SelectListItem>();

            //foreach (var indicator in intermediateIndicators)
            //{
            //    indicators.Add(new SelectListItem()
            //    {
            //        Value = indicator.Id.ToString(),
            //        Text = indicator.Name + "=>" + indicator.SubComponent.Name
            //    });
            //}

            //ViewBag.IntermediateIndicatorId = indicators;

            return View(result);
        }
      

        public ActionResult _GetIntermediateIndicators(int id)
        {
            // This action receives a subcomponent Id and get all the intermediate indicators for thea subcomponent

            var intermediateIndicators = this._context.IntermediateIndicators.Where(indicator => indicator.SubComponentId == id && indicator.DeletedFlag == false).ToList();

            //create a select list 

            ViewBag.IntermediateIndicatorSelectList = new SelectList(intermediateIndicators, "Id", "Name");
            return PartialView();
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Result result)
        {
            if (ModelState.IsValid)
            {
                //check if it is a new request or not

                if (result.Id == 0)
                {

                    //change the date to today

                    result.Date = DateTime.Now;
                    //add new result

                    this._context.Results.Add(result);
                    this._context.SaveChanges();

                    TempData["SuccessMessage"] = "Results recorded successfully";

                    return RedirectToAction("Index");
                }

                //once results have been entered, they cant be altered

                TempData["SuccessMessage"] = "You can not alter results once entered";

                
            }
            var user_id = User.Identity.GetUserId();


            var user = this._context.Users.Include(u1 => u1.SubComponents).SingleOrDefault(u => u.Id == user_id);

            if (user == null)
            {
                return HttpNotFound();
            }

            var subComponents = user.SubComponents.ToList();

            ViewBag.SubcomponentId = new SelectList(subComponents, "Id", "Name");

            result.Date = DateTime.Now;
            result.Value = 0;

            return View("Create", result);
        }
    }
}
