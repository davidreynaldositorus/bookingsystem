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
    public class HomeController : Controller
    {
        private BananaEntities db = new BananaEntities();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Orders()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Services()
        {
            var services = db.Services.Include(s => s.Types);
            return View(services.ToList().OrderByDescending(r => r.id));
        }
    }
}