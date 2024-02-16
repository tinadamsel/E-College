using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Subject : Basemodel
    {
        public string SubjectDescription { get; set; }

        public int? Duration { get; set; }
    }
}
