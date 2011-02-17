using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HipChatClientTests
{
    [TestClass]
    public class TestHipChapClient
    {
        [TestMethod]
        [ExpectedException(typeof(HipChat.HipChatApiWebException))]
        public void TestAuthenticationException()
        {
            var client = new HipChat.HipChatClient("X");
            client.ListRooms();
        }
    }
}
