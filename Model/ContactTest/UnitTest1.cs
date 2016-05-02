using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ContactTest
{
    [TestClass]
    public class UnitTest1
    {
        //Empty Constructor Test
        [TestMethod]
        public void EmptyConstructor()
        {
            Contact.ContactClient cC = new Contact.ContactClient();
            
        }

        //Constructor Test
        [TestMethod]
        public void Constructor()
        {
            Contact.ContactClient cC = new Contact.ContactClient()
        }
    }
}
