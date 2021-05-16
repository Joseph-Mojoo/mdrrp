using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using mdrrp_prototype.Models;

namespace mdrrp_prototype.Controllers
{   [Authorize(Roles = "Administrator,Project Manager")]
    public class ReportController : Controller
    {
        private  ApplicationDbContext _context { get; set; }

        public ReportController()
        {
            this._context = new ApplicationDbContext();
        }
        // GET: Report
        public ActionResult Index()
        {
            //This method displays a form that allows the user to select parameters of a report to be generated


            return View();
        }
        [HttpGet]
        public ActionResult Create()
        {
            //get all projects

            var projects = this._context.Projects.Where(p => p.DeletedFlag == false).ToList();

         
            SelectList projectSelectList = new SelectList(projects,"Id","Code","Please Select Project");

            IEnumerable<SelectListItem> durationList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "First Quarter",
                    Value = "1",
                },
                new SelectListItem()
                {
                    Text = "Second Quarter",
                    Value = "2",
                },

                new SelectListItem()
                {
                    Text = "Third Quarter",
                    Value = "3",
                },

                new SelectListItem()
                {
                    Text = "Fourth Quarter",
                    Value = "4",
                },

                new SelectListItem()
                {
                    Text = "All",
                    Value = "5",
                },

            };

            ViewBag.Duration = durationList;

