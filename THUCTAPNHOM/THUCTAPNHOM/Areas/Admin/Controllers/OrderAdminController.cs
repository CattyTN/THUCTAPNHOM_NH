
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;

namespace THUCTAPNHOM.Areas.Admin.Controllers
{
    public class OrderAdminController : Controller
    {
        Shop db = new Shop();
        // GET: Admin/Order
        public ActionResult Index()
        {
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            var result = db.TRANSACTIONs.ToList();
            return View(result);
        }

        [HttpPost]
        public ActionResult EditOrder()
        {
            var id = new SqlParameter("@id", TempData["Tran_ID"]);
            var status = new SqlParameter("@status", 1);
            db.Database.ExecuteSqlCommand("UpdateTransactionStatus @id, @status", id, status);
            return RedirectToAction("~/Areas/Admin/Views/Order/Index.cshtml");
        }

        public ActionResult Confirm_Order(string id)
        {
            var tran_id = new SqlParameter("@id", id);
            var result = db.Database.ExecuteSqlCommand("exec confirm_TRANSACTION @id", tran_id);
            return Content("1");
        }

        public ActionResult Report(string id)
        {
            var tran_id = new SqlParameter("@id", id);
            var tran_id_2 = new SqlParameter("@id_2", id);
            var result = db.TRANSACTIONs.SqlQuery("get_TRANSACTION_from_transaction_id @id", tran_id).ToList();
            var product = db.Database.SqlQuery<Order_Products>("get_PRODUCT_from_TRANSACTION @id_2", tran_id_2).ToList();
            ViewBag.Product_List = product;
            return View(result);
        }

        [HttpPost]
        public ActionResult Filter_Order()
        {

            var order_status = Request.Form["order_property"];
            List<TRANSACTION> order_list = new List<TRANSACTION>();
            var all_transaction = db.TRANSACTIONs.ToList();
            foreach (var a in all_transaction)
            {
                if (a.status.ToString().Trim() == order_status.Trim())
                {
                    order_list.Add(a);
                }
            }
            return View("Index", order_list);
        }
    }

}