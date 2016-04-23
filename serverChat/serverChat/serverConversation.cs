using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    public class ServerConversation
    {
        private string name;

        // Get Conversation Name
        //
        // Get the conversation name
        // @return a string containing the conversation name
        public string GetConversationName()
        {
            return name;
        }

        // Set Conversation Name
        //
        // Set the conversation name
        // @param convName A string containing the conversation name
        public void SetConversationName(string convName)
        {
            name = convName;
        }
    }
}
