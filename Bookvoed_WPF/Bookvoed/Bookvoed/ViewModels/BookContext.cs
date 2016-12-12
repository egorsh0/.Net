using System.Data.Entity;

namespace Bookvoed
{
    class BookContext : DbContext
    {
        public DbSet<Author> Authors { get; set; }
        public DbSet<Book> Books { get; set; }
    }
}
