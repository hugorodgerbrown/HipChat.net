using System;
using System.Xml.Serialization;

namespace HipChat.Entities
{
    /// <summary>
    /// Strongly-typed entity representing a HipChat User - https://www.hipchat.com/docs/api/method/rooms/history
    /// </summary>
    public class File
    {
        /// <summary>
        /// Name of file
        /// </summary>
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// size of file
        /// </summary>
        [XmlElement(ElementName = "size")]
        public string Size { get; set; }

        /// <summary>
        /// url of file
        /// </summary>
        [XmlElement(ElementName = "url")]
        public string Url { get; set; }

        public File(string name, string size, string url)
        {
            this.Name = name;
            this.Size = size;
            this.Url = url;
        }

        public File() { }
    }
}
