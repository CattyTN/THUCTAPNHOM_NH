using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;
using System.Data.SqlClient;
namespace THUCTAPNHOM.Areas.Sales.Controllers
{
    public class ProductController : Controller
    {
        Shop db = new Shop();
        // GET: Product
        public ActionResult Product()
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var result_product = db.Database.SqlQuery<PRODUCT>("exec selectallfromPRODUCT").ToList();
            int qty = result_product.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result_product[i]);
            }

            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;

            //          Dữ liệu cho mini cart
            if(ViewBag.is_logined == 1)
            {
                List<ItemInCart> itemlist = new List<ItemInCart>();
                itemlist = Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;
            }
            return View(productpluslist);
        }
        public ActionResult Product_Detail(string product_id)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();

            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var result_product = db.Database.SqlQuery<PRODUCT>("exec selectallfromPRODUCT").ToList();
            int qty = result_product.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result_product[i]);
            }
            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;
            ViewBag.product_id = product_id;

            //  Lấy review

            var product_id_var = new SqlParameter("@product_id", product_id);
            var result_review = db.Database.SqlQuery<REVIEW>("exec get_REVIEW_from_product_id @product_id", product_id_var).ToList();

            List<REVIEW> reviewlist = new List<REVIEW>();
            foreach (REVIEW a in result_review)
            {
                reviewlist.Add(a);
            }
            ViewBag.review = reviewlist;
            ViewBag.review_qty = reviewlist.Count();

            //          Dữ liệu cho mini cart
            if (ViewBag.is_logined == 1)
            {
                List<ItemInCart> itemlist = new List<ItemInCart>();
                itemlist = Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;
            }

            return View("~/Areas/Sales/Views/Product/Product_Detail.cshtml", productpluslist);
        }

        public ActionResult Add_To_Cart1(string product_id)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            List<PRODUCT> productlist = new List<PRODUCT>();
            string user = ViewBag.user_logined;
            string size = "S";
            int item_qty = 1;
            Add_To_Cart(user, Int32.Parse(product_id), item_qty, size, productlist);
            return View("~/Areas/Sales/Views/Product/Product.cshtml", productlist);
        }
        [HttpPost]
        public ActionResult Add_To_Cart2(FormCollection form, string product_id)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var result = db.Database.SqlQuery<PRODUCT>("exec selectallfromPRODUCT").ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            ViewBag.qty = qty;

            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);

            if (ViewBag.is_logined == 1)
            {
                string user = ViewBag.user_logined;
                string size = form.Get("option-1");
                if (size == null)
                {
                    size = "S";
                }
                int item_qty = Int32.Parse(form.Get("quantity").ToString());
                Add_To_Cart(user, Int32.Parse(product_id), item_qty, size, productlist);
            }
            else
            {

            }
            ViewBag.product_id = product_id;
            return View("~/Areas/Sales/Views/Product/Product_Detail.cshtml", productpluslist);
        }

        public ActionResult Get_Product_Base_On_Product_Group(string group_id)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();

            int id_int = Int32.Parse(group_id);
            var id_var = new SqlParameter("@group_id", id_int);

            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_from_PRODUCT_GROUP @group_id", id_var).ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }

            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;
            //          data cho minicart
            if (ViewBag.is_logined == 1)
            {
                List<ItemInCart> itemlist = new List<ItemInCart>();
                itemlist = Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;
            }

            return View("~/Areas/Sales/Views/Product/Product.cshtml", productpluslist);
        }
        public ActionResult Get_Product_Base_On_Price(int along)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();


            var along_var = new SqlParameter("@along", along);

            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_base_on_price @along", along_var).ToList();

            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }

            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;

            //          data cho minicart
            if (ViewBag.is_logined == 1)
            {
                List<ItemInCart> itemlist = new List<ItemInCart>();
                itemlist = Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;
            }

            return View("~/Areas/Sales/Views/Product/Product.cshtml", productpluslist);
        }

        public ActionResult Get_Product_Base_On_Brand(string brand)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();

            var brand_var = new SqlParameter("@brand", brand);
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_base_on_brand @brand", brand_var).ToList();


            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }

            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;
            //          data cho minicart
            if (ViewBag.is_logined == 1)
            {
                List<ItemInCart> itemlist = new List<ItemInCart>();
                itemlist = Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;
            }
            return View("~/Areas/Sales/Views/Product/Product.cshtml", productpluslist);
        }

        public ActionResult Get_Product_Base_On_Category(string name)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();

            var name_var = new SqlParameter("@name", name);

            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();

            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_from_CATEGORY @name", name_var).ToList();

            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }

            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;

            //          data cho minicart
            if (ViewBag.is_logined == 1)
            {
                List<ItemInCart> itemlist = new List<ItemInCart>();
                itemlist = Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;
            }

            return View("~/Areas/Sales/Views/Product/Product.cshtml", productpluslist);
        }

        public void Add_To_Cart(string user, int product_id, int item_qty, string size, List<PRODUCT> productlist)
        {

            //          Lấy tất cả sản phẩm cho vào 1 list, để sau khi thêm product vào item, quay lại view vẫn hiển thị được full product
            var result = db.Database.SqlQuery<PRODUCT>("exec selectallfromPRODUCT").ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }
            ViewBag.qty = qty;
            ViewBag.product_id = product_id;

            //          Lấy thông tin member đang đăng nhập và product đã được chọn (để tạo item)
            var product_id_var = new SqlParameter("@product_id", product_id);
            var member_username = new SqlParameter("@username", user);
            var result_product = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_product_id @product_id", product_id_var).ToList();
            var result_member = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", member_username).ToList();
            string price = result_product[0].price.ToString();
            string cart_id = result_member[0].member_id.ToString();

            bool is_exist = false;
            //          Kiểm tra xem đã tồn tại sản phẩm đó trong giỏ hàng chưa
            var product_id_var3 = new SqlParameter("@product_id", product_id);
            var cart_id_var2 = new SqlParameter("@cart_id", cart_id);
            var result_check = db.Database.SqlQuery<CART_ITEM>("exec get_CART_ITEM_from_cart_id_and_product_id @cart_id, @product_id", cart_id_var2, product_id_var3).ToList();
            if (result_check.Count() > 0)
            {
                foreach (var a in result_check)
                {
                    if (a.size.ToString().Trim() == size.ToString().Trim())
                    {
                        int new_qty = Int32.Parse(result_check[0].qty.ToString());
                        new_qty = new_qty + 1;
                        var new_qty_var = new SqlParameter("@new_qty", new_qty);
                        var product_id_var4 = new SqlParameter("@product_id", product_id);
                        var size_var = new SqlParameter("@size", a.size);
                        var result_change = db.Database.ExecuteSqlCommand("exec change_CART_ITEM_qty @product_id, @size, @new_qty", product_id_var4, size_var, new_qty_var);
                        is_exist = true;
                    }
                }
            }
            if (is_exist == false)
            {
                //          Tạo ITEM bằng thông tin của member(lấy cart_id) và product đó
                var product_id_var2 = new SqlParameter("@product_id", product_id);
                var price_var = new SqlParameter("@price", price);
                var cart_id_var = new SqlParameter("@cart_id", cart_id);
                var qty_var = new SqlParameter("@qty", item_qty);
                var size_var = new SqlParameter("@size", size);
                var result_exec = db.Database.ExecuteSqlCommand("exec create_CART_ITEM @cart_id, @product_id, @qty, @price, @size", cart_id_var, product_id_var2, qty_var, price_var, size_var);
            }
        }


        public ActionResult Add_To_CartAjax(string id)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            List<PRODUCT> productlist = new List<PRODUCT>();
            string user = ViewBag.user_logined;
            string size = "S";
            int item_qty = 1;
            Add_To_Cart(user, Int32.Parse(id), item_qty, size, productlist);
            return Content("1");
        }

        public ActionResult Search(string key_word)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            var key_word_var = new SqlParameter("@key_word", key_word);
            string username = HttpContext.Application["user_logined"].ToString().Trim();

            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var result = db.Database.SqlQuery<PRODUCT>("exec get_PRODUCT_from_key_word @key_word", key_word_var).ToList();

            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                productlist.Add(result[i]);
            }

            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = qty;

            //          data cho minicart
            if (ViewBag.is_logined == 1)
            {
                List<ItemInCart> itemlist = new List<ItemInCart>();
                itemlist = Get_Data(username, itemlist);
                ViewBag.itemlist = itemlist;
            }

            return View("~/Areas/Sales/Views/Product/Product.cshtml", productpluslist);
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

        public ActionResult Filter(FormCollection f)
        {
            ViewBag.user_logined = HttpContext.Application["user_logined"];
            ViewBag.is_logined = HttpContext.Application["is_logined"];
            ViewBag.user_name = HttpContext.Application["user_name"];
            string username = HttpContext.Application["user_logined"].ToString().Trim();

            var price = f.Get("option-price");
            var brand_string = f.Get("price_checkbox");
            List<PRODUCT> productlist = new List<PRODUCT>();
            List<PRODUCT_Plus> productpluslist = new List<PRODUCT_Plus>();
            var result_product = db.Database.SqlQuery<PRODUCT>("exec selectallfromPRODUCT").ToList();

            int price_begin = 1;
            int price_end = 1;

            if (price == null && brand_string == null)
            {
                for (int i = 0; i < result_product.Count(); i++)
                {
                    productlist.Add(result_product[i]);
                }
            }

            else if (price != null && brand_string == null)
            {
                if (f.Get("option-price") == "1")
                {
                    price_begin = 1000000;
                    price_end = 5000000;
                }
                if (f.Get("option-price") == "2")
                {
                    price_begin = 5000000;
                    price_end = 10000000;
                }
                if (f.Get("option-price") == "3")
                {
                    price_begin = 10000000;
                    price_end = 15000000;
                }
                if (f.Get("option-price") == "4")
                {
                    price_begin = 15000000;
                    price_end = 20000000;
                }
                for (int i = 0; i < result_product.Count(); i++)
                {
                    if ((result_product[i].price < price_end) && (result_product[i].price > price_begin))
                    {
                        productlist.Add(result_product[i]);
                    }
                }

            }

            else if (price == null && brand_string != null)
            {
                for (int i = 0; i < result_product.Count(); i++)
                {
                    if (brand_string.Contains(result_product[i].brand) == true)
                    {
                        productlist.Add(result_product[i]);
                    }
                }
            }
            else if (price != null && brand_string != null)
            {
                if (f.Get("option-price") == "1")
                {
                    price_begin = 1000000;
                    price_end = 5000000;
                }
                if (f.Get("option-price") == "2")
                {
                    price_begin = 5000000;
                    price_end = 10000000;
                }
                if (f.Get("option-price") == "3")
                {
                    price_begin = 10000000;
                    price_end = 15000000;
                }
                if (f.Get("option-price") == "4")
                {
                    price_begin = 15000000;
                    price_end = 20000000;
                }

                for (int i = 0; i < result_product.Count(); i++)
                {
                    if ((brand_string.Contains(result_product[i].brand) == true) && (result_product[i].price < price_end) && (result_product[i].price > price_begin))
                    {
                        productlist.Add(result_product[i]);
                    }
                }
            }
            Mix_PRODUCT_And_PRODUCT_Plus(productlist, productpluslist);
            ViewBag.qty = productlist.Count();
            //          data cho minicart
            List<ItemInCart> itemlist = new List<ItemInCart>();
            itemlist = Get_Data(username, itemlist);
            ViewBag.itemlist = itemlist;

            return View("~/Areas/Sales/Views/Product/Product.cshtml", productpluslist);
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

        public ActionResult Get_Cart_Qty(string user_logined)
        {
            var user = new SqlParameter("@username", user_logined);
            var result_member = db.Database.SqlQuery<MEMBER>("exec get_MEMBER_from_username @username", user).ToList();
            int cart_id = result_member[0].member_id;

            // Lấy cart của member đó và từ đó lấy ra những Item nằm trong cart đó.
            var cart_id_var = new SqlParameter("@cart_id", cart_id);
            var result_cart_item = db.Database.SqlQuery<CART_ITEM>("exec get_CART_ITEM_from_cart_id @cart_id", cart_id_var).ToList();

            var qty = result_cart_item.Count();
            return Content(qty.ToString());
        }

        public ActionResult Add_Review_Check(string product_id, string username)
        {
            bool check = false;
            var username_var = new SqlParameter("@username", username);
            var result_transaction = db.Database.SqlQuery<TRANSACTION>("exec get_TRANSACTION_from_username @username", username_var).ToList();
            int[] check_arr = new int[20];
            for (int k = 0; k < 20; k++)
            {
                check_arr[k] = 0;
            }
            int i = 0;
            foreach (TRANSACTION a in result_transaction)
            {
                var transaction_id_var = new SqlParameter("@transaction_id", a.transaction_id);
                var product_id_var = new SqlParameter("@product_id", product_id);
                check_arr[i] = db.Database.SqlQuery<int>("exec check_ITEM_in_TRANSACTION @transaction_id, @product_id", transaction_id_var, product_id_var).FirstOrDefault();
                if (check_arr[i] == 1)
                {
                    check = true;
                    break;
                }
            }
            if (check == true)
            {
                return Content("1");
            }
            else
            {
                return Content("0");
            }
        }
        public ActionResult Add_Review(string username, string product_id, string review)
        {
            DateTime date = DateTime.Now;
            string a = date.ToString("dd/MM/yyyy");
            var datetime_var = new SqlParameter("@date_post", a);
            var username_var = new SqlParameter("@username", username);
            var product_id_var = new SqlParameter("@product_id", product_id);
            var review_var = new SqlParameter("@content", review);
            var result = db.Database.ExecuteSqlCommand("exec create_REVIEW @content, @username, @product_id, @date_post", review_var, username_var, product_id_var, datetime_var);
            return Content("1");
        }
    }
}