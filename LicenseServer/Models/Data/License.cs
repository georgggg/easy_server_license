using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseServer.Models
{
    public class License
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public string Guid { get; set; }
        public string Type { get; set; }
        public DateTime Expiration { get; set; }
        public DateTime IssueDate { get; set; }
        public string Signature { get; set; }

        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual List<LicenseFeature> LicenseFeatures { get; set; }
    }

    public class LicenseViewModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int ExpirationInDays { get; set; }

        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
    }
}