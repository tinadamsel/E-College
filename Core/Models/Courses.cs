using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Core.DB.ECollegeEnums;

namespace Core.Models
{
	public class Courses
	{
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime DateCreated { get; set; }
        //public CourseEnum CoursesForEachLevel { get; set; }
        public string? UserId { get; set; }
        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser Users { get; set; }
    }
   
}
