using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Modum.Models.BaseModels.Models.BaseStructure;
using Modum.Models.BaseModels.Models.LTCs;
using Modum.Models.BaseModels.Models.Payment;
using Modum.Models.BaseModels.Models.PaymentStructure;
using Modum.Models.BaseModels.Models.ProductStructure;
using Modum.Models.MainModel;

namespace Modum.DataAccess
{
    public class DataContext : IdentityDbContext<ApplicationUser>
    {
        private readonly bool seedDb;

        public DataContext(DbContextOptions<DataContext> options, bool seedDb = true)
            : base(options)
        {
            if (this.seedDb)
            {
                Database.EnsureCreated();
            }
         
            this.seedDb = seedDb;
        }


        public DbSet<Cart> Cart { get; set; }

        public DbSet<Category> Category { get; set; }

        public DbSet<Subcategory> Subcategory { get; set; }

        public DbSet<MainCategory> MainCategory { get; set; }

        public DbSet<Notifications> Notifications { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Statistics> Statistics { get; set; }

        public DbSet<Favourites> Favourites { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<ShortUserModel> BannedUsers { get; set; }

        public DbSet<ProductSizesHelpingTable> ProductSizesHelpingTable { get; set; }

        public DbSet<LTC> LTCs { get; set; }

        public DbSet<Worker> Workers { get; set; }

        public DbSet<Order> Orders { get; set; }

        private MainCategory Men { get; set; } = new MainCategory();
        private MainCategory Women { get; set; } = new MainCategory();
        private MainCategory Girls { get; set; } = new MainCategory();
        private MainCategory Boys { get; set; } = new MainCategory();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (this.seedDb)
            {
                SeedMainCategories();
                modelBuilder
                    .Entity<MainCategory>()
                    .HasData
                    (this.Men
                    , this.Women
                    , this.Boys
                    , this.Girls);
                modelBuilder.Entity<Category>().Navigation(e => e.Subcategories).AutoInclude();
                modelBuilder.Entity<Cart>().Navigation(e => e.CartItems).AutoInclude();
                modelBuilder.Entity<Favourites>().Navigation(e => e.Products).AutoInclude();
                modelBuilder.Entity<Product>().Navigation(e => e.LTCs).AutoInclude();
                modelBuilder.Entity<Subcategory>().Navigation(e => e.Category).AutoInclude();
                modelBuilder.Entity<Worker>().Navigation(e => e.AppUser).AutoInclude();
                modelBuilder.Entity<Order>().Navigation(e => e.Products).AutoInclude();
                modelBuilder.Entity<Order>().Navigation(e => e.ApplicationUser).AutoInclude();
                modelBuilder.Entity<ProductSizesHelpingTable>().Navigation(e => e.Product).AutoInclude();

                modelBuilder.Entity<ProductSizesHelpingTable>()
                    .HasIndex(ci => new { ci.ProductSize })
                    .IsUnique();

                modelBuilder.Entity<IdentityRole>().HasData(
                     new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                     new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" },
                     new IdentityRole { Id = "3", Name = "Worker", NormalizedName = "WORKER" },
                     new IdentityRole { Id = "4", Name = "SuperAdmin", NormalizedName = "SUPERADMIN" }
                 );

            }

            base.OnModelCreating(modelBuilder);
        }
        private void SeedMainCategories()
        {
            this.Men = new MainCategory()
            {
                Id = Guid.NewGuid(),
                Name = "Men",
            };
            this.Women = new MainCategory()
            {
                Id = Guid.NewGuid(),
                Name = "Women",
            };
            this.Girls = new MainCategory()
            {
                Id = Guid.NewGuid(),
                Name = "Girls",
            };
            this.Boys = new MainCategory()
            {
                Id = Guid.NewGuid(),
                Name = "Boys",
            };
        }

    }
}