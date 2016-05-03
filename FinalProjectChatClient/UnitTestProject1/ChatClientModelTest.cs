using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinalProjectChatClient;

namespace ChatClientTest
{
    [TestClass]
    public class ChatClientModelTest
    {
        [TestMethod]
        public void ContactListTest()
        {
            ChatClientModel CCM = new ChatClientModel();

        }

        [TestMethod]
        public void ConversationListTest()
        {
            ChatClientModel CCM = new ChatClientModel();

        }

        [TestMethod]
        public void DisplayNameTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            Assert.AreEqual("", CCM.DisplayName);
        }

        [TestMethod]
        public void ErrorFlagTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            Assert.AreEqual(false, CCM.ErrorFlag);
        }

        [TestMethod]
        public void UsernameTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            Assert.AreEqual("", CCM.Username);
        }

        [TestMethod]
        public void StateTest()
        {
            ChatClientModel CCM = new ChatClientModel();

        }

        [TestMethod]
        public void StatusTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            Assert.AreEqual("Online", CCM.Status);
        }

        [TestMethod]
        public void WaitFlagTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            Assert.AreEqual(false, CCM.WaitFlag);
        }

        [TestMethod]
        public void ContactListSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();
        }

        [TestMethod]
        public void ConversationListSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();
        }

        [TestMethod]
        public void DisplayNameSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            CCM.DisplayName = "Bob";
            Assert.AreEqual("Bob", CCM.DisplayName);
        }

        [TestMethod]
        public void ErrorFlagSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            CCM.ErrorFlag = true;
            Assert.AreEqual(true, CCM.ErrorFlag);
        }

        [TestMethod]
        public void UsernameSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            CCM.Username = "Bob";
            Assert.AreEqual("Bob", CCM.Username);
        }

        [TestMethod]
        public void StateSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();

        }

        [TestMethod]
        public void StatusSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            CCM.Status = "Offline";
            Assert.AreEqual("Offline", CCM.Status);
        }

        [TestMethod]
        public void WaitFlagSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            CCM.WaitFlag = true;
            Assert.AreEqual(true, CCM.WaitFlag);
        }
    }
}
