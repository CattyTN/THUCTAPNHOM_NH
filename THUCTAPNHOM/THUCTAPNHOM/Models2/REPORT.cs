namespace THUCTAPNHOM.Models2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("REPORT")]
    public partial class REPORT
    {
        [Key]
        public int report_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime? report_date { get; set; }

        public int qty { get; set; }

        public int amount { get; set; }

        public int? member_id { get; set; }

        public virtual MEMBER MEMBER { get; set; }
    }
}
