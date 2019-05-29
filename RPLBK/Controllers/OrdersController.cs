using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RPLBK.Models;
using kodeunik;

namespace RPLBK.Controllers
{
    public class OrdersController : Controller
    {
        private BananaEntities db = new BananaEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Services).Include(o => o.Users);
            if (Session["email"].ToString() == "admin@gmail.com")
            {
                return View(orders.ToList());
            }
            else
            {
                //(Session["email"] == orders.ToList().Where(user => user.id_user != 9))
                var id_users = Convert.ToInt32(Session["id"]);
                var order = db.Orders.Include(r => r.Users).Where(r=>r.id_user == id_users);
                //var reviews = db.Reviews.Include(r => r.Restaurant).Include(r => r.User).Where(r => r.User.UserId == test)
                return View(order.ToList());
            }
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create/5
        public ActionResult Create(int? id)
        {
            //Services service = new Services();

            ViewBag.id_layanan = new SelectList(db.Services, "id", "jenis");
            ViewBag.id_user = new SelectList(db.Users, "id", "nama");
            
           // Services service = db.Services.Find(id);
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orders orders, int id)
        {


            if (ModelState.IsValid)
            {
                //string query = "SELECT * FROM Services WHERE id=@id";
                Services service = db.Services.Find(id);
                orders.id_layanan = id;
                //Session["email"] = user.email.ToString();
                orders.id_user = Convert.ToInt32(Session["id"]);
                orders.tanggaldipesan = DateTime.Now;
                orders.status = "Request";
                orders.kode = "NULL";
                orders.biaya = service.harga * orders.jumlah;
                db.Orders.Add(orders);

                


                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.id_layanan = new SelectList(db.Services, "id", "foto", orders.id_layanan);
            //ViewBag.id_user = new SelectList(db.Users, "id", "nama", orders.id_user);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.id_layanan = new SelectList(db.Services, "id", "foto", orders.id_layanan);
            ViewBag.id_user = new SelectList(db.Users, "id", "nama", orders.id_user);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,id_user,id_layanan,tanggalpemesanan,tanggaldipesan,biaya,status")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_layanan = new SelectList(db.Services, "id", "foto", orders.id_layanan);
            ViewBag.id_user = new SelectList(db.Users, "id", "nama", orders.id_user);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
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
        public ActionResult Terima(int? id)
        {
            if (id==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders ==null)
            {
                return HttpNotFound(); 
            }
            Class1 generator = new Class1();
            string kode = generator.KodeUnik("TBN");

            orders.status = "Diterima";
            orders.kode = kode;
            db.Entry(orders).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Tolak(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            //Class1 generator = new Class1();
            //string kode = generator.KodeUnik("TBN");

            orders.status = "Ditolak";
            //orders.kode = kode;
            db.Entry(orders).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
