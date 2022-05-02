using System.ComponentModel;
using System.Xml.Serialization;

namespace ReadFromXML.Models
{
    [Serializable]
    public class Message
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        [DisplayName("Signatur")]
        public string? Sender { get; set; }
        public DateTime Date { get; set; }
    }
}
