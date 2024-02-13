using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Employee_Management_Peoject.Models
{
    public enum Designation { GM=1,Manager,Developer,Engineer,Accountant,Analyist };
    public enum Performance { High=1,Mediam,Low}
    public class Department
    {
        public int DepartmentId { get; set; }
        [Required, StringLength(50), Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        //navigation
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
    public class Skill
    {
        public int SkillId { get; set; }
        [Required, StringLength(50), Display(Name = "SkillName")]
        public string SkillName { get; set; }
        //navigation
        public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
    public class Employee
    {
        public int EmployeeId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }
   
        [Required, Column(TypeName = "date"), Display(Name = "Join Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }
        public bool Status { get; set; }
        public string Picture { get; set; }
        //foreign key
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        [ForeignKey("Skill")]
        public int SkillId { get; set; }
        //nevigation
        public virtual Department Department  { get; set; }
        public virtual Skill Skill  { get; set; }
        public virtual ICollection<EmployeeDetails> EmployeeDetails { get; set; } = new List<EmployeeDetails>();
    }
    public class EmployeeDetails
    {
        public int EmployeeDetailsId { get; set; }
        public Designation Designation { get; set; }
        public int Experiance { get; set; }
        public string PriviousCompany { get; set; }
        public decimal Salary { get; set; }
        public Performance Performance { get; set; }
        [Required,ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public virtual Employee Employee  { get; set; }
    }
    public class EmployeeDbContext : DbContext
    {
        public DbSet<Department> Departments  { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<EmployeeDetails> EmployeeDetails { get; set; }
    }
}