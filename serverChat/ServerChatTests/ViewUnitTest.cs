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

            ServerView viewtest = new ServerView();
        }

    }
}
