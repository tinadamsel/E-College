using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Assignment : Basemodel
    {
        public string Description { get; set; }
        public bool? IsSubmitted { get; set; }
        public DateTime ValidUntilDate { get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Courses? Courses { get; set; }
    }
}
