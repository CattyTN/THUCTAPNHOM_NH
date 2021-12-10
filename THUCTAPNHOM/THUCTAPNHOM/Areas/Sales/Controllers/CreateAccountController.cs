using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using THUCTAPNHOM.Models2;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace THUCTAPNHOM.Areas.Sales.Controllers
{
    public class CreateAccountController : Controller
    {
        Shop db = new Shop();
        // GET: CreateAccount
        [HttpGet]
        public ActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateAccount(FormCollection form)
        {
            string username = form.Get("customer[username]").ToString();
            string pass = form.Get("customer[password]").ToString();
            string address = form.Get("customer[address]").ToString();
            string phone = form.Get("customer[phone]").ToString();
            string name = form.Get("customer[name]").ToString();
            var u = new SqlParameter("@username", username);

            List<PRODUCT> product1list = new List<PRODUCT>();
            Home_Load(product1list);

            var result = db.Database.SqlQuery<MEMBER>("exec getMEMBERfromusername @username", u).ToList();
            int check = result.Count();
            if (check != 0)
            {
                return View();
            }
            else
            {
                var username2var = new SqlParameter("@username", username);
                var passvar = new SqlParameter("@password", MD5Hash(pass));
                var namevar = new SqlParameter("@name", name);
                var phonevar = new SqlParameter("@phone", phone);
                var addressvar = new SqlParameter("@address", address);
                var result2 = db.Database.ExecuteSqlCommand("exec createaccount @username, @name, @password, @phone, @address", username2var, namevar, passvar, phonevar, addressvar);
                return View("~/Areas/Sales/Views/Home/Home.cshtml", product1list);
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

        public void Home_Load(List<PRODUCT> product1list)
        {
            int doda = 2;
            var id_var = new SqlParameter("@group_id", doda);
            var result = db.Database.SqlQuery<PRODUCT>("exec get_product_from_PRODUCT_GROUP @group_id", id_var).ToList();
            int qty = result.Count();
            for (int i = 0; i < qty; i++)
            {
                product1list.Add(result[i]);
            }
            ViewBag.qty = qty;
        }
    }
}