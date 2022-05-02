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
        /// Load messages
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static List<Message> Load(IFormFile file)
        {
            var messages = new List<Message>();
            if (file != null)
            {
                messages = ReadFromStream(file.OpenReadStream());
            }
            return messages;
        }

        /// <summary>
        /// Read content from the stream
        /// </summary>
        /// <param name="fileStream"></param>
        /// <returns></returns>
        public static List<Message> ReadFromStream(Stream fileStream)
        {
            var messages = new List<Message>();
            var xmlSerializer = SetupSerializer();
            using (var reader = new StreamReader(fileStream))
            {
                messages = xmlSerializer.Deserialize(reader) as List<Message> ?? new List<Message>();
            }
            return messages;
        }

        /// <summary>
        /// Setup the serializer
        /// </summary>
        /// <returns></returns>
        public static XmlSerializer SetupSerializer()
        {
            XmlRootAttribute xRoot = new XmlRootAttribute();
            xRoot.ElementName = "Messages";
            xRoot.IsNullable = true;
            var xmlSerializer = new XmlSerializer(typeof(List<Message>), xRoot);
            return xmlSerializer;
        }

        /// <summary>
        /// Save messages
        /// </summary>
        /// <param name="messages"></param>
        public static void Save(List<Message> messages)
        {
            var xmlSerializer = SetupSerializer();
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
            using (var writer = new StreamWriter(path))
            {
                xmlSerializer.Serialize(writer, messages);
            }
        }

        /// <summary>
        /// Read the content from the saved xml and add the new message to the list and return it
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static List<Message> Add(Message message)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
            var messages = ReadFromStream(File.OpenRead(path));
            message.Date = DateTime.Now;
            var maxId = messages.OrderByDescending(m => m.Id).FirstOrDefault()?.Id ?? 0;
            message.Id = maxId + 1;
            message.Sender = String.IsNullOrEmpty(message.Sender) ? "Anonym skribent" : message.Sender;
            messages.Add(message);
            return messages;
        }

        /// <summary>
        /// Read the saved xml and remove the message with selected id from the list and return it
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public static List<Message> Delete(int Id)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, FileName);
            var messages = ReadFromStream(File.OpenRead(path));
            var messagesToDelete = messages.FirstOrDefault(s => s.Id == Id) ?? new Message();
            messages.Remove(messagesToDelete);
            return messages;
        }
    }
}
