using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace HipChat.Entities
{
    /// <summary>
    /// Strongly-typed entity representing a list of HipChat rooms - https://www.hipchat.com/docs/api/method/rooms/list
    /// </summary>
    [Serializable]
    [XmlRoot("rooms")]
    public class Rooms
    {
        /// <summary>
        /// list of rooms
        /// </summary>
        [XmlElement("room")]
        public Room[] RoomList { get; set; }
    }
}
