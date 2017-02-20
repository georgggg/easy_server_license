using LicenseServer.Models;
using LicenseServer.Models.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LicenseServer.App_Start
{
    public class DatabaseInitializer : CreateDatabaseIfNotExists<DataContext>
    {
        private void SeedSystemData(DataContext context)
        {
            this.SeedSystemUsers(context);
        }

        private void SeedSystemUsers(DataContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new ApplicationUserManager(userStore);

            var applicationUserManager = new ApplicationUserManager(new UserStore<Models.ApplicationUser>(context));
            List<ApplicationUser> users = new List<ApplicationUser>() {
                new ApplicationUser{ UserName = "admin"},
        };
            foreach (ApplicationUser appUser in users)
            {
                var result = userManager.Create(appUser, "Pass$123");
            }
        }
    }
}