using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;

namespace THUCTAPNHOM.Areas.Admin.Controllers
{
    public class CategoryAdminController : Controller
    {
        Shop db = new Shop();
        // GET: Admin/Category
        public ActionResult Index()
        {
            var result = db.CATEGORies.ToList();
            return View(result);
        }

        [HttpPost]
        public ActionResult AddCategory(FormCollection fc)
        {
            var name = new SqlParameter("@name", fc["category"]);
            var group_id = new SqlParameter("@group_id", fc["group_id"]);
            db.Database.ExecuteSqlCommand("AddCategory @name, @group_id", name, group_id);

            return RedirectToAction("~/Areas/Admin/Views/Category/Index.cshtml");
        }

        [HttpPost]
        public ActionResult EditCategory(FormCollection fc)
        {
            var id = new SqlParameter("@id", TempData["category_id"]);
            var name = new SqlParameter("@name", fc["name"]);
            var group_id = new SqlParameter("@group_id", fc["group_id"]);

            db.Database.ExecuteSqlCommand("EditCategory @id, @name, @group_id", id, name, group_id);
            return View("~/Areas/Admin/Views/Category/Index.cshtml");
        }

        [HttpPost]
        public ActionResult DeleteCategory()
        {
            var id = new SqlParameter("@id", TempData["category_id"]);
            db.Database.ExecuteSqlCommand("DeleteCategory @id", id);
            return View("~/Areas/Admin/Views/Category/Index.cshtml");
        }
    }
}