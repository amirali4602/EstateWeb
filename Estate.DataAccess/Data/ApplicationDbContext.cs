using Microsoft.EntityFrameworkCore;
using Estate.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Estate.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BuildingDirection> BuildingDirections { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cooling> Coolings { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<Heating> Heatings { get; set; }
        public DbSet<HotWaterSupplier> HotWaterSuppliers { get; set; }
        public DbSet<Toilet> Toilet { get; set; }
        public DbSet<FloorMaterial> floorMaterials { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<SmsDto> smsDtos { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<BuildingDirection>().HasData(
                new BuildingDirection { Id = 1, Name = "شمالی" },
                new BuildingDirection { Id = 2, Name = "شرقی" },
                new BuildingDirection { Id = 3, Name = "غربی" },
                new BuildingDirection { Id = 4, Name = "جنوبی" }
                );
            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "آپارتمان مسکونی" },
                new Category { Id = 2, Name = "آپارتمان اداری" },
                new Category { Id = 3, Name = "آپارتمان موقعیت اداری" },
                new Category { Id = 4, Name = "خانه ، ویلا ، کلنگی" },
                new Category { Id = 5, Name = "مجتمع آپارتمانی ، مستغلات" },
                new Category { Id = 6, Name = "زمین" },
                new Category { Id = 7, Name = "تجاری ، مغازه" },
                new Category { Id = 8, Name = "باغ و باغچه" },
                new Category { Id = 9, Name = "صنعتی و زراعی" },
                new Category { Id = 10, Name = "املاک مشارکت در ساخت" }

                );
            builder.Entity<Cooling>().HasData(
                new Cooling { Id = 1, Name = "کولر آبی" },
                new Cooling { Id = 2, Name = "کولر گازی" },
                new Cooling { Id = 3, Name = "داکت اسپلیت" },
                new Cooling { Id = 4, Name = "اسپلیت" },
                new Cooling { Id = 5, Name = "فن کوئل" }

                );
            builder.Entity<DocumentType>().HasData(
                new DocumentType { Id = 1, Name = "تک‌برگ" },
                new DocumentType { Id = 2, Name = "منگوله‌دار" },
                new DocumentType { Id = 3, Name = "قول‌نامه‌ای" },
                new DocumentType { Id = 4, Name = "سایر" }

                );
            builder.Entity<FloorMaterial>().HasData(
                new FloorMaterial { Id = 1, Name = "سرامیک" },
                new FloorMaterial { Id = 2, Name = "پارکت چوب" },
                new FloorMaterial { Id = 3, Name = "پارکت لمینت" },
                new FloorMaterial { Id = 4, Name = "سنگ" },
                new FloorMaterial { Id = 5, Name = "کف‌پوش PVC" },
                new FloorMaterial { Id = 6, Name = "موکت" },
                new FloorMaterial { Id = 7, Name = "موزائیک" }
                
                );
            builder.Entity<Heating>().HasData(
                new Heating { Id = 1, Name = "بخاری" },
                new Heating { Id = 2, Name = "شوفاژ" },
                new Heating { Id = 3, Name = "فن کوئل" },
                new Heating { Id = 4, Name = "از کف" },
                new Heating { Id = 5, Name = "داکت اسپلیت" },
                new Heating { Id = 6, Name = "اسپلیت" },
                new Heating { Id = 7, Name = "شومینه" }

                );
            builder.Entity<HotWaterSupplier>().HasData(
                new HotWaterSupplier { Id = 1, Name = "آبگرم‌کن" },
                new HotWaterSupplier { Id = 2, Name = "موتورخانه" },
                new HotWaterSupplier { Id = 3, Name = "پکیج" }

                );
            builder.Entity<Toilet>().HasData(
                new Toilet { Id = 1, Name = "ایرانی" },
                new Toilet { Id = 2, Name = "فرنگی" },
                new Toilet { Id = 3, Name = "ایرانی و فرنگی" }

                );




        }

    }
}

