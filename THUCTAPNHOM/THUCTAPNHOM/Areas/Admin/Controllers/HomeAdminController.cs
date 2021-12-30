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
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            var mem = db.MEMBERs.ToList();
            var order = db.Database.SqlQuery<TRANSACTION>("SelectFinishTransaction").ToList();
            var item = db.ITEM_SOLDs.ToList();
            //  Số lượng thành viên
            ViewBag.Member_count = mem.Count();
            //  Tổng số đơn hàng
            ViewBag.Order_count = order.Count();
            int qty = 0, total = 0;

            foreach (var o in order)
            {
                total += o.amount;
            }

            foreach (var o in item)
            {
                qty += o.qty;

            }
            //  Số sản phẩm đã bán và tổng doanh thu
            ViewBag.Amount = qty;
            ViewBag.Total = total;

            var topproduct = db.PRODUCTs.SqlQuery("exec SelectTopProduct").ToList();
            ViewBag.TopProduct = topproduct;

            var category = db.CATEGORies.ToArray();
            Dictionary<int, string> p = new Dictionary<int, string>();
            foreach (var c in category)
            {
                p.Add(c.category_id, c.name);
            }
            ViewBag.Category = p;

            var topmem = db.Database.SqlQuery<Mem_Cart>("exec SelectTopMember").ToList();
            ViewBag.TopMem = topmem;
            return View();
        }

    }
}