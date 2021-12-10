using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace THUCTAPNHOM.Models2
{
    public class ITEM_SOLD
    {
        [Key]
        public int item_sold_id { get; set; }

        public int? product_id { get; set; }

        public int qty { get; set; }

        public int? price { get; set; }


        public string size { get; set; }

        public int? transaction_id { get; set; }

        public virtual TRANSACTION TRANSACTION { get; set; }

    }


}



