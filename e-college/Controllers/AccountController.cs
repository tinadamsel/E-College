﻿using Core.Config;
using Core.DB;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace e_college.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserHelper _userHelper;
        private readonly IGeneralConfiguration _generalConfiguration;
        private readonly IDropDownHelper _dropdownHelper;

        public AccountController(AppDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserHelper userHelper, IGeneralConfiguration generalConfiguration, IDropDownHelper dropdownHelper)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _userHelper = userHelper;
            _generalConfiguration = generalConfiguration;
            _dropdownHelper = dropdownHelper;
        }
        [HttpGet]
        public IActionResult StudentRegistration()
        {
            return View();
        }
        
        [HttpPost]
        public async Task<JsonResult> StudentRegistration(string userDetails, string eduLevel)
        {
            if (userDetails != null && eduLevel != null)
            {
                var appUserViewModel = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userDetails);
                if (appUserViewModel != null)
                {
                    var checkUsername = _userHelper.CheckForUserName(appUserViewModel.Username);
                    if (checkUsername)
                    {
                        return Json(new { isError = true, msg = "UserName Already Exists" });
                    }
                    var checkEmail = await _userHelper.FindByEmailAsync(appUserViewModel.Email).ConfigureAwait(false);
                    if (checkEmail != null)
                    {
                        return Json(new { isError = true, msg = "Email Already Exists" });
                    }
                    if (appUserViewModel.Password != appUserViewModel.ConfirmPassword)
                    {
                        return Json(new { isError = true, msg = "Password and Confirm password do not match" });
                    }
                    if (appUserViewModel.Password.Length < 8)
                    {
                        return Json(new { isError = true, msg = "Password must be from 8 characters" });
                    }
                    var createStudent = await _userHelper.RegisterStudent(appUserViewModel, eduLevel).ConfigureAwait(false);
                    if (createStudent)
                    {
                        return Json(new { isError = false, msg = "Registration Successful, Login to continue" });
                    }
                    return Json(new { isError = true, msg = "Unable to register" });
                }
            }
            return Json(new { isError = true, msg = "Network Error" });
        }

        public IActionResult Careers()
        {
            ViewBag.Subjects = _dropdownHelper.DropdownOfSubject();
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> StaffRegistration(string userDetails, string staffPosition, string appLetter, string validId)
        {
            if (userDetails != null && appLetter != null && validId != null)
            {
                var appUserViewModel = JsonConvert.DeserializeObject<ApplicationUserViewModel>(userDetails);
                if (appUserViewModel != null)
                {
                    var checkUsername = _userHelper.CheckForUserName(appUserViewModel.Username);
                    if (checkUsername)
                    {
                        return Json(new { isError = true, msg = "UserName Already Exists" });
                    }
                    var checkEmail = await _userHelper.FindByEmailAsync(appUserViewModel.Email).ConfigureAwait(false);
                    if (checkEmail != null)
                    {
                        return Json(new { isError = true, msg = "Email Already Exists" });
                    }
                    if (appUserViewModel.Password != appUserViewModel.ConfirmPassword)
                    {
                        return Json(new { isError = true, msg = "Password and Confirm password do not match" });
                    }
                    if (appUserViewModel.Password.Length < 8)
                    {
                        return Json(new { isError = true, msg = "Password must be from 8 characters" });
                    }
                    var createStaff = await _userHelper.RegStaff(appUserViewModel, staffPosition, appLetter, validId).ConfigureAwait(false);
                    if (createStaff)
                    {
                        return Json(new { isError = false, msg = "Application successful. Thank you for your interest. You will receive a mail from us soonest", });
                    }
                }
                return Json(new { isError = true, msg = "Unable to register" });
            }
            return Json(new { isError = true, msg = "Network Error" });
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> Login(string email, string password)
        {
            if (email != null && password != null)
            {
                var checkIfSuspended = _userHelper.CheckIfUserIsSuspended(email);
                if (checkIfSuspended)
                {
                    return Json(new { isError = true, msg = "You are still under suspension. Login when your suspension period expires" });
                }
                var staffIsApproved = _userHelper.CheckIfStaffIsApproved(email);
                if (!staffIsApproved)
                {
                    return Json(new { isError = true, msg = "Your application has not been approved yet" });
                }
                var filterSpace = email.Replace(" ", "");
                var existingUser = _userHelper.FindByEmailAsync(filterSpace).Result;
                if (existingUser != null)
                {
                    var PasswordSignIn = await _signInManager.PasswordSignInAsync(existingUser, password, true, true).ConfigureAwait(false);

                    if (PasswordSignIn.Succeeded)
                    {
                        var url = "";
                        var userRole = await _userManager.GetRolesAsync(existingUser).ConfigureAwait(false);
                        if (userRole.FirstOrDefault().ToLower().Contains("primary"))
                        {
                            url = "/Student/Index";
                        }
                        if (userRole.FirstOrDefault().ToLower().Contains("secondary"))
                        {
                            url = "/Student/Index";
                        }
                        if (userRole.FirstOrDefault().ToLower().Contains("tertiary"))
                        {
                            url = "/Student/Index";
                        }
                        if (userRole.FirstOrDefault().ToLower().Contains("superadmin"))
                        {
                            url = "/SuperAdmin/Index";
                        }
                        if (userRole.FirstOrDefault().ToLower().Contains("humanresource"))
                        {
                            url = "/HumanResource/Index";
                        }
                        if (userRole.FirstOrDefault().ToLower().Contains("staff"))
                        {
                            url = "/AcademicStaff/Index";
                        }
                        //if (userRole.FirstOrDefault().ToLower().Contains("admissionofficer"))
                        //{
                        //    url = "/AdmissionOAfficer/Index";
                        //}
                        //if (userRole.FirstOrDefault().ToLower().Contains("librarianofficer"))
                        //{
                        //    url = "/LibrarianOfficer/Index";
                        //}
                        //if (userRole.FirstOrDefault().ToLower().Contains("accountofficer"))
                        //{
                        //    url = "/AccountOfficer/Index";
                        //}
                        //if (userRole.FirstOrDefault().ToLower().Contains("examsofficer"))
                        //{
                        //    url = "/ExamsOfficer/Index";
                        //}
                        //if (userRole.FirstOrDefault().ToLower().Contains("academics"))
                        //{
                        //    url = "/VCAcademics/Index";
                        //}
                        //if (userRole.FirstOrDefault().ToLower().Contains("studentaffairs"))
                        //{
                        //    url = "/VCStudentsAffairs/Index";
                        //}
                        return Json(new { isError = false, dashboard = url });
                    }
                }
                return Json(new { isError = true, msg = "Account does not exist,Contact your Admin" });
            }
            return Json(new { isError = true, msg = "Username and Password Required" });
        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }
    }
}
