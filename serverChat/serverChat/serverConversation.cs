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
        private List<string> messageHistory;
        private List<ServerUser> participants;

        // Add Message To History
        //
        // Add message to the message history
        // @param message The current message
        public void AddMessageToHistory(string message)
        {
            messageHistory.Add(message);
        }

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

        // Get Message History
        //
        // Get the message history of the conversation
        // @return a string list containing the messages of the conversation
        public List<string> GetMessageHistory()
        {
            return messageHistory;
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

        public void addParticipant(ServerUser participant)
        {
            participants.Add(participant);
        }

        public void clearHistory()
        {
            messageHistory.Clear();
        }

        public void deleteConversation()
        {
            
        }

        public void leave(ServerUser user)
        {
            participants.Remove(user);
        }

        public void sendMessage(string message)
        {
            messageHistory.Add(message);
            currentMessage = message;
        }

    }
}
