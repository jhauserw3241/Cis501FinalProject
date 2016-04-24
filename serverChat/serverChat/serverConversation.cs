﻿using System;
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
        private List<string> participants;

        // Constructor
        public ServerConversation()
        {
        }

        // Constructor
        //
        // @param convName The conversation name
        public ServerConversation(string convName)
        {
            name = convName;
        }

        // Add Message To History
        //
        // Add message to the message history
        // @param message The current message
        public void AddMessageToHistory(string message)
        {
            messageHistory.Add(message);
        }

        // Add Participant
        //
        // Add a participant to the conversation
        // @param username The username of the user to add
        public void AddParicipant(string username)
        {
            participants.Add(username);
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
    }
}
