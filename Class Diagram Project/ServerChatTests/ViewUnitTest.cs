using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using serverChat;

namespace ServerChatTests
{
    [TestClass]
    class ViewUnitTest
    {

        ServerModel data = new ServerModel();
        ServerView view;

        public void intializeTest()
        {
            ServerConversation conv1 = new ServerConversation();
            ServerConversation conv2 = new ServerConversation();
            ServerConversation conv3 = new ServerConversation();
            ServerConversation conv4 = new ServerConversation();
            List<ServerConversation> convs = new List<ServerConversation> { conv1, conv2, conv3, conv4 };
            data.SetConversationList(convs);

            ServerUser pedro = new ServerUser("pedro");
            ServerUser joy = new ServerUser("joy");
            ServerUser anthony = new ServerUser("anthony");
            ServerUser ryan = new ServerUser("ryan");
            List<ServerUser> users = new List<ServerUser> { pedro, joy, anthony, ryan };
            data.SetUserList(users);

            List<string> strs = new List<string> { "str1", "str2", "str3", "str4" };
            List<List<string>> lists = new List<List<string>> { strs, strs, strs, strs };
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            data.SetContactRelationshipList(dict);

            view = new ServerView(data);
        }

        // Test the view constructor
        //
        // Create and intialize a model and then create the view  object
        [TestMethod]
        public void CreateViewObj()
        {
            ServerModel datatest = new ServerModel();
           
            ServerConversation conv1 = new ServerConversation();
            ServerConversation conv2 = new ServerConversation();
            ServerConversation conv3 = new ServerConversation();
            ServerConversation conv4 = new ServerConversation();
            List<ServerConversation> convs = new List<ServerConversation> { conv1, conv2, conv3, conv4 };
            datatest.SetConversationList(convs);

            ServerUser pedro = new ServerUser("pedro");
            ServerUser joy = new ServerUser("joy");
            ServerUser anthony = new ServerUser("anthony");
            ServerUser ryan = new ServerUser("ryan");
            List<ServerUser> users = new List<ServerUser> { pedro, joy, anthony, ryan };
            datatest.SetUserList(users);

            List<string> strs = new List<string> { "str1", "str2", "str3", "str4" };
            List<List<string>> lists = new List<List<string>> { strs, strs, strs, strs };
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            datatest.SetContactRelationshipList(dict);

            ServerView viewtest = new ServerView(datatest);
        }


        // Test the view constructor
        //
        // Create and intialize a model and then create the view  object
        [TestMethod]
        public void DisplayPolpulateMethods()
        {
            ServerModel datatest = new ServerModel();

            ServerConversation conv1 = new ServerConversation();
            ServerConversation conv2 = new ServerConversation();
            ServerConversation conv3 = new ServerConversation();
            ServerConversation conv4 = new ServerConversation();
            List<ServerConversation> convs = new List<ServerConversation> { conv1, conv2, conv3, conv4 };
            datatest.SetConversationList(convs);

            ServerUser pedro = new ServerUser("pedro");
            ServerUser joy = new ServerUser("joy");
            ServerUser anthony = new ServerUser("anthony");
            ServerUser ryan = new ServerUser("ryan");
            List<ServerUser> users = new List<ServerUser> { pedro, joy, anthony, ryan };
            datatest.SetUserList(users);

            List<string> strs = new List<string> { "str1", "str2", "str3", "str4" };
            List<List<string>> lists = new List<List<string>> { strs, strs, strs, strs };
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            datatest.SetContactRelationshipList(dict);

            ServerView viewtest = new ServerView(datatest);
        }



    }
}
