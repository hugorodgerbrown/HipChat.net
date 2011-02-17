using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;

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
        public string Token { get { return token; } set { Token = value; } }

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
                    throw new ArgumentException("Sender name must be 15 characters or less.", "Sender");
                }
                sender = value;
            }
        }

        #region constructors
        public HipChatClient(string token)
        {
            this.token = token;
        }

        public HipChatClient(string token, ApiResponseFormat format)
            : this(token)
        {
            this.format = format;
        }

        public HipChatClient(string token, int room)
            : this(token)
        {
            this.room = room;
        }

        public HipChatClient(string token, int room, ApiResponseFormat format)
            : this(token, room)
        {
            this.format = format;
        }

        #endregion constructors

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        public string SendMessage(string message, int room, string from)
        {
            #region validation
            if (string.IsNullOrEmpty(Token))
                throw new InvalidOperationException("You must set the Token property before calling the SendMessage method.");
            if (room == int.MinValue)
                throw new InvalidOperationException("You must set the RoomId property before calling the SendMessage method.");
            if (string.IsNullOrEmpty(from))
                throw new InvalidOperationException("You must set the From property before calling the SendMessage method.");
            if (string.IsNullOrEmpty(message))
                throw new InvalidOperationException("You cannot send a blank message.");
            #endregion validation

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(FormatMessageUri(from, message));
            return HttpUtils.CallApi(request);
        }

        /// <summary>
        /// Sends a message to a chat room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        public void SendMessage(string message, int room)
        {
            SendMessage(message, room, this.From);
        }

        /// <summary>
        /// Sends a message to a room.
        /// </summary>
        /// <param name="message">The message to send - can contain some HTML and must be valid XHTML.</param>
        public void SendMessage(string message)
        {
            SendMessage(message, this.RoomId, this.From);
        }


        /// <summary>
        /// Returns the list of available rooms. 
        /// </summary>
        /// <returns>The raw JSON/XML API response</returns>
        public string ListRooms()
        {
            #region validation
            if (string.IsNullOrEmpty(Token))
                throw new InvalidOperationException("You must set the Token property before calling the SendMessage method.");
            #endregion validation

            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(FormatRoomsListUri());
            return HttpUtils.CallApi(request);
        }

        /// <summary>
        /// Formats the URI for the /rooms/message API (http://www.hipchat.com/docs/api/method/rooms/message)
        /// </summary>
        private string FormatMessageUri(string sender, string message)
        {
            var url = string.Format(@"https://api.hipchat.com/v1/rooms/message?auth_token={0}&room_id={1}&format={2}&notify={3}&from={4}&message={5}",
                this.Token,
                this.RoomId,
                this.Format.ToString().ToLower(),
                this.NotifyAsChar,
                sender,
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


    }
}
