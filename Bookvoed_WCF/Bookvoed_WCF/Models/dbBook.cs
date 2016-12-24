using System.Runtime.Serialization;

namespace Bookvoed_WCF
{
    [DataContract]
    public class dbBook
    {
        [DataMember]
        public string BookId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public string Series { get; set; }
        [DataMember]
        public string Subject { get; set; }
    }
}