using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LicenseServer.Models;
using LicenseServer.Models.Data;
using System.IO;
using System.Net.Mime;

namespace LicenseServer.Controllers
{
    public class LicensesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: Licenses
        public ActionResult Index()
        {
            var licenses = db.Licenses.Include(l => l.Customer).Include(l => l.Product);
            return View(licenses.ToList());
        }

        // GET: Licenses/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = db.Licenses.Find(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }

        // GET: Licenses/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name");
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name");
            return View();
        }

        // POST: Licenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ProductId,CustomerId,ExpirationInDays")] LicenseViewModel licenseViewEntity)
        {

            
            if (licenseViewEntity.ProductId > 0 && licenseViewEntity.CustomerId > 0 && licenseViewEntity.ExpirationInDays > 0)
            {
                var expiration = DateTime.Now.AddDays(licenseViewEntity.ExpirationInDays);
                licenseViewEntity.Product = db.Products.FirstOrDefault(x => x.Id == licenseViewEntity.ProductId);
                licenseViewEntity.Customer = db.Customers.FirstOrDefault(x => x.Id == licenseViewEntity.CustomerId);

                var newLicense = Portable.Licensing.License.New()
                    .WithUniqueIdentifier(Guid.NewGuid())
                    .As(Portable.Licensing.LicenseType.Standard)
                    .ExpiresAt(expiration)
                    .WithProductFeatures(new Dictionary<string, string>
                                                  {
                                                      {"Feature1", "true"},
                                                      {"Feature2", "false"},
                                                  })
                    .LicensedTo(licenseViewEntity.Customer.Name, licenseViewEntity.Customer.Email)
                    .CreateAndSignWithPrivateKey( licenseViewEntity.Product.PrivateKey, ConfigRepository.passPhrase);

                License license = new License();
                license.ProductId = licenseViewEntity.ProductId;
                license.CustomerId = licenseViewEntity.CustomerId;
                license.Guid = newLicense.Id.ToString();
                license.IssueDate = DateTime.Now;
                license.Expiration = expiration;
                license.Type = "standard";
                license.Signature = newLicense.Signature;

                //Clear related entities:
                license.Product = null;
                license.Customer = null;

                db.Licenses.Add(license);
                db.SaveChanges();

                //Saving license file
                var fileName = getLicenseFileName(license);
                using (var stream = new System.IO.StreamWriter(HttpContext.Server.MapPath(String.Format("{0}{1}", "~/License_files/", fileName))))
                {
                    newLicense.Save(stream);
                    stream.Close();
                }

                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", licenseViewEntity.CustomerId);
            ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", licenseViewEntity.ProductId);
            return View(licenseViewEntity);
        }

        public ActionResult Download(int id)
        {
            var license = db.Licenses.FirstOrDefault(x => x.Id == id);
            if (license != null)
            {
                var fileName = getLicenseFileName(license);
                var filePath = HttpContext.Server.MapPath(String.Format("{0}{1}", "~/License_files/",fileName));
                if (System.IO.File.Exists(filePath))
                {
                    return File(filePath, MediaTypeNames.Text.Xml, fileName);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        private string getLicenseFileName(License license)
        {
            return String.Format("{0}_{1}_{2}_{3}", license.ProductId.ToString(), license.CustomerId.ToString(), license.IssueDate.ToString("yyyyMMddHHmmss"), "license.lic");
        }
        //// GET: Licenses/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    License license = db.Licenses.Find(id);
        //    if (license == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", license.CustomerId);
        //    ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", license.ProductId);
        //    return View(license);
        //}

        //// POST: Licenses/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,ProductId,CustomerId,Guid,Type,Expiration,IssueDate,Signature")] License license)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(license).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Name", license.CustomerId);
        //    ViewBag.ProductId = new SelectList(db.Products, "Id", "Name", license.ProductId);
        //    return View(license);
        //}

        // GET: Licenses/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            License license = db.Licenses.Find(id);
            if (license == null)
            {
                return HttpNotFound();
            }
            return View(license);
        }

        // POST: Licenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            License license = db.Licenses.Find(id);
            db.Licenses.Remove(license);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
