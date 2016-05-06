using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml;
using System.Text;
using System.Collections.Generic;
using serverChat;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace ServerChatTests
{
    [TestClass]
    public class ControllerUnitTest
    {

        // Test the user constructor
        //
        // Create a user object
        [TestMethod]
        public void CreateUserObj()
        {
            GuiController cont = new GuiController();
        }       
    }
}
