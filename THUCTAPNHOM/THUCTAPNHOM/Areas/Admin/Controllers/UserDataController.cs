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
    public class UserDataController : Controller
    {
        Shop db = new Shop();
        // GET: Admin/UserData
        public ActionResult Index()
        {
            var result = db.MEMBERs.ToList();
            return View(result);
        }

        public ActionResult UserInfor()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DeleteMember()
        {
            var id = new SqlParameter("@id", System.Data.SqlDbType.Int) { Value = TempData["delete_id"] };
            db.Database.ExecuteSqlCommand("DeleteMember @id", id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult UpdateMember(FormCollection fc)
        {
            var id = new SqlParameter("@id", fc["id"]);
            var password = new SqlParameter("@password", fc["password"]);
            var phone = new SqlParameter("@phone", fc["phone"]);
            var address = new SqlParameter("@address", fc["address"]);

            db.Database.ExecuteSqlCommand("UpdateMember @id, @password, @phone, @address", id, password, phone, address);
            return View("UserInfor");
        }
    }
}