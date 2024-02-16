using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class SubjectViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool Active { get; set; }
        public DateTime DateCreated { get; set; }
        public string SubjectDescription { get; set; }
        public int? Duration { get; set; }
    }
}
