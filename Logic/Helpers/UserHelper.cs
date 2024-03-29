﻿using Core.Config;
using Core.DB;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Logic.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.DB.ECollegeEnums;

namespace Logic.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGeneralConfiguration _generalConfiguration;
        private readonly IEmailService _emailService;

        public UserHelper(AppDbContext context, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IGeneralConfiguration generalConfiguration, IEmailService emailService)
        {
            _signInManager = signInManager;
            _context = context;
            _userManager = userManager;
            _generalConfiguration = generalConfiguration;
            _emailService = emailService;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.Users.Where(s => s.Email == email)?.FirstOrDefaultAsync();
        }
        public ApplicationUser FindByUserName(string username)
        {
            return _userManager.Users.Where(s => s.UserName == username).FirstOrDefault();
        }
        public int GetAllUser()
        {
            return _userManager.Users.Where(x => !x.Deactivated).Count();
        }
        public string GetUserRole(string userId)
        {
            if (userId != null)
            {
                var userRole = _context.UserRoles.Where(x => x.UserId == userId).FirstOrDefault();
                if (userRole != null)
                {
                    var roles = _context.Roles.Where(x => x.Id == userRole.RoleId).FirstOrDefault();
                    if (roles != null)
                    {
                        return roles.Name;
                    }
                }
            }
            return null;
        }
        public string GetUserId(string username)
        {
            return _userManager.Users.Where(s => s.UserName == username)?.FirstOrDefaultAsync().Result.Id?.ToString();
        }
        public ApplicationUser FindById(string Id)
        {
            return _userManager.Users.Where(s => s.Id == Id).FirstOrDefault();
        }
        public string GetCurrentUserId(string username)
        {
            try
            {
                if (username != null)
                {
                    return _userManager.Users.Where(s => s.UserName == username)?.FirstOrDefault()?.Id?.ToString();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
        public bool CheckForUserName(string userName)
        {
            if (userName != null)
            {
                var checkUserName = _context.ApplicationUser.Where(x => x.UserName == userName && !x.Deactivated).FirstOrDefault();
                if (checkUserName != null)
                {
                    return true;
                }
            }
            return false;
        }
        public int GetCurrentSession(string edulevel)
        {
            if (edulevel.ToLower().Contains("primary1"))
            {
                return (int)CurrentSession.PrimaryOne;
            }
            if (edulevel.ToLower().Contains("primary2"))
            {
                return (int)CurrentSession.PrimaryTwo;
            }
            if (edulevel.ToLower().Contains("primary3"))
            {
                return (int)CurrentSession.PrimaryThree;
            }
            if (edulevel.ToLower().Contains("primary4"))
            {
                return (int)CurrentSession.PrimaryFour;
            }
            if (edulevel.ToLower().Contains("primary5"))
            {
                return (int)CurrentSession.PrimaryFive;
            }
            if (edulevel.ToLower().Contains("primary6"))
            {
                return (int)CurrentSession.PrimarySix;
            }
            if (edulevel.ToLower().Contains("secondary1"))
            {
                return (int)CurrentSession.JSOne;
            }
            if (edulevel.ToLower().Contains("secondary2"))
            {
                return (int)CurrentSession.JSTwo;
            }
            if (edulevel.ToLower().Contains("secondary3"))
            {
                return (int)CurrentSession.JSThree;
            }
            if (edulevel.ToLower().Contains("secondary4"))
            {
                return (int)CurrentSession.SSOne;
            }
            if (edulevel.ToLower().Contains("secondary5"))
            {
                return (int)CurrentSession.SSTwo;
            }
            if (edulevel.ToLower().Contains("secondary6"))
            {
                return (int)CurrentSession.SSThree;
            }
            return 0;
        }
        public bool CheckIfUserIsSuspended(string email)
        {
            if (email != null)
            {
                var getUser = _context.ApplicationUser.Where(x => x.Email == email && x.Id != null && !x.Deactivated).FirstOrDefault();
                if (getUser != null)
                {
                    var checkForSuspension = _context.Suspensions.Where(x => x.UserId == getUser.Id && x.IsSuspended).FirstOrDefault();
                    if (checkForSuspension != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public List<Subject> GetAllSubjects()
        {
            var subjects = _context.Subjects.Where(x => x.Id > 0 && x.Active).Select(a => new Subject
            {
                Id = a.Id,
                Name= a.Name,
            }).ToList();
            return subjects;
        }
        public bool CheckIfStaffIsApproved(string email)
        {
            if (email != null)
            {
                var getUser = _context.ApplicationUser.Where(x => x.Email == email && x.Id != null && x.IsStudent == false).FirstOrDefault();
                if (getUser == null)
                {
                    return true;
                }
                else
                {
                    var documentation = _context.StaffDocuments.Where(x => x.UserId == getUser.Id && (bool)x.IsApproved).FirstOrDefault();
                    if (documentation != null)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public async Task<bool> RegisterStudent(ApplicationUserViewModel userDetails, string edulevel)
        {
            try
            {
                if (userDetails != null && edulevel != null)
                {
                    var currentSession = GetCurrentSession(edulevel);
                    
                    var user = new ApplicationUser();
                    user.UserName = userDetails.Username;
                    user.Email = userDetails.Email;
                    user.FirstName = userDetails.FirstName;
                    user.LastName = userDetails.LastName;
                    user.PhoneNumber = userDetails.Phonenumber;
                    user.DateRegistered = DateTime.Now;
                    user.Deactivated = false;
                    user.Address = userDetails.Address;
                    user.Country = userDetails.Country;
                    user.State = userDetails.State;
                    user.DOB = userDetails.DOB;
                    user.OtherName = userDetails.OtherName;
                    user.IsStudent = true;
                    if (edulevel.ToLower().Contains("primary"))
                    {
                        user.AcademicLevel = AcademicLevel.Primary;
                        user.CurrentSession = (CurrentSession?)currentSession;
                        var createUser = await _userManager.CreateAsync(user, userDetails.Password).ConfigureAwait(false);
                        if (createUser.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Primary").ConfigureAwait(false);
                            return true;
                        }
                    }
                    if (edulevel.ToLower().Contains("secondary"))
                    {
                        user.AcademicLevel = AcademicLevel.Secondary;
                        user.CurrentSession = (CurrentSession?)currentSession;
                        var createUser = await _userManager.CreateAsync(user, userDetails.Password).ConfigureAwait(false);
                        if (createUser.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Secondary").ConfigureAwait(false);
                            return true;
                        }
                    }
                    if (edulevel.ToLower().Contains("tertiary"))
                    {
                        user.AcademicLevel = AcademicLevel.Tertiary;
                        user.CurrentSession = CurrentSession.YearOne;
                        var createUser = await _userManager.CreateAsync(user, userDetails.Password).ConfigureAwait(false);
                        if (createUser.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Tertiary").ConfigureAwait(false);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<bool> RegStaff(ApplicationUserViewModel userDetails, string staffPosition, string appLetter, string validId)
        {
            try
            {
                if (userDetails != null)
                {
                    var user = new ApplicationUser();
                    user.UserName = userDetails.Username;
                    user.Email = userDetails.Email;
                    user.FirstName = userDetails.FirstName;
                    user.LastName = userDetails.LastName;
                    user.PhoneNumber = userDetails.Phonenumber;
                    user.DateRegistered = DateTime.Now;
                    user.Deactivated = false;
                    user.Address = userDetails.Address;
                    user.Country = userDetails.Country;
                    user.State = userDetails.State;
                    user.DOB = userDetails.DOB;
                    user.OtherName = userDetails.OtherName;
                    user.IsStudent = false;
                    if (userDetails.SubjectId > 0)
                    {
                        user.StaffType = StaffType.AcademicStaff;
                        var createStaff = await _userManager.CreateAsync(user, userDetails.Password).ConfigureAwait(false);
                        if (createStaff.Succeeded)
                        {
                            await _userManager.AddToRoleAsync(user, "Staff").ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        user.StaffType = StaffType.NonAcademicStaff;
                        var createUser = await _userManager.CreateAsync(user, userDetails.Password).ConfigureAwait(false);
                        if (createUser.Succeeded)
                        {
                            if (staffPosition.ToLower().Contains("humanresource"))
                            {
                                await _userManager.AddToRoleAsync(user, "HumanResource").ConfigureAwait(false);
                            }
                            if (staffPosition.ToLower().Contains("academics"))
                            {
                                await _userManager.AddToRoleAsync(user, "ViceChancellorAcademics").ConfigureAwait(false);
                            }
                            if (staffPosition.ToLower().Contains("admissionofficer"))
                            {
                                await _userManager.AddToRoleAsync(user, "AdmissionOfficer").ConfigureAwait(false);
                            }
                            if (staffPosition.ToLower().Contains("librarianofficer"))
                            {
                                await _userManager.AddToRoleAsync(user, "LibrarianOfficer").ConfigureAwait(false);
                            }
                            if (staffPosition.ToLower().Contains("accountofficer"))
                            {
                                await _userManager.AddToRoleAsync(user, "AccountOfficer").ConfigureAwait(false);
                            }
                            if (staffPosition.ToLower().Contains("examsofficer"))
                            {
                                await _userManager.AddToRoleAsync(user, "ExamsOfficer").ConfigureAwait(false);
                            }
                            if (staffPosition.ToLower().Contains("businessdevofficer"))
                            {
                                await _userManager.AddToRoleAsync(user, "BusinessDevOfficer").ConfigureAwait(false);
                            }
                            if (staffPosition.ToLower().Contains("publicrelofficer"))
                            {
                                await _userManager.AddToRoleAsync(user, "PublicRelOfficer").ConfigureAwait(false);
                            }
                            if (staffPosition.ToLower().Contains("studentaffairs"))
                            {
                                await _userManager.AddToRoleAsync(user, "ViceChancellorStudentAffairs").ConfigureAwait(false);
                            }
                        }
                    }
                    AddStaffDocuments(user.Id, staffPosition, (int)userDetails.SubjectId, validId, appLetter);
                    if (user.Id != null)
                    {
                        string toEmail = _generalConfiguration.AdminEmail;
                        string subject = "Staff Application Submission";
                        string message = "Hello, <br> A staff application has been submitted, by" + "<b>" + user?.FirstName + user?.LastName + "</b> on" + user.DateRegistered.ToString()  + ". <br/> <br/> Please, endeavor to review the pending application and make the necessary move. <br/> Thank you!!!";

                        _emailService.SendEmail(toEmail, subject, message);
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool AddStaffDocuments(string userId, string staffPosition, int subjectId, string validId, string appLetter)
        {
            if (userId != null && validId != null  && appLetter != null)
            {
                var position = 0;

                if (subjectId == 0 || subjectId < 0)
                {
                    position = GetStaffPosition(staffPosition);
                }
                var addStaff = new StaffDocumentation()
                {
                    DateCreated = DateTime.Now,
                    Active = true,
                    StaffStatus = StaffStatus.Pending,
                    UserId = userId,
                    StaffPosition = subjectId != 0 ? StaffPosition.AcademicStaff : (StaffPosition)position,
                    SubjectId = subjectId != 0 ? subjectId : 0,
                    ApplicationLetter = appLetter,
                    Identification = validId,
                    Resume = "N/A",
                };
                _context.Add(addStaff);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public int GetStaffPosition(string staffPosition)
        {
            if (staffPosition.ToLower().Contains("humanresource"))
            {
                return (int)StaffPosition.HumanResource;
            }
            if (staffPosition.ToLower().Contains("vicechancelloracademics"))
            {
                return (int)StaffPosition.ViceChancellorAcademics;
            }
            if (staffPosition.ToLower().Contains("admissionofficer"))
            {
                return (int)StaffPosition.AdmissionOfficer;
            }
            if (staffPosition.ToLower().Contains("librarianofficer"))
            {
                return (int)StaffPosition.LibrarianOfficer;
            }
            if (staffPosition.ToLower().Contains("accountofficer"))
            {
                return (int)StaffPosition.AccountOfficer;
            }
            if (staffPosition.ToLower().Contains("examsofficer"))
            {
                return (int)StaffPosition.ExamsOfficer;
            }
            if (staffPosition.ToLower().Contains("businessdevofficer"))
            {
                return (int)StaffPosition.BusinessDevOfficer;
            }
            if (staffPosition.ToLower().Contains("publicrelofficer"))
            {
                return (int)StaffPosition.PublicRelOfficer;
            }
            if (staffPosition.ToLower().Contains("vicechancellorstudentaffairs"))
            {
                return (int)StaffPosition.ViceChancellorStudentAffairs;
            }
            return 0;
        }
        public List<StaffDocumentationViewModel> GetPendingApplications()
        {
             var getApplications = new List<StaffDocumentationViewModel>();
                 getApplications = _context.StaffDocuments.Where(x => x.Id > 0 && x.Active && x.StaffStatus == StaffStatus.Pending).Include(x => x.Users).Include(x => x.Subject)
                .Select(x => new StaffDocumentationViewModel()
                {
                    Id = x.Id,
                    Name = x.Users.FirstName + "" + x.Users.LastName,
                    DateCreated= x.DateCreated,
                    ApplicationLetter = x.ApplicationLetter,
                    StaffPosition = x.StaffPosition,
                    SubjectId = x.SubjectId,
                    UserId = x.UserId,
                    Identification= x.Identification,  
                    Resume = x.Resume,
                    Active= x.Active,
                    CourseName = x.Subject.Name,
                }).ToList();
            return getApplications;
        }
        //public string GetValidId()
        //{
        //    var iD = _context.StaffDocuments.Where(x => x.Id > 0 && x.Active && x.StaffStatus == StaffStatus.Pending)
        //    return null;
        //}
        public StaffDocumentation GetCoverLetter(int id)
        {
            var getCoverLetter = _context.StaffDocuments.Where(x => x.Id == id && x.Active).FirstOrDefault();
            if (getCoverLetter != null)
            {
                return getCoverLetter;
            }
            return null;
        }
        public bool CheckIfApproved(int id)
        {
            if (id > 0)
            {
                return _context.StaffDocuments.Where(x => x.Id == id && x.StaffStatus == StaffStatus.Approved).Any();
            }
            return false;
        }
        public bool CheckIfDeclined(int id)
        {
            if (id > 0)
            {
                return _context.StaffDocuments.Where(x => x.Id == id && x.StaffStatus == StaffStatus.Rejected).Any();
            }
            return false;
        }
        public bool ApproveApplication(int id)
        {
            string toEmailBug = _generalConfiguration.DeveloperEmail;
            string subjectEmailBug = " Exception Message on Ecollege";
            try
            {
                if (id > 0)
                {
                    var appApprove = _context.StaffDocuments.Where(x => x.Id == id && x.StaffStatus == StaffStatus.Pending).Include(x => x.Users).FirstOrDefault();
                    if (appApprove != null)
                    {
                        appApprove.IsApproved = true;
                        appApprove.DateOfApproval = DateTime.Now;
                        appApprove.StaffStatus = StaffStatus.Approved;
                        _context.Update(appApprove);
                        _context.SaveChanges();

                        if (appApprove?.Users?.Email != null)
                        {
                            string toEmail = appApprove?.Users?.Email;
                            string subject = "Hooray!!!, Application Approved ";
                            string message = "Hello " + "<b>" + appApprove?.Users?.FirstName + "" + appApprove?.Users?.LastName + ", </b>" + "<br> your application for the post of " + appApprove?.StaffPosition + " on our platform has been approved.  You can now login with the email and password submitted during registration. <br> <br> We are happy to have you and we look forward to having a nice working relationship with you. <br> <br>" +
                              " Once again, Congratulations !!! " +
                              "<br> HR, Ecollege Team";

                            _emailService.SendEmail(toEmail, subject, message);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                string message = "Exception " + ex.Message + " and inner exception:" + ex.InnerException.Message + "  Occured at " + DateTime.Now;
                _emailService.SendEmail(toEmailBug, subjectEmailBug, message);
                throw;
            }
        }

        public bool RejectApplication(int id)
        {
            string toEmailBug = _generalConfiguration.DeveloperEmail;
            string subjectEmailBug = "Exception Message on Ecollege";
            try
            {
                if (id > 0)
                {
                    var rejectApprove = _context.StaffDocuments.Where(x => x.Id == id && x.StaffStatus == StaffStatus.Pending).Include(x => x.Users).FirstOrDefault();
                    if (rejectApprove != null)
                    {
                        rejectApprove.StaffStatus = StaffStatus.Rejected;
                        _context.Update(rejectApprove);
                        _context.SaveChanges();

                        if (rejectApprove?.Users?.Email != null)
                        {
                            string toEmail = rejectApprove?.Users?.Email;
                            string subject = "Sorry, Application Declined ";
                            string message = "Hello " + "<b>" + rejectApprove?.Users?.FirstName + "" + rejectApprove?.Users?.LastName + ", </b>" + "<br> your application for the post of " + rejectApprove?.StaffPosition + " on our platform has been declined. We thank you for your interest, but we can not move further with you. Keep on visiting our platform for other available positions. " + " <br> <br> We wish you well in your future endeavours <br> " +
                              "HR, Ecollege Team";

                            _emailService.SendEmail(toEmail, subject, message);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                string message = "Exception " + ex.Message + " and inner exception:" + ex.InnerException.Message + "  Occured at " + DateTime.Now;
                _emailService.SendEmail(toEmailBug, subjectEmailBug, message);
                throw;
            }
        }

        public int GetTotalAcademicStaff()
        {
            return _context.ApplicationUser.Where(a => a.Id != null && !a.Deactivated && a.IsStudent == false && a.StaffType == StaffType.AcademicStaff).Count();
        }
        public int GetTotalNonAcademicStaff()
        {
            return _context.ApplicationUser.Where(a => a.Id != null && !a.Deactivated && a.IsStudent == false && a.StaffType == StaffType.NonAcademicStaff).Count();
        }


    }
}
