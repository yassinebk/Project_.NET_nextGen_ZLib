using Project.Models;
using Microsoft.EntityFrameworkCore;

namespace Project.Data
{
    public class CoreModelsDataContext : DbContext
    {
        public CoreModelsDataContext(DbContextOptions<CoreModelsDataContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
    }
}