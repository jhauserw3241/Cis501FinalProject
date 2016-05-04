using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    class AddContactMessage
    {
        private ServerUser contact = new ServerUser();
        private Message msg = new Message();

        // Constructor
        //
        // @param cont The contact that is going to be added
        public AddContactMessage(ServerUser cont)
        {
            contact = cont;
        }

        // Get Message
        //
        // @return a string containing the XML serialized message
        public string GetMessage()
        {
            msg.AddElement("action", "addCont");
            msg.AddElement("username", contact.GetUsername());
            msg.AddElement("dispName", contact.GetName());
            msg.AddElement("state", contact.GetStatus().ToString());
            return msg.Serialize();
        }
    }
}
