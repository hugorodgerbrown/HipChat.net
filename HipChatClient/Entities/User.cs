using System;
using System.Xml.Serialization;

namespace HipChat.Entities
{
    /// <summary>
    /// Strongly-typed entity representing a HipChat User - https://www.hipchat.com/docs/api/method/rooms/history
    /// </summary>
    public class User
    {
        /// <summary>
        /// Name of user
        /// </summary>
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// id of user
        /// </summary>
        [XmlElement(ElementName = "user_id")]
        public string Id { get; set; }

        public User(string name, string id)
        {
            this.Name = name;
            this.Id = id;
        }

        public User() { }
    }
}
