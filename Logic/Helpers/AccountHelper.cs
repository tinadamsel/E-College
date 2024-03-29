﻿using Core.Models;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Logic.Helpers
{
    public class AccountHelper : IAccountHelper
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountHelper(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void CreateSuperAdminAccount()
        {
            var superUser = _userManager.GetUsersInRoleAsync("SuperAdmin").Result;
            if (!superUser.Any())
            {
                var unknown = new ApplicationUser()
                {
                    FirstName = "Super",
                    LastName  = "Admin",
                    Email = "martihills4real@gmail.com",
                    UserName = "martihills4real@gmail.com.com",
                    DateRegistered = DateTime.Now,
                    DateModified = DateTime.Now,
                    Deactivated = false,
                };
                var pass = "5DD4C8A8EB0E9937D";
                var createdAccount = _userManager.CreateAsync(unknown, pass).Result;
                if (createdAccount.Succeeded)
                {
                    var endhere = _userManager.AddToRoleAsync(unknown, "SuperAdmin").Result;
                }
            }
        }
    }
}
