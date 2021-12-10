using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;

namespace THUCTAPNHOM.Areas.Admin.Controllers
{
    public class HomeAdminController : Controller
    {
        Shop db = new Shop();
        // GET: Admin/Home
        public ActionResult Index()
        {
            var mem = db.MEMBERs.ToList();
            var order = db.TRANSACTIONs.ToList();
            ViewBag.Member_count = mem.Count();
            ViewBag.Order_count = order.Count();
            int amount = 0, total = 0;
            foreach (var item in order)
            {
                amount += item.amount;

            }
            ViewBag.Amount = amount;
            ViewBag.Total = total;

            return View();
        }

    }
}