using ReadFromXML.Models;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ReadFromXML.Helpers
{
    public class XMLHelper
    {
        const string FileName = "Data.xml";

        /// <summary>
        /// Open selected file,
        /// save its content to a new file and then return the content
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<Message> Load(IFormFile file)
        {
            var messages = new List<Message>();
            if (file != null)
            {
                XmlRootAttribute xRoot = new XmlRootAttribute();
                xRoot.ElementName = "Messages";
                xRoot.IsNullable = true;
                var xmlSerializer = new XmlSerializer(typeof(List<Message>), xRoot);
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    messages = xmlSerializer.Deserialize(reader) as List<Message> ?? new List<Message>();
                }
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
                using (var writer = new StreamWriter(path))
                {
                    xmlSerializer.Serialize(writer, messages);
                }
            }
            return messages;
        }

        /// <summary>
        /// Read the content from the saved xml and add the new message to the list and overwrite the old file
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<Message> Add(Message message)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Messages";
            xRoot.IsNullable = true;
            var xmlSerializer = new XmlSerializer(typeof(List<Message>), xRoot);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
            var messages = new List<Message>();
            using (var reader = new StreamReader(path))
            {
                messages = xmlSerializer.Deserialize(reader) as List<Message> ?? new List<Message>();
                message.Date = DateTime.Now;
                var maxId = messages.OrderByDescending(m => m.Id).FirstOrDefault()?.Id ?? 0;
                message.Id = maxId + 1;
                message.Sender = String.IsNullOrEmpty(message.Sender) ? "Anonym skribent" : message.Sender;
                messages.Add(message);
            }
            using (var writer = new StreamWriter(path))
            {
                xmlSerializer.Serialize(writer, messages);
            }
            return messages;
        }

        /// <summary>
        /// Read the saved xml and remove the message with selected id and overwrite the old file
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<Message> Delete(int Id)
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Messages";
            xRoot.IsNullable = true;
            var xmlSerializer = new XmlSerializer(typeof(List<Message>), xRoot);
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
            var messages = new List<Message>();
            using (var reader = new StreamReader(path))
            {
                messages = xmlSerializer.Deserialize(reader) as List<Message> ?? new List<Message>();
                var messagesToDelete = messages.FirstOrDefault(s => s.Id == Id) ?? new Message();
                messages.Remove(messagesToDelete);
            }
            using (var writer = new StreamWriter(path))
            {
                xmlSerializer.Serialize(writer, messages);
            }
            return messages;
        }
    }
}
