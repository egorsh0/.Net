using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Bookvoed
{
    public class Author
    {
        [Key]
        public string nameAuthor { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
