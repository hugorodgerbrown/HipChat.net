HipChapClient library

This is a C# library used to wrap the HipChat API (https://www.hipchat.com/docs/api)

The API is very clear and concise, so there's not much to add to this - this library just makes the formatting of the URL a bit simpler.
The API returns either XML or JSON, and an obvious next step is to add native class support for the response types, however, I haven't done this,
partly because for the listing of rooms JSON is a good bet, as it can be squirted direct to a web page for rendering, and because for the sending 
of messages, I don't really care if a message gets there or not - this is NOT for mission ciritical, transactional, guaranteed delivery!

The API supports four methods:
1. Fetching a list of available chat rooms
2. Sending a message to a room
3. Fetching the chat history from a room
4. Showing the room details (who's in it etc.)

Access to the API is managed by the inclusion of an auth_token with each request. The auth_tokens are managed from the HipChat admin panel, by the account holder.

The core use case for this library is to allow developers to include 'bots' in chat rooms. The sub-cases included to support this are therefore:
1. Administer the 'bot' - which would involve selecting a room from a list of rooms, and entering the auth_token, and possibly a fixed "sender" id.
2. Sending the messages.

I have not, therefore, coded up the chat history or room details API methods.

Usage:


TODO:
1. Proper validation of message XHTML to ensure it won't be rejected by the API
2. Add in history/show methods
3. Convert return values into native classes?
4. Some unit tests ;-)