using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;
using Castle.Windsor;
using HipChat;
using Castle.Windsor.Installer;
using System.Reflection;

namespace HipChatClientTests
{
    [TestClass]
    public class TestHipChatClient
    {
        private static HipChatClient defaultClient;

        [ClassInitialize]
        public static void TestWindsorInstaller(TestContext context)
        {
            IWindsorContainer container = new WindsorContainer();
            container.Install(new HipChatClientInstaller());
            defaultClient = container.Resolve<HipChatClient>("ChatClient");
            defaultClient.From += defaultClient.GetHashCode().ToString();
//            defaultClient.SendMessage("TestWindsorInstaller");
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
            var client = new HipChat.HipChatClient(defaultClient.Token);
            var x = 0;
            foreach (HipChat.Entities.Room room in client.YieldRooms())
            {
                x++;
                Console.WriteLine(room);
            }
            Assert.IsTrue(x > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSenderLengthExecption()
        {
            var client = new HipChat.HipChatClient("X");
            client.AutoTruncate = false;
            client.From = "ABCDEFGHIJKLMNOP";
        }

        [TestMethod]
        public void TestSenderLengthTruncate()
        {
            var client = new HipChat.HipChatClient("X");
            client.From = "ABCDEFGHIJKLMNOP";
        }

        [TestMethod]
        public void TestListRoomsAsJson()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, HipChat.HipChatClient.ApiResponseFormat.JSON);
            var json = client.ListRooms();
            // not the most scientific test, but it's sunday night
            Assert.IsTrue(json.Contains("{"));
        }

        [TestMethod]
        public void TestListRoomsAsXml()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, HipChat.HipChatClient.ApiResponseFormat.XML);
            var xml = client.ListRooms();
            // not the most scientific test, but it's sunday night
            Assert.IsTrue(xml.StartsWith("<?xml version=\"1.0\" encoding=\"utf-8\"?>"));
        }

        [TestMethod]
        public void TestListRoomsAsNativeObjects()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, HipChat.HipChatClient.ApiResponseFormat.XML);
            var rooms = client.ListRoomsAsNativeObjects();
            Assert.IsInstanceOfType(rooms, typeof(List<HipChat.Entities.Room>));
            Assert.IsTrue(rooms.Count > 0); //HACK: the number of rooms is variable, so just check that it's greater than 0 - bit if a hack
        }

        [TestMethod]
        public void TestSendMessage_Message()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId, defaultClient.From);
            client.SendMessage(MethodBase.GetCurrentMethod().Name);
        }

		[TestMethod]
		public void TestSendMessage_HtmlMessage()
		{
			var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId, defaultClient.From);
			client.SendMessage(MethodBase.GetCurrentMethod().Name + " <a href='http://en.wikiquote.org/wiki/Pulp_Fiction#Dialogue'>Quotable &amp; questionable (?) gems from &quot;Pulp Fiction&quot;</a> =)");
		}

		[TestMethod]
		public void TestSendMessage_UnicodeAuthorAndMessage()
		{
			var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId, "lǝʌɐd ツ");
			client.SendMessage(MethodBase.GetCurrentMethod().Name + " ಠ_ಠ");
		}

		[TestMethod]
        public void TestSendMessage_Message_Red()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId, defaultClient.From);
            client.SendMessage(MethodBase.GetCurrentMethod().Name, HipChatClient.BackgroundColor.red);
        }

        [TestMethod]
        public void TestSendMessage_Message_Purple()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId, defaultClient.From);
            client.SendMessage(MethodBase.GetCurrentMethod().Name, HipChatClient.BackgroundColor.purple);
        }

        [TestMethod]
        public void TestSendMessage_Message_green_notify()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId, defaultClient.From);
            client.SendMessage(MethodBase.GetCurrentMethod().Name, HipChatClient.BackgroundColor.green, true);
        }

        [TestMethod]
        public void TestSendMessage_Message_From()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId);
            client.SendMessage(MethodBase.GetCurrentMethod().Name, defaultClient.From);
        }

        [TestMethod]
        public void TestSendMessage_Message_From_Notify()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId);
            client.SendMessage(MethodBase.GetCurrentMethod().Name, defaultClient.From, true);
        }

        [TestMethod]
        public void TestSendMessage_Message_Room_From()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token);
            client.SendMessage(MethodBase.GetCurrentMethod().Name, defaultClient.RoomId, defaultClient.From);
        }

        [TestMethod]
        public void TestSendMessage_Message_Room_From_Notify()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token);
            client.SendMessage(MethodBase.GetCurrentMethod().Name, defaultClient.RoomId, defaultClient.From, true);
        }

        [TestMethod]
        public void TestStaticSendMessage_Message_Room_From()
        {
            HipChat.HipChatClient.SendMessage(defaultClient.Token, defaultClient.RoomId, defaultClient.From, MethodBase.GetCurrentMethod().Name);
        }

        [TestMethod]
        public void TestStaticSendMessage_Message_Room_From_Notify()
        {
            HipChat.HipChatClient.SendMessage(defaultClient.Token, defaultClient.RoomId, defaultClient.From, MethodBase.GetCurrentMethod().Name, true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSendMessageEmptyException()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token);
            client.SendMessage("", defaultClient.RoomId, defaultClient.From);
        }

        [TestMethod]
        public void TestSendMessageTruncate()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token);
            client.SendMessage(GetReallyLongMessage(MethodBase.GetCurrentMethod().Name, 5000), defaultClient.RoomId, defaultClient.From);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestSendMessageTooLongException()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token) { AutoTruncate = false };
            client.SendMessage(GetReallyLongMessage(MethodBase.GetCurrentMethod().Name, 5000), defaultClient.RoomId, defaultClient.From);
        }

        [TestMethod]
        public void TestGetRoomHistory()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId);
            var s = client.RoomHistory(DateTime.Today);
            System.Diagnostics.Trace.WriteLine(s.Length > 50 ? s.Substring(0, 50) : s);
            Assert.IsNotNull(s);
        }


        [TestMethod]
        public void TestUserAlertingWithTextFormatMessage()
        {
            var client = new HipChat.HipChatClient(defaultClient.Token, defaultClient.RoomId.ToString(),HipChatClient.MessageFormat.text);
            client.SendMessage("@all this is a test message notifing all users in the room",
                defaultClient.From);
        }
        /// <summary>
        /// Helper method that generates a string longer than a certain length
        /// </summary>
        /// <param name="callingMethod">The name of the calling method - used as the message prefix</param>
        /// <param name="minLength">The minimum length required - returned string will be longer than this</param>
        /// <returns>A string longer than minLength</returns>
        private string GetReallyLongMessage(string callingMethod, int minLength)
        {
            var s = new StringBuilder(callingMethod);
            while (s.Length <= minLength)
            {
                s.Append(", The quick brown fox jumped over the lazy dog");
            }
            return s.ToString();
        }
    }
}
