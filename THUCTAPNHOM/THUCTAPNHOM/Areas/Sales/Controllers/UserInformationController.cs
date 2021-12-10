using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;

namespace THUCTAPNHOM.Areas.Sales.Controllers
{
    public class UserInformationController : Controller
    {
        Shop db = new Shop();
        // GET: UserInfomation
        public ActionResult UserInformation()
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();

            var user_var = new SqlParameter("@username", ViewBag.user_logined);
            var result = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user_var).ToList();
            MEMBER user = new MEMBER();
            user = result[0];

            //          data cho minicart
            List<ItemInCart> itemlist = new List<ItemInCart>();
            itemlist = Get_Data(username, itemlist);
            ViewBag.itemlist = itemlist;

            return View(user);
        }

        public ActionResult update_information(string member_id, string name, string phone, string address)
        {
            var member_id_var = new SqlParameter("@member_id", member_id);
            var name_var = new SqlParameter("@name", name);
            var phone_number_var = new SqlParameter("@phone_number", phone);
            var address_var = new SqlParameter("@address", address);
            var result = db.Database.ExecuteSqlCommand("exec update_MEMBER_information @member_id, @name, @phone_number, @address", member_id_var, name_var, phone_number_var, address_var);
            return Content("1");
        }

        public ActionResult Change_Password()
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];

            var user_var = new SqlParameter("@username", ViewBag.user_logined);
            var result = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user_var).ToList();
            MEMBER user = new MEMBER();
            user = result[0];
            return View(user);
        }
        public ActionResult Pass_Change_Action(string user, string a, string b, string c)
        {
            string passhash = MD5Hash(a);
            var pass_var = new SqlParameter("@password", passhash);
            var user_var = new SqlParameter("@username", user);
            var user_var2 = new SqlParameter("@username", user);
            var new_pass_var = new SqlParameter("@newpass", MD5Hash(b));
            var result = db.Database.SqlQuery<MEMBER>("exec getMEMBERfromusernameandpass @username, @password", user_var, pass_var).ToList();
            if (result.Count() != 1)
            {
                return Content("1");
            }
            else
            {
                if (b.Trim() == c.Trim())
                {
                    var result2 = db.Database.ExecuteSqlCommand("exec change_password @username, @newpass", user_var2, new_pass_var);
                    return Content("0");
                }
                else
                {
                    return Content("2");
                }

            }
        }


        public string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        public List<ItemInCart> Get_Data(string username, List<ItemInCart> itemincartlist)
        {
            var user = new SqlParameter("@username", username);
            var result_member = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user).ToList();
            int cart_id = result_member[0].member_id;

            // Lấy cart của member đó và từ đó lấy ra những Item nằm trong cart đó.
            var cart_id_var = new SqlParameter("@cart_id", cart_id);
            var result_cart_item = db.Database.SqlQuery<CART_ITEM>("exec get_CART_ITEM_from_cart_id @cart_id", cart_id_var).ToList();

            // Thêm những item đó vào 1 list rồi gửi sang View
            List<CART_ITEM> cart_itemlist = new List<CART_ITEM>();
            for (int i = 0; i < result_cart_item.Count(); i++)
            {
                cart_itemlist.Add(result_cart_item[i]);
            }

            //          Tạo list product tương ứng với list cart_item
            List<PRODUCT> productlist = new List<PRODUCT>();
            foreach (var a in cart_itemlist)
            {
                var p = new SqlParameter("@product_id", a.product_id);
                var result_product = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_product_id @product_id", p).ToList();
                productlist.Add(result_product[0]);
            }
            //          Tạo list ItemInCart để hiển thị trong cart
            for (int i = 0; i < cart_itemlist.Count(); i++)
            {
                ItemInCart a = new ItemInCart();
                a.product_id = Int32.Parse(cart_itemlist[i].product_id.ToString());
                a.price = productlist[i].price;
                a.name = productlist[i].name;
                a.qty = Int32.Parse(cart_itemlist[i].qty.ToString());
                a.size = cart_itemlist[i].size;
                a.image_link = productlist[i].image_link;
                itemincartlist.Add(a);
            }
            return itemincartlist;
        }
    }
}