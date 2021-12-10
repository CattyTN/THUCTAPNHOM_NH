namespace THUCTAPNHOM.Models2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BRAND")]
    public partial class BRAND
    {
        [Key]
        public int brand_id { get; set; }

        [StringLength(50)]
        public string brand_name { get; set; }

        [StringLength(100)]
        public string description { get; set; }
    }
}
