using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    class UpdateConversationMessage : Message
    {
        private ServerConversation conv = new ServerConversation();
        private Message msg = new Message();

        // Constructor
        //
        // @param c The conversation object to be added
        public UpdateConversationMessage(ServerConversation c)
        {
            conv = c;
        }

        // Get Message
        //
        // Get the XML seriliazed message to add the conversation
        // @return a string that contains the conversation information
        public string GetMessage()
        {
            // Convert the participants list to a single string
            string parUNames = string.Join(",", conv.GetParticipantListUsernames());

            // Create message
            msg.AddElement("action", "udConv");
            msg.AddElement("conv", conv.GetConversationName());
            msg.AddElement("par", parUNames);
            return msg.Serialize();
        }
    }
}
