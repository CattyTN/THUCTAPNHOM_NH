namespace THUCTAPNHOM.Models2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TRANSACTION")]
    public partial class TRANSACTION
    {
        [Key]
        public int transaction_id { get; set; }

        public byte status { get; set; }

        public int? member_id { get; set; }

        [Required]
        [StringLength(50)]
        public string member_name { get; set; }


        public bool payment { get; set; }

        [StringLength(500)]
        public string delivery { get; set; }

        [Required]
        [StringLength(20)]
        public string member_phone_number { get; set; }

        public int amount { get; set; }

        public virtual MEMBER MEMBER { get; set; }
    }
}
