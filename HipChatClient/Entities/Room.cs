using System;
using System.Xml.Serialization;

namespace HipChat.Entities
{
    /// <summary>
    /// Strongly-typed entity representing a HipChat room - https://www.hipchat.com/docs/api/method/rooms/list
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName="room")]
    public class Room
    {
        /// <summary>
        /// Unique identifier - used in the SendMessage API method
        /// </summary>
        [XmlElement (ElementName="room_id")]
        public int Id { get; set; }

        /// <summary>
        /// Name of the room.
        /// </summary>
        [XmlElement(ElementName = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Current topic.
        /// </summary>  
        [XmlElement(ElementName = "topic")]
        public string Topic { get; set; }

        /// <summary>
        /// Time of last activity (sent message) in the room in UNIX time (UTC). May be 0 in rare cases when the time is unknown.
        /// </summary>
        [XmlElement(ElementName = "last_active")]
        public string XmlLastActive
        {
            get { return LastActive.ToString("s"); }
            set { LastActive = HttpUtils.ConvertUnixTime(value); }
        }

        [XmlIgnore]
        public DateTime LastActive
        {
            get;
            set;
        }

        /// <summary>
        /// User ID of the room owner.
        /// </summary>
        [XmlElement(ElementName = "owner_user_id")]
        public int Owner { get; set; }

        public Room(int id, string name, string topic, DateTime lastActive, int owner)
        {
            this.Id = id;
            this.Name = name;
            this.Topic = topic;
            this.LastActive = lastActive;
            this.Owner = owner;
        }

        public override string ToString()
        {
            return string.Format("Id:{0},Name:{1}",Id,Name);
        }

        public Room()
        {}
    }
}
