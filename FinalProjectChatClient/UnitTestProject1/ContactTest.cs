using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FinalProjectChatClient;

namespace ChatClientTest
{
    [TestClass]
    public class ContactTest
    {
        [TestMethod]
        public void ContactUsernameTest()
        {
            Contact cnt = new Contact("Joy", "Joy_KSU", "Online");
            Assert.AreEqual("Joy", cnt.Username);
        }

        [TestMethod]
        public void ContactDisplayTest()
        {
            Contact cnt = new Contact("Joy", "Joy_KSU", "Online");
            Assert.AreEqual("Joy_KSU", cnt.DisplayName);
        }

        [TestMethod]
        public void ContactStatusTest()
        {
            Contact cnt = new Contact("Joy", "Joy_KSU", "Online");
            Assert.AreEqual("Online", cnt.Status);
        }

        [TestMethod]
        public void ContactToStringTest()
        {
            Contact cnt = new Contact("Joy", "Joy_KSU", "Online");
            Assert.AreEqual("Joy_KSU : Joy : Online", cnt.ToString());
        }

        [TestMethod]
        public void ContactDisplaySetTest()
        {
            Contact cnt = new Contact("Joy", "Joy_KSU", "Online");
            cnt.DisplayName = "Joy_CIS";
            Assert.AreEqual("Joy_CIS", cnt.DisplayName);
        }

        [TestMethod]
        public void ContactStatusSetTest()
        {
            Contact cnt = new Contact("Joy", "Joy_KSU", "Online");
            cnt.Status = "Offline";
            Assert.AreEqual("Offline", cnt.Status);
        }
    }
 }
