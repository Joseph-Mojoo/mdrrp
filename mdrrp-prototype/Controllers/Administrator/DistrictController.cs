using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mdrrp_prototype.Models;

namespace mdrrp_prototype.Controllers.Administrator
{
    [Authorize(Roles = "Administrator,Project Manager")]
    public class DistrictController : Controller
    {

        //define a database context object 

        private ApplicationDbContext _context { get; set; }

        //define a constructor that initiates the database context to be used by methods on the project controller
        public DistrictController()
        {
            this._context = new ApplicationDbContext();
        }
        // GET: District
        public ActionResult Index()
        {
            //get all districts from the database

            var districts = _context.Districts.Where(d => d.DeletedFlag == false).ToList();


            return View(districts);
        }

        public ActionResult Create()
        {
            //This method returns a view that allows the administrator to create a district

            return View();
        }

        public ActionResult Save(District district)
        {
            //check the validity of the data sent

            if (ModelState.IsValid)
            {

                //check if the district name is already in the system

                if (this._context.Districts.Any(d => d.Name == district.Name && d.DeletedFlag == false))
                {
                    ModelState.AddModelError("Name","A district by this name is already present in the system");

                    return View("Create");
                }
                //check if this is a request to create a new district or an update


                if (district.Id == 0)
                {
                    //create a new district

                    this._context.Districts.Add(district);
                    this._context.SaveChanges();

                    TempData["SuccessMessage"] = "New district created in the system";
                }
                else
                {
                    //check if a district by the Id sent is indeed in the system

                    var districtDb = this._context.Districts.SingleOrDefault(d => d.Id == district.Id);

                    if (districtDb == null)
                    {
                        // The request might be bogus so return a page not found

                        return HttpNotFound();
                    }

                    //update the district details accordingly
                    districtDb.Name = district.Name;
                    
                    //save changes

                    this._context.SaveChanges();
                    


                    TempData["SuccessMessage"] = "District Name updated";

                   
                }
                return RedirectToAction("Index");

            }
            else
            {
                
                
                return View("Create", district);
            }
            
        }


        public ActionResult Delete(int id)
        {
            //search for the district by the Id given
            var district = this._context.Districts.SingleOrDefault(d => d.Id == id);

            if (district == null)
            {
                //if Id was not found then throw a page not found exception
                return HttpNotFound();
            }
            //update the district
            district.DeletedFlag = true;
            this._context.SaveChanges();

            TempData["SuccessMessage"] = string.Format("District {0} removed from the sysem", district.Name);

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            //search for the district by the Id given
            var district = this._context.Districts.SingleOrDefault(d => d.Id == id);

            if (district == null)
            {
                //if Id was not found then throw a page not found exception
                return HttpNotFound();
            }

            return View(district);
        }
    }
}