using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    class RemoveContactMessage : Message
    {
        private ServerUser contact = new ServerUser();
        private Message msg = new Message();

        // Constructor
        //
        // @param cont The contact that is going to be added
        public RemoveContactMessage(ServerUser cont)
        {
            contact = cont;
        }

        // Get Message
        //
        // @return a string containing the XML serialized message
        public string GetMessage()
        {
            msg.AddElement("action", "rmCont");
            msg.AddElement("source", contact.GetUsername());
            return msg.Serialize();
        }
    }
}
