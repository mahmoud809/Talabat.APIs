﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser() 
                {
                    DisplayName = "Mahmoud Salah",
                    Email = "mahmoud.salah@gmail.com",
                    UserName = "mahmoud.salah",
                    PhoneNumber = "01122334455" 

                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
