using Core.Models;
using Logic.IHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Helpers
{
    public class Template : ITemplate
    {
        public List<string> DefaultRoles()
        {
            var roles = new[] { "SuperAdmin", "Staff", "Admin", "Primary", "Secondary", "Tertiary", "HumanResource", "ViceChancellorAcademics", "AdmissionOfficer", "LibrarianOfficer", "AccountOfficer", "ExamsOfficer", "BusinessDevOfficer", "PublicRelOfficer", "ViceChancellorStudentAffairs" };
            return roles.ToList();
        }
    }
}
