using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;

namespace THUCTAPNHOM.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        Shop db = new Shop();
        // GET: Admin/Category
       
        public ActionResult Index()
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            var result = db.CATEGORies.ToList();
            return View(result);
        }

        [HttpPost]
       
        public ActionResult AddCategory(FormCollection fc)
        {
            var cate = db.CATEGORies.ToList();
            var id = new SqlParameter("@id", cate.Last().category_id + 1);
            var name = new SqlParameter("@name", fc["category"]);
            var group_id = new SqlParameter("@group_id", fc["group_id"]);

            db.Database.ExecuteSqlCommand("AddCategory @id, @name, @group_id", id, name, group_id);

            return RedirectToAction("Index");
        }
       
        [HttpGet]
        public ActionResult EditCategory(string category_id, string category_name)
        {
            var id = new SqlParameter("@id", category_id);
            var name = new SqlParameter("@name", category_name);
            //var group_id = new SqlParameter("@group_id", fc["group_id"]);

            db.Database.ExecuteSqlCommand("EditCategory @id, @name", id, name);
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            var result = db.CATEGORies.ToList();
            return RedirectToAction("Index");
        }
        
        [HttpGet]
        public ActionResult DeleteCategory(string delete_id)
        {
            var id = new SqlParameter("@id", delete_id);
            db.Database.ExecuteSqlCommand("DeleteCategory @id", id);
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Filter(string filter)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            //  Lọc danh mục theo nhóm sản phẩm
            var type = new SqlParameter("@type", filter);
            if (type.Value.ToString() == "3")
            {
                var result = db.CATEGORies.ToList();
                return View("Index", result);
            }
            else
            {
                var result = db.CATEGORies.SqlQuery("FilterCategory @type", type).ToList();
                return View("Index", result);
            }
        }
    }
}