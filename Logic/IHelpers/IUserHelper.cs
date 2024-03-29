﻿using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface IUserHelper
    {
        bool CheckForUserName(string userName);
        bool CheckIfUserIsSuspended(string email);
        Task<ApplicationUser> FindByEmailAsync(string email);
        ApplicationUser FindById(string Id);
        ApplicationUser FindByUserName(string username);
        string GetCurrentUserId(string username);
        string GetUserId(string username);
        Task<bool> RegStaff(ApplicationUserViewModel userDetails, string staffPosition, string appLetter, string validId);
        //Task<bool> RegisterPriStudent(ApplicationUserViewModel userDetails, string edulevel);
        //Task<bool> RegisterSecStudent(ApplicationUserViewModel userDetails, string edulevel);
        Task<bool> RegisterStudent(ApplicationUserViewModel userDetails, string edulevel);
        string GetUserRole(string userId);
        List<Subject> GetAllSubjects();
        bool CheckIfStaffIsApproved(string email);
        List<StaffDocumentationViewModel> GetPendingApplications();
        StaffDocumentation GetCoverLetter(int id);
        bool CheckIfApproved(int id);
        bool CheckIfDeclined(int id);
        bool ApproveApplication(int id);
        bool RejectApplication(int id);
        int GetTotalAcademicStaff();
        int GetTotalNonAcademicStaff();
        //Task<bool> RegisterUniStudent(ApplicationUserViewModel userDetails, string edulevel);
    }
}
