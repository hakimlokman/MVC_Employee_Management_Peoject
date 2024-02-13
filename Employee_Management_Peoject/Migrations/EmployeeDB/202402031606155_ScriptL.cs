namespace Employee_Management_Peoject.Migrations.EmployeeDB
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScriptL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Departments",
                c => new
                    {
                        DepartmentId = c.Int(nullable: false, identity: true),
                        DepartmentName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.DepartmentId);
            
            CreateTable(
                "dbo.Employees",
                c => new
                    {
                        EmployeeId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        JoinDate = c.DateTime(nullable: false, storeType: "date"),
                        Status = c.Boolean(nullable: false),
                        Picture = c.String(),
                        DepartmentId = c.Int(nullable: false),
                        SkillId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.SkillId, cascadeDelete: true)
                .Index(t => t.DepartmentId)
                .Index(t => t.SkillId);
            
            CreateTable(
                "dbo.EmployeeDetails",
                c => new
                    {
                        EmployeeDetailsId = c.Int(nullable: false, identity: true),
                        Designation = c.Int(nullable: false),
                        Experiance = c.Int(nullable: false),
                        PriviousCompany = c.String(),
                        Salary = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Performance = c.Int(nullable: false),
                        EmployeeId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.EmployeeDetailsId)
                .ForeignKey("dbo.Employees", t => t.EmployeeId, cascadeDelete: true)
                .Index(t => t.EmployeeId);
            
            CreateTable(
                "dbo.Skills",
                c => new
                    {
                        SkillId = c.Int(nullable: false, identity: true),
                        SkillName = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.SkillId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Employees", "SkillId", "dbo.Skills");
            DropForeignKey("dbo.EmployeeDetails", "EmployeeId", "dbo.Employees");
            DropForeignKey("dbo.Employees", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.EmployeeDetails", new[] { "EmployeeId" });
            DropIndex("dbo.Employees", new[] { "SkillId" });
            DropIndex("dbo.Employees", new[] { "DepartmentId" });
            DropTable("dbo.Skills");
            DropTable("dbo.EmployeeDetails");
            DropTable("dbo.Employees");
            DropTable("dbo.Departments");
        }
    }
}
