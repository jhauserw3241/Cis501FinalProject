using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using serverChat;

namespace ServerChatTests
{
    [TestClass]
    public class UserUnitTest
    {
        ServerUser user = new ServerUser("userTest");

        [TestMethod]
        public void CreateUserObj()
        {
            ServerUser testConv = new ServerUser("userObjTest");
        }

        [TestMethod]
        public void GetUserName()
        {
            Assert.AreEqual("userTest", user.GetUsername());
        }

        [TestMethod]
        public void GetName()
        {
            Assert.AreEqual("userTest", user.GetName());
        }

        [TestMethod]
        public void SetName()
        {
            string name = "Pedro";
            user.SetName(name);
            Assert.AreEqual(name, user.GetName());
        }

        [TestMethod]
        public void SetGetStatus()
        {
            bool status = false;
            user.SetStatus(status);
            Assert.AreEqual(status, user.GetStatus());
        }

        [TestMethod]
        public void SetGetIPAddress()
        {
            string Ip = "130.140.80.32";
            user.SetIpAddress(Ip);
            Assert.AreEqual(Ip, user.GetIpAddress());
        }

        [TestMethod]
        public void SetGetPassword()
        {
            string password = "mypassword";
            user.setPassword(password);
            Assert.AreEqual(password, user.GetPassword());
        }

        [TestMethod]
        public void AddGetContactList()
        {
            ServerUser pedro = new ServerUser("pedro");
            ServerUser joy = new ServerUser("joy");
            ServerUser anthony = new ServerUser("anthony");
            ServerUser ryan = new ServerUser("ryan");

            List <ServerUser> users = new List<ServerUser> {pedro, joy, anthony, ryan};

            foreach (ServerUser u in users) user.AddContact(u);

            Assert.AreEqual(users[0], user.GetContactList()[0]);
            Assert.AreEqual(users[1], user.GetContactList()[1]);
            Assert.AreEqual(users[2], user.GetContactList()[2]);
            Assert.AreEqual(users[3], user.GetContactList()[3]);
        }

        [TestMethod]
        public void GetContactListUserName()
        {
            ServerUser pedro = new ServerUser("pedro");
            ServerUser joy = new ServerUser("joy");
            ServerUser anthony = new ServerUser("anthony");
            ServerUser ryan = new ServerUser("ryan");

            List<ServerUser> users = new List<ServerUser> { pedro, joy, anthony, ryan };

            foreach (ServerUser u in users) user.AddContact(u);

            Assert.AreEqual(users[0].GetUsername(), user.GetContactListUsernames()[0]);
            Assert.AreEqual(users[1].GetUsername(), user.GetContactListUsernames()[1]);
            Assert.AreEqual(users[2].GetUsername(), user.GetContactListUsernames()[2]);
            Assert.AreEqual(users[3].GetUsername(), user.GetContactListUsernames()[3]);
        }

        [TestMethod]
        public void RemoveContact()
        {
            ServerUser pedro = new ServerUser("pedro");
            ServerUser joy = new ServerUser("joy");
            ServerUser anthony = new ServerUser("anthony");
            ServerUser ryan = new ServerUser("ryan");

            List<ServerUser> users = new List<ServerUser> { pedro, joy, anthony, ryan };

            foreach(ServerUser u in users) user.AddContact(u);

            user.RemoveContact(pedro);

            Assert.IsFalse(user.GetContactList().Contains(pedro));
        }

        [TestMethod]
        public void RemoveContactByUserName()
        {
            ServerUser pedro = new ServerUser("pedro");
            ServerUser joy = new ServerUser("joy");
            ServerUser anthony = new ServerUser("anthony");
            ServerUser ryan = new ServerUser("ryan");

            List<ServerUser> users = new List<ServerUser> { pedro, joy, anthony, ryan };

            foreach (ServerUser u in users) user.AddContact(u);

            user.RemoveContact(pedro.GetUsername());

            Assert.IsFalse(user.GetContactList().Contains(pedro));
        }
    }
}