using Employee_Management_Peoject.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Employee_Management_Peoject.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly EmployeeDbContext db= new EmployeeDbContext();
        // GET: Departments
        public ActionResult Index()
        {
            return View(db.Departments.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "DepartmentId,DepartmentName")] Department department)
        {
            if (ModelState.IsValid)
            {

                db.Departments.Add(department);
                db.SaveChanges();
                return PartialView("_success");
            }
            else
            {
                return PartialView("_error");

            }
        }
        public ActionResult Edit(int? id)
        {
            Department department = db.Departments.Find(id);
            return View(department);
        }
        [HttpPost]
        public ActionResult Edit(Department department)
        {
            if (ModelState.IsValid)
            {

                db.Entry(department).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");

            }
            return PartialView("_error");
        }
        public ActionResult Delete(int? id)
        {
            Department department= db.Departments.Find(id);
            db.Departments.Remove(department);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

    }
}
