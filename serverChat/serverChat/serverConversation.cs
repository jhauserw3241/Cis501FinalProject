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
        private string currentMessage;

        // Get Conversation Name
        //
        // Get the conversation name
        // @return a string containing the conversation name
        public string GetConversationName()
        {
            return name;
        }

        // Get Current Message
        //
        // Get the current message in the conversation
        // @return a string containing the current message
        public string GetCurrentMessage()
        {
            return currentMessage;
        }

        // Set Conversation Name
        //
        // Set the conversation name
        // @param convName A string containing the conversation name
        public void SetConversationName(string convName)
        {
            name = convName;
        }

        // Set Current Message
        //
        // Set the current message in the conversation
        // @param message A string containing the current message
        public void SetCurrentMessage(string message)
        {
            currentMessage = message;
        }
    }
}
