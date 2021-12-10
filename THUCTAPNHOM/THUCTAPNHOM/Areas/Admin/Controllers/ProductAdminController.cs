using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;

namespace THUCTAPNHOM.Areas.Admin.Controllers
{
    public class ProductAdminController : Controller
    {
        Shop db = new Shop();
        // GET: Admin/Product
        public ActionResult Index()
        {
            var result = db.PRODUCTs.ToList();
            var category = db.Database.SqlQuery<CATEGORY>("exec get_all_from_CATEGORY").ToList();
            var brand = db.BRANDs.ToList();
            List<string> p1 = new List<string>();
            foreach (var item in brand)
            {
                p1.Add(item.brand_name);
            }
            Dictionary<int, string> p = new Dictionary<int, string>();
            foreach (var item in category)
            {
                p.Add(item.category_id, item.name);
            }
            ViewBag.Brand = p1;
            ViewBag.Category = p;
            return View(result);
        }

        public string GetUrl(HttpPostedFileBase file)
        {
            string path = "";

            if (ModelState.IsValid)
            {
                try
                {
                    if (file != null)
                    {
                        path = Path.Combine(Server.MapPath("/ASSETS/assets/images/product-images/"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                    }
                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while file uploading.";
                }
            }
            path = path.Replace('\\', '/');
            path = path.Substring(26);
            return path;
        }

        [HttpPost]
        public ActionResult AddProduct(FormCollection fc, HttpPostedFileBase file)
        {
            var category_id = new SqlParameter("@category_id", fc["category_id"]);
            var name = new SqlParameter("@name", fc["productname"]);
            var price = new SqlParameter("@price", fc["price"]);
            var content = new SqlParameter("@content", fc["content"]);
            var brand = new SqlParameter("@brand", fc["brand"]);
            var size = new SqlParameter("@size", fc["size"]);
            var sale_id = new SqlParameter("@sale_id", fc["sale_id"]);
            var sold = new SqlParameter("@sold", "0");
            var image_list = new SqlParameter("@image_list", "");
            string path = GetUrl(file);
            var image_link = new SqlParameter("@image_link", path.ToString());

            db.Database.ExecuteSqlCommand("AddProduct @name, @category_id, @sale_id, @price, @brand, @sold, @size, @content, @image_link, @image_list",
                                                        name, category_id, sale_id, price, brand, sold, size, content, image_link, image_list);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteProduct()
        {
            var id = new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = TempData["delete_id"] };
            db.Database.ExecuteSqlCommand("DeleteProduct @id", id);
            return View("Index");
        }

        [HttpPost]
        public ActionResult EditProduct(FormCollection fc)
        {
            return View("Index");
        }
    }
}