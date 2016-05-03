using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using serverChat;

namespace ServerChatTests
{
    [TestClass]
    public class ModelUnitTest
    {
        ServerModel model = new ServerModel();

        // Test the model constructor
        //
        // Create a model object
        [TestMethod]
        public void CreateUserObj()
        {
            ServerModel testUser = new ServerModel();
        }

        // Test SetConversationList() and GetConversationList() methods
        //
        // Compare the value returned by the method with the value assigned to the object
        [TestMethod]
        public void SetGetConversationList()
        {
            ServerConversation conv1 = new ServerConversation();
            ServerConversation conv2 = new ServerConversation();
            ServerConversation conv3 = new ServerConversation();
            ServerConversation conv4 = new ServerConversation();

            List<ServerConversation> convs = new List<ServerConversation> { conv1, conv2, conv3, conv4};

            model.SetConversationList(convs);

            Assert.AreEqual(convs, model.GetConversationList());            
        }

        // Test SetUserList() and GetUserList() method
        //
        // Create a list of users and use the SetUserList() method on the model to set it 
        // and then compare with value returned by the GetUserList() method
        [TestMethod]
        public void SetGetUserList()
        {
            ServerUser pedro = new ServerUser("pedro");
            ServerUser joy = new ServerUser("joy");
            ServerUser anthony = new ServerUser("anthony");
            ServerUser ryan = new ServerUser("ryan");

            List<ServerUser> users = new List<ServerUser> { pedro, joy, anthony, ryan };

            model.SetUserList(users);

            Assert.AreEqual(users, model.GetUserList());
        }
        

        // Test SetUserList() and GetUserList() method
        //
        // Create a list of users and use the SetUserList() method on the model to set it 
        // and then compare with value returned by the GetUserList() method
        [TestMethod]
        public void SetGetRealationshipDic()
        {
            List<string> strs = new List <string>{ "str1", "str2", "str3", "str4"};
            List<List<string>> lists = new List <List<string>>{ strs, strs, strs, strs};

            Dictionary <string, List<string>> dict = new Dictionary<string, List<string>>();

            model.SetContactRelationshipList(dict);
            Assert.AreEqual(dict, model.GetContactRelationshipDict());

        }
        }
    }