using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LicenseServer.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Company{ get; set; }
        public string Address { get; set; }
        public string Telephone { get; set; }
        public string Email { get; set; }
    }
}