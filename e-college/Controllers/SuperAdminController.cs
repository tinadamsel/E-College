using Core.DB;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using System.Data;

namespace e_college.Controllers
{
    //[Authorize(Roles = "SuperAdmin")]
    public class SuperAdminController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserHelper _userHelper;
        private readonly ISuperAdminHelper _superAdminHelper;
        public SuperAdminController(AppDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserHelper userHelper, ISuperAdminHelper superAdminHelper)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _userHelper = userHelper;
            _superAdminHelper = superAdminHelper;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var user = _userHelper.GetCurrentUserId(User.Identity.Name);
            var students = _superAdminHelper.GetTotalStudents();
            var staff = _superAdminHelper.GetTotalStaff();
            var subjects = _superAdminHelper.GetTotalSubjects();
            var model = new ApplicationUserViewModel()
            {
                TotalStaff = staff,
                TotalStudents = students,
                TotalSubjects = subjects,
            };
            return View(model);
        }
        [HttpGet]
        public IActionResult Subject()
        {
            var subjectDetails = _superAdminHelper.GetSubjects();
            return View(subjectDetails);
            
        }
        public JsonResult CreateSubject(string subjectDetails)
        {
            if (subjectDetails != null)
            {
                var subjectViewModel = JsonConvert.DeserializeObject<SubjectViewModel>(subjectDetails);
                if (subjectViewModel != null)
                {
                    var checkSubjectName = _superAdminHelper.CheckExistingSubjectName(subjectViewModel?.Name);
                    if (!checkSubjectName)
                    {
                        var subject = _superAdminHelper.CreateSubject(subjectViewModel);
                        if (subject)
                        {
                            return Json(new { isError = false, msg = "Subject Created Successfully" });
                        }
                        return Json(new { isError = true, msg = "Unable to Create" });
                    }
                    return Json(new { isError = true, msg = "Subject Name Already Exists" });
                }
            }
            return Json(new { isError = true, msg = "Network Failure" });
        }

        public JsonResult EditSubject(int Id)
        {
            if (Id > 0)
            {
                var subjectToEdit = _superAdminHelper.GetSubjectToEdit(Id);
                if (subjectToEdit != null)
                {
                    return Json(subjectToEdit);
                }
                return Json(new { isError = true, msg = "Unable To Get Subject" });
            }
            return Json(new { isError = true, msg = "Network Error" });
        }

        public JsonResult EditedSubject(string editSubject)
        {
            if (editSubject != null)
            {
                var subjectViewModel = JsonConvert.DeserializeObject<SubjectViewModel>(editSubject);
                if (subjectViewModel != null)
                {
                    var editSub = _superAdminHelper.SaveEditedSubject(subjectViewModel);
                    if (editSub)
                    {
                        return Json(new { isError = false, msg = "Subject Edited Successfully" });
                    }
                    return Json(new { isError = true, msg = "Unable to Edit" });
                }
            }
            return Json(new { isError = true, msg = "Network Error" });
        }

        public JsonResult DeleteSubject(int id)
        {
            if (id > 0)
            {
                var deleteSub = _superAdminHelper.DeleteSubject(id);
                if (deleteSub)
                {
                    return Json(new { isError = false, msg = "Subject Deleted successfully" });
                }
                return Json(new { isError = true, msg = "Unable To Delete Subject" });
            }
            return Json(new { isError = true, msg = "Network Error" });
        }

        //public JsonResult TestToEdit(int id)
        //{
        //    ViewBag.Layout = _userHelper.GetRoleLayout();
        //    if (id > 0)
        //    {
        //        var testToEdit = _userHelper.GetTestToEdit(id);
        //        if (testToEdit != null)
        //        {
        //            ViewBag.Specimens = _dropdownHelper.DropdownOfSpecimen();
        //            return Json(new { isError = false, data = testToEdit });
        //        }
        //    }
        //    return Json(new { isError = true, msg = "Unable To Get test" });
        //}
      
    }
}
