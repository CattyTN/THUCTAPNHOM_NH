namespace THUCTAPNHOM.Models2
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Shop : DbContext
    {
        public Shop()
            : base("name=Shopp")
        {
        }

        public virtual DbSet<CART> CARTs { get; set; }
        public virtual DbSet<CART_ITEM> CART_ITEM { get; set; }
        public virtual DbSet<CATEGORY> CATEGORies { get; set; }
        public virtual DbSet<MEMBER> MEMBERs { get; set; }
        public virtual DbSet<PRODUCT> PRODUCTs { get; set; }
        public virtual DbSet<PRODUCT_GROUP> PRODUCT_GROUP { get; set; }
        public virtual DbSet<REPORT> REPORTs { get; set; }
        public virtual DbSet<SALE> SALES { get; set; }

        public virtual DbSet<BRAND> BRANDs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TRANSACTION> TRANSACTIONs { get; set; }

        public virtual DbSet<ITEM_SOLD> ITEM_SOLDs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CATEGORY>()
                .HasMany(e => e.PRODUCTs)
                .WithRequired(e => e.CATEGORY)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MEMBER>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<MEMBER>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<MEMBER>()
                .Property(e => e.phone_number)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.brand)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.size)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.image_link)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT>()
                .Property(e => e.image_list)
                .IsUnicode(false);

            modelBuilder.Entity<PRODUCT_GROUP>()
                .HasMany(e => e.CATEGORies)
                .WithRequired(e => e.PRODUCT_GROUP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TRANSACTION>()
                .Property(e => e.member_phone_number)
                .IsUnicode(false);
        }
    }
}
