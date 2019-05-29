using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RPLBK.Models;
using System.IO;

namespace RPLBK.Controllers
{
    public class ServicesController : Controller
    {
        private BananaEntities db = new BananaEntities();

        // GET: Services
        public ActionResult Index()
        {
            var services = db.Services.Include(s => s.Types);
            return View(services.ToList());
        }

        public ActionResult Services()
        {
            var services = db.Services.Include(s => s.Types);
            return View(services.ToList().OrderByDescending(r => r.id));
        }

        // GET: Services/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            return View(services);
        }

        // GET: Services/Create
        public ActionResult Create()
        {
            ViewBag.jenis = new SelectList(db.Types, "id", "jenis");
            return View();
        }

        // POST: Services/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,jenis,durasi,kapasitas,harga,foto")] Services services, HttpPostedFileBase FileUpload)
        {
            if (ModelState.IsValid)
            {
                string file_name = Path.GetFileName(FileUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Template/img"), file_name);
                FileUpload.SaveAs(path);

                services.foto = file_name;
                db.Services.Add(services);
                db.SaveChanges();
                return RedirectToAction("Services");
            }

            ViewBag.jenis = new SelectList(db.Types, "id", "jenis", services.jenis);
            return View(services);
        }

        // GET: Services/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            ViewBag.jenis = new SelectList(db.Types, "id", "jenis", services.jenis);
            return View(services);
        }

        // POST: Services/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Services services, HttpPostedFileBase FileUpload)
        {
            if (ModelState.IsValid)
            {
                string file_name = Path.GetFileName(FileUpload.FileName);
                string path = Path.Combine(Server.MapPath("~/Content/Template/img"), file_name);
                FileUpload.SaveAs(path);

                services.foto = file_name;
                db.Services.Add(services);
                db.Entry(services).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Services");
            }
            ViewBag.jenis = new SelectList(db.Types, "id", "jenis", services.jenis);
            return View(services);
        }

        // GET: Services/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Services services = db.Services.Find(id);
            if (services == null)
            {
                return HttpNotFound();
            }
            db.Services.Remove(services);
            db.SaveChanges();
            return RedirectToAction("Services");
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
