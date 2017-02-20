using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseServer.Models.Data
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        public DataContext() : base("DataContext", throwIfV1Schema: true)
        {
        }
        public static DataContext Create()
        {
            return new DataContext();
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<LicenseFeature> LicenseFeatures { get; set; }
   

    }
}