using System.Data.Entity;

namespace Bookvoed
{
    public class BookContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            Database.SetInitializer<BookContext>(null);
            base.OnModelCreating(modelBuilder);
        }
    }
}
