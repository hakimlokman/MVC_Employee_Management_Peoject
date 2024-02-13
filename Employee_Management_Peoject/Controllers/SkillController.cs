using Employee_Management_Peoject.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Employee_Management_Peoject.Controllers
{
    public class SkillController : Controller
    {
        private readonly EmployeeDbContext db= new EmployeeDbContext();
        // GET: Skill
        public ActionResult Index(string skill)
        {
            
            List<Skill> modelData;
            if (string.IsNullOrEmpty(skill))
                modelData = db.Skills.ToList();
            else
                modelData = db.Skills.Where(c => c.SkillName.ToLower().Equals(skill.ToLower())).ToList();
            return View( modelData);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create([Bind(Include = "SkillId,SkillName")] Skill skill )
        {
            if (ModelState.IsValid)
            {

                db.Skills.Add(skill);
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
            Skill vehicleModel = db.Skills.Find(id);
            return View(vehicleModel);
        }
        [HttpPost]
        public ActionResult Edit(Skill vehicleModel)
        {
            if (ModelState.IsValid)
            {

                db.Entry(vehicleModel).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return PartialView("_success");

            }
            return PartialView("_error");
        }
        public ActionResult Delete(int? id)
        {
            Skill vehicleModel = db.Skills.Find(id);
            db.Skills.Remove(vehicleModel);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}