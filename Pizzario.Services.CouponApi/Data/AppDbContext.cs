using Microsoft.EntityFrameworkCore;
using Pizzario.Services.CouponApi.Models;

namespace Pizzario.Services.CouponApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Coupons>Coupons { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Coupons>().HasData(new Coupons
            {
                CouponId = 1,
                CouponCode = "15OFF",
                DiscountAmount = 15,
                MinAmount = 20
            });


            modelBuilder.Entity<Coupons>().HasData(new Coupons
            {
                CouponId = 2,
                CouponCode = "20OFF",
                DiscountAmount = 20,
                MinAmount = 40
            });
            
            modelBuilder.Entity<Coupons>().HasData(new Coupons
            {
                CouponId = 3,
                CouponCode = "5OFF",
                DiscountAmount = 5,
                MinAmount = 15
            });
            
            modelBuilder.Entity<Coupons>().HasData(new Coupons
            {
                CouponId = 4,
                CouponCode = "50OFF",
                DiscountAmount = 50,
                MinAmount = 80
            });
            
            modelBuilder.Entity<Coupons>().HasData(new Coupons
            {
                CouponId = 5,
                CouponCode = "65OFF",
                DiscountAmount = 65,
                MinAmount = 100
            });
        }
    }
}
