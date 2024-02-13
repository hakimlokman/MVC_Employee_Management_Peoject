using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Employee_Management_Peoject.Models.ViewModel
{
    public class EmployeeEditModel
    {
        public int EmployeeId { get; set; }
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, Column(TypeName = "date"), Display(Name = "Join Date"), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime JoinDate { get; set; }
        public bool Status { get; set; }
        public HttpPostedFileBase Picture { get; set; }
        [Display(Name ="Department")]
        public int DepartmentId { get; set; }
        [Display(Name = "Skill")]

        public int SkillId { get; set; }

        public virtual List<EmployeeDetails> EmployeeDetails { get; set; } = new List<EmployeeDetails>();
    }
}