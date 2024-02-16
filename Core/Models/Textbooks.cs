using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Core.DB.ECollegeEnums;

namespace Core.Models
{
    public class Textbooks : Basemodel
    {
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? SubjectId { get; set; }
        [Display(Name = "Subject")]
        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }
        public TextbookEnum CoursesForEachLevel { get; set; }
        public string? UserId { get; set; }
        [Display(Name = "User")]
        [ForeignKey("UserId")]
        public virtual ApplicationUser? Users { get; set; }
        public string AddedBy { get; set; }
        public bool? IsApproved { get; set; }
    }
}
