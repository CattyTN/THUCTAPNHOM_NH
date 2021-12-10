using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;
using System.Data.SqlClient;

namespace THUCTAPNHOM.Areas.Sales.Controllers
{
    public class HomeController : Controller
    {
        Shop db = new Shop();
        [HttpGet]
        public ActionResult Home()
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();

            int doda = 2;
            var id_var = new SqlParameter("@group_id", doda);
            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_from_PRODUCT_GROUP @group_id", id_var).ToList();
            int qty = result.Count();
            List<PRODUCT> product1list = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            for (int i = 0; i < qty; i++)
            {
                product1list.Add(result[i]);
            }
            ViewBag.qty = qty;
            Mix_PRODUCT_And_PRODUCT_Plus(product1list, productpluslist);
            if(ViewBag.is_logined == 1) { 
                List<ItemInCart> itemlist = new List<ItemInCart>();
                itemlist = Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;
            }

            return View("~/Areas/Sales/Views/Home/Home.cshtml", productpluslist);
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