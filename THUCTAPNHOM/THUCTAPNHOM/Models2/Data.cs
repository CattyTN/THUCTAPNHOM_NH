using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using THUCTAPNHOM.Models2;


namespace THUCTAPNHOM.Models2
{
    public class Data
    {
        Shop p = new Shop();

        public int Login(string username, string password)
        {
            object[] para =
            {
                new SqlParameter("@username", username),
                new SqlParameter("@password", password)
            };

            var ret = p.MEMBERs.SqlQuery("CheckMember @username, @password", para).ToList();
            if (ret.Count() > 0)
            {
                if(ret[0].role == 0)
                {
                    return 2;
                }               
            }
            return 0;
        }

        public int Register(string name, string username, string password, string address, string phone)
        {
            object[] para =
            {
                new SqlParameter("@name", name),                
                new SqlParameter("@username", username),
                new SqlParameter("@password", password), 
                new SqlParameter("@address", address),
                new SqlParameter("@phone", phone)
            };
            p.Database.ExecuteSqlCommand("AddMember @name, @username, @password, @address, @phone", para);

            return 0;
        }

    }
}