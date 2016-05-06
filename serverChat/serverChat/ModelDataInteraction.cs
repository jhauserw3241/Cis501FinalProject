using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverChat
{
    // Model Data Interaction
    //
    // Handle any sort of interaction between the other controllers and the model
    class ModelDataInteraction
    {
        private ServerModel data = ServerModel.Instance;

        // Constructor
        public ModelDataInteraction()
        {
        }

        #region Check Availability
        // Is Username Used
        //
        // Check if the username is currently being used by another user
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

        // Is Conversation Name Used
        //
        // Check if the conversation name is currently being used by another conversation
        // @param name The conversation name being requested
        public bool IsConvNameUsed(string name)
        {
            List<ServerConversation> convList = data.GetConversationList();
            int size = convList.Count;
            for (int i = 0; i < size; i++)
            {
                if (name == convList.ElementAt(i).GetConversationName())
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Convert Data Types
        // Convert String to Status
        //
        // Convert the string to a status type
        // @param stringStatus The string containing the status
        // @return the status type
        public STATUS ConvertStringStatus(string stringStatus)
        {
            switch (stringStatus)
            {
                case "Online":
                    return STATUS.Online;
                case "Away":
                    return STATUS.Away;
                case "Offline":
                    return STATUS.Offline;
                default:
                    return STATUS.Offline;
            }
        }
        #endregion

        #region Create Objects
        // Create Conversation
        //
        // Create conversation and add it to the list of conversations
        // @param name The conversation name of the new conversation
        // @return the error message string if there is one+
        public string CreateConversation(string name)
        {
            // Check if the username is already being used
            if (IsConvNameUsed(name))
            {
                return "Conversation name is already being used.";
            }

            // Create conversation object from conversation info
            ServerConversation conv = new ServerConversation(name);

            // Add conversation to list
            AddConvToList(conv);

            return "";
        }

        // Create User
        //
        // Create user and add it to list of users
        // @param uname The username of the new user
        // @param pass The password of the new user
        // @return the error message string if there is one
        public string CreateUser(string uname, string pass)
        {
            // Check if the username is already being used
            if (IsUsernameUsed(uname))
            {
                return "Username is already being used.";
            }

            // Create user object from user info
            ServerUser user = new ServerUser(uname);
            user.SetPassword(pass);
            user.SetStatus(STATUS.Online);
            user.SetID(GetNextId());

            // Add user to list
            AddUserToList(user);

            return "";
        }
        #endregion

        #region Destroy Objects
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
        #endregion

        #region Get Model Info
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
                    // Return the conversation with the specified name
                    ServerConversation conv = convList.ElementAt(i);
                    if (conv.GetConversationName() == name)
                    {
                        return conv;
                    }
                }
            }

            return null;
        }

        // Get Next ID
        //
        // Get the next ID
        // @return an integer with the ID
        public int GetNextId()
        {
            int id = data.GetId();
            data.SetNextId(id + 1);
            return id;
        }

        // Get User Object
        //
        // Get the user object from the list in the model
        // @arg username The username of the requested user object
        // @return the user object
        public ServerUser GetUserObj(string username)
        {
            List<ServerUser> usersList = data.GetUserList();
            int size = usersList.Count;

            if (size != 0)
            {
                for (int i = 0; i < size; i++)
                {
                    ServerUser curUser = usersList.ElementAt(i);
                    if (curUser.GetUsername() == username)
                    {
                        return curUser;
                    }
                }
            }

            return null;
        }
        #endregion

        #region Modify Data
        // Add User To List
        //
        // Add the user to the list of all of the users in the model
        // @arg user The current user
        public void AddUserToList(ServerUser user)
        {
            UpdateUserList(null, user);
        }

        // Add Conversation To List
        //
        // Add the conversation to the list of all conversations in the model
        // @param conv The current conversation
        public void AddConvToList(ServerConversation conv)
        {
            UpdateConvList(null, conv);
        }

        // Change Conversation Name
        //
        // Change the conversation name
        // @param conv The current conversation
        // @param name The new name for the conversation
        // @return the updated conversation object
        public ServerConversation ChangeConvName(ServerConversation conv, string name)
        {
            ServerConversation newConv = conv;
            newConv.SetConversationName(name);
            UpdateConvList(conv, newConv);
            return newConv;
        }

        // Remove User From List
        //
        // Remove the user from the list in the model
        // @arg username The username for the current user
        public void RemoveUserFromList(string username)
        {
            UpdateUserList(GetUserObj(username), null);
        }

        // Update Contact Relationships
        //
        // Update the contact relationships for all the user involved
        // @param users The list of users who want to become contacts
        // @return whether or not the action was successful
        public bool UpdateContactRelationships(List<ServerUser> users, string action)
        {
            // Update all the user's contact lists
            int size = users.Count;
            for (int i = 0; i < size; i++)
            {
                // Get relevant information
                ServerUser[] tmpUsers = new ServerUser[size];
                users.CopyTo(tmpUsers);
                List<ServerUser> tempUsers = tmpUsers.ToList();
                ServerUser oldCurUser = tempUsers.ElementAt(i);
                ServerUser newCurUser = oldCurUser;
                tempUsers.Remove(newCurUser);

                // Update the contact list of the current user
                int tempSize = tempUsers.Count;
                for (int j = 0; j < tempSize; j++)
                {
                    ServerUser tempUser = tempUsers.ElementAt(j);
                    if (action == "add")
                    {
                        newCurUser.AddContact(tempUser);
                    }
                    else if (action == "remove")
                    {
                        newCurUser.AddContact(tempUser);
                    }
                }

                // Update the model data
                UpdateUserList(oldCurUser, newCurUser);
            }

            return true;
        }

        // Update User Status
        //
        // Update the user status in the model
        // @param username The username for the current user
        // @param status The new status for the current user
        public void UpdateUserStatus(string username, STATUS newStatus)
        {
            ServerUser oldUser = GetUserObj(username);
            ServerUser newUser = oldUser;
            newUser.SetStatus(newStatus);

            UpdateUserList(oldUser, newUser);
        }

        // Update Conversation List
        //
        // Update the conversation list in the model
        // @param rConv The conversation element to remove
        // @param aConv The conversation element to add
        public void UpdateConvList(ServerConversation rConv, ServerConversation aConv)
        {
            List<ServerConversation> convList = data.GetConversationList();

            // Check if the conversation object to remove has been specified
            if (rConv != null)
            {
                convList.Remove(rConv);
            }

            // Check if the conversation object to add has been specified
            if (aConv != null)
            {
                convList.Add(aConv);
            }

            data.SetConversationList(convList);
        }

        // Update User List
        //
        // Update the user list in the model
        // @param rUser The user element to remove
        // @param aUser The user element to add
        public void UpdateUserList(ServerUser rUser, ServerUser aUser)
        {
            List<ServerUser> userList = data.GetUserList();

            // Check if the user object to remove has been specified
            if (rUser != null)
            {
                userList.Remove(rUser);
            }

            // Check if the user object to add has been specified
            if (aUser != null)
            {
                userList.Add(aUser);
            }

            data.SetUserList(userList);
        }
        #endregion

        #region Modify Objects
        // Add Participant To Conversation
        //
        // Add the specified participant to the specified conversation
        // @arg convName The name of the conversation
        // @arg username The username of the new participant
        // @return error if it arises
        public string AddParticipantToConversation(string convName, string username)
        {
            // Get conversation
            ServerConversation oldConv = GetConvObj(convName);
            if (oldConv == new ServerConversation())
            {
                return "Conversation doesn't exist.";
            }

            // Get user
            ServerUser user = GetUserObj(username);
            if (user == new ServerUser())
            {
                return "User doesn't exist.";
            }

            // Add the user to the list of participants in the model
            ServerConversation newConv = oldConv;
            newConv.AddParicipant(user);
            UpdateConvList(oldConv, newConv);

            return "";
        }

        // Add Participants To Conversation
        //
        // Add the specified participants to the specified conversation
        // @arg convName The name of the conversation
        // @arg usernames The string delimited list of usernames to be added
        // @return an error created during the process if there is one
        public string AddParsToConv(string convName, string usernames)
        {
            string[] newParUnames = usernames.Split(',');
            for (int i = 0; i < newParUnames.Length; i++)
            {
                string curError = AddParticipantToConversation(convName, newParUnames[i]);
                if (curError != "")
                {
                    return curError;
                }
            }

            return "";
        }

        
        // Remove Participant From Conversation
        // 
        // Remove the user from the conversation in the model
        // @arg convName The name of the conversation
        // @arg username The username of the current user
        // @return An error message if one occurs.
        public string RemoveParticipantFromConversation(string convName, string username)
        {
            ServerConversation conv = data.GetConversationList().Find(x => x.GetConversationName().Equals(convName));
            ServerUser par = data.GetUserList().Find(x => x.GetUsername().Equals(username));

            if (conv != null)
            {
                if (par != null)
                {
                    if (conv.ContainsParticipant(par))
                    {
                        conv.RemoveParicipant(par);
                        return "";
                    }
                    else
                    {
                        return "Conversation doesn't contain the user.";
                    }
                }
                else
                {
                    return "User does not exist.";
                }
            }
            else
            {
                return "Conversation does not exist.";
            }
        }
        #endregion
    }
}
