using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace HipChat
{
    /// <summary>
    /// Class used to encapsulate core HipChat API methods. This is the core class that is used to interact with the API.
    /// </summary>
    /// <remarks>
    /// https://www.hipchat.com/docs/api
    /// </remarks>
    public class HipChatClient
    {
        private int room = int.MinValue;
        private string sender = string.Empty;
        private ApiResponseFormat format = ApiResponseFormat.JSON;
        private bool notify = false;
        private string token = string.Empty;

        /// <summary>
        /// If True, Sender and Message values are automatically truncated if they are too long.
        /// </summary>
        public bool AutoTruncate { get; set; }

        /// <summary>
        /// Used to determine the format of the API response (JSON is default)
        /// </summary>
        public enum ApiResponseFormat { JSON = 0, XML = 1 }

        /// <summary>
        /// Desired response format: json or xml. (default: json)
        /// </summary>
        public ApiResponseFormat Format { get { return format; } set { format = value; } }

        /// <summary>
        /// Boolean flag of whether or not this message should trigger a notification for people in the room (based on their individual notification preferences). 0 = false, 1 = true. (default: 0)
        /// </summary>
        public bool Notify { get { return notify; } set { notify = value; } }

        /// <summary>
        /// Returns the Notify property as 0 or 1 instead of False, True (read-only)
        /// </summary>
        private char NotifyAsChar { get { return notify ? '1' : '0'; } }

        /// <summary>
        /// The API authentication token - this is managed through the HipChat account admin panel.
        /// </summary>
        public string Token { get { return token; } set { token = value; } }

        /// <summary>
        /// The numeric id of the room to which to send a message
        /// </summary>
        public int RoomId { get { return room; } set { room = value; } }

        /// <summary>
        /// Name the message will appear be sent from. Must be less than 15 characters long. May contain letters, numbers, -, _, and spaces.
        /// </summary>
        public string From
        {
            get { return sender; }
            set
            {
                if (value.Length > 15)
                {
                    if (AutoTruncate)
                    {
                        sender = value.Substring(0, 15);
                    }
                    else
                    {
                        throw new ArgumentException("Sender name must be 15 characters or less.", "Sender");
                    }
                }
                else
                {
                    sender = value;
                }
            }
        }

        #region constructors
        public HipChatClient() { AutoTruncate = true; }

        public HipChatClient(string token): this()
        {
            this.Token = token;
        }

        public HipChatClient(string token, ApiResponseFormat format)
            : this(token)
        {
            this.Format = format;
        }

        public HipChatClient(string token, int room)
            : this(token)
        {
            this.RoomId = room;
        }

        public HipChatClient(string token, int room, string from)
            : this(token, room)
        {
            this.From = from;
        }

        public HipChatClient(string token, int room, ApiResponseFormat format)
            : this(token, room)
        {
            this.Format = format;
        }

        #endregion constructors

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        public static void SendMessage(string token, int room, string from, string message)
        {
            // create a local instance of HipChatClient, as then we get the validation
            var client = new HipChatClient(token, room);
            client.SendMessage(message, from);
        }

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        public static void SendMessage(string token, int room, string from, string message, bool notify)
        {
            // create a local instance of HipChatClient, as then we get the validation
            var client = new HipChatClient(token, room);
            client.SendMessage(message, from, notify);
        }

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        public void SendMessage(string message, int room, string from)
        {
            this.RoomId = room;
            this.From = from;
            SendMessage(message);
        }

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        /// <param name="notify">If true, the message triggers a "ping" sound when it hits the room.</param>
        public void SendMessage(string message, int room, string from, bool notify)
        {
            this.Notify = notify;
            SendMessage(message, room, from);
        }

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        /// <param name="room">The id of the room to send the message to - sets the RoomId property.</param>
        public void SendMessage(string message, int room)
        {
            this.RoomId = room;
            SendMessage(message);
        }

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        /// <param name="room">The id of the room to send the message to - sets the RoomId property.</param>
        /// <param name="notify">If true, the message triggers a "ping" sound when it hits the room.</param>
        public void SendMessage(string message, int room, bool notify)
        {
            this.Notify = notify;
            SendMessage(message, room);
        }

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        /// <param name="from">The name of the sender - sets the From property.</param>
        public void SendMessage(string message, string from)
        {
            this.From = from;
            SendMessage(message);
        }

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        /// <param name="from">The name of the sender - sets the From property.</param>
        /// <param name="notify">If true, the message triggers a "ping" sound when it hits the room.</param>
        public void SendMessage(string message, string from, bool notify)
        {
            this.Notify = notify;
            SendMessage(message, from);
        }

        /// <summary>
        /// Sends a message to a room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        public void SendMessage(string message)
        {
            #region validation
            if (string.IsNullOrEmpty(Token))
                throw new InvalidOperationException("You must set the Token property before calling the SendMessage method.");
            if (RoomId == int.MinValue)
                throw new InvalidOperationException("You must set the RoomId property before calling the SendMessage method.");
            if (string.IsNullOrEmpty(From))
                throw new InvalidOperationException("You must set the From property before calling the SendMessage method.");
            if (string.IsNullOrEmpty(message))
                throw new ArgumentException("You cannot send a blank message.", "message");
            if (message.Length > 5000)
                if (AutoTruncate)
                {
                    message = message.Substring(0, 4997) + "...";
                }
                else
                {
                    throw new ArgumentException("Message must be less than 5000 characters. See https://www.hipchat.com/docs/api/method/rooms/message for more details.", "message");
                }
            #endregion validation

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(FormatMessageUri(message));
            request.Method = "POST";
            // the API method currently only returns a fixed "sent" value, so there's no point returning it.
            // if something went wrong we'll get an HipChatApiWebException that will contain the message
            HttpUtils.CallApi(request);
        }


        /// <summary>
        /// Returns the list of available rooms as XML/JSON 
        /// </summary>
        /// <returns>The raw JSON/XML API response (format is determined by Format property)</returns>
        public string ListRooms()
        {
            #region validation
            if (string.IsNullOrEmpty(Token))
                throw new InvalidOperationException("You must set the Token property before calling the API.");
            #endregion validation

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(FormatRoomsListUri());
            return HttpUtils.CallApi(request);
        }

        /// <summary>
        /// Returns the list of available rooms as native C# objects
        /// </summary>
        /// <returns>A List<> containing strongly-typed Entities.Room objects</returns>
        public List<Entities.Room> ListRoomsAsNativeObjects()
        {
            this.Format = ApiResponseFormat.XML;
            XmlSerializer s = new XmlSerializer(typeof(Entities.Rooms));
            Entities.Rooms theRooms = s.Deserialize(new StringReader(ListRooms())) as Entities.Rooms;
            return new List<Entities.Room>(theRooms.RoomList);
        }

        


        /// <summary>
        /// Yields each individual room as strongly-typed Entities.Room object
        /// </summary>
        /// <example>
        /// foreach ( Room room in client.YieldRooms() )
        /// {
        ///     // do something with room
        /// }
        /// </example>
        public IEnumerable<Entities.Room> YieldRooms()
        {
            this.Format = ApiResponseFormat.XML;
            XmlDocument x = new XmlDocument();
            x.LoadXml(ListRooms());

           // might be neater to deserialize the response using XmlSerializer 
            foreach (XmlElement e in x.DocumentElement.ChildNodes)
            {
                Console.WriteLine(e.InnerText);

                int id = int.Parse(e.SelectSingleNode("room_id").InnerText);
                string name = e.SelectSingleNode("name").InnerText;
                string topic = e.SelectSingleNode("topic").InnerText;
                DateTime active = HttpUtils.ConvertUnixTime((e.SelectSingleNode("last_active").InnerText));
                int owner = int.Parse(e.SelectSingleNode("owner_user_id").InnerText);

                yield return new Entities.Room(id, name, topic, active, owner);
            }
        }

        /// <summary>
        /// Returns the history as native C# objects
        /// </summary>
        /// <returns>A List<> containing strongly-typed Entities.Message objects</returns>
        public List<Entities.Message> ListHistoryAsNativeObjects(DateTime dt)
        {
            this.Format = ApiResponseFormat.XML;
            XmlSerializer s = new XmlSerializer(typeof(Entities.Messages));
            Entities.Messages theMessages = s.Deserialize(new StringReader(RoomHistory(dt))) as Entities.Messages;
            return new List<Entities.Message>(theMessages.MessageList);
        }

        public List<Entities.Message> ListHistoryAsNativeObjects()
        {
            this.Format = ApiResponseFormat.XML;
            XmlSerializer s = new XmlSerializer(typeof(Entities.Messages));
            Entities.Messages theMessages = s.Deserialize(new StringReader(RoomHistory())) as Entities.Messages;
            return new List<Entities.Message>(theMessages.MessageList);
        }

        /// <summary>
        /// Returns the chat history of a single room on a single day.
        /// </summary>
        /// <returns>The raw JSON/XML API response (format is determined by Format property)</returns>
        public string RoomHistory(DateTime date)
        {
            #region validation
            if (string.IsNullOrEmpty(Token))
                throw new InvalidOperationException("You must set the Token property before calling the API.");
            if (RoomId == int.MinValue)
                throw new InvalidOperationException("You must set the RoomId property before calling the SendMessage method.");
            if (date == null)
                throw new ArgumentNullException("date", "You must pass in a valid date for the history API.");
            #endregion validation

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(FormatRoomsHistoryUri(date));
            return HttpUtils.CallApi(request);
        }

        public string RoomHistory()
        {
            #region validation
            if (string.IsNullOrEmpty(Token))
                throw new InvalidOperationException("You must set the Token property before calling the API.");
            if (RoomId == int.MinValue)
                throw new InvalidOperationException("You must set the RoomId property before calling the SendMessage method.");
           
            #endregion validation

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(FormatRoomsHistoryUri());
            return HttpUtils.CallApi(request);

        }

        /// <summary>
        /// Formats the URI for the /rooms/message API (http://www.hipchat.com/docs/api/method/rooms/message)
        /// </summary>
        private string FormatMessageUri(string message)
        {
            var url = string.Format(@"https://api.hipchat.com/v1/rooms/message?auth_token={0}&room_id={1}&format={2}&notify={3}&from={4}&message={5}",
                this.Token,
                this.RoomId,
                this.Format.ToString().ToLower(),
                this.NotifyAsChar,
                this.From,
                message);
            return Uri.EscapeUriString(url);
        }

        /// <summary>
        /// Formats the URI for the /rooms/list API (http://www.hipchat.com/docs/api/method/rooms/list)
        /// </summary>
        private string FormatRoomsListUri()
        {
            return string.Format("https://api.hipchat.com/v1/rooms/list?format={0}&auth_token={1}",
                this.Format.ToString().ToLower(),
                this.Token);
        }

        /// <summary>
        /// Formats the URI for the /rooms/history API (http://www.hipchat.com/docs/api/method/rooms/history)
        /// </summary>
        /// <param name="date">The API requires a date - so this is it :-)</param>
        /// <returns>The URL to use</returns>
        private string FormatRoomsHistoryUri(DateTime date)
        {
            return string.Format("https://api.hipchat.com/v1/rooms/history?format={0}&auth_token={1}&room_id={2}&date={3}",
                this.Format.ToString().ToLower(),
                this.Token,
                this.RoomId,
                date.ToString("yyyy-MM-dd"));
        }

        private string FormatRoomsHistoryUri()
        {
            return string.Format("https://api.hipchat.com/v1/rooms/history?format={0}&auth_token={1}&room_id={2}&date={3}",
               this.Format.ToString().ToLower(),
               this.Token,
               this.RoomId,
               "recent");
        }

    }
}
