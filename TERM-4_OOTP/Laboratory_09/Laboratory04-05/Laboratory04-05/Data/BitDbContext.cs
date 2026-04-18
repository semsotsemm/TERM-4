using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace Laboratory04_05.Data
{
    [Table("customers")]
    public class CustomerEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("username")]
        public string Username { get; set; }

        [Required]
        [MaxLength(50)]
        [Column("email")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("phone_number")]
        public string PhoneNumber { get; set; }

        [Required]
        [MaxLength(70)]
        [Column("password")]
        public string Password { get; set; }

        public virtual ICollection<BasketItemEntity> BasketItems { get; set; } = new List<BasketItemEntity>();
    }

    [Table("products")]
    public class ProductEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("product_name")]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("product_color")]
        public string Color { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("production_country")]
        public string ProductionCountry { get; set; }

        [Required]
        [MaxLength(20)]
        [Column("product_size")]
        public string Size { get; set; }

        [Column("count")]
        public int Quantity { get; set; }

        [Required]
        [MaxLength(200)]
        [Column("description")]
        public string Description { get; set; }

        [Required]
        [MaxLength(100)]
        [Column("short_description")]
        public string ShortDescription { get; set; }

        [Column("rating")]
        public decimal Rating { get; set; }

        [Column("price")]
        public int Price { get; set; }

        [Column("discount")]
        public short Discount { get; set; }

        [Column("product_image")]
        public byte[] Image { get; set; }

        public virtual ICollection<BasketItemEntity> BasketItems { get; set; } = new List<BasketItemEntity>();
    }

    [Table("baskets")]
    public class BasketItemEntity
    {
        [Key]
        [Column("basket_id")]
        public int BasketId { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("customer_id")]
        public int CustomerId { get; set; }

        [Column("product_id")]
        public int ProductId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public virtual CustomerEntity Customer { get; set; }

        [ForeignKey(nameof(ProductId))]
        public virtual ProductEntity Product { get; set; }
    }

    [Table("admins")]
    public class AdminEntity
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        [Column("admin_name")]
        public string AdminName { get; set; }

        [Required]
        [MaxLength(70)]
        [Column("password")]
        public string Password { get; set; }
    }

    public class BitDbContext : DbContext
    {
        public BitDbContext()
            : base("BitDbContext")
        {
            Configuration.LazyLoadingEnabled = true;
            Configuration.ProxyCreationEnabled = true;
        }

        public virtual DbSet<CustomerEntity> Customers { get; set; }
        public virtual DbSet<ProductEntity> Products { get; set; }
        public virtual DbSet<BasketItemEntity> Baskets { get; set; }
        public virtual DbSet<AdminEntity> Admins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CustomerEntity>()
                .HasMany(c => c.BasketItems)
                .WithRequired(b => b.Customer)
                .HasForeignKey(b => b.CustomerId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ProductEntity>()
                .HasMany(p => p.BasketItems)
                .WithRequired(b => b.Product)
                .HasForeignKey(b => b.ProductId)
                .WillCascadeOnDelete(true);
        }
    }
}

