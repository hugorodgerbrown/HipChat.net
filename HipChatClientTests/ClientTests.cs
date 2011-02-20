using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HipChatClientTests
{
    [TestClass]
    public class TestHipChapClient
    {
        const string AUTH_TOKEN = "";

        [TestMethod]
        [ExpectedException(typeof(HipChat.HipChatApiWebException))]
        public void TestAuthenticationException()
        {
            var client = new HipChat.HipChatClient("X");
            client.ListRooms();
        }

        [TestMethod]
        public void TestListRooms()
        {
            var client = new HipChat.HipChatClient(AUTH_TOKEN);
            foreach (HipChat.Entities.Room room in client.YieldRooms())
            {
                Assert.AreEqual(1, 1);
            }
        }
    }
}
