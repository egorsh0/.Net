using System.Data.Entity;
using System.Runtime.Serialization;

namespace Bookvoed_WCF
{
    [DataContract]
    public class BookContext : DbContext
    {
        [DataMember]
        public DbSet<Author> Authors { get; set; }
        [DataMember]
        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasOptional(x => x.Author).WithMany(x => x.Books);
        }

        public BookContext() : base("MyDb")
        { }
    }
}