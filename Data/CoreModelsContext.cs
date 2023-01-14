using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Project.Models;
using Microsoft.EntityFrameworkCore;
using Project.Controllers;

namespace Project.Data
{
    public class CoreModelsDataContext : IdentityDbContext<User>
    {
        public CoreModelsDataContext(DbContextOptions<CoreModelsDataContext> options) : base(options)
        {
        }

        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Publisher> Publishers { get; set; }

        public List<Book> SearchBook(string key)
        {
            // return Books.Where(b => (b.BookAuthor.FullName.Contains(key) || b.Title.Contains(key))).ToList();
            return Books.Where(b => (b.Title.ToLower().Contains(key.ToLower()) || b.Summary.ToLower().Contains(key.ToLower()))).ToList();

        }
    }
}