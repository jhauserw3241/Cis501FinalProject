﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace serverChat
{
    public class ServerController : WebSocketBehavior
    {
        private ServerModel data = new ServerModel();
        public event ServerOutputHandler output;

        #region Manipulate Class
        // Constructor
        public ServerController() : this(null)
        {
        }

        // Constructor
        //
        // @arg d The model object instance
        public ServerController(ServerModel d)
        {
            // Update the server objects
            data = d;
        }

        // Destructor
        ~ServerController()
        {
        }
        #endregion

        #region Handle Client Input
        // On Open
        //
        // Handle actions to take when the server is first started
        protected override void OnOpen()
        {
        }

        // On Message
        //
        // Handle actions when a message is received from one of the clients
        // @param e The client message information
        protected override void OnMessage(MessageEventArgs e)
        {
        }
        #endregion

        // Add Contact To List
        //
        // Add the contact to the list of all of the users in the model
        // @arg user The current user
        public void AddContactToList(ServerUser user)
        {
            // Get the list of all contacts from the model
            List<ServerUser> userList = data.GetUserList();

            // Add the current user object
            userList.Add(user);

            // Set the current user list to the model user list
            data.SetUserList(userList);
        }

        // Add Conversation
        //
        // Add conversation to the list of conversations
        // @param name The conversation name of the new conversation
        public void AddConversation(string name)
        {
            // Create conversation object from conversation info
            ServerConversation conv = new ServerConversation(name);

            // Add conversation to list
            List<ServerConversation> convList = data.GetConversationList();
            convList.Add(conv);

            // Update the view
            output("UpdateConversationList", convList);
        }

        // Add Participant To Conversation
        //
        // Add the specified participant to the specified conversation
        // @arg convName The name of the conversation
        // @arg username The username of the new participant
        // @return whether or not the participant was added to the conversation
        public bool AddParticipantToConversation(string convName, string username)
        {
            Dictionary<string, List<string>> convRelationships = data.GetContactRelationshipDict();
            List<string> participants = GetConvParticipants(convName);

            // Determine if the current conversation exists or not
            if (participants == new List<string>())
            {
                return false;
            }

            // Put the username of the new participant into the list of participants
            participants.Add(username);

            // Remove the current conversation from the list
            convRelationships.Remove(convName);

            // Add the new list of participants to the contact relationship list
            convRelationships.Add(convName, participants);

            // Set the current list of names to the list on the server
            data.SetContactRelationshipList(convRelationships);

            return true;
        }

        // Add User
        //
        // Add user to list of users
        // @param username The username of the new user
        // @param dName The display name of the new user
        // @param status The status of the new user
        public void AddUser(string username, string dName, STATUS status)
        {
            // Create user object from user info
            ServerUser user = new ServerUser();
            user.SetUsername(username);
            user.SetName(dName);
            user.SetStatus(status);

            // Add user to list
            List<ServerUser> userList = data.GetUserList();
            userList.Add(user);

            // Update the view
            output("UpdateUserList", userList);
        }

        // Get Conversation Object
        //
        // Get the conversation from the list in the model
        // @arg name The name of the conversation
        // @return the conversation object
        public ServerConversation GetConvObj(string name)
        {
            List<ServerConversation> convList = data.GetConversationList();
            int convListLen = convList.Count;

            if (convListLen != 0)
            {
                for (int i = 0; i < convListLen; i++)
                {
                    // TODO: Check if the conversation is in the list and return it
                    return new ServerConversation();
                }
            }

            return new ServerConversation();
        }

        // Get Conversation Participants
        //
        // Get the list of participants from the specified conversation
        // @arg convName The name of the conversation
        public List<string> GetConvParticipants(string convName)
        {
            // Get list of all conversation relationships
            Dictionary<string, List<string>> convRelationships = data.GetContactRelationshipDict();

            // Get the list of participants in the current conversation
            List<string> participants = new List<string>();
            convRelationships.TryGetValue(convName, out participants);

            return participants;
        }

        // Get Conversation Relationship List
        //
        // Get the conversation relationship from the list in the model
        // @arg groupName The name of the group of contacts
        // @return the list of usernames for the specified group
        public List<string> GetConvRelList(string groupName)
        {
            Dictionary<string, List<string>> groupList = data.GetContactRelationshipDict();
            int relationLen = groupList.Count;

            if (relationLen != 0)
            {
                for (int i = 0; i < relationLen; i++)
                {
                    // TODO: Check if the conversation relationship list and return username list
                    return new List<string>();
                }
            }

            return new List<string>();
        }

        // Get User Object
        //
        // Get the user object from the list in the model
        // @arg username The username of the requested user object
        // @return the user object
        public ServerUser GetUserObj(string username)
        {
            List<ServerUser> usersList = data.GetUserList();
            int usersListLen = usersList.Count;

            if (usersListLen != 0)
            {
                for (int i = 0; i < usersListLen; i++)
                {
                    // TODO: Check if the user's username matches and return it
                    return new ServerUser();
                }
            }

            return new ServerUser();
        }

        // Is Username Used
        //
        // Check if hte username is currently being used by another user
        // @param name The username being requested
        public bool IsUsernameUsed(string name)
        {
            List<ServerUser> userList = data.GetUserList();
            int size = userList.Count;
            for (int i = 0; i < size; i++)
            {
                if (name == userList.ElementAt(i).GetUsername())
                {
                    return true;
                }
            }

            return false;
        }

        // Remove Conversation From List
        //
        // Remove the conversation from the list in the model
        // @arg name The name of the conversation
        public void RemoveConvFromList(string name)
        {
            // Get the list of all conversations from the model
            List<ServerConversation> convList = new List<ServerConversation>();

            // Get the conversation object from the list in the model
            ServerConversation conv = GetConvObj(name);

            // Remove the conversation object from the list of all conversations from the model
            convList.Remove(conv);

            // Set the conversation list to the updated list
            data.SetConversationList(convList);
        }

        // Remove Participant From Conversation Participants
        //
        // Remove the user with the specified username
        // @arg username The username for the current user
        // @arg name The name of the conversation
        public void RemoveParFromConvPars(string username, string name)
        {
            // Get the list of contact relationships
            Dictionary<string, List<string>> contRels = data.GetContactRelationshipDict();

            // Get the list of participants from the current conversation
            List<string> participants = GetConvParticipants(name);

            // Remove the current conversation group from the list
            contRels.Remove(name);

            // Add new user to the current contact relationship
            participants.Add(username);

            // Add the new contact relationship
            contRels.Add(name, participants);

            // Set the new list of participants as a new relationship
            data.SetContactRelationshipList(contRels);
        }

        // Remove User From List
        //
        // Remove the user from the list in the model
        // @arg username The username for the current user
        public void RemoveUserFromList(string username)
        {
            // Get the list of all users from the model
            List<ServerUser> userList = data.GetUserList();

            // Get the user object from the list in the model
            ServerUser user = GetUserObj(username);

            // Remove the user object from the list of all users from the model
            userList.Remove(user);

            // Set the user list to the updated list
            data.SetUserList(userList);
        }
    }
}
