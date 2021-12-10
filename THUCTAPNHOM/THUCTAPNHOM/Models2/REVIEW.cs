using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace THUCTAPNHOM.Models2
{
    public class REVIEW
    {
        public int review_id { get; set; }

        [StringLength(50)]
        public string username { get; set; }

        public string review_text { get; set; }

        public int product_id { get; set; }

        public string date_post { get; set; }
    }
}