using Microsoft.EntityFrameworkCore;
using Pizzario.Services.ShoppingCartApi.Models;

namespace Pizzario.Services.ShoppingCartApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<CartDetails> CartDetails { get; set; }
        public DbSet<CartHeaders> CartHeaders { get; set; }


    }
}
