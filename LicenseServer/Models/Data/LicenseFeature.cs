using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseServer.Models
{
    public class LicenseFeature
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAllowed { get; set; }
    }
}