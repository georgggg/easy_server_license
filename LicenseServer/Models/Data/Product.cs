using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseServer.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PrivateKey{ get; set; }
        public string PublicKey { get; set; }
        public virtual List<LicenseFeature> LicenseFeatures { get; set; }
    }
}