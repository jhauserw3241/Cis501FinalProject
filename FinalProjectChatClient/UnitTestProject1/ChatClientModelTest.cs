using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinalProjectChatClient;

namespace ChatClientTest
{
    [TestClass]
    public class ChatClientModelTest
    {
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
            Assert.AreEqual(FlowState.Entry, CCM.State);
        }

        [TestMethod]
        public void StatusTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            Assert.AreEqual("Offline", CCM.Status);
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
            System.Collections.Generic.List<Contact> cont = new System.Collections.Generic.List<Contact>() { new Contact("Bob", "Bob_KSU", "Offline") };
            CCM.ContactList = cont;
            Assert.AreEqual(cont, CCM.ContactList);
        }

        [TestMethod]
        public void ConversationListSetTest()
        {
            ChatClientModel CCM = new ChatClientModel();
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>> conv = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.List<string>>() { { "New_Conv", new System.Collections.Generic.List<string>() { "Bob" } } };
            CCM.ConversationList = conv;
            Assert.AreEqual(conv, CCM.ConversationList);
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
            CCM.State = FlowState.Access;
            Assert.Equals(FlowState.Access, CCM.State);
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
