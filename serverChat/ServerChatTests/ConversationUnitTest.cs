using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using serverChat;

namespace ServerChatTests
{
    [TestClass]
    public class ConversationUnitTest
    {
        ServerConversation conversation = new ServerConversation("ConversationTest");
        
        [TestMethod]
        public void CreateConversationObj()
        {
            ServerConversation testConv = new ServerConversation("convObjTest");
        }

        [TestMethod]
        public void AddGetConversationMsgHis()
        {
            List<string> messages = new List<string> { "Message 1", "Message 2", "Message 3" };

            conversation.AddMessageToHistory(messages[0]);
            conversation.AddMessageToHistory(messages[1]);
            conversation.AddMessageToHistory(messages[2]);

            Assert.AreEqual(messages[0], conversation.GetMessageHistory()[0]);          
            Assert.AreEqual(messages[1], conversation.GetMessageHistory()[1]);
            Assert.AreEqual(messages[2], conversation.GetMessageHistory()[2]);
        }

        [TestMethod]
        public void SetGetConversationCurrMsg()
        {
            string currmsg = "Last Message";
            conversation.SetCurrentMessage(currmsg);
            Assert.AreEqual(currmsg, conversation.GetCurrentMessage());
        }

        [TestMethod]
        public void GetConversationName()
        {
            Assert.AreEqual("ConversationTest", conversation.GetConversationName());
        }

        [TestMethod]
        public void SetConversationName()
        {
            string convname = "ConversationName";
            conversation.SetConversationName(convname);
            Assert.AreEqual(convname, conversation.GetConversationName());
        }

        [TestMethod]
        public void AddGetConversationParticipants()
        {
            List<string> usernames = new List<string> { "pedro", "joy", "ryan", "anthony" };
            foreach (string u in usernames) conversation.AddParicipant(u);
            Assert.AreEqual(usernames[0], conversation.GetParicipantList()[0]);
            Assert.AreEqual(usernames[1], conversation.GetParicipantList()[1]);
            Assert.AreEqual(usernames[2], conversation.GetParicipantList()[2]);
            Assert.AreEqual(usernames[3], conversation.GetParicipantList()[3]);
        }

        [TestMethod]
        public void RemoveConversationParticipant()
        {
            List<string> usernames = new List<string> { "pedro", "joy", "ryan", "anthony" };
            foreach (string u in usernames) conversation.AddParicipant(u);
            string user = "joy";
            conversation.RemoveParicipant(user);
            Assert.IsFalse(conversation.GetParicipantList().Contains(user));
        }
    }
}
