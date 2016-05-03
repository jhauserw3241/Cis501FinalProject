using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Windows.Forms;

namespace serverChat
{
    public class ServerController : WebSocketBehavior
    {
        private ServerModel data = new ServerModel();
        public event ServerOutputHandler Output;
        private int sessionCt = 0;

        #region Handle View Output
        // Handle Generic Input
        //
        // Handle input that provides the generic event arugments
        // @param sender The component that raised the event
        // @param e The supplied arguments
        public void HandleGenericInput(object sender, EventArgs e)
        {
            Output("Debug", sessionCt.ToString());
            switch (((Button)sender).Name)
            {
                case "usersButton":
                    Output("UpdateUserList", "");
                    break;
                case "convButton":
                    Output("UpdateConversationList", "");
                    break;
                default:
                    Output("InvalidInputOption", "");
                    break;
            }
        }

        // Handle Mouse Input
        //
        // Handle input that provides the mouse event arugments
        // @param sender The component that raised the event
        // @param e The supplied arguments
        public void HandleMouseInput(object sender, MouseEventArgs e)
        {
            switch(((ListBox)sender).Name)
            {
                case "eleListBox":
                    Output("ProvideEleName", ((ListBox)sender).SelectedIndex);
                    break;
                default:
                    Output("InvalidInputOption");
                    break;
            }
        }
        #endregion

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
            Dictionary<string, string> input = DeserializeXml(e.Data);
            string outputXml = "";
            Dictionary<string, string> outputDict = new Dictionary<string, string>();

            sessionCt = Sessions.IDs.Count();

            //if (!msg.ContainsKey("action")) return;

            switch (input["action"])
            {
                case "sign":
                    outputXml = ProcessSignUpRequest(input);
                    Sessions.Broadcast(outputXml);
                    break;
                case "login":
                    outputXml = ProcessLoginRequest(input);
                    Sessions.Broadcast(outputXml);
                    break;
                case "addCont":
                    // TODO: Send update to everyone involved
                    outputDict = ProcessAddContactRequest(input);
                    if (outputDict.ContainsKey("all"))
                    {
                        Sessions.Broadcast(outputDict["all"]);
                    }
                    else
                    {
                        Sessions.Broadcast(outputDict["source"]);
                    }
                    break;
                case "rmCont":
                    // TODO: Verify contact is part of user contact list
                    // TODO: Remove contact

                    break;
                case "udConv":
                    // TODO: Verify conversation exists
                    // TODO: Update the specified aspects of the conversation

                    break;
                case "udCont":
                    // TODO: Verify user existence
                    // TODO: Update the specified parts of the user object

                    break;
                case "msg":
                    // TODO: Pass message

                    break;
                default:
                    // TODO: Pass error message

                    break;
            }
        }
        #endregion

        #region Process Input
        // Process Sign Up Request
        //
        // Process a request for a new user to be created
        // @param uInfo The information for the new user
        // @return a string containing the xml response
        public string ProcessSignUpRequest(Dictionary<string, string> uInfo)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            // Create the user
            string error = CreateUser(uInfo["username"], uInfo["password"]);

            // Check if the creation was succesful
            if (error == "")
            {
                output.Add("action", "sign");
            }
            else
            {
                output.Add("action", "error");
                output.Add("error", error);
            }

