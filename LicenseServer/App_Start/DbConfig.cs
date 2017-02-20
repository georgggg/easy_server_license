using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using LicenseServer.Models.Data;

namespace LicenseServer.App_Start
{
    public static class DbConfig
    {
        public static void Config()
        {
            Database.SetInitializer<Models.Data.DataContext>(new DatabaseInitializer());
            using (var context = new DataContext())
            {
                if (!context.Database.Exists())
                {
                    new DatabaseInitializer().InitializeDatabase(context);
                }
                else
                {

                }
            }


        }
    }
}