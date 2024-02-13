namespace Employee_Management_Peoject.Migrations.EmployeeDB
{
    using Employee_Management_Peoject.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Employee_Management_Peoject.Models.EmployeeDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\EmployeeDB";
        }

        protected override void Seed(Employee_Management_Peoject.Models.EmployeeDbContext db)
        {
            db.Departments.AddRange(new Department[]
             {
               new Department{DepartmentName="Accounts"},
               new Department{DepartmentName="Software Testing"}


             });
            db.Skills.AddRange(new Skill[]
            {
                 new Skill{SkillName="CA-Part1"},
                 new Skill{SkillName="C#"}
            });
            db.SaveChanges();
            Employee e = new Employee
            {
                Name = "Lucky Hardman",
                DepartmentId = 1,
                SkillId = 1,
                JoinDate = DateTime.Now.AddDays(-5),
                Status = true,
                Picture = "1.jpg"
            };
            e.EmployeeDetails.Add(new EmployeeDetails { Designation = Designation.Accountant, Experiance = 5, PriviousCompany = "ShelTack", Salary = 10000, Performance = Performance.Mediam });

            db.Employees.Add(e);
            db.SaveChanges();

        }
    }
}
