using System;
using System.Xml.Serialization;

namespace HipChat.Entities
{
    /// <summary>
    /// Strongly-typed entity representing a HipChat Message - https://www.hipchat.com/docs/api/method/rooms/history
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Internal date of message for XmlSerializer
        /// </summary>
        [XmlElement(ElementName = "date")]
        public string XmlDate
        {
            get { return Date.ToString("s"); }
            set { Date = DateTimeOffset.Parse(value).DateTime; }
        }

        [XmlIgnore]
        public DateTime Date
        {
            get;
            set;
        }
        
        /// <summary>
        /// User that sent the message
        /// </summary>
        [XmlElement(ElementName = "from")]
        public User From { get; set; }

        /// <summary>
        /// file attachment if there was one.
        /// </summary>  
        [XmlElement(ElementName = "file")]
        public File Attachment { get; set; }

        /// <summary>
        /// text of the message
        /// </summary>
        [XmlElement(ElementName = "message")]
        public string Text { get; set; }

        public Message(DateTime dt, User from, File file , string message)
        {
            this.Date = dt;
            this.From = from;
            this.Attachment = file;
            this.Text = message;
        }

        public Message()
        { }

    }
}
