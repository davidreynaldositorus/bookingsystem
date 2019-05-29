using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RPLBK.Models;

namespace RPLBK.Controllers
{
    public class UsersController : Controller
    {
        private BananaEntities db = new BananaEntities();
        

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,nama,email,nomorhp,password")] Users users)
        {
            if (ModelState.IsValid)
            {
                using(BananaEntities db = new BananaEntities())
                {
                    db.Users.Add(users);
                    db.SaveChanges();
                }
                ModelState.Clear();
                
            }

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

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(Users user)
        {
            if (ModelState.IsValid)
            {
                using (BananaEntities db = new BananaEntities())
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                }
                ModelState.Clear();
                
            }
            Session["id"] = user.id.ToString();
            Session["email"] = user.email.ToString();
            return RedirectToAction("Index", "Home");
        }

        //LogIn
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["id"] = null;
            Session["email"] = null;
            return RedirectToAction("Index", "Home");
        }



        [HttpPost]
        public ActionResult Login(Users users)
        {
            using (BananaEntities db = new BananaEntities())
            {
                           
            var usr = db.Users.Single(u => u.email == users.email && u.password == users.password);
                if (usr != null)
                {
                    Session["id"] = usr.id.ToString();
                    Session["email"] = usr.email.ToString();
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    ModelState.AddModelError("", "Email atau Password yang anda masukkan salah");
                }
            }
            return View();
        }

    }
}