            ViewBag.ProjectId = projectSelectList;
            return View();
        }

        public List<ProjectReportData> GetProjectReportData(ReportForm report)
        {
           
                //get the project for the report to be generated 

                var project = this._context.Projects.Include(p => p.ProjectComponents).Include(p1 => p1.ProjectIndicators).Single(p2 => p2.Id == report.ProjectId);

               

                var projectIndicators = project.ProjectIndicators.ToList();

                int? reportYear;

                reportYear = report.Year == 0 ? null : report.Year;

                List<ProjectReportData> reportData = new List<ProjectReportData>();

                //loop through the indicators to record data for each indicator 

                int id = 1;

                foreach (var indicator in projectIndicators)
                {
                    //loop through project indicators extracting their intermediate indicators
                    var intermediateIndicators = this._context.IntermediateIndicators
                        .Where(i => i.ProjectIndicatorId == indicator.Id).Include(i2 => i2.Results).ToList();

                    var sumOfIntermediateIndicatorCollectedData = 0;

                    //get the sum of the intermediate indicators 
                    foreach (var intermediateIndicator in intermediateIndicators)
                    {
                        sumOfIntermediateIndicatorCollectedData +=
                            this.SumOfIntermediateIndicatorCollectedData(intermediateIndicator, reportYear, int.Parse(report.Duration));
                    }

                    reportData.Add(new ProjectReportData()
                    {
                        Id = indicator.Id,
                        BaseLine = indicator.BaseLine,
                        Target = indicator.Target,
                        IndicatorName = indicator.Name,
                        Remaining = (indicator.Target - sumOfIntermediateIndicatorCollectedData),
                        IntermediateIndicators = intermediateIndicators,
                        ProjectName = project.Name.ToUpper()+ " PROJECT REPORT"
                    });


                }

                


                return reportData;




        }

        public ActionResult DownloadReport( string reportType, string year, string duration, string projectId)
        {

            

            var report = new ReportForm()
            {
                Id = 0,
                Duration = duration,
                Year = int.Parse(year),
                ProjectId = int.Parse(projectId)

            };
            var reportData = this.GetProjectReportData(report);


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports/ProjectCrystalReport.rpt")));
            rd.SetDataSource(reportData);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            if (reportType == "csv")
            {
                //Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.ExcelWorkbook);
                //stream.Seek(0, SeekOrigin.Begin);



                //return File(stream, "Application/xlsx", "ProjectReport.xlsx");

                //get the project name 

                var projectname = this._context.Projects.FirstOrDefault(p => p.Id == report.ProjectId).Name;

                StringBuilder builder = new StringBuilder();

                builder.AppendLine("Project Level Indicator Name,Target,BaseLine Name,Remaining");


                foreach (var item in reportData)
                {
                    builder.AppendLine($"{item.IndicatorName},{item.Target},{item.BaseLine},{item.Remaining}");


                }
                return File(Encoding.UTF8.GetBytes(builder.ToString()), "text/csv", $"{projectname} Project Report.csv");

            }
            else
            {
                Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
                stream.Seek(0, SeekOrigin.Begin);



                return File(stream, "Application/pdf", "ProjectReport.pdf");
            }


           

        }
        [HttpPost]
        public ActionResult DisplayReport(ReportForm report)
        {
            //This method displays a form to the administrator or manager 

            if (ModelState.IsValid)
            {
               //get the project for the report to be generated 


               var reportData = this.GetProjectReportData(report);
               var project = this._context.Projects.SingleOrDefault(p => p.Id == report.ProjectId);

               if (project == null)
               {
                   return HttpNotFound();
               }

               ViewBag.ProjectName = project.Name;

               ViewBag.ReportData = report;

               return View(reportData);





            }
            else
            {
                var projects = this._context.Projects.Where(p => p.DeletedFlag == false).ToList();


                SelectList projectSelectList = new SelectList(projects, "Id", "Code", "Please Select Project");

                IEnumerable<SelectListItem> durationList = new List<SelectListItem>()
                {
                    new SelectListItem()
                    {
                        Text = "First Quarter",
                        Value = "1",
                    },
                    new SelectListItem()
                    {
                        Text = "Second Quarter",
                        Value = "2",
                    },

                    new SelectListItem()
                    {
                        Text = "Third Quarter",
                        Value = "3",
                    },

                    new SelectListItem()
                    {
                        Text = "Fourth Quarter",
                        Value = "4",
                    },

                    new SelectListItem()
                    {
                        Text = "All",
                        Value = "5",
                    },

                };

                ViewBag.Duration = durationList;

                ViewBag.ProjectId = projectSelectList;
                return View("Create", report);
            }

           
        }

        private int SumOfIntermediateIndicatorCollectedData(IntermediateIndicator intermediate, int? year, int quarter)
        {

            List<decimal> collectedData;

            IQueryable<Result> IqueryableData;

            if (year != null)
            {
                IqueryableData = this._context.Results.Where(r => r.IntermediateIndicatorId == intermediate.Id)
                    .Where(r1 => r1.Date.Year == year);

            }
            else
            {
                IqueryableData = this._context.Results.Where(r => r.IntermediateIndicatorId == intermediate.Id);

            }

            //get the data based on the quarter selected
            IqueryableData = this.GetYearQuarters(IqueryableData, quarter);
            

            collectedData = IqueryableData.Select(i => i.Value).ToList();

            var sumOfCollectedData = (int)collectedData.Sum();

            return sumOfCollectedData;

        }

        private IQueryable<Result> GetYearQuarters(IQueryable<Result> IqueryableData, int quarter)
        {
            switch (quarter)
            {
                case 1:

                   return IqueryableData = IqueryableData.Where(r => r.Date.Month >= 1 && r.Date.Month <= 3);
                    

                case 2:

                   return IqueryableData = IqueryableData.Where(r => r.Date.Month >= 4 && r.Date.Month <= 6);
                  

                case 3:

                   return IqueryableData = IqueryableData.Where(r => r.Date.Month >= 7 && r.Date.Month <= 9);
                   

                case 4:

                   return IqueryableData = IqueryableData.Where(r => r.Date.Month >= 10 && r.Date.Month <= 12);
                

                default: return IqueryableData;

            }
        }
        [HttpGet]
        public ActionResult GetProjectRunningYears(int? id = null )
        {
            Project project = null;

            project = id == null ? this._context.Projects.First() : this._context.Projects.SingleOrDefault(p => p.Id == id);


            //get the number of years a project runs for 

            var closingDate = project.ClosingDate;

            var startDate = project.ApprovalDate;


            var dateDiff = (int)((closingDate - startDate).TotalDays / 365);

            List<string> yearsList = new List<string>();

            //loop through date difference to add years

            for (int i = 0; i <= dateDiff; i++)
            {
                var year = startDate.Year + i;
                yearsList.Add(year.ToString());
            }

            //only add the all option on years select list items if at least a year was returned

          


            ViewBag.Years = yearsList;

            return PartialView();
        }
    }
}