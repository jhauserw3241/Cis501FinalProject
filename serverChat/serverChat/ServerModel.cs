using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    class ServerModel
    {
        // Declare the server lists
        private List<serverConversation> conversationList = new List<serverConversation>();

        // Get Conversation List
        //
        // Get the list of all conversations that have been created
        // @return the list of server conversations
        List<serverConversation> GetConversationList()
        {
            return conversationList;
        }

        // Set Conversation List
        //
        // Set the list of all conversations that have been created
        // @arg list A list of conversations created since the server started
        void SetConversationList(List<serverConversation> list)
        {
            conversationList = list;
        }
    }
}
