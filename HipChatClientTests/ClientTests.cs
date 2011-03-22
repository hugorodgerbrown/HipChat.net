using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Castle.Windsor;
using HipChat;
using Castle.Windsor.Installer;

namespace HipChatClientTests
{
    [TestClass]
    public class TestHipChatClient
    {
        private static HipChatClient DefaultClient;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(new HipChatClientInstaller());
            DefaultClient = container.Resolve<HipChatClient>("ChatClient1");
        }

        [TestMethod]
        [ExpectedException(typeof(HipChat.HipChatApiWebException))]
        public void TestAuthenticationException()
        {
            var client = new HipChatClient(){Token="XYZ", RoomId=123};
            client.ListRooms();
        }

        [TestMethod]
        public void TestYieldRooms()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token);
            var x = 0;
            foreach (HipChat.Entities.Room room in client.YieldRooms())
            {
                x++;
            }
            Assert.IsTrue(x > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSenderLengthExecption()
        {
            var client = new HipChat.HipChatClient("X");
            client.From = "ABCDEFGHIJKLMNOP";
        }

        [TestMethod]
        public void TestListRoomsAsJson()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token, HipChat.HipChatClient.ApiResponseFormat.JSON);
            var json = client.ListRooms();
            // not the most scientific test, but it's sunday night
            Assert.IsTrue(json.Contains("{"));
        }

        [TestMethod]
        public void TestListRoomsAsXml()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token, HipChat.HipChatClient.ApiResponseFormat.XML);
            var xml = client.ListRooms();
            // not the most scientific test, but it's sunday night
            Assert.IsTrue(xml.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
        }

        [TestMethod]
        public void TestListRoomsAsNativeObjects()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token, HipChat.HipChatClient.ApiResponseFormat.XML);
            var rooms = client.ListRoomsAsNativeObjects();
            Assert.IsInstanceOfType(rooms, typeof(List<HipChat.Entities.Room>));
            Assert.AreEqual(3, rooms.Count);
        }

        [TestMethod]
        public void TestSendMessage1()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token, DefaultClient.RoomId, DefaultClient.From);
            client.SendMessage("TestSendMessage1");
        }

        [TestMethod]
        public void TestSendMessage2()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token, DefaultClient.RoomId);
            client.SendMessage("TestSendMessage2", DefaultClient.From);
        }

        [TestMethod]
        public void TestSendMessage3()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token);
            client.SendMessage("TestSendMessage3", DefaultClient.RoomId, DefaultClient.From);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSendMessageEmptyException()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token);
            client.SendMessage("", DefaultClient.RoomId, DefaultClient.From);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSendMessageTooLongException()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token);
            var s = new StringBuilder();
            while (s.Length <= 5000)
            {
                s.Append("The quick brown fox jumped over the lazy dog");
            }
            client.SendMessage(s.ToString(), DefaultClient.RoomId, DefaultClient.From);
        }

        [TestMethod]
        public void TestSendMessageStatic()
        {
            HipChat.HipChatClient.SendMessage(DefaultClient.Token, DefaultClient.RoomId, DefaultClient.From, "TestSendMessageStatic");
        }

        [TestMethod]
        public void TestGetRoomHistory()
        {
            var client = new HipChat.HipChatClient(DefaultClient.Token, DefaultClient.RoomId);
            var s = client.RoomHistory(DateTime.Today.AddDays(-1));
            System.Diagnostics.Trace.WriteLine(s.Length > 50 ? s.Substring(0, 50) : s);
            Assert.IsNotNull(s);
        }
    }
}
