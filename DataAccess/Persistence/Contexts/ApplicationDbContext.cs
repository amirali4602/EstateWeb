using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DataAccess.Persistence.Contexts;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Page> Pages { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<SmsDto> smsDtos { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "ام دی اف اتریشی" },
            new Category { Id = 2, Name = "ام دی اف ملامینه پاک چوب" },
            new Category { Id = 3, Name = "ام دی اف ملامینه پویا" },
            new Category { Id = 4, Name = "ایزوفام سینکرونایز" },
            new Category { Id = 5, Name = "ایزوفام ملامینه" },
            new Category { Id = 6, Name = "ایزوفام هایگلاس" },
            new Category { Id = 7, Name = "نئوپان ملامینه پویا" },
            new Category { Id = 8, Name = "هایگلاس AGE" },
            new Category { Id = 9, Name = "هایگلاس آلکی" },
            new Category { Id = 10, Name = "بدون دسته‌بندی" }

            );
    }

}

