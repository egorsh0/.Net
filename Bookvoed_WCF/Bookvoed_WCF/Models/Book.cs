using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Bookvoed_WCF
{
    [DataContract]
    public class Book
    {
        [DataMember]
        [Key]
        public string BookId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Series { get; set; }
        [DataMember]
        public string Subject { get; set; }
        [DataMember]
        public virtual Author Author { get; set; }
    }
}