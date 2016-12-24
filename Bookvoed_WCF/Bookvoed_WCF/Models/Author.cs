using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bookvoed_WCF
{
    [DataContract]
    public class Author
    {
        [DataMember]
        [Key]
        public string nameAuthor { get; set; }
        [DataMember]
        public virtual List<Book> Books { get; set; }
    }
}