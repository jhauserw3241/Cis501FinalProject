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
        private static ServerModel data = new ServerModel();
        private static GuiController cont = new GuiController(data);

        [TestMethod]
        public void DeserializeXmlTest1()
        {
            string message = "<udConv conv=\"Conv_Name\" par=\"newUser420,admin\" msg=\"This is working!\" />";

            // Getting the version of the output from the message
            Dictionary<string, string> output1A = cont.DeserializeXml(message);

            // Create a dictionary containing the correct information
            Dictionary<string, string> output2 = new Dictionary<string, string>();
            output2.Add("action", "udConv");
            output2.Add("conv", "Conv_Name");
            output2.Add("asdf", "abc");
            output2.Add("pars", "newUser420,admin");
            output2.Add("msg", "This is working!");

            int size = output1A.Count;
            Assert.AreEqual(size, output2.Count);
            List<string> dict1Keys = new List<string>(output1A.Keys);
            List<string> dict2Keys = new List<string>(output2.Keys);
            List<string> dict1Values = new List<string>(output1A.Values);
            List<string> dict2Values = new List<string>(output2.Values);
            for (int i = 0; i < size; i++)
            {
                Assert.AreEqual(dict1Keys[i], dict2Keys[i]);
                Assert.AreEqual(dict1Values[i], dict2Values[i]);
            }
            
        }
    }
}
