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

        // Test the user constructor
        //
        // Create a user object
        [TestMethod]
        public void CreateUserObj()
        {
            ServerUser testUser = new ServerUser("userObjTest");
        }

        // Test GetUsername() method
        //
        // Compare the value returned by the method with the value assigned to the object
        [TestMethod]
        public void GetUserName()
        {
            Assert.AreEqual("userTest", user.GetUsername());
        }

        // Test GetName() method
        //
        // Compare the value returned by the method with the value assigned to the object
        [TestMethod]
        public void GetName()
        {
            Assert.AreEqual("userTest", user.GetName());
        }

        // Test SetName() method
        //
        // Use the method to set a value and then compare the value assigned with the value returned by the GetName() method
        [TestMethod]
        public void SetName()
        {
            string name = "Pedro";
            user.SetName(name);
            Assert.AreEqual(name, user.GetName());
        }

        // Test SetStatus() and GetStatus() methods
        //
        // Set a value with the set method and then compare with the value returned by 
        // the get method
        [TestMethod]
        public void SetGetStatus()
        {
            STATUS status = STATUS.Offline;
            user.SetStatus(status);
            Assert.AreEqual(status, user.GetStatus());
        }

        // Test SetIpAddress() and GetIpAddress() methods
        //
        // Set a value with the set method and then compare with the value returned 
        // by the get method
        [TestMethod]
        public void SetGetIPAddress()
        {
            string Ip = "130.140.80.32";
            user.SetIpAddress(Ip);
            Assert.AreEqual(Ip, user.GetIpAddress());
        }

        // Test SetPassword() and GetPassword() methods
        //
        // Set a value with the set method and then compare with the value returned by the get method
        [TestMethod]
        public void SetGetPassword()
        {
            string password = "mypassword";
            user.SetPassword(password);
            Assert.AreEqual(password, user.GetPassword());
        }

        // Test AddContact() and GetContactList() method
        //
        // Create a list of users and add each one to the user contact list and then compare
        // each element of the list returned by the GetContactList() method with the list 
        // created before
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

        // Test GetContactListUsername() method
        //
        // Create a list of users and add each one to the user contact list and then compare
        // each element of the list returned by the method with the value returned by the 
        // GetUsername() method of each user in the list created before
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

        // Test RemoveContact() method
        //
        // Create a list of users and add each one to the user contact list, then use the 
        // RemoveContact() method to remove a specific user and then check if the value 
        // returned by the Contains() method of the GetContactList is false. 
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

        // Test RemoveContact() method
        //
        // Create a list of users and add each one to the user contact list, then use the 
        // RemoveContact() method to remove a specific user and then check if the value 
        // returned by the Contains() method of the GetContactList is false.
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