using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HipChatClientTests
{
    [TestClass]
    public class TestHipChatClient
    {
        const string TEST_AUTH_TOKEN = "";
        const int TEST_ROOM_ID = 0;
        const string TEST_SENDER = "UnitTests";

        [TestMethod]
        [ExpectedException(typeof(HipChat.HipChatApiWebException))]
        public void TestAuthenticationException()
        {
            var client = new HipChat.HipChatClient("X");
            client.ListRooms();
        }

        [TestMethod]
        public void TestYieldRooms()
        {
            var client = new HipChat.HipChatClient(TEST_AUTH_TOKEN);
            var x = 0;
            foreach (HipChat.Entities.Room room in client.YieldRooms())
            {
                x++;
            }
            Assert.IsTrue(x > 0);
        }

        [TestMethod]
        public void TestListRoomsAsJson()
        {
            var client = new HipChat.HipChatClient(TEST_AUTH_TOKEN, HipChat.HipChatClient.ApiResponseFormat.JSON);
            var json = client.ListRooms();
            // not the most scientific test, but it's sunday night
            Assert.IsTrue(json.Contains("{"));
        }

        [TestMethod]
        public void TestListRoomsAsXml()
        {
            var client = new HipChat.HipChatClient(TEST_AUTH_TOKEN, HipChat.HipChatClient.ApiResponseFormat.XML);
            var xml = client.ListRooms();
            // not the most scientific test, but it's sunday night
            Assert.IsTrue(xml.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
        }

        [TestMethod]
        public void TestSendMessage1()
        {
            var client = new HipChat.HipChatClient(TEST_AUTH_TOKEN, TEST_ROOM_ID, "UnitTests");
            client.SendMessage("TestSendMessage1");
        }

        [TestMethod]
        public void TestSendMessage2()
        {
            var client = new HipChat.HipChatClient(TEST_AUTH_TOKEN,TEST_ROOM_ID);
            client.SendMessage("TestSendMessage2", TEST_SENDER);
        }

        [TestMethod]
        public void TestSendMessage3()
        {
            var client = new HipChat.HipChatClient(TEST_AUTH_TOKEN);
            client.SendMessage("TestSendMessage3",TEST_ROOM_ID, TEST_SENDER);
        }

        [TestMethod]
        public void TestSendMessageStatic()
        {
            HipChat.HipChatClient.SendMessage(TEST_AUTH_TOKEN, TEST_ROOM_ID, "UnitTests", "TestSendMessageStatic");
        }
    }
}
