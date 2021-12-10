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
    public class LoginController : Controller
    {
        Shop db = new Shop();
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            HttpContext.Application["is_logined"] = 0;
            HttpContext.Application["user_logined"] = "";
            HttpContext.Application["user_name"] = "";
            return View("~/Areas/Sales/Views/Login/Login.cshtml");
        }
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string username = form.Get("customer[email]").ToString();
            string pass = form.Get("customer[password]").ToString();
            var u = new SqlParameter("@username", username);
            var p = new SqlParameter("@password", MD5Hash(pass));
            var result = db.Database.SqlQuery<MEMBER>("exec getMEMBERfromusernameandpass @username, @password", u, p).ToList();
            int check = result.Count();
            if (check != 0)
            {
                HttpContext.Application["user_logined"] = username;
                HttpContext.Application["user_name"] = result[0].name;
                HttpContext.Application["is_logined"] = 1;
                ViewBag.user_logined = HttpContext.Application["user_logined"];
                ViewBag.is_logined = HttpContext.Application["is_logined"];
                ViewBag.user_name = HttpContext.Application["user_name"];

                List<ItemInCart> itemlist = new List<ItemInCart>();
                Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;

                int doda = 2;
                var id_var = new SqlParameter("@group_id", doda);
                var result_product = db.Database.SqlQuery<PRODUCT>("exec get_product_from_PRODUCT_GROUP @group_id", id_var).ToList();
                int qty = result_product.Count();
                List<PRODUCT> product1list = new List<PRODUCT>();
                List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

                for (int i = 0; i < qty; i++)
                {
                    product1list.Add(result_product[i]);
                }
                ViewBag.qty = qty;

                Mix_PRODUCT_And_PRODUCT_Plus(product1list, productpluslist);
                if (result[0].role == 0)
                {
                    ViewBag.user_role = 1;
                    return View("~/Areas/Admin/Views/HomeAdmin/Index.cshtml");
                }
                return View("~/Areas/Sales/Views/Home/Home.cshtml", productpluslist);

            }
            else
            {
                return View("~/Areas/Sales/Views/Login/Login.cshtml");
            }
        }
        public ActionResult Logout()
        {
            HttpContext.Application["is_logined"] = 0;
            return View("~/Areas/Sales/Views/Login/Login.cshtml");
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

        public void Mix_PRODUCT_And_PRODUCT_Plus(List<PRODUCT> productlist, List<PRODUCT_Plus> productpluslist)
        {

            var result_sale = db.Database.SqlQuery<SALE>("exec get_all_from_SALES").ToList();
            foreach (var a in productlist)
            {
                PRODUCT_Plus c = new PRODUCT_Plus();
                c.product_id = a.product_id;
                c.category_id = a.category_id;
                c.sale_id = a.sale_id;
                c.name = a.name;
                c.price = a.price;
                c.brand = a.brand;
                c.sold = a.sold;
                c.size = a.size;
                c.content = a.content;
                c.image_link = a.image_link;
                foreach (var b in result_sale)
                {
                    if (b.sale_id == a.sale_id)
                    {
                        c.sale_name = b.sale_name;
                        c.percent = b.percent;
                    }
                }
                productpluslist.Add(c);
            }
        }

        // Getdata for ItemInCart
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