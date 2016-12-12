using System.ComponentModel.DataAnnotations;

namespace Bookvoed
{
    public class Book
    {
        [Key]
        public string BookId {get; set; }
        public string Name { get; set; }
        public string Series { get; set; }
        public string Subject { get; set; }

        public virtual Author Author { get; set; }
    }
}
