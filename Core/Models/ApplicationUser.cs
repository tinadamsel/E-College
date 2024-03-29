﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;
using System.Security.Principal;
using System.Security.Cryptography;
using System.ComponentModel;
using System.Security.Policy;
using System.Threading;
using Core.DB;
using static Core.DB.ECollegeEnums;

namespace Core.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Display(Name = "First Name")]
		public virtual string FirstName { get; set; }
		[Display(Name = "Last Name")]
		public virtual string LastName { get; set; }
        [Display(Name = "Other Name")]
        public virtual string? OtherName { get; set; }
        [Display(Name = "Name")]
		[NotMapped]
		public string Name => FirstName + " " + LastName + "" + OtherName;
		[Display(Name = "Date Registered")]
		public DateTime DateRegistered { get; set; }
		public string? Country { get; set; }
		public string? State { get; set; }
		public string? Address { get; set; }
		public DateTime? DOB { get; set; }
		public AcademicLevel? AcademicLevel { get; set; }
		public CurrentSession? CurrentSession { get; set; }
		public StaffType? StaffType { get; set; }
		public bool Deactivated { get; set; }
		public bool? IsStudent { get; set; }
        public DateTime? DateOfApproval { get; set; }
        public DateTime? DateModified { get; set; }
	}

}
