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

namespace LicenseServer.Controllers
{
    public class LicenseFeaturesController : Controller
    {
        private DataContext db = new DataContext();

        // GET: LicenseFeatures
        public ActionResult Index()
        {
            return View(db.LicenseFeatures.ToList());
        }

        // GET: LicenseFeatures/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicenseFeature licenseFeature = db.LicenseFeatures.Find(id);
            if (licenseFeature == null)
            {
                return HttpNotFound();
            }
            return View(licenseFeature);
        }

        // GET: LicenseFeatures/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LicenseFeatures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsAllowed")] LicenseFeature licenseFeature)
        {
            if (ModelState.IsValid)
            {
                db.LicenseFeatures.Add(licenseFeature);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(licenseFeature);
        }

        // GET: LicenseFeatures/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicenseFeature licenseFeature = db.LicenseFeatures.Find(id);
            if (licenseFeature == null)
            {
                return HttpNotFound();
            }
            return View(licenseFeature);
        }

        // POST: LicenseFeatures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsAllowed")] LicenseFeature licenseFeature)
        {
            if (ModelState.IsValid)
            {
                db.Entry(licenseFeature).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(licenseFeature);
        }

        // GET: LicenseFeatures/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LicenseFeature licenseFeature = db.LicenseFeatures.Find(id);
            if (licenseFeature == null)
            {
                return HttpNotFound();
            }
            return View(licenseFeature);
        }

        // POST: LicenseFeatures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LicenseFeature licenseFeature = db.LicenseFeatures.Find(id);
            db.LicenseFeatures.Remove(licenseFeature);
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