            return SerializeXml(output);
        }

        // Process Login Request
        //
        // Process a request for an existing user to login
        // @param uInfo The information for the existing user
        // @return a string containing the xml response
        public string ProcessLoginRequest(Dictionary<string, string> uInfo)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();

            // Get the user object that matches the provided username
            ServerUser user = GetUserObj(uInfo["username"]);

            // Check if the user exists
            if (user == new ServerUser())
            {
                output.Add("action", "error");
                output.Add("error", "The username isn't valid.");
                return SerializeXml(output);
            }

            // Check if the password is correct
            if (user.GetPassword() != uInfo["password"])
            {
                output.Add("action", "error");
                output.Add("error", "The password isn't correct.");
                return SerializeXml(output);
            }

            // Set the user status to online
            UpdateUserStatus(uInfo["username"], STATUS.Online);

            // Get the user contact info to send to the user
            string contUsernames = string.Join(",", user.GetContactListUsernames());
            string contDispNames = string.Join(",", user.GetContactListNames());
            string contStatuses = string.Join(",", user.GetContactListStatuses());

            output.Add("action", "login");
            output.Add("dispName", user.GetName());
            output.Add("contUsername", contUsernames);
            output.Add("contDispName", contDispNames);
            output.Add("contState", contStatuses);

            return SerializeXml(output);
        }

        // Process Add Contact Request
        //
        // Process a request to add a contact to a specific user
        // @param uInfo The information for the existing user
        // @return a dictionary containing the username and the xml response
        public Dictionary<string, string> ProcessAddContactRequest(Dictionary<string, string> uInfo)
        {
            Dictionary<string, string> dataToSer = new Dictionary<string, string>();
            Dictionary<string, string> output = new Dictionary<string, string>();
            string sourceUsername = uInfo["source"];
            string newContactUsername = uInfo["username"];

            ServerUser sourceUser = GetUserObj(sourceUsername);
            // Verify that the source user is an existing user
            if (sourceUser == new ServerUser())
            {
                dataToSer.Add("action", "error");
                dataToSer.Add("error", "The source user doesn't exist.");
                output.Add("all", SerializeXml(dataToSer));
                return output;
            }

            // Verify that the new contact isn't already a contact
            if (sourceUser.GetContactListUsernames().Contains(newContactUsername))
            {
                dataToSer.Add("action", "error");
                dataToSer.Add("error", "The user to be added as a contact is already a contact.");
                output.Add("all", SerializeXml(dataToSer));
                return output;
            }

            // Verify that the new contact is an existing user
            ServerUser newContact = GetUserObj(newContactUsername);
            if (newContact == new ServerUser())
            {
                dataToSer.Add("action", "error");
                dataToSer.Add("error", "The user to be added as a contact doesn't exist.");
                output.Add("all", SerializeXml(dataToSer));
                return output;
            }

            // Update the contact relationships of all the users involved
            List<ServerUser> users = new List<ServerUser>();
            users.Add(sourceUser);
            users.Add(newContact);
            UpdateContactRelationships(users);

            // Get output information
            output.Add("source", GetSerAddContInfo(newContact));
            output.Add("newCont", GetSerAddContInfo(sourceUser));

            return output;
        }

        // Get Serialized Add Contact Information
        //
        // Get the serialized string of the contact in the format of an added contact
        // @param user The user to get the information from
        // @return the XML string for the contact information
        public string GetSerAddContInfo(ServerUser user)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            output.Add("action", "addCont");
            output.Add("username", user.GetUsername());
            output.Add("dispName", user.GetName());
            output.Add("state", user.GetStatus().ToString());
            return SerializeXml(output);
        }
        #endregion

        #region Modify Model Data
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
        public bool UpdateContactRelationships(List<ServerUser> users)
        {
            // Update all the user's contact lists
            int size = users.Count;
            for (int i = 0; i < size; i++)
            {
                // Get relevant information
                List<ServerUser> tempUsers = users;
                ServerUser oldCurUser = users.ElementAt(i);
                ServerUser newCurUser = oldCurUser;
                tempUsers.Remove(newCurUser);

                // Update the contact list of the current user
                int tempSize = tempUsers.Count;
                for (int j = 0; j < tempSize; j++)
                {
                    ServerUser tempUser = tempUsers.ElementAt(j);
                    newCurUser.AddContact(tempUser);
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

            // Add user to list
            AddUserToList(user);

            return "";
        }
        #endregion

        #region Modify Objects
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
        #endregion

        #region Create Response
        // Get User Obj Message
        //
        // Get the output message that is from the new or previously
        // existing user object
        // @param userObj The object for the user
        // @return a string that creates a new user on the client side
        public string GetUserObjMessage(ServerUser user)
        {
            // Convert the contact list to a single string
            string contUNames = string.Join(",", user.GetContactListUsernames());
            string contDNames = string.Join(",", user.GetContactListNames());

            Dictionary<string, string> output = new Dictionary<string, string>();
            output.Add("action", "login");
            output.Add("username", user.GetUsername());
            output.Add("dispName", user.GetName());
            output.Add("password", user.GetPassword());
            output.Add("contUsername", contUNames);
            output.Add("contDisplName", contDNames);
            output.Add("state", "Online");
            return SerializeXml(output);
        }

        // Get Conversation Message
        //
        // Get the output message that is for the new information for the conversation
        // @param convObj The object for the conversation
        // @return a string that creates a new conversation on the client side
        public string GetConvMessage(ServerConversation convObj)
        {
            // Convert the participants list to a single string
            string parUNames = string.Join(",", convObj.GetParticipantListUsernames());

            Dictionary<string, string> output = new Dictionary<string, string>();
            output.Add("action", "udConv");
            output.Add("conv", convObj.GetConversationName());
            output.Add("par", parUNames);
            return SerializeXml(output);
        }

        // Get the Add Contact Message
        //
        // Get the output message that is for adding a new contact to the user's profile
        // @param contact The user object for the new contact
        // @return a string that creates a new contact on the client side
        public string GetAddContMessage(ServerUser contact)
        {
            Dictionary<string, string> output = new Dictionary<string, string>();
            output.Add("action", "addCont");
            output.Add("username", contact.GetUsername());
            output.Add("dispName", contact.GetName());
            output.Add("state", contact.GetStatus().ToString());
            return SerializeXml(output);
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

            return new ServerUser();
        }
        #endregion

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

        #region XML Serialization
        // Deserialize XML
        //
        // Deserialize the provided XML
        // @param msg The message containing the unparsed XML string
        // @return the dictionary containing the keywords and their values
        public Dictionary<string, string> DeserializeXml(string msg)
        {
            // Convert the message string to a XML object
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(msg);

            // Initialize the dictionary
            Dictionary<string, string> output = new Dictionary<string, string>();

            // Add the action element to the dictionary
            output.Add("action", xml.DocumentElement.Name);

            // Go through all the nodes in the provided XML object
            foreach (XmlElement node in xml)
            {
                // Go through all the attributes for the provided node
                XmlAttributeCollection attList = node.Attributes;
                foreach (XmlAttribute at in attList)
                {
                    // Add the current attribute and its value to the dictionary
                    output.Add(at.LocalName, at.Value);
                }
            }

            return output;
        }

        // Serialize XML
        //
        // Serialize the provided XML
        // @param infoDict The dictionary containing the information to be convert to XML
        // @return a string containing the message with the unparsed XML string
        public string SerializeXml(Dictionary<string, string> infoDict)
        {
            string output = "<";

            // Add the correct action to the XML string
            if (infoDict.ContainsKey("action"))
            {
                output += (infoDict["action"] + " ");

                infoDict.Remove("action");
            }
            else
            {
                output += "error ";
            }

            // Add each of the attributes to the XML string
            int size = infoDict.Count;
            for (int i = 0; i < size; i++)
            {
                KeyValuePair<string, string> curEle = infoDict.ElementAt(i);

                output += string.Format("{0}=\"{1}\" ", curEle.Key, curEle.Value);

                infoDict.Remove(curEle.Key);
            }

            output += "/>";

            return output;
        }
        #endregion
    }
}
