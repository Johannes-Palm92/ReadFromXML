namespace ReadFromXML.Models
{
    public class MessageViewModel
    {
        public Message Message { get; set; }
        public List<Message> Messages { get; set; }

        public MessageViewModel()
        {
            Message = new Message();
            Messages = new List<Message>();
        }
    }
}
