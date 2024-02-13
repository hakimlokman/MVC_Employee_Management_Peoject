using PagedList;
using Employee_Management_Peoject.Models;
using Employee_Management_Peoject.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.Mvc;

namespace Employee_Management_Peoject.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly EmployeeDbContext db=new EmployeeDbContext();
        // GET: Employee
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult ShowIndex(int pg=1)
        {
            var data = db.Employees
                         .Include(x => x.Department)
                         .Include(x => x.Skill)
                         .Include(x => x.EmployeeDetails)
                         .OrderBy(x => x.EmployeeId)
                         .ToPagedList(pg, 5);       
            return PartialView("_ShowIndex",data );
        }
        public ActionResult Create()
        {
            return View();
        }
        public ActionResult CreateForm()
        {
            EmployeeViewModel model = new EmployeeViewModel();
            model.EmployeeDetails.Add(new EmployeeDetails());
            ViewBag.Skilll = db.Skills.ToList();
            ViewBag.Departments = db.Departments.ToList();
            return PartialView("_CreateForm", model);
        }
        [HttpPost]
        public ActionResult Create(EmployeeViewModel model,string act="")
        {

            if (act == "add")
            {
                model.EmployeeDetails.Add(new EmployeeDetails());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.EmployeeDetails.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act == "insert")
            {
                if(ModelState.IsValid)
                {
                    var employee = new Employee
                    {
                        DepartmentId= model.DepartmentId,
                        SkillId=model.SkillId,
                        Name=model.Name,
                        JoinDate=model.JoinDate,
                        Status=model.Status,
                    };

                    string ext = Path.GetExtension(model.Picture.FileName);
                    string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                    string savePath = Path.Combine(Server.MapPath("~/Images"), f);
                    model.Picture.SaveAs(savePath);
                    employee.Picture = f;
                    db.Employees.Add(employee);
                    db.SaveChanges();

                    foreach (var e in model.EmployeeDetails)
                    {
                        db.Database.ExecuteSqlCommand($"spInsertEmployeeDetails {(int)e.Designation},{(int)e.Experiance},{e.PriviousCompany},{e.Salary},{(int)e.Performance},{employee.EmployeeId}");

                    }
                    EmployeeViewModel viewModel = new EmployeeViewModel()
                    {
                        Name = "",
                        JoinDate = DateTime.Today
                    };
                    viewModel.EmployeeDetails.Add(new EmployeeDetails());
                    ViewBag.Skilll = db.Skills.ToList();
                    ViewBag.Departments = db.Departments.ToList();
                    foreach (var e in ModelState.Values)
                    {
                        e.Value = null;
                    }
                    return View("_CreateForm", viewModel);
                }
            }
            ViewBag.Skilll = db.Skills.ToList();
            ViewBag.Departments = db.Departments.ToList();
            ModelState.Clear();
            return View("_CreateForm", model);
        }
        public ActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }
        public ActionResult EditForm(int id)
        {
            var data = db.Employees.FirstOrDefault(x=>x.EmployeeId == id);
            if (data == null) return new HttpNotFoundResult();
            db.Entry(data).Collection(x => x.EmployeeDetails).Load();
            EmployeeEditModel model = new EmployeeEditModel
            {
                EmployeeId = id,
                DepartmentId = data.DepartmentId,
                SkillId = data.SkillId,
                Name = data.Name,
                JoinDate = data.JoinDate,
                Status = data.Status,
                EmployeeDetails = data.EmployeeDetails.ToList()
            };
            ViewBag.Skilll = db.Skills.ToList();
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.CurrentPic = data.Picture;
            return PartialView("_EditForm", model);
        }
        [HttpPost]
        public ActionResult Edit(EmployeeEditModel model, string act = "")
        {
            if (act == "add")
            {
                model.EmployeeDetails.Add(new EmployeeDetails());
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act.StartsWith("remove"))
            {
                int index = int.Parse(act.Substring(act.IndexOf("_") + 1));
                model.EmployeeDetails.RemoveAt(index);
                foreach (var e in ModelState.Values)
                {
                    e.Errors.Clear();
                    e.Value = null;
                }
            }
            if (act == "update")
            {
                if (ModelState.IsValid)
                {
                    var employee=db.Employees.FirstOrDefault(x=>x.EmployeeId==model.EmployeeId);
                    if(employee == null) return new HttpNotFoundResult();
                    employee.Name = model.Name;
                    employee.JoinDate = model.JoinDate;
                    employee.Status = model.Status;
                    employee.SkillId = model.SkillId;
                    employee.DepartmentId = model.DepartmentId;
                    if(model.Picture != null)
                    {
                        string ext = Path.GetExtension(model.Picture.FileName);
                        string f = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ext;
                        string savePath = Path.Combine(Server.MapPath("~/Images"), f);
                        model.Picture.SaveAs(savePath);
                        employee.Picture = f;
                    }
                    else
                    {

                    }
                    db.SaveChanges();
                    db.Database.ExecuteSqlCommand($"EXEC DeleteEmployeeDetailsByEmployeeID {employee.EmployeeId}");
                    foreach (var e in model.EmployeeDetails)
                    {
                        db.Database.ExecuteSqlCommand($" EXEC spInsertEmployeeDetails {(int)e.Designation},{(int)e.Experiance},{e.PriviousCompany},{e.Salary},{(int)e.Performance},{employee.EmployeeId}");
                    }
                    TempData["SuccessMessage"] = "Employee updated successfully.";
                }
            }
            ViewBag.Skilll = db.Skills.ToList();
            ViewBag.Departments = db.Departments.ToList();
            ViewBag.CurrentPic = db.Employees.FirstOrDefault(x => x.EmployeeId == model.EmployeeId)?.Picture;
            return View("_EditForm", model);
        }
        public ActionResult Delete(int? id)
        {
            var employee = db.Employees.Find(id);
            if (employee != null)
            {
                var deleteDetails = db.EmployeeDetails.Where(x => x.EmployeeId == id).ToList();
                db.EmployeeDetails.RemoveRange(deleteDetails);
                db.Employees.Remove(employee);
                db.SaveChanges();
                TempData["DeleteMessage"] = "Employee deleted successfully.";
                return RedirectToAction("Index");
            }
            return View();
        }
        public ActionResult Aggrigate()
        {
            int numberOfEmployee = db.Employees.Count();
            ViewBag.numberOfEmployee = numberOfEmployee;

            int totalSalary = (int)db.EmployeeDetails.Sum(x => x.Salary);
            ViewBag.totalSalary = totalSalary;

            int MaxSalary = (int)db.EmployeeDetails.Max(x => x.Salary);
            ViewBag.MaxSalary = MaxSalary;

            int MinSalary = (int)db.EmployeeDetails.Min(x => x.Salary);
            ViewBag.MinSalary = MinSalary;

            int AvgSalary = (int)db.EmployeeDetails.Average(x => x.Salary);
            ViewBag.AvgSalary = AvgSalary;
            return View();
        }
    }
}