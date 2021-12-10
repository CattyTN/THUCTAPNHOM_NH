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

        [HttpPost]
        public ActionResult AddReport(FormCollection fc)
        {
            var tran_id = new SqlParameter("@tran_id", System.Data.SqlDbType.Int) { Value = TempData["tran_id"] };
            var employee_id = new SqlParameter("@employee_id", System.Data.SqlDbType.Int) { Value = fc["employee_id"] };
            var mem_id = new SqlParameter("@mem_id", System.Data.SqlDbType.Int) { Value = TempData["mem_id"] };
            var amount = new SqlParameter("@amount", System.Data.SqlDbType.Int) { Value = TempData["amount"] };
            var qty = new SqlParameter("@qty", System.Data.SqlDbType.Int) { Value = TempData["qty"] };
            var product_id = new SqlParameter("@product_id", System.Data.SqlDbType.Int) { Value = TempData["product_id"] };
            var date = new SqlParameter("@date", fc["date"]);
            var result = db.REPORTs.SqlQuery("CheckReport @tran_id", tran_id).ToList();
            if (result.Count() != 0)
            {
                ViewBag.Message = "Giao dịch này đã được báo cáo!";
                var ret = db.TRANSACTIONs.ToList();
                return View("~/Areas/Admin/Views/Order/Index.cshtml", ret);
            }

            db.Database.ExecuteSqlCommand("AddReport @tran_id, @employee_id, @mem_id,  @amount, @qty, @product_id, @date",
                                                    tran_id, employee_id, mem_id, amount, qty, product_id, date);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(FormCollection fc)
        {
            var type = new SqlParameter("@type", fc["type"]);
            if (type.Value.ToString() == "3")
            {
                var result = db.TRANSACTIONs.ToList();
                return View("Index", result);
            }
            else
            {
                var result = db.TRANSACTIONs.SqlQuery("FilterTransaction @type", type).ToList();
                return View("~/Areas/Admin/Views/Order/Index.cshtml", result);
            }
        }
    }
}