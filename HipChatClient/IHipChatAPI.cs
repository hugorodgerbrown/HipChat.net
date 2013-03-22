using System;
using System.Collections.Generic;

namespace HipChat
{
    /// <summary>
    /// Provide a simplified interface to the HipChatClient for normal usages.
    /// </summary>
    public interface IHipChatAPI
    {
        /// <summary>
        /// Retrieves a list of all rooms that the API key has access to
        /// </summary>
        IList<Entities.Room> GetRooms();

        /// <summary>
        /// Retrieves the last 75 messages sent in the room.
        /// </summary>
        IList<Entities.Message> GetRoomHistory(int RoomID);

        /// <summary>
        /// Retrieves the chat history for the room for the given day
        /// </summary>
        IList<Entities.Message> GetRoomHistory(int RoomID, DateTime Date);

        void SendMessage(int RoomID, String From, String Message);
    }

    public class HipChatAPI : IHipChatAPI
    {
        private readonly HipChatClient client;

        public HipChatAPI(String apiKey)
        {
            this.client = new HipChatClient(apiKey);
        }

        public IList<Entities.Room> GetRooms()
        {
            return this.client.ListRoomsAsNativeObjects();
        }

        public IList<Entities.Message> GetRoomHistory(int RoomID)
        {
            this.client.RoomId = RoomID;

            return this.client.ListHistoryAsNativeObjects();
        }

        public IList<Entities.Message> GetRoomHistory(int RoomID, DateTime Date)
        {
            this.client.RoomId = RoomID;

            return this.client.ListHistoryAsNativeObjects(Date);
        }

        public void SendMessage(int RoomID, String From, string Message)
        {
            this.client.RoomId = RoomID;

            this.client.SendMessage(Message, RoomID, From);
        }
    }
}