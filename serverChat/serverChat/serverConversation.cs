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
        private List<string> messageHistory = new List<string>();
        private List<ServerUser> participants = new List<ServerUser>();

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
        // @param par The user object for the new participant
        public void AddParicipant(ServerUser par)
        {
            participants.Add(par);
        }

        // Contains Participant
        //
        // Check whether the specified participant is in the conversation
        // @param par The server user object
        // @return whether or not the participant is in the conversation
        public bool ContainsParticipant(ServerUser par)
        {
            return participants.Contains(par);
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

        // Get Participant List
        //
        // Get the participant list of the conversation
        // @return a string list containing the usernames of the paricipants
        public List<ServerUser> GetParicipantList()
        {
            return participants;
        }

        // Get Participants List Usernames
        //
        // Get the list of usernames of the participants in the conversation
        // @return the list of usernames for the participant list
        public List<string> GetParticipantListUsernames()
        {
            List<string> usernames = new List<string>();
            int size = participants.Count;

            for (int i = 0; i < size; i++)
            {
                usernames.Add(participants.ElementAt(i).GetUsername());
            }

            return usernames;
        }

        // Remove Participant
        //
        // Remove a participant to the conversation
        // @param username The username of the user to remove
        public void RemoveParicipant(ServerUser par)
        {
            participants.Remove(par);
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
