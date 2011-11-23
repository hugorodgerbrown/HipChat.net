#HipChat.Net Client library

This is a C# library used to wrap the [HipChat API](https://www.hipchat.com/docs/api)

The API is very clear and concise, so there's not much to add to this - this library just makes the formatting of the URL a bit simpler. The API returns either XML or JSON, and the client library contains native POCO representations for those who like those sorts of things. 

NB This is NOT for mission critical, transactional, guaranteed delivery!

The API supports four methods:

1. Fetching a list of available chat rooms
2. Sending a message to a room
3. Fetching the chat history from a room
4. Showing the room details (who's in it etc.)

Access to the API is managed by the inclusion of an `auth_token` with each request. The auth_tokens are managed from the HipChat admin panel, by the account holder.

The core use case for this library is to allow developers to include 'bots' in chat rooms. The sub-cases included to support this are therefore:

1. Administer the 'bot' - which would involve selecting a room from a list of rooms, and entering the auth_token, and possibly a fixed "sender" id.
2. Sending the messages.

I have not, therefore, coded up the chat history or room details API methods.

```c#

Usage:
// to set up a local instance of the client
var client = new HipChatClient(auth_token, room_id, name_of_sender);
// send a message
client.SendMessage("Hello World!");

// for one-off use, there is a static method - NB this creates an instance internally, so 
// only use when making one-off calls.
HipChatClient.SendMessage(auth_token, room_id, name_of_sender, "Hello World!");
```

##TODO
1. Proper validation of message XHTML to ensure it won't be rejected by the API
2. Add in history/show methods